using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using ShopSphere.API.Exceptions;
using ShopSphere.API.Interfaces;
using ShopSphere.API.Mappings;
using ShopSphere.API.Models;
using ShopSphere.API.Repositories.Interfaces;
using ShopSphere.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly ProductService _productService;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<IFileService> _fileServiceMock;
        private readonly Mock<ILogger<ProductService>> _loggerMock;

        public ProductServiceTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();

            _categoryRepositoryMock = new Mock<ICategoryRepository>();

            _fileServiceMock = new Mock<IFileService>();

            _loggerMock = new Mock<ILogger<ProductService>>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            IMapper mapper = config.CreateMapper();

            _productService = new ProductService(
                _productRepositoryMock.Object,
                mapper,
                _loggerMock.Object,
                _fileServiceMock.Object,
                _categoryRepositoryMock.Object);
        }

        //[Fact]
        //public async Task GetProductByIdAsync_ShouldReturnProduct_WhenProductExists()
        //{
        //    // Arrange
        //    var product = new Product
        //    {
        //        Id = 1,
        //        Name = "Laptop",
        //        Price = 50000,
        //        Stock = 10,
        //        CategoryId = 1
        //    };

        //    _productRepositoryMock
        //        .Setup(x => x.GetByIdAsync(1))
        //        .ReturnsAsync(product);

        //    // Act
        //    var result = await _productService.GetProductByIdAsync(1);

        //    // Assert
        //    result.Should().NotBeNull();
        //    result.Id.Should().Be(1);
        //    result.Name.Should().Be("Laptop");
        //    result.Price.Should().Be(50000);
        //}

        [Fact]
        public async Task GetProductByIdAsync_ShouldThrowNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Laptop",
                Price = 50000,
                Stock = 10,
                CategoryId = 1
            };

            _productRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((Product?)null);

            Func<Task> act = async () =>
            {
                await _productService.GetProductByIdAsync(1);
            };

            await act.Should()
                     .ThrowAsync<NotFoundException>();
        }


    }
}
