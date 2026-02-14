using Xunit;
using logistics_visualization_demo.Models;
using logistics_visualization_demo.Controllers;
using System.Text.Json;

namespace logistics_visualization_demo.Tests
{
    public class ControllerTests
    {
        [Fact]
        public void GetDataFromOrderDetailsTable() 
        {
            /*
             when get route is called, it should return the data from the order details table in the database.
             */
            OrderDetail orderDetail = new OrderDetail
            {
                OrderId = 1,
                CompanyId = 1,
                ProductId = 1,
                Quantity = 5
            };
            string testOrderDetailsData = JsonSerializer.Serialize(orderDetail);

            //arrange
            //add controller and mock database context here
            //add data to check against here


            //act
            //call the get route here through the controller using the mock

            //assert
            //assert that the data returned from the get route is the same as the data set up in the arrange step
        }
    }
}
