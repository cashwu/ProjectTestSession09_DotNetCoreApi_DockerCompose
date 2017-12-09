using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Repository;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class WifiSpotController : Controller
    {
        private IWifiSpotRepository WifiSpotRepository { get; set; }

        public WifiSpotController(IWifiSpotRepository repository)
        {
            this.WifiSpotRepository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var source = await this.WifiSpotRepository.GetAll();
            return this.Ok(source);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return this.BadRequest
                (
                    new
                    {
                        message = "must input id"
                    }
                );
            }

            var source = await this.WifiSpotRepository.GetById(id);
            if (source == null)
            {
                return this.NotFound
                (
                    new
                    {
                        message = "not correct id, data not found"
                    }
                );
            }

            return this.Ok(source);
        }
    }
}