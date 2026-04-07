using EcommerceDev.Application.Queries.Products.GetAllProducts;
using EcommerceDev.Core.Entities;
using EcommerceDev.Core.Repositories;
using EcommerceDev.Infrastructure.Caching;
using Moq;

namespace EcommerceDev.UnitTests.Application
{
    public class GetAllProductsQueryTests
    {
        // Scenario 1: 3 products not in cache
        [Fact]
        public async Task ThreeProductsNotInCache_GetAllProductsIsCalled_ReturnCorrectValues()
        {
            // Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            var cacheServiceMock = new Mock<ICacheService>();

            var items = new List<Product>
            {
                new Product("Product A", "Description A", 1, "Brand A", 1, Guid.NewGuid()),
                new Product("Product B", "Description B", 1, "Brand B", 1, Guid.NewGuid()),
                new Product("Product C", "Description C", 1, "Brand C", 1, Guid.NewGuid())
            };

            productRepositoryMock.Setup(pr => pr.GetAll()).ReturnsAsync(items);

            // Act
            var query = new GetAllProductsQuery();
            var handler = new GetAllProductsQueryHandler(cacheServiceMock.Object, productRepositoryMock.Object);

            var result = await handler.HandleAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.NotEmpty(result.Data);

            productRepositoryMock.Verify(pr => pr.GetAll(), Times.Once);
        }

        // Scenario 2: 3 products in cache
        [Fact]
        public async Task ThreeProductsInCache_GetAllProductsIsCalled_ReturnCorrectValues()
        {
            // Arrange
            var productRepositoryMock = new Mock<IProductRepository>();
            var cacheServiceMock = new Mock<ICacheService>();

            var items = new List<GetAllProductsItemViewModel>
            {
                new() { Id = Guid.NewGuid(), Title = "Product A", Price = 1 },
                new() { Id = Guid.NewGuid(), Title = "Product B", Price = 2 },
                new() { Id = Guid.NewGuid(), Title = "Product C", Price = 3 },
            };

            cacheServiceMock.Setup(pr =>
                pr.GetAsync<List<GetAllProductsItemViewModel>>(It.IsAny<string>())).ReturnsAsync(items);

            // Act
            var query = new GetAllProductsQuery();
            var handler = new GetAllProductsQueryHandler(cacheServiceMock.Object, productRepositoryMock.Object);

            var result = await handler.HandleAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.NotEmpty(result.Data);

            cacheServiceMock.Verify(pr =>
                pr.GetAsync<List<GetAllProductsItemViewModel>>(It.IsAny<string>()), Times.Once);
            productRepositoryMock.Verify(pr => pr.GetAll(), Times.Never);
        }
    }
}
