using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using ExpectedObjects;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WebApplication1.Controllers;
using WebApplication1.Model;
using WebApplication1.Repository;

namespace WebApplication1.Tests.Controllers
{
    [TestClass]
    public class WifiSpotControllerTests
    {
        private IWifiSpotRepository WifiSpotRepository { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            this.WifiSpotRepository = Substitute.For<IWifiSpotRepository>();
        }

        [TestMethod]
        public async Task Get_取得全部資料()
        {
            // arrange
            Fixture fixture = new Fixture();
            var models = fixture.Build<NewTaipeiWifiSpot>().CreateMany(100).ToList();
            this.WifiSpotRepository.GetAll().Returns(Task.FromResult(models));

            var sut = new WifiSpotController(this.WifiSpotRepository);

            // act
            var actual = await sut.Get();

            // assert            
            actual.Should().NotBeNull();

            var actualResult = (OkObjectResult) actual;
            actualResult.StatusCode.Should().Be(200);
            actualResult.Value.Should().NotBeNull();

            var actualContent = (List<NewTaipeiWifiSpot>) actualResult.Value;
            actualContent.Any().Should().BeTrue();
            actualContent.Should().HaveCount(100);
        }

        [TestMethod]
        public async Task Get_取得單一資料_Id輸入空白_應回傳錯誤訊息()
        {
            // arrange
            var id = "";

            var sut = new WifiSpotController(this.WifiSpotRepository);

            // act
            var actual = await sut.Get(id);

            // assert
            actual.Should().NotBeNull();

            var actualResult = (ObjectResult) actual;
            actualResult.StatusCode.Should().Be(400);

            var actualContent = actualResult.Value.ToString();
            actualContent.Should().Contain("must input id");
        }

        [TestMethod]
        public async Task Get_取得單一資料_輸入的Id無法對映到資料_應回傳錯誤訊息()
        {
            // arrange
            var id = "CB9C5D30-2676-4C17-9804-E21B94388F85";

            this.WifiSpotRepository.GetById(id)
                .ReturnsForAnyArgs(Task.FromResult((NewTaipeiWifiSpot) null));

            var sut = new WifiSpotController(this.WifiSpotRepository);

            // act
            var actual = await sut.Get(id);

            // assert
            actual.Should().NotBeNull();

            var actualResult = (ObjectResult) actual;
            actualResult.StatusCode.Should().Be(404);

            var actualContent = actualResult.Value.ToString();
            actualContent.Should().Contain("not correct id, data not found");
        }

        [TestMethod]
        public async Task Get_取得單一資料_輸入Id可以對映到資料_應回傳指定資料()
        {
            // arrange
            var id = "ZZZITWF001126";
            var expected = new NewTaipeiWifiSpot
            {
                Id = "ZZZITWF001126",
                SpotName = "新北市政府體育處1F-2",
                Type = "NewTaipei",
                Company = "中華電信",
                District = "新莊區",
                Address = "242新北市新莊區和興街66號1樓",
                ApparatusName = "新北市政府教育局",
                Latitude = "25.041388",
                Longitude = "121.446441",
                Twd97X = "295146.60",
                Twd97Y = "2770311.80",
                Wgs84aX = "121.447500",
                Wgs84aY = "25.040278"
            };

            this.WifiSpotRepository.GetById(id)
                .ReturnsForAnyArgs(Task.FromResult(expected));

            var sut = new WifiSpotController(this.WifiSpotRepository);

            // act
            var actual = await sut.Get(id);

            // assert
            actual.Should().NotBeNull();

            var actualResult = (ObjectResult) actual;
            actualResult.StatusCode.Should().Be(200);

            var actualContent = (NewTaipeiWifiSpot) actualResult.Value;
            expected.ToExpectedObject().ShouldEqual(actualContent);

            actualContent.ShouldBeEquivalentTo(expected);
        }
    }
}