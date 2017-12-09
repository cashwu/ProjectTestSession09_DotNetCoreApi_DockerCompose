using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1.Model;
using WebApplication1.Repository.Tests.Setup;

namespace WebApplication1.Repository.Tests
{
    [TestClass]
    public class WifiSpotRepositoryTests
    {
        private DbContextOptions<SampledbContext> Options
        {
            get
            {
                var options = new DbContextOptionsBuilder<SampledbContext>()
                    .UseSqlServer(TestLocalDB.LocalDbSampleDbConnectionString)
                    .Options;

                return options;
            }
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            TestLocalDbProcess.CreateTable();
            TestLocalDbProcess.PrepareData();
        }

        [TestMethod]
        public async Task Get_取得全部資料()
        {
            using (var dbContext = new SampledbContext(this.Options))
            {
                var sut = new WifiSpotRepository(dbContext);

                // act
                var actual = await sut.GetAll();

                // assert
                actual.Any().Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task GetById_輸入資料不存在的id_應回傳null()
        {
            var id = "9CC727EC-8EBF-46EA-BEB5-48EB603F4523";

            using (var dbContext = new SampledbContext(this.Options))
            {
                var sut = new WifiSpotRepository(dbContext);

                // act
                var actual = await sut.GetById(id);

                // assert
                actual.Should().BeNull();
            }
        }

        [TestMethod]
        public async Task GetById_輸入資料存在的id_應回傳對映id的資料()
        {
            var id = "ZZZITWF000159";
            var expectedSpotName = "新北市淡水區公所";

            using (var dbContext = new SampledbContext(this.Options))
            {
                var sut = new WifiSpotRepository(dbContext);

                // act
                var actual = await sut.GetById(id);

                // assert
                actual.Should().NotBeNull();
                actual.Id.Should().Be(id);
                actual.SpotName.Should().Be(expectedSpotName);
            }
        }
    }
}