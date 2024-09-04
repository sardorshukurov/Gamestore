using Gamestore.Domain.Entities.Users;

namespace Gamestore.BLL.DTOs.Role;

public record UpdateRoleRequest(
    Guid Id,
    string Name,
    Permissions Permissions);
