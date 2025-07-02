using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.WinForms;
using MySql.Data.MySqlClient;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
namespace _ARMEX
{
    public static class ChartHelper
    {

        // 각 제품별(코드) 연도별 누적 처리량(막대그래프용) 데이터 조회 및 반환
        public static Dictionary<string, Dictionary<int, int>> LoadChartData(string connStr)
        {
            var yearRange = Enumerable.Range(2020, 5).ToList();
            var data = new Dictionary<string, Dictionary<int, int>>()
    {
        { "chair", yearRange.ToDictionary(y => y, y => 0) },
        { "cabinet", yearRange.ToDictionary(y => y, y => 0) },
        { "table", yearRange.ToDictionary(y => y, y => 0) }
    };

            using var conn = new MySqlConnection(connStr);
            conn.Open();
            var cmd = new MySqlCommand(@"
        SELECT 
            p.code_name,
            YEAR(o.order_time) AS year,
            SUM(i.Pquantity) AS total_processed
        FROM order_items i
        JOIN products p ON i.product_id = p.product_id
        JOIN orders o ON i.order_id = o.order_id
        WHERE YEAR(o.order_time) BETWEEN 2020 AND 2024
        GROUP BY p.code_name, YEAR(o.order_time)", conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string code = reader.GetString("code_name");
                int year = reader.GetInt32("year");
                int total = reader.GetInt32("total_processed");

                if (data.ContainsKey(code) && data[code].ContainsKey(year))
                    data[code][year] = total;
            }

            return data;
        }









    }

}
