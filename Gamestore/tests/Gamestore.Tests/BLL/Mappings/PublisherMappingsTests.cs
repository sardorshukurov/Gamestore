using Gamestore.BLL.DTOs.Publisher;
using Gamestore.Domain.Entities.Games;

namespace Gamestore.Tests.BLL.Mappings;

public class PublisherMappingsTests
{
    private readonly IFixture _fixture;

    public PublisherMappingsTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void AsResponseMapsCorrectly()
    {
        // Arrange
        var dto = _fixture.Create<Publisher>();

        // Act
        var response = dto.ToResponse();

        // Assert
        response.Id.Should().Be(dto.Id);
        response.CompanyName.Should().Be(dto.CompanyName);
        response.Description.Should().Be(dto.Description);
        response.HomePage.Should().Be(dto.HomePage);
    }

    [Fact]
    public void AsShortResponseMapsCorrectly()
    {
        // Arrange
        var dto = _fixture.Create<Publisher>();

        // Act
        var response = dto.ToShortResponse();

        // Assert
        response.Id.Should().Be(dto.Id);
        response.CompanyName.Should().Be(dto.CompanyName);
    }

    [Fact]
    public void AsShortResponseFromShortDtoMapsCorrectly()
    {
        // Arrange
        var dto = _fixture.Create<Publisher>();

        // Act
        var response = dto.ToShortResponse();

        // Assert
        response.Id.Should().Be(dto.Id);
        response.CompanyName.Should().Be(dto.CompanyName);
    }

    [Fact]
    public void AsCreatePublisherDtoMapsCorrectly()
    {
        // Arrange
        var request = _fixture.Create<CreatePublisherRequest>();

        // Act
        var entity = request.ToEntity();

        // Assert
        entity.CompanyName.Should().Be(request.CompanyName);
        entity.HomePage.Should().Be(request.HomePage);
        entity.Description.Should().Be(request.Description);
    }
}