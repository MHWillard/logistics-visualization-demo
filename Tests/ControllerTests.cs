using Xunit;
using logistics_visualization_demo.Controllers;

namespace logistics_visualization_demo.Tests
{
    public class ControllerTests
    {
        [Fact]
        public void PollingTable_ReturnsData() 
        {
            //behavior: when a route on the Datacontroller polls a table for data, it returns that data, in the format of company name/total income/date (which has month and year)
            //arrange
            //add any mocks here or stubs for the actual database and the controller
            DataController DataController = new DataController();

            //act
            string result = DataController.GetRecords();

            //assert
            Assert.Equal("Shiphole, $5000, March 2020", result);
        }
    }
}
