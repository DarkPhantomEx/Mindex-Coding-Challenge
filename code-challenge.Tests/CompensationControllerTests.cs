using challenge.Controllers;
using challenge.Data;
using challenge.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using code_challenge.Tests.Integration.Extensions;

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using code_challenge.Tests.Integration.Helpers;
using System.Text;

namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<TestServerStartup>()
                .UseEnvironment("Development"));

            _httpClient = _testServer.CreateClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }


        //Tests if a Compensation was Created succesfully
        [TestMethod]
        public void CreateCompensation_Returns_Created()
        {
            // Arrange                        
            var emp = new Employee()
            {                
                Department = "Complaints",
                FirstName = "Debbie",
                LastName = "Downer",
                Position = "Receiver",
            };

            var comp = new Compensation()
            {
                employee = emp,
                salary = 90000,
                effectiveDate = new int[] { 11, 01, 2019 },
            };

            var requestContent = new JsonSerialization().ToJson(comp);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation/",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var newComp = response.DeserializeContent<Compensation>();
            Assert.IsNotNull(comp.CompensationId);
            Assert.AreEqual(comp.employee, newComp.employee);
            Assert.AreEqual(comp.salary, newComp.salary);
            Assert.AreEqual(comp.effectiveDate, newComp.effectiveDate);
        }

        //Tests if a Get Request for Compensation tied to an Employee ID is succesful or not
        [TestMethod]
        public void GetCompensationById_Returns_Ok()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var expectedFirstName = "John";
            var expectedLastName = "Lennon";
            var expectedSalary = 100100;
            var expectedEffectiveDate = new int[3] {10,01,1970};

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var comp = response.DeserializeContent<Compensation>();
            Assert.AreEqual(expectedSalary, comp.salary);
            Assert.AreEqual(expectedFirstName, comp.employee.FirstName);
            Assert.AreEqual(expectedLastName, comp.employee.LastName);
            Assert.AreEqual(expectedEffectiveDate, comp.effectiveDate);
        }

    }
}
