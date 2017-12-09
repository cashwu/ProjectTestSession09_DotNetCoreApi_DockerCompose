using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Model;

namespace WebApplication1.Repository
{
    public class WifiSpotRepository : IWifiSpotRepository
    {
        private SampledbContext DbContext { get; set; }

        public WifiSpotRepository(SampledbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        /// <summary>
        /// 取得所有熱點資料.
        /// </summary>
        /// <returns>List&lt;NewTaipeiWifiSpot&gt;.</returns>
        public Task<List<NewTaipeiWifiSpot>> GetAll()
        {
            var models = this.DbContext.NewTaipeiWifiSpot.ToListAsync();
            return models;
        }

        /// <summary>
        /// 以 Id 取得指定的熱點資料.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;NewTaipeiWifiSpot&gt;.</returns>
        public Task<NewTaipeiWifiSpot> GetById(string id)
        {
            var model = this.DbContext.NewTaipeiWifiSpot.FirstOrDefaultAsync(x => x.Id == id);
            return model;
        }
    }
}