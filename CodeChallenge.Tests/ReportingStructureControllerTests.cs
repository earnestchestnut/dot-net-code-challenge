using System.Net;
using System.Net.Http;
using CodeChallenge.Models;
using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeChallenge.Tests.Integration
{
    [TestClass]
    public class ReportingStructureControllerTests
	{

        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        // Attribute ClassInitialize requires this signature
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer();
            _httpClient = _testServer.NewClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void GetReportingStructure_Returns_Ok()
        {
            // Arrange
            string employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reportingstructure/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var reportingstructure = response.DeserializeContent<ReportingStructure>();
            var johnLennon = reportingstructure.Employee;
            var paulMcCartny = reportingstructure.Employee.DirectReports[0];
            var ringoStarr = reportingstructure.Employee.DirectReports[1];

            Assert.AreEqual(reportingstructure.NumberOfReports, 4);
            Assert.AreEqual(johnLennon.DirectReports.Count, 2);
            Assert.AreEqual(paulMcCartny.DirectReports.Count, 0);
            Assert.AreEqual(ringoStarr.DirectReports.Count, 2);

        }

        [TestMethod]
        public void GetReportingStructure_Returns_NotFound()
        {
            // Arrange
            string employeeId = "54f29133-c0f6-4416-a01b-71790315c800";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reportingstructure/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        }
    }
}

