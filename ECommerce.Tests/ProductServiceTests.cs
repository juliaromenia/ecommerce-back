//using ECommerce.Application.DTOs.Products;
//using ECommerce.Application.Interfaces;
//using ECommerce.Application.Services;
//using ECommerce.Domain.Entities;
//using Moq;

//namespace ECommerce.Tests
//{
//    public class ProductServiceTests
//    {
//        [Fact]
//        public async Task CreatAsync_ShouldCreateProduct_WhenValidRequest()
//        {
//            // Arrange
//            //criamos um objeto false que representa a interface
//            var repositoryMock = new Mock<IProductRepository>();

//            //criamos uma simulação do banco de dados, onde quando o método AddAsync for chamado, ele irá atribuir um Id ao produto criado
//            repositoryMock
//                .Setup(r => r.AddAsync(It.IsAny<Product>()))
//                .Callback<Product>(product => product.Id = 1);

//            var service = new ProductService(repositoryMock.Object);

//            var request = new CreateProductRequestDto
//            {
//                Name = "Test Product",
//                Description = "Test Description",
//                Price = 10.0m,
//                Stock = 10,
//                CategoryId = 1
//            };

//            // Act
//            var result = await service.CreateAsync(request);

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal(1, result.Id);
//            Assert.Equal("Test Product", result.Name);
//            Assert.Equal(10.0m, result.Price);
//            Assert.Equal(10, result.Stock);

//            //verifica se o método AddAsync(no repository) foi chamado apenas uma vez
//            repositoryMock.Verify(
//                r => r.AddAsync(It.IsAny<Product>()),
//                Times.Once);
//        }

//        [Fact]
//        public async Task CreateAsync_ShouldThrowException_WhenPriceIsZero()
//        {
//            // Arrange
//            var repositoryMock = new Mock<IProductRepository>();

//            var service = new ProductService(repositoryMock.Object);

//            var request = new CreateProductRequestDto
//            {
//                Name = "Keyboard",
//                Description = "Mechanical keyboard",
//                Price = 0,
//                Stock = 10,
//                CategoryId = 1
//            };

//            // Act
//            var action = () => service.CreateAsync(request);

//            // Assert
//            await Assert.ThrowsAsync<ArgumentException>(action);

//            repositoryMock.Verify(
//                r => r.AddAsync(It.IsAny<Product>()),
//                Times.Never);
//        }

//        [Fact]
//        public async Task CreateAsync_ShouldThrowException_WhenStockIsNegative()
//        {
//            // Arrange
//            var repositoryMock = new Mock<IProductRepository>();

//            var service = new ProductService(repositoryMock.Object);

//            var request = new CreateProductRequestDto
//            {
//                Name = "Keyboard",
//                Description = "Mechanical keyboard",
//                Price = 299.90m,
//                Stock = -1,
//                CategoryId = 1
//            };

//            // Act
//            var action = () => service.CreateAsync(request);

//            // Assert
//            await Assert.ThrowsAsync<ArgumentException>(action);

//            repositoryMock.Verify(
//                r => r.AddAsync(It.IsAny<Product>()),
//                Times.Never);
//        }

//        [Fact]
//        public async Task CreateAsync_ShouldThrowException_WhenNameIsEmpty()
//        {
//            // Arrange
//            var repositoryMock = new Mock<IProductRepository>();

//            var service = new ProductService(repositoryMock.Object);

//            var request = new CreateProductRequestDto
//            {
//                Name = "",
//                Description = "Mechanical keyboard",
//                Price = 299.90m,
//                Stock = 10,
//                CategoryId = 1
//            };

//            // Act
//            var action = () => service.CreateAsync(request);

//            // Assert
//            await Assert.ThrowsAsync<ArgumentException>(action);

//            repositoryMock.Verify(
//                r => r.AddAsync(It.IsAny<Product>()),
//                Times.Never);
//        }
//    }
//}
