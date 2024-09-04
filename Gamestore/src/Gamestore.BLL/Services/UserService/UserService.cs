using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using Gamestore.BLL.DTOs.User;
using Gamestore.Common.Exceptions.BadRequest;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Gamestore.BLL.Services.UserService;

public class UserService(
    IHttpClientFactory httpClientFactory,
    IConfiguration configuration) : IUserService
{
    private readonly HttpClient _authClient = httpClientFactory.CreateClient("AuthAPI");

    public async Task<AuthResponse> Login(AuthRequest request)
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

        throw new Exception();
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

    private record AuthClientResponse(
        string Email,
        string FirstName,
        string LastName);
}
