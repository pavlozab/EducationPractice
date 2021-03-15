using System;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NLog;
using ProductRest.Controllers;
using ProductRest.Data.Contracts;
using ProductRest.Dtos;
using ProductRest.Models;
using Xunit;

namespace ProductRest.Product.UnitTests
{
    public class ProductControllerTests // FIXME
    {
        private readonly Mock<IProductsRepository> _repositoryStub = new();
        private readonly Mock<ILogger<ProductController>> _loggerStub = new();
        private readonly Mock<IMapper> _mappingStub = new();
        
        [Fact]
        public async Task GetProductAsync_WithUnexistingProduct_ReturnsNotFound()
        {
            // Arrange
            _repositoryStub.Setup(repo => repo.GetProductAsync(It.IsAny<Guid>()))
                .ReturnsAsync((ProductDto) null);

            var controller = new ProductController(_repositoryStub.Object, _loggerStub.Object, _mappingStub.Object);

            // Act
            var result = await controller.GetProduct(Guid.NewGuid());

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetProductAsync_WithUnexistingProduct_ReturnsExpectedProduct()
        {
            // Arrange
            var expectedProduct = CreatedProductDto();
            
            _repositoryStub.Setup(repo => repo.GetProductAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedProduct);
            
            var controller = new ProductController(_repositoryStub.Object, _loggerStub.Object, _mappingStub.Object);

            // Act
            var result = await controller.GetProduct(Guid.NewGuid());

            // Assert
            result.Result.Should().BeOfType<OkResult>();
            result.Value.Should().BeEquivalentTo(
                expectedProduct,
                option => option.ComparingByMembers<ProductDto>()
                );
        }

        [Fact]
        public async Task GetProductsAsync_WithUnexistingProduct_ReturnsAllProducts() 
        {
            // Arrange 
            var expectedProducts = new[] {CreatedProductDto(), CreatedProductDto(), CreatedProductDto()};
            
            var filter = new QueryParametersModel();
            
            _repositoryStub.Setup(repo => repo.GetProductsAsync(filter))
                .ReturnsAsync(expectedProducts);
            
            var controller = new ProductController(_repositoryStub.Object, _loggerStub.Object, _mappingStub.Object);

            // Act
            var actualProducts = await controller.GetProducts(filter);

            // Assert
            actualProducts.Should().BeEquivalentTo(
                expectedProducts,
                options => options.ComparingByMembers<ProductDto>()
                );
        }
        
        [Fact]
        public async Task CreateProductAsync_WithProductToCreate_ReturnsCreatedProduct() 
        {
            // Arrange
            var productToCreate = new CreateProductDto() // FIXME
            {
                AddressLine = "new address",
                PostalCode = "12345",
                Country = "Country",
                City = "City",
                FaxNumber = "+380991111111",
                PhoneNumber = "+380990000000"
            };
            
            var controller = new ProductController(_repositoryStub.Object, _loggerStub.Object, _mappingStub.Object);
            
            // Act
            var result = await controller.CreateProduct(productToCreate);
            
            // Assert
            var createdProduct = (result.Result as CreatedAtActionResult).Value as ProductDto;
            
            productToCreate.Should().BeEquivalentTo(
                createdProduct,
                options => options.ComparingByMembers<ProductDto>().ExcludingMissingMembers()
                );
            createdProduct.Id.Should().NotBeEmpty();
        }
        
        [Fact]
        public async Task UpdateProductAsync_WithExistingProduct_ReturnsNoContent() 
        {
            // Arrange
            var existingProduct = CreatedProductDto();
            
            _repositoryStub.Setup(repo => repo.GetProductAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingProduct);

            var productId = existingProduct.Id;
            var productToUpdate = new CreateProductDto() // FIXME
            {
                AddressLine = "new address",
                PostalCode = "12345",
                Country = "Country",
                City = "City",
                FaxNumber = "+380991111111",
                PhoneNumber = "+380990000000"
            };
            
            var controller = new ProductController(_repositoryStub.Object, _loggerStub.Object, _mappingStub.Object);
            
            
            // Act
            var result = await controller.UpdateProduct(productId, productToUpdate);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteProductAsync_WithExistingProduct_ReturnsNoContent()
        {
            // Arrange
            var existingProduct = CreatedProductDto();
            _repositoryStub.Setup(repo => repo.GetProductAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingProduct);

            var controller = new ProductController(_repositoryStub.Object, _loggerStub.Object, _mappingStub.Object);
            
            
            // Act
            var result = await controller.DeleteProduct(existingProduct.Id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        private ProductDto CreatedProductDto() // FIXME Random obj 
        {
            return new()
            {
                Id = Guid.NewGuid(),
                AddressLine = "new address",
                PostalCode = "12345",
                Country = "Country",
                City = "City",
                FaxNumber = "+380991111111",
                PhoneNumber = "+380990000000"
            };
        }
    }
}