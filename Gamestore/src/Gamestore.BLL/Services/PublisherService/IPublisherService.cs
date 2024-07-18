using Gamestore.BLL.DTOs.Publisher;

namespace Gamestore.BLL.Services.PublisherService;

public interface IPublisherService
{
    Task<ICollection<PublisherDto>> GetAllAsync();

    Task<PublisherDto?> GetByIdAsync(Guid id);

    Task UpdateAsync(UpdatePublisherDto dto);

    Task DeleteAsync(Guid id);

    Task CreateAsync(CreatePublisherDto dto);

    Task<ICollection<PublisherDto>> GetByCompanyNameAsync(string companyName);

    Task<PublisherDto?> GetAllByGameKeyAsync(string gameKey);
}