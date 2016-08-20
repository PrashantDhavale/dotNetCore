namespace OBDSim.Tests.UnitTests
{
    using OBDSim.Controllers;
    using OBDSim.Models;
    using OBDSim.Providers;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;
    using Microsoft.Extensions.Logging;


    public class OBDSimUnitTests
    {
        [Fact]
        public void GetOBDSimDetails_ReturnsOk()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<OBDSimController>>();
            var mockProvider = new Mock<IOBDSimProvider>();
            mockProvider.Setup(pro => pro.GetOBDSims(1)).Returns(GetTestOBDSims(1));

            var controller = new OBDSimController(mockProvider.Object, mockLogger.Object);

            // Act
            var result = controller.GetOBDSims(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnModel = Assert.IsType<List<OBDSimModel>>(okResult.Value);
        }

        private List<OBDSimModel> GetTestOBDSims(int customerId)
        {
            List<OBDSimModel> l = new List<OBDSimModel>();
            for (int i = 0; i < 10; i++)
            {
                l.Add(new OBDSimModel
                {
                    FromDate = DateTime.Today.AddDays(-i),
                    ToDate = DateTime.Today.AddDays(-i + 2),
                    Reason = "Test OBDSim"
                });
            }
            return l;
        }
    }
}
