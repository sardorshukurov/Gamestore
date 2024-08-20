namespace Gamestore.Common.Exceptions.NotFound;
public class PublisherNotFoundException : NotFoundException
{
    public PublisherNotFoundException(Guid id)
        : base($"Publisher with id: {id} not found")
    {
    }

    public PublisherNotFoundException(string companyName)
        : base($"Publisher with name: {companyName} not found")
    {
    }
}
