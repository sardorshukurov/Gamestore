using Gamestore.BLL.DTOs.Publisher;
using Gamestore.DAL.Entities;

namespace Gamestore.Tests.BLL.Mappings;

public class PublisherMappingsTests
{
    private readonly IFixture _fixture;

    public PublisherMappingsTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void AsDtoMapsCorrectly()
    {
        // Arrange
        var publisher = _fixture.Create<Publisher>();

        // Act
        var dto = publisher.ToDto();

        // Assert
        dto.Id.Should().Be(publisher.Id);
        dto.CompanyName.Should().Be(publisher.CompanyName);
        dto.Description.Should().Be(publisher.Description);
        dto.HomePage.Should().Be(publisher.HomePage);
    }

    [Fact]
    public void AsShortDtoMapsCorrectly()
    {
        // Arrange
        var publisher = _fixture.Create<Publisher>();

        // Act
        var shortDto = publisher.ToShortDto();

        // Assert
        shortDto.Id.Should().Be(publisher.Id);
        shortDto.CompanyName.Should().Be(publisher.CompanyName);
    }

    [Fact]
    public void AsEntityMapsCorrectly()
    {
        // Arrange
        var dto = _fixture.Create<CreatePublisherDto>();

        // Act
        var publisher = dto.ToEntity();

        // Assert
        publisher.CompanyName.Should().Be(dto.CompanyName);
        publisher.Description.Should().Be(dto.Description);
        publisher.HomePage.Should().Be(dto.HomePage);
    }

    [Fact]
    public void UpdateEntityUpdatesCorrectly()
    {
        // Arrange
        var dto = _fixture.Create<UpdatePublisherDto>();
        var publisher = _fixture.Create<Publisher>();

        // Act
        dto.UpdateEntity(publisher);

        // Assert
        publisher.CompanyName.Should().Be(dto.CompanyName);
        publisher.Description.Should().Be(dto.Description);
        publisher.HomePage.Should().Be(dto.HomePage);
    }
}