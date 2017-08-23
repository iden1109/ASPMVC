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
    public class SelectizeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public string GetMaterials()
        {
            VPriceMonthDetailDAL dal = new VPriceMonthDetailDAL();
            List<VPriceMonthDetail> dalList = dal.Query(104, DateTime.Now.AddYears(-2), DateTime.Now);
            Mapper.Initialize(m => m.CreateMap<VPriceMonthDetail, Material>());
            List<Material> materialList = Mapper.Map<List<VPriceMonthDetail>, List<Material>>(dalList);

            return JSonTool.SerializeJson(materialList);
        }
    }
}