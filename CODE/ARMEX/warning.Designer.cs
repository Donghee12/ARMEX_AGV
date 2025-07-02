namespace _ARMEX
{
    partial class warning
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
            label12 = new Label();
            btnConfirm = new Button();
            panel1 = new Panel();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label12
            // 
            label12.Font = new Font("맑은 고딕", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
            label12.ForeColor = Color.Black;
            label12.Location = new Point(78, 35);
            label12.Name = "label12";
            label12.Size = new Size(239, 76);
            label12.TabIndex = 0;
            label12.Text = "물체를 잡지 못했습니다\r\n관리자를 호출하세요";
            label12.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnConfirm
            // 
            btnConfirm.Font = new Font("맑은 고딕", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 129);
            btnConfirm.Location = new Point(111, 129);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(172, 75);
            btnConfirm.TabIndex = 1;
            btnConfirm.Text = "경고 확인";
            btnConfirm.UseVisualStyleBackColor = true;
            btnConfirm.Click += btnConfirm_Click;
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(label12);
            panel1.Controls.Add(btnConfirm);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(389, 230);
            panel1.TabIndex = 2;
            // 
            // warning
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(389, 230);
            Controls.Add(panel1);
            Name = "warning";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "warning";
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Label label12;
        private Button btnConfirm;
        private Panel panel1;
    }
}