using EntityApp.Api.Dtos;
using EntityApp.Api.Services;
using FluentAssertions;

namespace EntityApp.Api.Tests;

public class EntityServiceTests
{
    [Fact]
    public async Task CreateEntity_ShouldReturnEntityWithValidId()
    {
        // Arrange
        var service = new EntityService();
        var dto = new CreateEntityDto("Test Item");

        // Act
        var result = await service.CreateEntityAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
        result.Name.Should().Be("Test Item");
    }

    [Fact]
    public async Task GetEntitiesAsync_ShouldSupportPagination()
    {
        var service = new EntityService();
        var result = await service.GetEntitiesAsync(1, 5);
        result.Items.Should().NotBeEmpty();
        result.PageSize.Should().Be(5);
    }
}