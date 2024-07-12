using Gamestore.BLL.DTOs.Platform;

namespace Gamestore.BLL.Services.PlatformService;

public interface IPlatformService
{
    Task<ICollection<PlatformDto>> GetAllAsync();

    Task<PlatformDto> GetByIdAsync(Guid id);

    Task UpdateAsync(UpdatePlatformDto dto);

    Task DeleteAsync(Guid id);

    Task CreateAsync(CreatePlatformDto dto);

    Task<ICollection<PlatformDto>> GetAllByGameKeyAsync(string gameKey);
}