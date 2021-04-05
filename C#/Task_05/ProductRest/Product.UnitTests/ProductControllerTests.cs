using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ProductRest.Controllers;
using ProductRest.Services.Contracts;
using Xunit;

namespace Product.UnitTests
{
    public class ProductControllerTests // FIXME  Bearer authorization
    {
        private readonly Mock<IProductService> _productService = new();
        private readonly Mock<ILogger<ProductController>> _loggerStub = new();

        [Fact]
        public async Task GetProductAsync_WithUnexistingProduct_ReturnsNotFound()
        {
            // Arrange
            _productService.Setup(serv => serv.GetOne(It.IsAny<Guid>()))
                .ReturnsAsync((ProductRest.Entities.Product) null);

            var controller = new ProductController(_productService.Object, _loggerStub.Object);

            // Act
            var result = await controller.GetProduct(Guid.NewGuid());

            // Assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GetProductAsync_WithExistingProduct_ReturnsExpectedProduct()
        {
            // Arrange
            var expectedProduct = TestProductDto();
            
            _productService.Setup(serv => serv.GetOne(It.IsAny<Guid>()))
                .ReturnsAsync(expectedProduct);
            
            var controller = new ProductController(_productService.Object, _loggerStub.Object);

            // Act
            var actionResult = await controller.GetProduct(expectedProduct.Id);
            
            // Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
        }
        
        private ProductRest.Entities.Product TestProductDto()
        {
            return new()
            {
                Id = Guid.NewGuid(),
                AddressLine = "new address",
                PostalCode = "12345",
                Country = "Country",
                City = "City",
                FaxNumber = "+380994081678",
                PhoneNumber = "+380999088267"
            };
        }
    }
}