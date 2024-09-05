using Gamestore.BLL.DTOs.Publisher;
using Gamestore.BLL.Services.PublisherService;
using Gamestore.Common.Exceptions.NotFound;
using Gamestore.DAL.Repository;
using Gamestore.Domain.Entities.Games;

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
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior(recursionDepth: 1));

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

        _publisherRepostioryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(publishers);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Count.Should().Be(publishers.Count);
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
        actualPublisher.Id.Should().Be(expectedPublisher.Id);
    }

    [Fact]
    public async Task UpdateAsyncWhenPublisherExistsShouldUpdatePublisher()
    {
        // Arrange
        var publisherRequest = _fixture.Create<UpdatePublisherRequest>();
        var publisher = _fixture.Create<Publisher>();

        _publisherRepostioryMock.Setup(r => r.GetByIdAsync(publisherRequest.Id)).ReturnsAsync(publisher);

        // Act
        await _service.UpdateAsync(publisherRequest);

        // Assert
        _publisherRepostioryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsyncWhenPublisherDoesNotExistShouldThrowException()
    {
        // Arrange
        var publisherDto = _fixture.Create<UpdatePublisherRequest>();
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
        var publisherRequest = _fixture.Create<CreatePublisherRequest>();
        var publisher = publisherRequest.ToEntity();

        _publisherRepostioryMock
            .Setup(r => r.CreateAsync(publisher))
            .Returns(Task.CompletedTask);

        // Act
        await _service.CreateAsync(publisherRequest);

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
        result.Id.Should().Be(publisher.Id);
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
        result.Id.Should().Be(publisher.Id);
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