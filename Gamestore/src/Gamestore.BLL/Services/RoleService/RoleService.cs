using Gamestore.BLL.DTOs.Role;
using Gamestore.BLL.DTOs.User;
using Gamestore.Common.Exceptions.NotFound;
using Gamestore.DAL.Repository;
using Gamestore.Domain.Entities.Users;

namespace Gamestore.BLL.Services.RoleService;

public class RoleService(IRepository<UserRole> userRoleRepository) : IRoleService
{
    public async Task<IEnumerable<UserRoleResponse>> GetRolesAsync()
    => (await userRoleRepository.GetAllAsync()).Select(ur => ur.ToResponse());

    public async Task<UserRoleResponse> GetRoleByIdAsync(Guid id)
    {
        var role = await userRoleRepository.GetByIdAsync(id)
            ?? throw new UserRoleNotFoundException(id);

        return role.ToResponse();
    }

    public async Task<Permissions> GetRolePermissionsAsync(Guid roleId)
    {
        var role = await userRoleRepository.GetByIdAsync(roleId)
            ?? throw new UserRoleNotFoundException(roleId);

        return role.Permissions;
    }

    public async Task DeleteRoleAsync(Guid roleId)
    {
        var role = await userRoleRepository.GetByIdAsync(roleId)
            ?? throw new UserRoleNotFoundException(roleId);

        await userRoleRepository.DeleteByIdAsync(role.Id);
        await userRoleRepository.SaveChangesAsync();
    }

    public async Task AddRoleAsync(CreateRoleRequest request)
    {
        var entity = request.ToEntity();

        await userRoleRepository.CreateAsync(entity);
        await userRoleRepository.SaveChangesAsync();
    }
}
