using Gamestore.BLL.DTOs.Platform;

namespace Gamestore.BLL.Services.PlatformService;

public interface IPlatformService
{
    Task<ICollection<PlatformShortDto>> GetAllAsync();

    Task<PlatformShortDto?> GetByIdAsync(Guid id);

    Task UpdateAsync(UpdatePlatformDto dto);

    Task DeleteAsync(Guid id);

    Task CreateAsync(CreatePlatformDto dto);

    Task<ICollection<PlatformShortDto>> GetAllByGameKeyAsync(string gameKey);
}