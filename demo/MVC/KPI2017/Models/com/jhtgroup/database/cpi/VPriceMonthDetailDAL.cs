using com.jhtgroup.database.common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace com.jhtgroup.database.cpi
{
	/// <summary>
	/// V_PriceMonthDetail View operator
	/// 大宗物料行情 by month
	/// </summary>
	public class VPriceMonthDetailDAL : BaseDAL<VPriceMonthDetail>
	{
		public VPriceMonthDetailDAL()
		{
			setConnectionstring(ConnectionString.CPI);
		}

		/// <summary>
		/// 查詢
		/// </summary>
		/// <param name="category">Category Code</param>
		/// <param name="startDate">Start DateTime</param>
		/// <param name="endDate">End DateTime</param>
		/// <returns></returns>
		public List<VPriceMonthDetail> Query(int category, DateTime startDate, DateTime endDate)
		{
			if (category == null || category <= 0)
				throw new ArgumentNullException("Argument[category] can't be empty!");

			string sql = @"select 
								ID
								,CommodityB
								,Commodity
								,Price
								,FactorB
								,Factor
								,Baseline
								,Bias
								,Year
								,Month
								,PID
								,Category
								,CategoryB
								,Locale
								,Source
								,Currency
								,Unit
								,IsBaseline
								,IsFactor
								,IsComposite
							from V_PriceMonthDetail
							where Category=@Category
							and CAST(CAST([Year]AS varchar) + '-' + CAST([Month] AS varchar) + '-' + CAST(1 AS varchar) AS DATETIME) between @startDate and @endDate
							order by year, month, commodity;";
			try
			{
				resetParameter();
				addParameter("@Category", category);
				addParameter("@startDate", startDate);
				addParameter("@endDate", endDate);
				List<VPriceMonthDetail> list = queryWithParameters(sql);
				return list;
			}
			catch (SqlException ex)
			{
			}
			return null;
		}
	}
}