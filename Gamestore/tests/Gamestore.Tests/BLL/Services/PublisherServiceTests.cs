using Gamestore.BLL.DTOs.Publisher;
using Gamestore.BLL.Services.PublisherService;
using Gamestore.Common.Exceptions;
using Gamestore.DAL.Entities;
using Gamestore.DAL.Repository;

namespace Gamestore.Tests.BLL.Services;

public class PublisherServiceTests
{
    private readonly IFixture _fixture;

    private readonly Mock<IRepository<Game>> _gameRepositoryMock;
    private readonly Mock<IRepository<Publisher>> _publisherRepostioryMock;

    private readonly PublisherService _service;

    public PublisherServiceTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());

        _gameRepositoryMock = _fixture.Freeze<Mock<IRepository<Game>>>();
        _publisherRepostioryMock = _fixture.Freeze<Mock<IRepository<Publisher>>>();

        _service = new PublisherService(_publisherRepostioryMock.Object, _gameRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsyncShouldReturnCorrectData()
    {
        // Arrange
        var publisherList = _fixture.CreateMany<Publisher>();
        var publishers = publisherList.ToList();
        var publisherDtoList = publishers.Select(p => p.ToDto()).ToList();

        _publisherRepostioryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(publishers);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().BeEquivalentTo(publisherDtoList);
        _publisherRepostioryMock.Verify(x => x.GetAllAsync(), Times.Once());
    }

    [Fact]
    public async Task GetByIdAsyncReturnsPublisherWhenPublisherIsFound()
    {
        // Arrange
        var expectedPublisher = _fixture.Create<Publisher>();
        Guid id = expectedPublisher.Id;

        _publisherRepostioryMock
            .Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync(expectedPublisher);

        // Act
        var actualPublisher = await _service.GetByIdAsync(id);

        // Assert
        actualPublisher.Should().BeEquivalentTo(expectedPublisher.ToDto());
    }

    [Fact]
    public async Task UpdateAsyncWhenPublisherExistsShouldUpdatePublisher()
    {
        // Arrange
        var publisherDto = _fixture.Create<UpdatePublisherDto>();
        var publisher = _fixture.Create<Publisher>();

        _publisherRepostioryMock.Setup(r => r.GetByIdAsync(publisherDto.Id)).ReturnsAsync(publisher);

        // Act
        await _service.UpdateAsync(publisherDto);

        // Assert
        _publisherRepostioryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsyncWhenPublisherDoesNotExistShouldThrowException()
    {
        // Arrange
        var publisherDto = _fixture.Create<UpdatePublisherDto>();
        _publisherRepostioryMock.Setup(r => r.GetByIdAsync(publisherDto.Id)).ReturnsAsync((Publisher)null);

        // Assert
        await Assert.ThrowsAsync<PublisherNotFoundException>(() => _service.UpdateAsync(publisherDto));
    }

    [Fact]
    public async Task DeleteAsyncShouldCallDeleteAndSaveChanges()
    {
        // Arrange
        var id = Guid.NewGuid();

        _publisherRepostioryMock
            .Setup(r => r.DeleteByIdAsync(id))
            .Returns(Task.CompletedTask);

        // Act
        await _service.DeleteAsync(id);

        // Assert
        _publisherRepostioryMock.Verify(r => r.DeleteByIdAsync(id), Times.Once);
        _publisherRepostioryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsyncShouldCallCreateAndSaveChangesMethods()
    {
        // Arrange
        var publisherDto = _fixture.Create<CreatePublisherDto>();
        var publisher = publisherDto.ToEntity();

        _publisherRepostioryMock
            .Setup(r => r.CreateAsync(publisher))
            .Returns(Task.CompletedTask);

        // Act
        await _service.CreateAsync(publisherDto);

        // Assert
        _publisherRepostioryMock.Verify(r => r.CreateAsync(It.IsAny<Publisher>()), Times.Once);
        _publisherRepostioryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByCompanyNameAsyncShouldReturnPublisherDto()
    {
        // Arrange
        var publisher = _fixture.Create<Publisher>();
        string companyName = publisher.CompanyName;

        _publisherRepostioryMock.Setup(r => r.GetOneAsync(p => p.CompanyName == companyName)).ReturnsAsync(publisher);

        // Act
        var result = await _service.GetByCompanyNameAsync(companyName);

        // Assert
        result.Should().BeEquivalentTo(publisher.ToDto());
    }

    [Fact]
    public async Task GetByGameKeyAsyncShouldReturnPublisherDto()
    {
        // Arrange
        var game = _fixture.Create<Game>();
        var publisher = _fixture.Create<Publisher>();
        string gameKey = game.Key;

        _gameRepositoryMock.Setup(r => r.GetOneAsync(g => g.Key == gameKey)).ReturnsAsync(game);
        _publisherRepostioryMock.Setup(r => r.GetByIdAsync(game.PublisherId)).ReturnsAsync(publisher);

        // Act
        var result = await _service.GetByGameKeyAsync(gameKey);

        // Assert
        result.Should().BeEquivalentTo(publisher.ToDto());
    }

    [Fact]
    public async Task GetByGameKeyAsyncWhenGameNotFoundShouldThrowGameNotFoundException()
    {
        // Arrange
        string gameKey = "key";

        _gameRepositoryMock.Setup(r => r.GetOneAsync(g => g.Key == gameKey)).ReturnsAsync((Game)null);

        // Assert
        await Assert.ThrowsAsync<GameNotFoundException>(() => _service.GetByGameKeyAsync(gameKey));
    }
}