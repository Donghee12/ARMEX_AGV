namespace _ARMEX
{
    partial class Login
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tb_pw = new TextBox();
            tb_id = new TextBox();
            bt_login = new Button();
            pb_login = new PictureBox();
            pb_pw = new PictureBox();
            pl_loginpw = new Panel();
            pl_pw = new Panel();
            pl_login = new Panel();
            panel1 = new Panel();
            pictureBox1 = new PictureBox();
            bt_exit = new Button();
            ((System.ComponentModel.ISupportInitialize)pb_login).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pb_pw).BeginInit();
            pl_loginpw.SuspendLayout();
            pl_pw.SuspendLayout();
            pl_login.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // tb_pw
            // 
            tb_pw.Font = new Font("맑은 고딕", 18F, FontStyle.Regular, GraphicsUnit.Point, 129);
            tb_pw.ForeColor = SystemColors.GrayText;
            tb_pw.Location = new Point(40, 1);
            tb_pw.Name = "tb_pw";
            tb_pw.Size = new Size(330, 39);
            tb_pw.TabIndex = 2;
            tb_pw.Enter += tb_pw_Enter;
            tb_pw.Leave += tb_pw_Leave;
            // 
            // tb_id
            // 
            tb_id.BackColor = Color.White;
            tb_id.Font = new Font("맑은 고딕", 18F, FontStyle.Regular, GraphicsUnit.Point, 129);
            tb_id.ForeColor = SystemColors.GrayText;
            tb_id.Location = new Point(40, 1);
            tb_id.Name = "tb_id";
            tb_id.Size = new Size(330, 39);
            tb_id.TabIndex = 1;
            tb_id.Enter += tb_id_Enter;
            tb_id.Leave += tb_id_Leave;
            // 
            // bt_login
            // 
            bt_login.BackColor = Color.FromArgb(255, 128, 0);
            bt_login.FlatStyle = FlatStyle.Flat;
            bt_login.Font = new Font("맑은 고딕", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 129);
            bt_login.ForeColor = Color.White;
            bt_login.Location = new Point(20, 156);
            bt_login.Name = "bt_login";
            bt_login.Size = new Size(370, 40);
            bt_login.TabIndex = 3;
            bt_login.Text = "Login";
            bt_login.UseVisualStyleBackColor = false;
            bt_login.Click += bt_login_Click;
            // 
            // pb_login
            // 
            pb_login.BackColor = Color.White;
            pb_login.BackgroundImage = Properties.Resources.스크린샷_2025_05_22_151844_removebg_preview;
            pb_login.BackgroundImageLayout = ImageLayout.Zoom;
            pb_login.Location = new Point(0, 0);
            pb_login.Name = "pb_login";
            pb_login.Size = new Size(39, 39);
            pb_login.TabIndex = 8;
            pb_login.TabStop = false;
            // 
            // pb_pw
            // 
            pb_pw.BackColor = Color.White;
            pb_pw.BackgroundImage = Properties.Resources.스크린샷_2025_05_22_151849_removebg_preview;
            pb_pw.BackgroundImageLayout = ImageLayout.Zoom;
            pb_pw.Location = new Point(0, 0);
            pb_pw.Name = "pb_pw";
            pb_pw.Size = new Size(40, 40);
            pb_pw.TabIndex = 9;
            pb_pw.TabStop = false;
            // 
            // pl_loginpw
            // 
            pl_loginpw.BackColor = Color.FromArgb(50, 50, 50);
            pl_loginpw.Controls.Add(pl_pw);
            pl_loginpw.Controls.Add(pl_login);
            pl_loginpw.Controls.Add(bt_login);
            pl_loginpw.Location = new Point(20, 225);
            pl_loginpw.Name = "pl_loginpw";
            pl_loginpw.Size = new Size(411, 213);
            pl_loginpw.TabIndex = 10;
            // 
            // pl_pw
            // 
            pl_pw.BackColor = Color.White;
            pl_pw.Controls.Add(pb_pw);
            pl_pw.Controls.Add(tb_pw);
            pl_pw.Location = new Point(20, 79);
            pl_pw.Name = "pl_pw";
            pl_pw.Size = new Size(370, 40);
            pl_pw.TabIndex = 12;
            // 
            // pl_login
            // 
            pl_login.BackColor = Color.White;
            pl_login.Controls.Add(tb_id);
            pl_login.Controls.Add(pb_login);
            pl_login.Location = new Point(20, 21);
            pl_login.Name = "pl_login";
            pl_login.Size = new Size(370, 40);
            pl_login.TabIndex = 11;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(pictureBox1);
            panel1.Controls.Add(pl_loginpw);
            panel1.Location = new Point(416, 103);
            panel1.Name = "panel1";
            panel1.Size = new Size(452, 460);
            panel1.TabIndex = 11;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.ChatGPT_Image_2025년_6월_5일_오후_03_03_45_removebg_preview;
            pictureBox1.Location = new Point(20, 54);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(397, 169);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 11;
            pictureBox1.TabStop = false;
            // 
            // bt_exit
            // 
            bt_exit.BackColor = Color.FromArgb(255, 128, 0);
            bt_exit.FlatStyle = FlatStyle.Flat;
            bt_exit.Font = new Font("맑은 고딕", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
            bt_exit.ForeColor = Color.Black;
            bt_exit.Location = new Point(457, 614);
            bt_exit.Name = "bt_exit";
            bt_exit.Size = new Size(370, 40);
            bt_exit.TabIndex = 12;
            bt_exit.Text = "시스템 종료";
            bt_exit.UseVisualStyleBackColor = false;
            bt_exit.Click += bt_exit_Click;
            // 
            // Login
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(1264, 681);
            ControlBox = false;
            Controls.Add(bt_exit);
            Controls.Add(panel1);
            Name = "Login";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pb_login).EndInit();
            ((System.ComponentModel.ISupportInitialize)pb_pw).EndInit();
            pl_loginpw.ResumeLayout(false);
            pl_pw.ResumeLayout(false);
            pl_pw.PerformLayout();
            pl_login.ResumeLayout(false);
            pl_login.PerformLayout();
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private TextBox tb_id;
        private TextBox tb_pw;
        private Button bt_login;
        private PictureBox pb_login;
        private PictureBox pb_pw;
        private Panel pl_loginpw;
        private Panel pl_login;
        private Panel pl_pw;
        private Panel panel1;
        private PictureBox pictureBox1;
        private Button bt_exit;
    }
}
