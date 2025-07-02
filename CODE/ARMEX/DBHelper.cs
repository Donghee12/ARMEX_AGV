using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace _ARMEX
{
    public static class DBHelper
    {
        private static string connStr = "server=127.0.0.1;user=root;password=1234;database=factory_db;";
        // 주문 내역을 DataGridView에 불러와 표시
        public static void LoadOrderData(DataGridView dgv, string connStr)
        {
            string query = @"
        SELECT 
            o.order_id AS '주문번호',
            o.company_name AS '회사명',
            p.code_name AS '제품명',
            i.quantity AS '주문 수량',
            i.Pquantity AS '처리 수량',
            o.status AS '주문 상태'
        FROM orders o
        JOIN order_items i ON o.order_id = i.order_id
        JOIN products p ON p.product_id = i.product_id
        WHERE o.status != '처리됨'
        ORDER BY o.order_id ASC, p.product_id ASC;";

            using var conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                var adapter = new MySqlDataAdapter(query, conn);
                var dt = new DataTable();
                adapter.Fill(dt);
                dgv.DataSource = dt;

                dgv.DefaultCellStyle.Font = new Font("맑은 고딕", 10);
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgv.RowHeadersVisible = false;
                dgv.AllowUserToAddRows = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("주문 데이터 불러오기 실패: " + ex.Message);
            }
        }





        // (3일 초과) 감지 이미지 및 경로 DB에서 정리
        public static void DeleteOldEmergencyImages(int daysThreshold)
        {
            using var conn = new MySqlConnection(connStr);
            conn.Open();

            var cutoff = DateTime.Now.AddDays(-daysThreshold);
            var selectCmd = new MySqlCommand(@"
        SELECT image_path 
        FROM emergency_logs 
        WHERE event_time < @cutoff AND image_path IS NOT NULL", conn);
            selectCmd.Parameters.AddWithValue("@cutoff", cutoff);

            using var reader = selectCmd.ExecuteReader();
            var filesToDelete = new List<string>();
            while (reader.Read())
            {
                filesToDelete.Add(reader.GetString(0));
            }
            reader.Close();

            foreach (var file in filesToDelete)
            {
                try
                {
                    if (File.Exists(file)) File.Delete(file);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"파일 삭제 오류: {file} → {ex.Message}");
                }

                var deleteCmd = new MySqlCommand("UPDATE emergency_logs SET image_path = NULL WHERE image_path = @path", conn);
                deleteCmd.Parameters.AddWithValue("@path", file);
                deleteCmd.ExecuteNonQuery();
            }
        }

        // (1년 초과) 오래된 긴급 로그(DB) 삭제
        public static void DeleteOldEmergencyLogs(int daysThreshold)
        {
            using var conn = new MySqlConnection(connStr);
            conn.Open();

            var cutoff = DateTime.Now.AddDays(-daysThreshold);
            var deleteCmd = new MySqlCommand(@"
            DELETE FROM emergency_logs 
            WHERE event_time < @cutoff", conn);
            deleteCmd.Parameters.AddWithValue("@cutoff", cutoff);
            deleteCmd.ExecuteNonQuery();
        }

        // (5년 초과) 오래된 주문 및 주문 아이템 데이터 삭제
        public static void DeleteOldOrderData(int yearThreshold)
        {
            using var conn = new MySqlConnection(connStr);
            conn.Open();

            int targetYear = DateTime.Now.Year - yearThreshold;

            var deleteItemsCmd = new MySqlCommand(@"
            DELETE i FROM order_items i
            JOIN orders o ON i.order_id = o.order_id
            WHERE YEAR(o.order_time) < @cutoffYear", conn);
            deleteItemsCmd.Parameters.AddWithValue("@cutoffYear", targetYear);
            deleteItemsCmd.ExecuteNonQuery();

            var deleteOrdersCmd = new MySqlCommand(@"
            DELETE FROM orders 
            WHERE YEAR(order_time) < @cutoffYear", conn);
            deleteOrdersCmd.Parameters.AddWithValue("@cutoffYear", targetYear);
            deleteOrdersCmd.ExecuteNonQuery();
        }

        // 모든 오래된 기록(이미지/로그/주문) 정리(통합 호출)
        public static void CleanupAllOldRecords()
        {
            DeleteOldEmergencyImages(365);
            DeleteOldEmergencyLogs(365);
            DeleteOldOrderData(5);
        }



    }
}
