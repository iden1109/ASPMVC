using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KPI2017.Models.MaterialsMarket
{
    public class MaterialsLineChartList
    {
        private List<MaterialsLineChart> _materialChartList;


        public MaterialsLineChartList()
        {
            this._materialChartList = new List<MaterialsLineChart>();
        }


        public void AddMaterialChart(List<Material> materialList)
        {
            bool isFirst = true;
            if (materialList != null && materialList.Count > 0)
            {
                MaterialsLineChart materialChart = null;
                DateTime preDate = new DateTime();
                int count = 0;
                foreach (Material m in materialList)
                {
                    if (materialChart == null)
                        materialChart = new MaterialsLineChart(m.Date);

                    if (DateTime.Compare(m.Date, preDate) == 0)
                    {//同年同月放一起
                        materialChart.AddMaterial(m);
                    }
                    else
                    {//不同日期歸另一類
                        if (!isFirst)
                            _materialChartList.Add(materialChart);

                        materialChart = new MaterialsLineChart(m.Date);
                        materialChart.AddMaterial(m);

                        isFirst = false;
                    }


                    preDate = m.Date;
                    count++;

                    if(count == materialList.Count)
                        _materialChartList.Add(materialChart);
                }
            }
        }

        public List<MaterialsLineChart> GetMaterialChartList()
        {
            return this._materialChartList;
        }
    }
}