using FluentValidation;
using Gamestore.DAL.Repository;
using Gamestore.Domain.Entities.Users;

namespace Gamestore.BLL.DTOs.User;

public record RegisterUserRequest(
    string FirstName,
    string LastName,
    string Email,
    ICollection<Guid> Roles,
    string Password);

public class RegisterUserValidator : AbstractValidator<RegisterUserRequest>
{
    private readonly IRepository<UserRole> _userRoleRepository;

    public RegisterUserValidator(
        IRepository<UserRole> userRoleRepository)
    {
        _userRoleRepository = userRoleRepository;

        RuleFor(u => u.FirstName)
            .NotEmpty()
            .MinimumLength(3)
            .WithMessage("First name should contain at least 3 characters");

        RuleFor(u => u.LastName)
            .NotEmpty()
            .MinimumLength(3)
            .WithMessage("Last name should contain at least 3 characters");

        RuleFor(u => u.Email)
            .NotEmpty()
            .WithMessage("Email required");

        RuleFor(u => u.Roles)
            .Must((roleIds) => ContainExistingRoles(roleIds).Result)
            .WithMessage("All roles must exist");
    }

    private async Task<bool> ContainExistingRoles(ICollection<Guid> roleIds)
    {
        if (roleIds.Contains(Guid.Empty))
        {
            return false;
        }

        var existingRoleIds = await _userRoleRepository
            .GetAllByFilterAsync(ur => roleIds.Contains(ur.Id));

        return existingRoleIds.Count() == roleIds.Count;
    }
}