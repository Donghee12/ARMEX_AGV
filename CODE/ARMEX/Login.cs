using MySql.Data.MySqlClient;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Threading;
using System.Windows.Forms;



namespace _ARMEX

{
    public partial class Login : Form
    {
        private Main mainForm;
        private static bool isFirstLogin = true;
        public Login(Main main)
        {
            InitializeComponent();
            mainForm = main;
            this.AcceptButton = bt_login;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tb_id.Text = " ID";
            tb_id.ForeColor = Color.Gray;

            tb_pw.Text = " Password";
            tb_pw.ForeColor = Color.Gray;
            tb_pw.UseSystemPasswordChar = false;
            this.FormBorderStyle = FormBorderStyle.None;
        }



        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            mainForm.Close();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void tb_id_Enter(object sender, EventArgs e)
        {
            if (tb_id.Text == " ID")
            {
                tb_id.Text = "";
                tb_id.ForeColor = Color.Black;
            }
        }

        private void tb_id_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tb_id.Text))
            {
                tb_id.Text = " ID";
                tb_id.ForeColor = Color.Gray;
            }
        }

        private void tb_pw_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tb_pw.Text))
            {
                tb_pw.UseSystemPasswordChar = false;
                tb_pw.Text = " Password";
                tb_pw.ForeColor = Color.Gray;
            }
        }

        private void tb_pw_Enter(object sender, EventArgs e)
        {
            if (tb_pw.Text == " Password")
            {
                tb_pw.Text = "";
                tb_pw.ForeColor = Color.Black;
                tb_pw.UseSystemPasswordChar = true; // ●●●●● 표시
            }
        }

        private void bt_login_Click(object sender, EventArgs e)
        {
            string id = tb_id.Text.Trim();
            string pw = tb_pw.Text;

            string connStr = "server=127.0.0.1;port=3306;user=root;password=1234;database=factory_db;";

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    string query = "SELECT user_type FROM admin_users WHERE username = @id AND password = @pw";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@pw", pw);

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        string userType = result.ToString();

                        if (isFirstLogin && userType != "admin")
                        {
                            MessageBox.Show("최초 로그인은 관리자만 가능합니다");
                            return;
                        }
                        isFirstLogin = false;


                        Task.Run(() =>
                        {
                            Thread.Sleep(700);
                            this.Invoke(() =>
                            {
                                mainForm.ApplyUserRole(userType, id);  // 로그인 성공 후 권한 적용
                                mainForm.Show();                   // 숨겨진 메인 폼 표시
                                this.Hide();                       // 로그인 폼 숨김
                            });
                        });
                    }
                    else
                    {
                        MessageBox.Show("로그인 실패: 아이디 또는 비밀번호가 올바르지 않습니다.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("DB 연결 오류: " + ex.Message);
                }
            }
        }


        private void bt_exit_Click(object sender, EventArgs e)
        {
            // Main 폼이 열려있으면 닫아줌
            foreach (Form f in Application.OpenForms)
            {
                if (f is Main)
                {
                    f.Close();
                    break;
                }
            }
            // Login 폼 닫기 (자기 자신)
            this.Close();
            // 여기서만 Application.Exit() 호출
            Application.Exit();
        }
    }
}
