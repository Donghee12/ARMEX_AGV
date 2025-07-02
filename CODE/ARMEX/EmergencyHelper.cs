using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using OpenCvSharp;

namespace _ARMEX
{
    public static class EmergencyHelper
    {
        // 비상 상황 발생시 이미지 포함 로그를 DB와 로컬에 저장
        public static void InsertEmergencyLogWithImage(string username, string description, string connStr, Mat image)
        {
            try
            {
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string filename = $"emergency_{timestamp}.jpg";
                string savePath = Path.Combine(Application.StartupPath, "emergency_images", filename);

                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                Cv2.ImWrite(savePath, image);

                using var conn = new MySqlConnection(connStr);
                conn.Open();

                string query = @"
                    INSERT INTO emergency_logs (username, description, image_path) 
                    VALUES (@username, @description, @path);";

                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@path", savePath);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("긴급 로그 저장 실패: " + ex.Message);
            }
        }

        // 비상 상황 발생시 텍스트만 로그를 DB에 저장
        public static void InsertEmergencyLog(string username, string description, string connStr)
        {
            try
            {
                using var conn = new MySqlConnection(connStr);
                conn.Open();

                string query = @"
            INSERT INTO emergency_logs (username, description) 
            VALUES (@username, @description);";

                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("긴급 로그 저장 실패 (이미지 없음): " + ex.Message);
            }
        }




        // 비상 로그 전체를 DataGridView에 불러와 표시
        public static void LoadEmergencyLogs(DataGridView dgv, string connStr)
        {
            using var conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                string query = @"
                    SELECT 
                        log_id AS '번호',
                        username AS '사용자',
                        event_time AS '시간',
                        description AS '내용',
                        status AS '상태'
                    FROM emergency_logs
                    ORDER BY event_time DESC;";

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
                MessageBox.Show("긴급 로그 불러오기 실패: " + ex.Message);
            }
        }
        // 시스템의 현재 비상 상태(ON/OFF) 조회
        public static bool IsEmergencyActive(string connStr)
        {
            using var conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT is_emergency FROM system_status WHERE id = 1", conn);
                return Convert.ToBoolean(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                MessageBox.Show("비상 상태 확인 실패: " + ex.Message);
                return false;
            }
        }
        // 시스템의 비상 상태(ON/OFF) 설정
        public static void SetEmergencyState(bool isEmergency, string connStr)
        {
            using var conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                var cmd = new MySqlCommand("UPDATE system_status SET is_emergency = @val WHERE id = 1", conn);
                cmd.Parameters.AddWithValue("@val", isEmergency);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("비상 상태 설정 실패: " + ex.Message);
            }
        }
    }
}
