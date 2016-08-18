namespace OBDSim.Tests.IntegrationTests
{
    using OBDSim.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.TestHost;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Xunit;

    public class OBDSimIntegrationTests : IClassFixture<TestFixture<OBDSim.Startup>>
    {
        private readonly HttpClient _client;

        public OBDSimIntegrationTests(TestFixture<OBDSim.Startup> fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GetOBDSims_ReturnsOkForCorrectEmployeeId()
        {
            // Arrange & Act
            var response = await _client.GetAsync("/api/OBDSim/1");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetOBDSims_ReturnsBadRequestForIncorrectEmployeeId()
        {
            // Arrange & Act
            var response = await _client.GetAsync("/api/OBDSim/a123");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}