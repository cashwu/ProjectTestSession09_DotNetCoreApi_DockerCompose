using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Model;

namespace WebApplication1.Repository
{
    public interface IWifiSpotRepository
    {
        /// <summary>
        /// 取得所有熱點資料.
        /// </summary>
        /// <returns>List&lt;NewTaipeiWifiSpot&gt;.</returns>
        Task<List<NewTaipeiWifiSpot>> GetAll();

        /// <summary>
        /// 以 Id 取得指定的熱點資料.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;NewTaipeiWifiSpot&gt;.</returns>
        Task<NewTaipeiWifiSpot> GetById(string id);
    }
}