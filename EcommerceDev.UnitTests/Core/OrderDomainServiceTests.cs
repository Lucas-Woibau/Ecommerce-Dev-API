using EcommerceDev.Core.Entities;
using EcommerceDev.Core.Repositories;
using Moq;

namespace EcommerceDev.UnitTests.Core
{
    public class OrderDomainServiceTests
    {
        // Scenario 1: 100km with 5 units

        [Fact]
        public void Distance100kmAnd5Units_CalculateShipppingCostIsCalled_ReturnCorrectValue()
        {
            // Arrange
            var repositoryMock = new Mock<IProductRepository>();

            const int distanceKm = 100;
            var items = new List<OrderItem>
            {
                new OrderItem(Guid.NewGuid(), 5)
            };

            // Act
            var orderDomainService = new OrderDomainService(repositoryMock.Object);
            var result = orderDomainService.CalculateShippingCost(distanceKm, items);

            // Assert
            Assert.Equal(3_012.50m, result);
        }

        // Scenario 2: 0km with 10 units
        [Fact]
        public void Distance0kmAnd10Units_CalculateShipppingCostIsCalled_ReturnCorrectValue()
        {
            // Arrange
            var repositoryMock = new Mock<IProductRepository>();

            const int distanceKm = 0;
            var items = new List<OrderItem>
            {
                new OrderItem(Guid.NewGuid(), 10)
            };

            // Act
            var orderDomainService = new OrderDomainService(repositoryMock.Object);
            var result = orderDomainService.CalculateShippingCost(distanceKm, items);

            // Assert
            Assert.Equal(55, result);
        }

        // Scenario 3: 1km with 10 units
        [Fact]
        public void Distance1kmAnd10Units_CalculateShipppingCostIsCalled_ReturnCorrectValue()
        {
            // Arrange
            var repositoryMock = new Mock<IProductRepository>();

            const int distanceKm = 1;
            var items = new List<OrderItem>
            {
                new OrderItem(Guid.NewGuid(), 10)
            };

            // Act
            var orderDomainService = new OrderDomainService(repositoryMock.Object);
            var result = orderDomainService.CalculateShippingCost(distanceKm, items);

            // Assert
            Assert.Equal(55, result);
        }

        // Scenario 4: 50km item 1 (3 units), item 2 (7 units)
        [Fact]
        public void Distance50kmAnd3And7Units_CalculateShipppingCostIsCalled_ReturnCorrectValue()
        {
            // Arrange
            var repositoryMock = new Mock<IProductRepository>();

            const int distanceKm = 50;
            var items = new List<OrderItem>
            {
                new OrderItem(Guid.NewGuid(), 3),
                new OrderItem(Guid.NewGuid(), 7)
            };

            // Act
            var orderDomainService = new OrderDomainService(repositoryMock.Object);
            var result = orderDomainService.CalculateShippingCost(distanceKm, items);

            // Assert
            Assert.Equal(1_525m, result);
        }

        // Scenario 5: 20km with 0 items
        [Fact]
        public void Distance20kmAnd0Units_CalculateShipppingCostIsCalled_ThrowError()
        {
            // Arrange
            var repositoryMock = new Mock<IProductRepository>();

            const int distanceKm = 20;
            var items = new List<OrderItem>();

            // Act
            var orderDomainService = new OrderDomainService(repositoryMock.Object);
            Action action = () => orderDomainService.CalculateShippingCost(distanceKm, items);

            // Assert
            var exception = Assert.Throws<InvalidOperationException>(action);

            Assert.Equal("No items found.", exception.Message);
        }

        // Scenario 6: 500km with 10 items (error)
        [Fact]
        public void Distance500kmAnd10Units_CalculateShipppingCostIsCalled_ThrowError()
        {
            // Arrange
            var repositoryMock = new Mock<IProductRepository>();

            const int distanceKm = 500;
            var items = new List<OrderItem>
            {
                new OrderItem(Guid.NewGuid(), 10)
            };

            // Act
            var orderDomainService = new OrderDomainService(repositoryMock.Object);
            Action action = () => orderDomainService.CalculateShippingCost(distanceKm, items);

            // Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(action);

            Assert.StartsWith("Distance out of range.", exception.Message);
        }

        // Scenario 7: -10km with 10 units (error)
        [Fact]
        public void DistanceMinus10kmAnd10Units_CalculateShipppingCostIsCalled_ThrowError()
        {
            // Arrange
            var repositoryMock = new Mock<IProductRepository>();

            const int distanceKm = -10;
            var items = new List<OrderItem>
            {
                new OrderItem(Guid.NewGuid(), 10)
            };

            // Act
            var orderDomainService = new OrderDomainService(repositoryMock.Object);
            Action action = () => orderDomainService.CalculateShippingCost(distanceKm, items);

            // Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(action);

            Assert.StartsWith("Distance out of range.", exception.Message);
        }
    }
}
