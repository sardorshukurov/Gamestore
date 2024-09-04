using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using Gamestore.BLL.DTOs.Role;
using Gamestore.BLL.DTOs.User;
using Gamestore.Common.Exceptions.BadRequest;
using Gamestore.Common.Exceptions.NotFound;
using Gamestore.DAL.Repository;
using Gamestore.Domain.Entities.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Gamestore.BLL.Services.UserService;

public class UserService(
    IHttpClientFactory httpClientFactory,
    IConfiguration configuration,
    IRepository<User> userRepository,
    IRepository<UserRole> userRoleRepository) : IUserService
{
    private readonly HttpClient _authClient = httpClientFactory.CreateClient("AuthAPI");

    public async Task<IEnumerable<UserResponse>> GetAllAsync()
    {
        IEnumerable<User> localUsers = await userRepository.GetAllAsync();
        var result = await _authClient.GetAsync("/api/users");

        if (!result.IsSuccessStatusCode)
        {
            return localUsers.Select(u => u.ToResponse());
        }

        var remoteUsers = JsonConvert.DeserializeObject<List<AuthClientResponse>>(
            await result.Content.ReadAsStringAsync())!;

        var localUsersDict = localUsers.ToDictionary(u => u.Email, u => u);
        var remoteUsersDict = remoteUsers.ToDictionary(u => u.Email, u => u);

        // Synchronize data based on remote being the source of truth
        foreach (var remoteUser in remoteUsers)
        {
            if (localUsersDict.TryGetValue(remoteUser.Email, out User? value))
            {
                // Update local user if different
                var localUser = value;
                if (NeedsUpdate(localUser, remoteUser))
                {
                    UpdateLocalUserFromRemote(localUser, remoteUser);
                }
            }
            else
            {
                await CreateUserLocallyAsync(remoteUser);
            }
        }

        var usersToPotentiallyRemove = localUsers.Where(u => !remoteUsersDict.ContainsKey(u.Email));
        foreach (var user in usersToPotentiallyRemove)
        {
            await DeleteUserLocallyAsync(user);
        }

        await userRepository.SaveChangesAsync();
        return (await userRepository.GetAllAsync()).Select(u => u.ToResponse());
    }

    public async Task<AuthResponse> LoginAsync(AuthRequest request)
    {
        var serializedObject = JsonConvert.SerializeObject(request);
        var serializedRequest = new StringContent(serializedObject, Encoding.UTF8, "application/json");
        var result = await _authClient.PostAsync("/api/auth", serializedRequest);

        if (result.IsSuccessStatusCode)
        {
            var user = (await result
                .Content
                .ReadFromJsonAsync<AuthClientResponse>())!;
            var token = GenerateJwtToken(user);

            return new AuthResponse(token);
        }
        else if (result.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new AuthenticationException();
        }
        else if (result.StatusCode == HttpStatusCode.BadRequest)
        {
            throw new BadRequestException(result.Content.ToString() ?? string.Empty);
        }

        throw new Exception("Internal server error");
    }

    public async Task RegisterAsync(RegisterUserRequest request)
    {
        var roles = (await userRoleRepository
            .GetAllByFilterAsync(ur => request.RoleIds.Contains(ur.Id), true))
            .ToList();
        var entity = request.ToEntity(roles);

        var requestToService = new RegisterClientRequest(
            entity.Email,
            entity.FirstName,
            entity.LastName,
            request.Password,
            request.Password);

        var serializedObject = JsonConvert.SerializeObject(requestToService);
        var serializedRequest = new StringContent(serializedObject, Encoding.UTF8, "application/json");
        var result = await _authClient.PostAsync("/api/users", serializedRequest);

        if (result.IsSuccessStatusCode)
        {
            await userRepository.CreateAsync(entity);
            await userRepository.SaveChangesAsync();
        }
        else if (result.StatusCode == HttpStatusCode.BadRequest)
        {
            throw new BadRequestException(result.Content.ToString() ?? string.Empty);
        }
        else
        {
            throw new Exception("Internal server error");
        }
    }

    public async Task UpdateUserAsync(UpdateUserRequest request)
    {
        var user = await userRepository.GetOneAsync(u => u.Email == request.OriginalEmail, u => u.Roles)
            ?? throw new UserNotFoundException(request.OriginalEmail);

        var roles = (await userRoleRepository
            .GetAllByFilterAsync(ur => request.RoleIds.Contains(ur.Id), true))
            .ToList();

        request.UpdateEntity(user, roles);

        var requestToService = new UpdateUserClientRequest(
            user.Email,
            user.FirstName,
            user.LastName,
            request.Password,
            request.Password);

        var serializedObject = JsonConvert.SerializeObject(requestToService);
        var serializedRequest = new StringContent(serializedObject, Encoding.UTF8, "application/json");
        var result = await _authClient.PutAsync($"/api/users?originalEmail={request.OriginalEmail}", serializedRequest);

        if (result.IsSuccessStatusCode)
        {
            await userRepository.UpdateAsync(user.Id, user);
            await userRepository.SaveChangesAsync();
        }
        else if (result.StatusCode == HttpStatusCode.BadRequest)
        {
            throw new BadRequestException(result.Content.ToString() ?? string.Empty);
        }
        else
        {
            throw new Exception("Internal server error");
        }
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        var user = await userRepository.GetByIdAsync(userId)
            ?? throw new UserNotFoundException(userId);

        var serializedRequest = new StringContent(JsonConvert.SerializeObject(user.Email), Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri($"{_authClient.BaseAddress}api/users"),
            Content = serializedRequest,
        };
        await _authClient.SendAsync(request);
        await userRepository.DeleteByIdAsync(userId);
        await userRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<UserRoleResponse>> GetUserRolesAsync(Guid userId)
    {
        var user = await userRepository.GetOneAsync(u => u.Id == userId, u => u.Roles)
            ?? throw new UserNotFoundException(userId);

        return user.Roles.Select(r => r.ToResponse());
    }

    // TODO: add user roles
    private string GenerateJwtToken(AuthClientResponse user)
    {
        var authClaims = new List<Claim>()
        {
            new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new(ClaimTypes.NameIdentifier, user.Email),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Sub, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            configuration["JwtKey"]!));

        var token = new JwtSecurityToken(
            issuer: configuration["Issuer"],
            audience: configuration["Audience"],
            expires: DateTime.UtcNow.AddDays(1),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static bool NeedsUpdate(User localUser, AuthClientResponse remoteUser)
        => localUser.Email == remoteUser.Email
            && localUser.FirstName == remoteUser.FirstName
            && localUser.LastName == remoteUser.LastName;

    private static void UpdateLocalUserFromRemote(User localUser, AuthClientResponse remoteUser)
    {
        localUser.FirstName = remoteUser.FirstName;
        localUser.LastName = remoteUser.LastName;
        localUser.Email = remoteUser.Email;
    }

    private async Task CreateUserLocallyAsync(AuthClientResponse remoteUser)
    {
        var user = new User
        {
            FirstName = remoteUser.FirstName,
            LastName = remoteUser.LastName,
            Email = remoteUser.Email,
        };

        await userRepository.CreateAsync(user);
    }

    private async Task DeleteUserLocallyAsync(User user)
    {
        await userRepository.DeleteByIdAsync(user.Id);
    }

    private record AuthClientResponse(
        string Email,
        string FirstName,
        string LastName);

    private record RegisterClientRequest(
        string Email,
        string FirstName,
        string LastName,
        string Password,
        string ConfirmPassword);

    private record UpdateUserClientRequest(
        string Email,
        string FirstName,
        string LastName,
        string Password,
        string ConfirmPassword);
}
