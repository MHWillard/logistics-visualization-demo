using logistics_visualization_demo.Controllers;
using logistics_visualization_demo.Data;
using logistics_visualization_demo.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Xunit;

namespace logistics_visualization_demo.Tests
{
    public class ControllerTests
    {
        private readonly DbContextOptions<RecordContext> _options;

        public ControllerTests()
        {
            _options = new DbContextOptionsBuilder<RecordContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public void GetOrderDetailsFromOrderDetailsTable()
        {
            // arrange
            var testOrderDetail = new OrderDetail
            {
                OrderDetailId = 1,
                OrderId = 1,
                CompanyId = 1,
                ProductId = 1,
                Quantity = 5
            };

            using (var context = new RecordContext(_options))
            {
                context.OrderDetails.Add(testOrderDetail);
                context.SaveChanges();
            }

            using (var context = new RecordContext(_options))
            {
                var dataController = new DataController(context);

                // act
                string returnData = dataController.GetOrderDetails(orderId: 1);

                // assert
                var orderDetails = JsonSerializer.Deserialize<List<OrderDetail>>(returnData);
                Assert.NotNull(orderDetails);
                Assert.Single(orderDetails);
                Assert.Equal(testOrderDetail.OrderId, orderDetails![0].OrderId);
                Assert.Equal(testOrderDetail.CompanyId, orderDetails[0].CompanyId);
                Assert.Equal(testOrderDetail.ProductId, orderDetails[0].ProductId);
                Assert.Equal(testOrderDetail.Quantity, orderDetails[0].Quantity);
            }
        }

         [Fact]
        public void GetCompanyByCompanyId()
        {
            // arrange
            var testCompany = new Company
            {
                CompanyId = 1,
                Name = "Test Company"
            };

            using (var context = new RecordContext(_options))
            {
                context.Companies.Add(testCompany);
                context.SaveChanges();
            }

            using (var context = new RecordContext(_options))
            {
                var dataController = new DataController(context);

                // act
                string returnData = dataController.GetCompany(companyId: 1);

                // assert
                var company = JsonSerializer.Deserialize<Company>(returnData);
                Assert.NotNull(company);
                Assert.Equal(testCompany.CompanyId, company!.CompanyId);
                Assert.Equal(testCompany.Name, company.Name);
            }
        }

        [Fact]
        public void GetOrderByOrderId()
        {
            // arrange
            var testOrder = new Order
            {
                OrderId = 1,
                CompanyId = 1
            };

            using (var context = new RecordContext(_options))
            {
                context.Orders.Add(testOrder);
                context.SaveChanges();
            }

            using (var context = new RecordContext(_options))
            {
                var dataController = new DataController(context);

                // act
                string returnData = dataController.GetOrder(orderId: 1);

                // assert
                var order = JsonSerializer.Deserialize<Order>(returnData);
                Assert.NotNull(order);
                Assert.Equal(testOrder.OrderId, order!.OrderId);
                Assert.Equal(testOrder.CompanyId, order.CompanyId);
            }
        }

        [Fact]
        public void GetProductByProductId()
        {
            // arrange
            var testProduct = new Product
            {
                ProductId = 1,
                Name = "Test Product"
            };

            using (var context = new RecordContext(_options))
            {
                context.Products.Add(testProduct);
                context.SaveChanges();
            }

            using (var context = new RecordContext(_options))
            {
                var dataController = new DataController(context);

                // act
                string returnData = dataController.GetProduct(productId: 1);

                // assert
                var product = JsonSerializer.Deserialize<Product>(returnData);
                Assert.NotNull(product);
                Assert.Equal(testProduct.ProductId, product!.ProductId);
                Assert.Equal(testProduct.Name, product.Name);
            }
        }
    }
}