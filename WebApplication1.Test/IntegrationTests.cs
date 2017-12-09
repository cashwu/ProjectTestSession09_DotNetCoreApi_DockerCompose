using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1.Tests.Setup;

namespace WebApplication1.Tests
{
    [TestClass]
    public class IntegrationTests
    {
        private TestServer Server { get; set; }

        private HttpClient Client { get; set; }

        private const string Environment = "Test";

        [TestInitialize]
        public void TestInitialize()
        {
            // arrange
            var contentRootPath = this.GetContentRootPath();
            var builder = new WebHostBuilder()
                    .UseContentRoot(contentRootPath)
                    .ConfigureAppConfiguration
                    (
                        (builderContext, config) =>
                        {
                            config.AddJsonFile($"appsettings.{Environment}.json", optional: false, reloadOnChange: true);
                            config.AddEnvironmentVariables();
                        }
                    )
                    .UseEnvironment("Test")
                    .UseStartup<WebApplication1.Startup>();

            this.Server = new TestServer(builder);
            this.Client = this.Server.CreateClient();
        }

        private string GetContentRootPath()
        {
            var applicationBasePath = PlatformServices.Default.Application.ApplicationBasePath;
            var result = Path.Combine(applicationBasePath, @"..\..\..\..\WebApplication1");
            return result;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.Server?.Dispose();
            this.Client?.Dispose();
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            TestLocalDbProcess.CreateTable();
            TestLocalDbProcess.PrepareData();
        }

        [TestMethod]
        public async Task Test_取得全部資料()
        {
            // act
            var response = await this.Client.GetAsync("/api/WifiSpot");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            // assert
            responseString.Should().NotBeNullOrEmpty();
        }

        [TestMethod]
        public async Task Test_輸入Id_取得指定資料()
        {
            // arrange
            var targetId = "ZZZITWF001260";
            var expectedName = "新北市立圖書館蘆洲集賢分館-1_C";

            string requestUri = $"/api/WifiSpot/{targetId}";

            // act
            var response = await this.Client.GetAsync(requestUri);
            var responseString = await response.Content.ReadAsStringAsync();

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseString.Should().NotBeEmpty();
            responseString.Should().Contain(expectedName);
        }

        [TestMethod]
        public async Task Test_輸入Id_找不到對映資料_應回傳訊息()
        {
            // arrange
            var targetId = "0D94ABF4-E730-49FD-A3AB-54FE69612BDD";
            var expectedMessage = "not correct id, data not found";

            string requestUri = $"/api/WifiSpot/{targetId}";

            // act
            var response = await this.Client.GetAsync(requestUri);
            var responseString = await response.Content.ReadAsStringAsync();

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            responseString.Should().NotBeEmpty();
            responseString.Should().Contain(expectedMessage);
        }
    }
}