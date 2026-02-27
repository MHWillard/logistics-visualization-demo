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
        [Fact]
        public void GetDataFromOrderDetailsTable()
        {
            // arrange
            var options = new DbContextOptionsBuilder<RecordContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var testOrderDetail = new OrderDetail
            {
                OrderDetailId = 1,
                OrderId = 1,
                CompanyId = 1,
                ProductId = 1,
                Quantity = 5
            };

            using (var context = new RecordContext(options))
            {
                context.OrderDetails.Add(testOrderDetail);
                context.SaveChanges();
            }

            using (var context = new RecordContext(options))
            {
                var dataController = new DataController(context);

                // act
                string returnData = dataController.GetRecords(orderId: 1);

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
}