using Gamestore.BLL.DTOs.Platform;

namespace Gamestore.BLL.Services.PlatformService;

public interface IPlatformService
{
    Task<ICollection<PlatformShortResponse>> GetAllAsync();

    Task<PlatformShortResponse?> GetByIdAsync(Guid id);

    Task UpdateAsync(UpdatePlatformRequest request);

    Task DeleteAsync(Guid id);

    Task CreateAsync(CreatePlatformRequest request);

    Task<ICollection<PlatformShortResponse>> GetAllByGameKeyAsync(string gameKey);
}