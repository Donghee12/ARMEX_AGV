using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace _ARMEX
{
    public partial class EmergencyDetailForm : Form
    {
        private int logId;
        private string userRole;
        private string connStr;

        public EmergencyDetailForm(int logId, string userRole, string connStr)
        {
            InitializeComponent();
            this.logId = logId;
            this.userRole = userRole;
            this.connStr = connStr;
        }

        private void EmergencyDetailForm_Load(object sender, EventArgs e)
        {
            LoadLogDetail();

            this.FormBorderStyle = FormBorderStyle.None;

        }
        private void LoadLogDetail()
        {
            using var conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                string query = "SELECT username, event_time, description, status, image_path FROM emergency_logs WHERE log_id = @logId";
                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@logId", logId);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    lbl_username.Text = reader.GetString("username");
                    lbl_time.Text = reader.GetDateTime("event_time").ToString("yyyy-MM-dd HH:mm:ss");
                    tb_description.Text = reader.GetString("description");
                    tb_status.Text = reader.GetString("status");

                    // ✅ 이미지 불러오기
                    string imagePath = reader["image_path"]?.ToString();
                    if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                    {
                        pb_emergencyImage.Image = Image.FromFile(imagePath);
                    }
                    else
                    {
                        pb_emergencyImage.Image = null; // 또는 기본 이미지 설정
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("로그 세부 정보 불러오기 실패: " + ex.Message);
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            using var conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                string update = "UPDATE emergency_logs SET description = @desc, status = @status WHERE log_id = @logId";
                var cmd = new MySqlCommand(update, conn);
                cmd.Parameters.AddWithValue("@desc", tb_description.Text);
                cmd.Parameters.AddWithValue("@status", tb_status.Text);
                cmd.Parameters.AddWithValue("@logId", logId);
                cmd.ExecuteNonQuery();

                MessageBox.Show("수정이 완료되었습니다.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("수정 실패: " + ex.Message);
            }
        }
    }
}
