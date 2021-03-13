using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NLog;
using ProductRest.Controllers;
using ProductRest.Data.Contracts;
using ProductRest.Dtos;
using Xunit;

namespace ProductRest.Product.UnitTests
{
    public class ProductControllerTests
    {
        [Fact]
        public async Task GetProductAsync_WithUnexistingProduct_ReturnsNotFound()
        {
            // Arrange
            var repositoryStub = new Mock<IProductsRepository>();
            repositoryStub.Setup(repo => repo.GetProductAsync(It.IsAny<Guid>()))
                .ReturnsAsync((ProductDto) null);

            var loggerStub = new Mock<ILogger<ProductController>>();

            var mappingStub = new Mock<IMapper>();

            var controller = new ProductController(repositoryStub.Object, loggerStub.Object, mappingStub.Object);

            // Act
            var result = await controller.GetProduct(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}