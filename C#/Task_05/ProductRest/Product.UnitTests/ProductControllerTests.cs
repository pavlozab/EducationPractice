// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using AutoMapper;
// using FluentAssertions;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Logging;
// using Moq;
// using ProductRest.Controllers;
// using ProductRest.Data.Contracts;
// using ProductRest.Dto;
// using ProductRest.Dto.Product;
// using ProductRest.Entities;
// using ProductRest.Models;
// using ProductRest.Repositories.Contracts;
// using ProductRest.Responses;
// using ProductRest.Services.Contracts;
// using Xunit;
//
// namespace Product.UnitTests
// {
//     public class ProductControllerTests 
//     {
//         private readonly Mock<IProductService> _productService = new();
//         private readonly Mock<ILogger<ProductController>> _loggerStub = new();
//
//         [Fact]
//         public async Task GetProductAsync_WithUnexistingProduct_ReturnsNotFound()
//         {
//             // Arrange
//             _productService.Setup(serv => serv.GetProduct(It.IsAny<Guid>()))
//                 .ReturnsAsync((ProductRest.Entities.Product) null);
//
//             var controller = new ProductController(_productService.Object, _loggerStub.Object);
//
//             // Act
//             var result = await controller.GetProduct(Guid.NewGuid());
//
//             // Assert
//             result.Result.Should().BeOfType<NotFoundResult>();
//         }
//
//         [Fact]
//         public async Task GetProductAsync_WithExistingProduct_ReturnsExpectedProduct()
//         {
//             // Arrange
//             var expectedProduct = TestProductDto();
//             
//             _productService.Setup(serv => serv.GetProduct(It.IsAny<Guid>()))
//                 .ReturnsAsync(expectedProduct);
//             
//             var controller = new ProductController(_productService.Object, _loggerStub.Object);
//
//             // Act
//             var actionResult = await controller.GetProduct(expectedProduct.Id);
//             
//             // Assert
//             actionResult.Result.Should().BeOfType<OkObjectResult>();
//         }
//         
//         [Fact]
//         public async Task CreateProductAsync_WithProductToCreate_ReturnsCreatedProduct() 
//         {
//             // Arrange
//             var productToCreate = TestCreateProductDto();
//             var controller = new ProductController(_productService.Object, _loggerStub.Object);
//             
//             // Act
//             var result = await controller.CreateProduct(productToCreate);
//             
//             // Assert
//             result.Result.Should().BeOfType<CreatedAtActionResult>();
//
//             productToCreate.Should().BeEquivalentTo(
//                 createdProduct,
//                 options => options.ComparingByMembers<ProductRest.Entities.Product>().ExcludingMissingMembers()
//             );
//             
//             createdProduct.Id.Should().NotBeEmpty();
//         }
//         
//         [Fact]
//         public async Task UpdateProductAsync_WithProductToUpdate_ReturnsNoContent() 
//         {
//             // Arrange
//             var existingProduct = TestProductDto();
//             
//             _productService.Setup(serv => serv.GetProduct(It.IsAny<Guid>()))
//                 .ReturnsAsync(existingProduct);
//
//             var productId = existingProduct.Id;
//             var productToUpdate = TestCreateProductDto();
//             
//             var controller = new ProductController(_productService.Object, _loggerStub.Object);
//
//             // Act
//             var result = await controller.UpdateProduct(productId, productToUpdate);
//
//             // Assert
//             result.Should().BeOfType<NoContentResult>();
//         }
//         
//         [Fact]
//         public async Task UpdateProductAsync_WithUnexistingProduct_ReturnsNotFound() 
//         {
//             // Arrange
//             _productService.Setup(serv => serv.GetProductAsync(It.IsAny<Guid>()))
//                 .ReturnsAsync((ProductRest.Entities.Product)null);
//
//             var productToUpdate = TestCreateProductDto();
//             
//             var controller = new ProductController(_productService.Object, _loggerStub.Object, _mappingStub.Object);
//             
//             // Act
//             var result = await controller.UpdateProduct(Guid.NewGuid(), productToUpdate);
//
//             // Assert
//             result.Should().BeOfType<NotFoundResult>();
//         }
//
//         [Fact]
//         public async Task DeleteProductAsync_WithExistingProduct_ReturnsNoContent()
//         {
//             // Arrange
//             var existingProduct = TestProductDto();
//             _productService.Setup(serv => serv.GetProductAsync(It.IsAny<Guid>()))
//                 .ReturnsAsync(existingProduct);
//
//             var controller = new ProductController(_productService.Object, _loggerStub.Object, _mappingStub.Object);
//
//             // Act
//             var result = await controller.DeleteProduct(existingProduct.Id);
//
//             // Assert
//             result.Should().BeOfType<NoContentResult>();
//         }
//         
//         [Fact]
//         public async Task DeleteProductAsync_WithUnexistingProductId_ReturnsNotFound()
//         {
//             // Arrange
//             _productService.Setup(serv => serv.GetProductAsync(It.IsAny<Guid>()))
//                 .ReturnsAsync((ProductRest.Entities.Product)null);
//
//             var controller = new ProductController(_productService.Object, _loggerStub.Object, _mappingStub.Object);
//
//             // Act
//             var result = await controller.DeleteProduct(Guid.NewGuid());
//
//             // Assert
//             result.Should().BeOfType<NotFoundResult>();
//         }
//
//         [Fact]
//         public async Task GetProductsAsync_WithExistingProduct_ReturnsAllProducts() 
//         {
//             // Arrange 
//             var expectedProducts = new List<ProductRest.Entities.Product>() {TestProductDto(), TestProductDto(), TestProductDto()};
//             var filter = new QueryParametersModel();
//             
//             _productService.Setup(serv => serv.GetProductsAsync(It.IsAny<QueryParametersModel>()))
//                 .ReturnsAsync(expectedProducts);
//             
//             _productService.Setup(serv => serv.Count())
//                 .ReturnsAsync(expectedProducts.Count);
//             
//             var controller = new ProductController(_productService.Object, _loggerStub.Object, _mappingStub.Object);
//             var resultResponse = new PagedResponse<ProductRest.Entities.Product>(expectedProducts, filter, expectedProducts.Count);
//
//             // Act
//             var actualProducts = await controller.GetProducts(filter);
//
//             // Assert
//             actualProducts.Result.Should().BeOfType<OkObjectResult>();
//             var result = actualProducts.Result as OkObjectResult;
//
//             result.Value.Should().BeEquivalentTo(
//                 resultResponse,
//                 options => options.ComparingByMembers<PagedResponse<ProductRest.Entities.Product>>()
//             );
//         }
//         
//         private ProductRest.Entities.Product TestProductDto()
//         {
//             return new()
//             {
//                 Id = Guid.NewGuid(),
//                 AddressLine = "new address",
//                 PostalCode = "12345",
//                 Country = "Country",
//                 City = "City",
//                 FaxNumber = "+380994081678",
//                 PhoneNumber = "+380999088267"
//             };
//         }
//         
//         private CreateProductDto TestCreateProductDto() 
//         {
//             return new()
//             {
//                 AddressLine = "new address",
//                 PostalCode = "12345",
//                 Country = "Country",
//                 City = "City",
//                 FaxNumber = "+380994081678",
//                 PhoneNumber = "+380999088267"
//             };
//         }
//     }
// }