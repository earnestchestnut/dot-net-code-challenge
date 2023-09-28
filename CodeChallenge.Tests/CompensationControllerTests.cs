using System;
using System.Net;
using System.Net.Http;
using System.Text;
using CodeChallenge.Models;
using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeChallenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTests
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
        public void CreateCompensation_Returns_Created()
        {
            // Arrange
            string employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            CompensationCreateRequest compensationCreateRequest = new()
            {
                EmployeeId = employeeId,
                Salary = 400000000
            };

            var requestContent = new JsonSerialization().ToJson(compensationCreateRequest);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation", new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;
            long fourMillionDollars = 4000000;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            Compensation compensation = response.DeserializeContent<Compensation>();
            Assert.IsInstanceOfType(compensation.Employee, typeof(Employee));
            Assert.AreEqual(compensation.Salary / 100 == fourMillionDollars, true);
            Assert.IsInstanceOfType(compensation.Effectivedate, typeof(DateTime));

        }

        [TestMethod]
        public void CreateCompensation_Duplicate_Returns_Conflict()
        {
            // Arrange
            string employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            CompensationCreateRequest compensationCreateRequest = new()
            {
                EmployeeId = employeeId,
                Salary = 400000000
            };

            var requestContent = new JsonSerialization().ToJson(compensationCreateRequest);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation", new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assertion
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            // Wait a few seconds and try creating the same Compensation record again
            System.Threading.Thread.Sleep(2000);
            postRequestTask = _httpClient.PostAsync("api/compensation", new StringContent(requestContent, Encoding.UTF8, "application/json"));
            response = postRequestTask.Result;

            // Assertion
            Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);

        }

        [TestMethod]
        public void CreateCompensation_Null_EmployeeId_Returns_BadRequest()
        {
            // Arrange
            string employeeId = null;
            CompensationCreateRequest compensationCreateRequest = new()
            {
                EmployeeId = employeeId,
                Salary = 400000000
            };

            var requestContent = new JsonSerialization().ToJson(compensationCreateRequest);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation", new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        }

        [TestMethod]
        public void CreateCompensation_Employee_NotFound_Returns_BadRequest()
        {
            // Arrange
            string employeeId = "54f29133-c0f6-4416-a01b-71790315c800";
            CompensationCreateRequest compensationCreateRequest = new()
            {
                EmployeeId = employeeId,
                Salary = 400000000
            };

            var requestContent = new JsonSerialization().ToJson(compensationCreateRequest);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation", new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        }


        [TestMethod]
        public void GetCompensation_Returns_NotFound()
        {
            // Arrange
            string employeeId = "54f29133-c0f6-4416-a01b-71790315c800";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}

