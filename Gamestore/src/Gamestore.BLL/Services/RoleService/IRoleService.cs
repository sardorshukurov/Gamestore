using Gamestore.BLL.DTOs.Role;
using Gamestore.Domain.Entities.Users;

namespace Gamestore.BLL.Services.RoleService;

public interface IRoleService
{
    Task<IEnumerable<UserRoleResponse>> GetRolesAsync();

    Task<UserRoleResponse> GetRoleByIdAsync(Guid id);

    Task<Permissions> GetRolePermissionsAsync(Guid roleId);

    Task DeleteRoleAsync(Guid roleId);

    Task AddRoleAsync(CreateRoleRequest request);

    Task UpdateRoleAsync(UpdateRoleRequest request);
}
