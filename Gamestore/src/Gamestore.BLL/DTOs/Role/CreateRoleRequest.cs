using Gamestore.Domain.Entities.Users;

namespace Gamestore.BLL.DTOs.Role;

public record CreateRoleRequest(
    string Name,
    Permissions Permissions);
