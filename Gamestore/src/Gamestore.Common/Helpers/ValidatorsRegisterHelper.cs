using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Gamestore.Common.Helpers;

public static class ValidatorsRegisterHelper
{
    public static void RegisterValidators(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(s => s.FullName != null && s.FullName.StartsWith("Gamestore.", StringComparison.CurrentCultureIgnoreCase));

        assemblies.ToList().ForEach(x => { services.AddValidatorsFromAssembly(x, ServiceLifetime.Scoped); });
    }
}