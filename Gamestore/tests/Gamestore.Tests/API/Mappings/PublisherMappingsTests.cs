using Gamestore.API.DTOs.Publisher;
using Gamestore.BLL.DTOs.Publisher;

namespace Gamestore.Tests.API.Mappings;

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
        var dto = _fixture.Create<PublisherDto>();

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
        var dto = _fixture.Create<PublisherDto>();

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
        var dto = _fixture.Create<PublisherShortDto>();

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
        var dto = request.ToDto();

        // Assert
        dto.CompanyName.Should().Be(request.Publisher.CompanyName);
        dto.HomePage.Should().Be(request.Publisher.HomePage);
        dto.Description.Should().Be(request.Publisher.Description);
    }
}