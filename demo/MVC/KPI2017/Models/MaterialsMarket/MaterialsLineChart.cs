using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KPI2017.Models.MaterialsMarket
{
    public class MaterialsLineChart
    {
        private DateTime _date;
        public DateTime Date
        {
            get
            {
                return _date;
            }
        }
        private Dictionary<string, Material> _materialDict; // {Commodity, Material}
        public Dictionary<string, Material> Materials
        {
            get
            {
                return _materialDict;
            }
        }


        public MaterialsLineChart(DateTime dt)
        {
            this._date = dt;
            this._materialDict = new Dictionary<string, Material>();
        }


        public void AddMaterial(Material material)
        {
            if (!HasMaterial(material))
            {
                _materialDict.Add(material.Name, material);
            }
        }

        public bool HasMaterial(Material material)
        {
            if (_materialDict.ContainsKey(material.Name))
                return true;
            return false;
        }
    }
}