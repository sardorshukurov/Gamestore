using Gamestore.BLL.DTOs.Publisher;

namespace Gamestore.BLL.Services.PublisherService;

public interface IPublisherService
{
    Task<ICollection<PublisherResponse>> GetAllAsync();

    Task<PublisherResponse?> GetByIdAsync(Guid id);

    Task UpdateAsync(UpdatePublisherRequest request);

    Task DeleteAsync(Guid id);

    Task CreateAsync(CreatePublisherRequest request);

    Task<PublisherResponse?> GetByCompanyNameAsync(string companyName);

    Task<PublisherResponse?> GetByGameKeyAsync(string gameKey);
}