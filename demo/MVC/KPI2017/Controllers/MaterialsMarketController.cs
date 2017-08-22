using AutoMapper;
using com.jhtgroup.database.cpi;
using com.jhtgroup.web;
using KPI2017.Controllers.common;
using KPI2017.Models.MaterialsMarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KPI2017.Controllers
{
    /// <summary>
    /// 大宗物件行情
    /// </summary>
    public class MaterialsMarketController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 取保麗龍資料
        /// </summary>
        /// <param name="argu"></param>
        /// <returns></returns>
        public string GetPolystyrene(QueryArgus argu)
        {
            return GetLineChartData(argu, 104);
        }

        
        /// <summary>
        /// 取大宗物料線圖所需資料包裝成Message形式
        /// </summary>
        /// <param name="argu">QueryArgus</param>
        /// <param name="category">Category code</param>
        /// <returns>Message</returns>
        private string GetLineChartData(QueryArgus argu, int category)
        {
            if (argu == null || argu.startDate == null || argu.startDate == DateTime.MinValue || argu.startDate == DateTime.MaxValue)
                return JSonTool.SerializeJson(new Message("Start Date is required"));
            if (argu == null || argu.endDate == null || argu.endDate == DateTime.MinValue || argu.endDate == DateTime.MaxValue)
                return JSonTool.SerializeJson(new Message("End Date is required"));

            Message msg = new Message();
            try
            {
                List<MaterialsLineChart> chartList = GetLineChartData(category, argu.startDate, argu.endDate);
                msg.result = chartList;
                msg.count = chartList.Count;
            }
            catch (Exception ex)
            {
                msg.count = 0;
                msg.message = ex.Message;
            }
            return JSonTool.SerializeJson(msg);
        }

        /// <summary>
        /// 取大宗物料線圖所需資料
        /// </summary>
        /// <param name="category">Category code</param>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns></returns>
        private List<MaterialsLineChart> GetLineChartData(int category, DateTime startDate, DateTime endDate)
        {
            VPriceMonthDetailDAL dal = new VPriceMonthDetailDAL();
            List<VPriceMonthDetail> dalList = dal.Query(category, startDate, endDate);
            Mapper.Initialize(m => m.CreateMap<VPriceMonthDetail, Material>());
            List<Material> materialList = Mapper.Map<List<VPriceMonthDetail>, List<Material>>(dalList);

            MaterialsLineChartList mlcl = new MaterialsLineChartList();
            mlcl.AddMaterialChart(materialList);
            List<MaterialsLineChart> chartList = mlcl.GetMaterialChartList();
            return chartList;
        }
        


    }
}