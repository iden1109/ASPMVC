using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KPI2017.Models.MaterialsMarket
{
    public class Material
    {
        public int ID { get; set; }
        public string CommodityB { get; set; }
        public int Commodity { get; set; }
        public double Price { get; set; }
        public string FactorB { get; set; }
        public double Factor { get; set; }
        public double Baseline { get; set; }
        public double Bias { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int PID { get; set; }
        public int Category { get; set; }
        public string CategoryB { get; set; }
        public string Locale { get; set; }
        public string Source { get; set; }
        public string Currency { get; set; }
        public string Unit { get; set; }
        public bool IsBaseline { get; set; }
        public bool IsFactor { get; set; }
        public bool IsComposite { get; set; }

        public string Name
        {
            get
            {
                if (Locale != null && !Locale.Equals(""))
                    return CommodityB + " - " + Locale;

                return CommodityB;
            }
        }

        public double FactorPrice
        {
            get
            {
                if (IsFactor)
                    return Price * Factor;

                return Price;
            }
        }

        public DateTime Date
        {
            get
            {
                return new DateTime(Year, Month, 1);
            }
        }
    }
}