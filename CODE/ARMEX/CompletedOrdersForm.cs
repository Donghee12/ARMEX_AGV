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
    public partial class CompletedOrdersForm : Form
    {
        private string connStr;

        public CompletedOrdersForm(string connectionString)
        {
            InitializeComponent();
            connStr = connectionString;
        }

        private void CompletedOrdersForm_Load(object sender, EventArgs e)
        {
            LoadCompletedOrders();
        }


        private void LoadCompletedOrders()
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
        WHERE o.status = '처리됨'
        ORDER BY o.order_id;";

            using var conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                var adapter = new MySqlDataAdapter(query, conn);
                var dt = new DataTable();
                adapter.Fill(dt);

                dgv_complateorder.DataSource = dt;

                dgv_complateorder.DefaultCellStyle.Font = new Font("맑은 고딕", 10);
                dgv_complateorder.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgv_complateorder.RowHeadersVisible = false;
                dgv_complateorder.AllowUserToAddRows = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("처리된 주문 데이터 불러오기 실패: " + ex.Message);
            }
        }


    }
}
