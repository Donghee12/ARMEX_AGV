namespace _ARMEX
{
    partial class EmergencyDetailForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lbl_username = new Label();
            lbl_time = new Label();
            tb_description = new TextBox();
            tb_status = new TextBox();
            btn_save = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            panel1 = new Panel();
            panel2 = new Panel();
            panel3 = new Panel();
            panel4 = new Panel();
            pb_emergencyImage = new PictureBox();
            label5 = new Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pb_emergencyImage).BeginInit();
            SuspendLayout();
            // 
            // lbl_username
            // 
            lbl_username.AutoSize = true;
            lbl_username.Font = new Font("맑은 고딕", 14.25F, FontStyle.Bold);
            lbl_username.ForeColor = Color.Black;
            lbl_username.Location = new Point(114, 10);
            lbl_username.Name = "lbl_username";
            lbl_username.Size = new Size(69, 25);
            lbl_username.TabIndex = 0;
            lbl_username.Text = "사용자";
            // 
            // lbl_time
            // 
            lbl_time.AutoSize = true;
            lbl_time.Font = new Font("맑은 고딕", 14.25F, FontStyle.Bold);
            lbl_time.ForeColor = Color.Black;
            lbl_time.Location = new Point(114, 10);
            lbl_time.Name = "lbl_time";
            lbl_time.Size = new Size(50, 25);
            lbl_time.TabIndex = 1;
            lbl_time.Text = "시간";
            // 
            // tb_description
            // 
            tb_description.Font = new Font("맑은 고딕", 12F, FontStyle.Regular, GraphicsUnit.Point, 129);
            tb_description.Location = new Point(1, 42);
            tb_description.Multiline = true;
            tb_description.Name = "tb_description";
            tb_description.Size = new Size(416, 65);
            tb_description.TabIndex = 2;
            // 
            // tb_status
            // 
            tb_status.Font = new Font("맑은 고딕", 12F, FontStyle.Regular, GraphicsUnit.Point, 129);
            tb_status.Location = new Point(106, 1);
            tb_status.Multiline = true;
            tb_status.Name = "tb_status";
            tb_status.Size = new Size(314, 51);
            tb_status.TabIndex = 3;
            // 
            // btn_save
            // 
            btn_save.BackColor = Color.FromArgb(255, 128, 0);
            btn_save.FlatStyle = FlatStyle.Flat;
            btn_save.Font = new Font("맑은 고딕", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btn_save.ForeColor = Color.White;
            btn_save.Location = new Point(357, 382);
            btn_save.Name = "btn_save";
            btn_save.Size = new Size(165, 43);
            btn_save.TabIndex = 4;
            btn_save.Text = "저장";
            btn_save.UseVisualStyleBackColor = false;
            btn_save.Click += btn_save_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("맑은 고딕", 14.25F, FontStyle.Bold);
            label1.Location = new Point(8, 12);
            label1.Name = "label1";
            label1.Size = new Size(95, 25);
            label1.TabIndex = 5;
            label1.Text = "상세 사유";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("맑은 고딕", 14.25F, FontStyle.Bold);
            label2.Location = new Point(8, 12);
            label2.Name = "label2";
            label2.Size = new Size(95, 25);
            label2.TabIndex = 6;
            label2.Text = "현재 상태";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("맑은 고딕", 14.25F, FontStyle.Bold);
            label3.ForeColor = Color.Black;
            label3.Location = new Point(12, 10);
            label3.Name = "label3";
            label3.Size = new Size(59, 25);
            label3.TabIndex = 7;
            label3.Text = "USER";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("맑은 고딕", 14.25F, FontStyle.Bold);
            label4.ForeColor = Color.Black;
            label4.Location = new Point(12, 10);
            label4.Name = "label4";
            label4.Size = new Size(57, 25);
            label4.TabIndex = 8;
            label4.Text = "TIME";
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(label2);
            panel1.Controls.Add(tb_status);
            panel1.Location = new Point(449, 307);
            panel1.Name = "panel1";
            panel1.Size = new Size(420, 52);
            panel1.TabIndex = 9;
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(label1);
            panel2.Controls.Add(tb_description);
            panel2.Location = new Point(449, 182);
            panel2.Name = "panel2";
            panel2.Size = new Size(420, 108);
            panel2.TabIndex = 10;
            // 
            // panel3
            // 
            panel3.BackColor = Color.White;
            panel3.BorderStyle = BorderStyle.FixedSingle;
            panel3.Controls.Add(panel4);
            panel3.Controls.Add(label3);
            panel3.Controls.Add(lbl_username);
            panel3.Location = new Point(449, 59);
            panel3.Name = "panel3";
            panel3.Size = new Size(420, 100);
            panel3.TabIndex = 11;
            // 
            // panel4
            // 
            panel4.BackColor = Color.White;
            panel4.BorderStyle = BorderStyle.FixedSingle;
            panel4.Controls.Add(label4);
            panel4.Controls.Add(lbl_time);
            panel4.Location = new Point(-1, 50);
            panel4.Name = "panel4";
            panel4.Size = new Size(420, 50);
            panel4.TabIndex = 12;
            // 
            // pb_emergencyImage
            // 
            pb_emergencyImage.Location = new Point(17, 59);
            pb_emergencyImage.Name = "pb_emergencyImage";
            pb_emergencyImage.Size = new Size(411, 300);
            pb_emergencyImage.SizeMode = PictureBoxSizeMode.StretchImage;
            pb_emergencyImage.TabIndex = 12;
            pb_emergencyImage.TabStop = false;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("맑은 고딕", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 129);
            label5.ForeColor = Color.White;
            label5.Location = new Point(12, 21);
            label5.Name = "label5";
            label5.Size = new Size(139, 25);
            label5.TabIndex = 13;
            label5.Text = "▶ A구역 CAM";
            // 
            // EmergencyDetailForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(50, 50, 50);
            ClientSize = new Size(881, 438);
            Controls.Add(label5);
            Controls.Add(pb_emergencyImage);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(btn_save);
            Name = "EmergencyDetailForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "EmergencyDetailForm";
            Load += EmergencyDetailForm_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pb_emergencyImage).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lbl_username;
        private Label lbl_time;
        private TextBox tb_description;
        private TextBox tb_status;
        private Button btn_save;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Panel panel4;
        private PictureBox pb_emergencyImage;
        private Label label5;
    }
}