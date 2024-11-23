
//using NUnit.Framework;

//namespace ECommerce.Tests
//{
//    [Test]
//    public class ProductTests
//    {
//        private IOrderProcessorRepository _repository;

//        [SetUp]
//        public void Setup()
//        {
//            _repository = new OrderProcessorRepositoryImpl();
//        }

//        [Test]
//        public void CreateProduct_ShouldReturnTrue_WhenProductIsValid()
//        {
//            // Arrange
//            var product = new Product
//            {
//                ProductId = 1,
//                Name = "Laptop",
//                Price = 1200.50m,
//                Description = "High-performance laptop",
//                StockQuantity = 10
//            };

//            // Act
//            var result = _repository.CreateProduct(product);

//            // Assert
//            Assert.IsTrue(result, "Product was not created successfully.");
//        }
//    }
//}
