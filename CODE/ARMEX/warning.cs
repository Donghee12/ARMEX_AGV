using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _ARMEX
{
    public partial class warning : Form
    {

        private System.Windows.Forms.Timer blinkTimer;
        private bool isRed = true;
        public warning()
        {
            InitializeComponent();
            btnConfirm.FlatStyle = FlatStyle.Flat;
            btnConfirm.FlatAppearance.BorderSize = 0; // 테두리 두께 0
            blinkTimer = new System.Windows.Forms.Timer();
            blinkTimer.Interval = 500; // 0.5초마다 깜빡
            blinkTimer.Tick += BlinkButton;
            blinkTimer.Start();
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void BlinkButton(object sender, EventArgs e)
        {
            if (isRed)
            {
                btnConfirm.BackColor = Color.Red;
                btnConfirm.ForeColor = Color.White;
            }
            else
            {
                btnConfirm.BackColor = Color.White;
                btnConfirm.ForeColor = Color.Black;
            }
            isRed = !isRed;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            blinkTimer.Stop();
            this.Close();
        }
    }
}
