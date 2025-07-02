namespace _ARMEX
{
    partial class CompletedOrdersForm
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
            dgv_complateorder = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgv_complateorder).BeginInit();
            SuspendLayout();
            // 
            // dgv_complateorder
            // 
            dgv_complateorder.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv_complateorder.Location = new Point(2, 1);
            dgv_complateorder.Name = "dgv_complateorder";
            dgv_complateorder.Size = new Size(796, 449);
            dgv_complateorder.TabIndex = 0;
            // 
            // CompletedOrdersForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(dgv_complateorder);
            Name = "CompletedOrdersForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CompletedOrdersForm";
            Load += CompletedOrdersForm_Load;
            ((System.ComponentModel.ISupportInitialize)dgv_complateorder).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgv_complateorder;
    }
}