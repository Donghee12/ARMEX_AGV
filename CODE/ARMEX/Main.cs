using Compunet.YoloSharp;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.WinForms;
using MQTTnet;
using MQTTnet.Client;
using MySql.Data.MySqlClient;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using SkiaSharp;
using System;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
namespace _ARMEX
{
    public partial class Main : Form
    {
        private string userRole;
        private string userName;
        private string connStr = "server=127.0.0.1;user=root;password=1234;database=factory_db;";
        private CameraManager cam1 = new CameraManager();
        private CameraManager cam2 = new CameraManager();
        private bool cameraInitialized = false;
        private double currentExposure = -5; // 초기 노출값은 카메라에서 받아올 예정
        private bool servoBusy = false;
        private DateTime lastDetectionTime = DateTime.MinValue;  //객체 감지후 1초동안 새 감지 무시하기 위해
        private int pendingOrderId = -1;        //임시 저장소
        private int pendingProductId = -1;      //임시 저장소
        private string pendingLabel = null;     //임시 저장소
        private int currentVehicleNumber = -1; // 현재 차량 번호 저장
        private int currentChartIndex = 0;
        private readonly string[] productKeys = { "chair", "cabinet", "table" };
        private Dictionary<string, Dictionary<int, int>> chartData;



        private YoloPredictor predictor = new YoloPredictor(@"C:\Users\moble\Desktop\CODE\ARMEX\FA3_YOLO\0524_1.onnx");   //여기 경로 항상 확인하기 onnx파일 경로임

        private IMqttClient mqttClient;

        public Main()
        {
            InitializeComponent();
            this.Load += Main_Load;  // PictureBox 준비 이후 실행을 위해 Load 사용
            this.Shown += Main_Shown;
            cam1.OnYoloDetection += HandleDetection; //YOLO DETECTION EVENT HANDLER

            // 1. 폼 로드 등에서 아래 코드로 DrawMode 변경
            tab.Appearance = TabAppearance.Normal;  // Button 모양 방지
            tab.DrawMode = TabDrawMode.OwnerDrawFixed;
            tab.SizeMode = TabSizeMode.Fixed;
            tab.ItemSize = new System.Drawing.Size(314, 50); 
            tab.BackColor = Color.Black;  

            tab.DrawItem += tab_DrawItem;

            dgv_order.RowPrePaint += dgv_order_RowPrePaint;
            this.FormBorderStyle = FormBorderStyle.None;

        }

        // 탭 UI 사용자 정의 그리기
        private void tab_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl tab = sender as TabControl;
            Font f = new Font("맑은 고딕", 16, FontStyle.Bold); // 원하는 폰트와 크기

            Rectangle r = e.Bounds;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            // 선택된 탭이면 배경색 다르게
            if (e.Index == tab.SelectedIndex)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.Gray), r);
            }
            else if (e.Index == tab.SelectedIndex)
            {
                Rectangle fullCover = new Rectangle(r.X - 1, r.Y - 1, r.Width + 2, r.Height + 2);
                e.Graphics.FillRectangle(new SolidBrush(Color.Black), fullCover);  // 비선택 탭 색으로 덮음
            }

            // 텍스트 그리기
            e.Graphics.DrawString(tab.TabPages[e.Index].Text, f, Brushes.Black, r, sf);
        }

        // 폼 로드시 카메라 및 탭 등 초기화
        private void Main_Load(object sender, EventArgs e)
        {
            if (cameraInitialized) return;
            cameraInitialized = true;

            //  현재 탭 기억
            var originalTab = tab.SelectedTab;

            //  각 PictureBox가 있는 탭으로 전환 → Handle 생성 유도
            tab.SelectedTab = tabPage4;  // pb_video2,3
            Application.DoEvents();

            //  원래 탭으로 복귀
            tab.SelectedTab = originalTab;
            Application.DoEvents();

            //  cam1 시작 (pb_video1에 출력)
            cam1.EnableYolo(predictor); // cam1에만 YOLO 적용
            cam1.Start(0, pb_video1);
            cam1.SetMirror(pb_video3);

            Thread.Sleep(500);  // cam2 충돌 방지용 대기

            //  cam2 시작 (pb_video2에 출력)
            cam2.Start(1, pb_video2); // cam2에는 YOLO 적용하지 않음

            DBHelper.CleanupAllOldRecords();
            bt_reset.Enabled = false;

        }

        // 주문 DataGridView에 현재 차량 강조 색상 표시
        private void dgv_order_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var dgv = sender as DataGridView;
            if (dgv.Columns.Contains("주문번호")) // 컬럼 이름 확인
            {
                var row = dgv.Rows[e.RowIndex];
                var cellValue = row.Cells["주문번호"].Value;

                if (cellValue != null && int.TryParse(cellValue.ToString(), out int oid))
                {
                    if (oid == currentVehicleNumber && label_vehicle.Text != "차량 대기 중...")
                    {
                        // 현재 차량이면 주황색
                        row.DefaultCellStyle.BackColor = Color.Orange;
                        row.DefaultCellStyle.ForeColor = Color.Black;
                    }
                    else
                    {
                        // 아닌 경우 흰색
                        row.DefaultCellStyle.BackColor = Color.White;
                        row.DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
            }
        }

        // 폼이 표시될 때 초기 UI, MQTT 연결 등 수행
        private void Main_Shown(object sender, EventArgs e)
        {
            currentExposure = -5;
            lb_light.Text = $"현재 노출도: {currentExposure}";
            DBHelper.CleanupAllOldRecords();
            if (currentVehicleNumber == -1)
            {
                label_vehicle.Text = "차량 대기 중...";
            }
            else
            {
                label_vehicle.Text = $"차량 번호 : {currentVehicleNumber}, 처리 중";
                dgv_order.Refresh();
            }
            try
            {
                ConnectToBroker();
            }
            catch (Exception ex)
            {
                MessageBox.Show("MQTT 연결 실패: " + ex.Message);
            }


        }

        // 로그인 후 유저 권한별 탭 설정 및 데이터 로드
        public void ApplyUserRole(string role, string id)
        {
            this.userRole = role;
            this.userName = id;
            tab.TabPages.Clear();
            tab.TabPages.Add(tabPage1);

            if (role == "admin")
            {
                tab.TabPages.Add(tabPage2);
                tab.TabPages.Add(tabPage3);
                tab.TabPages.Add(tabPage4);
            }

            OptimizeDataGridView(dgv_order);
            OptimizeDataGridView(dgv_emergency);
            OptimizeDataGridView(dgv_completedorders);
            DBHelper.LoadOrderData(dgv_order, connStr);

            chartData = ChartHelper.LoadChartData(connStr); 
            DisplayCurrentChart(); 
            LoadCompletedOrders();

            EmergencyHelper.LoadEmergencyLogs(dgv_emergency, connStr);

            bool isEmergency = EmergencyHelper.IsEmergencyActive(connStr);
            bt_emergency.Enabled = !isEmergency;


            dgv_order.CellPainting += DataGridView_CellPainting;
            dgv_emergency.CellPainting += DataGridView_CellPainting;
            dgv_completedorders.CellPainting += DataGridView_CellPainting;
            dgv_completedorders.ReadOnly = true;
            dgv_order.ReadOnly = true;
        }

        // DataGridView 기본 스타일 적용
        private void OptimizeDataGridView(DataGridView dgv)
        {
            dgv.DefaultCellStyle.Font = new Font("맑은 고딕", 9);
            dgv.DefaultCellStyle.Padding = new Padding(3);
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.DefaultCellStyle.SelectionBackColor = Color.Orange;
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;

            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("맑은 고딕", 12, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.Orange;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgv.EnableHeadersVisualStyles = false;

            dgv.ColumnHeadersHeight = 22;
            dgv.RowTemplate.Height = 22;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.BackgroundColor = Color.White;
        }

        // 현재 차량만 주문 테이블에서 강조 표시
        private void HighlightOrderRowOnly()
        {
            if (currentVehicleNumber == -1) return;

            foreach (DataGridViewRow row in dgv_order.Rows)
            {
                if (row.Cells["주문번호"].Value != null &&
                    int.TryParse(row.Cells["주문번호"].Value.ToString(), out int orderId))
                {
                    if (orderId == currentVehicleNumber)
                    {
                       
                        row.DefaultCellStyle.BackColor = Color.Orange;
                        row.DefaultCellStyle.ForeColor = Color.Black;
                    }
                    else
                    {
                       
                        row.DefaultCellStyle.BackColor = Color.White;
                        row.DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
            }
        }

        // DataGridView 헤더 셀 페인팅(정렬 표시 포함)
        private void DataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex >= 0)
            {
                var dgv = sender as DataGridView;

                using (SolidBrush backColorBrush = new SolidBrush(Color.White))
                {
                    e.Graphics.FillRectangle(backColorBrush, e.CellBounds);
                }

                using (var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                using (var brush = new SolidBrush(e.CellStyle.ForeColor))
                {
                    e.Graphics.DrawString(e.FormattedValue?.ToString(), e.CellStyle.Font, brush, e.CellBounds, format);
                }

                SortOrder sortOrder = dgv.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection;
                if (sortOrder != SortOrder.None)
                {
                    System.Drawing.Point triangleMid = new System.Drawing.Point(e.CellBounds.Right - 10, e.CellBounds.Top + e.CellBounds.Height / 2);
                    System.Drawing.Point[] triangle = new System.Drawing.Point[3];

                    if (sortOrder == SortOrder.Ascending)
                    {
                        triangle[0] = new System.Drawing.Point(triangleMid.X - 4, triangleMid.Y + 2);
                        triangle[1] = new System.Drawing.Point(triangleMid.X + 4, triangleMid.Y + 2);
                        triangle[2] = new System.Drawing.Point(triangleMid.X, triangleMid.Y - 3);
                    }
                    else
                    {
                        triangle[0] = new System.Drawing.Point(triangleMid.X - 4, triangleMid.Y - 2);
                        triangle[1] = new System.Drawing.Point(triangleMid.X + 4, triangleMid.Y - 2);
                        triangle[2] = new System.Drawing.Point(triangleMid.X, triangleMid.Y + 3);
                    }

                    using (Brush b = new SolidBrush(Color.Black))
                    {
                        e.Graphics.FillPolygon(b, triangle);
                    }
                }

                e.Handled = true;
            }
        }




        // 폼 종료시 카메라 종료 등 자원 정리
        public void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                cam1.Stop();
                cam2.Stop();
                MessageBox.Show("프로그램이 종료됩니다");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"카메라 종료 중 오류: {ex.Message}");
            }

        }

        // 로그아웃 처리: 로그인 폼 재표시 및 Main 숨김
        private void bt_logout_Click(object sender, EventArgs e)
        {


            Login loginForm = new Login(this);
            loginForm.Show();

            this.Hide(); 
        }


        // 긴급정지 버튼 클릭: 비상신호 및 로그 저장
        private void bt_emergency_Click(object sender, EventArgs e)
        {

            string username = userName;
            string description = "긴급 정지 버튼이 눌렸습니다.";
            // here
            PublishMessage("PC_To/Arduino", $"MAIN:{255}");
            listBox1.Items.Add($"[{DateTime.Now:HH:mm:ss}] [PC_To/Arduino] : 메인 벨트 긴급정지");
            listBox1.TopIndex = listBox1.Items.Count - 1;

            PublishMessage("PC_To/Arduino", $"SECOND:{255}");
            listBox1.Items.Add($"[{DateTime.Now:HH:mm:ss}] [PC_To/Arduino] : A 벨트 긴급정지");
            listBox1.TopIndex = listBox1.Items.Count - 1;



            var frame = cam2.CaptureCurrentFrame();  // cam2에서 현재 영상 캡처
            if (frame != null && !frame.Empty())
            {
                EmergencyHelper.InsertEmergencyLogWithImage(username, description, connStr, frame);
            }
            else
            {
                MessageBox.Show("⚠ cam2 캡처 실패 – 로그만 저장됩니다.");
                EmergencyHelper.InsertEmergencyLog(username, description, connStr);  // 예외 처리로 대체
            }

            EmergencyHelper.SetEmergencyState(true, connStr);
            EmergencyHelper.LoadEmergencyLogs(dgv_emergency, connStr);

            bt_emergency.Enabled = false;
            MessageBox.Show("긴급 정지 버튼이 눌렸습니다 관리자를 호출하세요");
        }

        // 비상로그 더블클릭시 상세정보 폼 표시
        private void dgv_emergency_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int logId = Convert.ToInt32(dgv_emergency.Rows[e.RowIndex].Cells["번호"].Value);
            new EmergencyDetailForm(logId, userName, connStr).ShowDialog();
            EmergencyHelper.LoadEmergencyLogs(dgv_emergency, connStr);
        }

        // 재가동(재시작) 버튼 클릭: 긴급상태 해제 및 시스템 재동작
        private void bt_restart_Click(object sender, EventArgs e)
        {
            if (userRole == "admin")
            {
                EmergencyHelper.SetEmergencyState(false, connStr);

                bt_emergency.Enabled = true;


                // here

                PublishMessage("PC_To/Arduino", $"MAIN:{0}");
                listBox1.Items.Add($"[{DateTime.Now:HH:mm:ss}] [PC_To/Arduino] : 메인 벨트 재가동");
                listBox1.TopIndex = listBox1.Items.Count - 1;

                PublishMessage("PC_To/Arduino", $"SECOND:{0}");
                listBox1.Items.Add($"[{DateTime.Now:HH:mm:ss}] [PC_To/Arduino] : A 벨트 재가동");
                listBox1.TopIndex = listBox1.Items.Count - 1;

                PublishMessage("PC_To/Arduino", $"SERVO:{0}");
                listBox1.Items.Add($"[{DateTime.Now:HH:mm:ss}] [PC_To/Arduino] : A 게이트 열림");
                listBox1.TopIndex = listBox1.Items.Count - 1;

                PublishMessage("PC_To/Robot", "RESET");
                Thread.Sleep(50);
                PublishMessage("PC_To/PiCar", "RESET");
                Thread.Sleep(50);

                MessageBox.Show("시스템이 재가동되었습니다.");
            }
            else
            {
                MessageBox.Show("해당 기능은 관리자만 사용 가능합니다");
                return;
            }
        }



        // ---------- START OF MQTT -------------------

        // MQTT 브로커 연결 및 메시지 이벤트 등록
        private async void ConnectToBroker()
        {
            var factory = new MqttFactory();
            mqttClient = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("127.0.0.1", 1883)
                .WithCleanSession()
                .Build();

            mqttClient.ApplicationMessageReceivedAsync += async e =>
            {
                string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                string topic = e.ApplicationMessage.Topic;

                Invoke(() =>
                {
                    listBox1.Items.Add($"[{DateTime.Now:HH:mm:ss}][{topic}] {payload}");
                    listBox1.TopIndex = listBox1.Items.Count - 1;

                    if (topic == "ArCAR_To/PC")
                    {
                        if (int.TryParse(payload.Trim(), out int newVehicleNumber))
                        {
                            currentVehicleNumber = newVehicleNumber;
                            label_vehicle.Text = $"차량 번호 {currentVehicleNumber} 처리 중";
                            listBox1.Items.Add($"[{DateTime.Now:HH:mm:ss}] [SYSTEM] 주문번호: {currentVehicleNumber}");
                            listBox1.TopIndex = listBox1.Items.Count - 1;
                            DBHelper.LoadOrderData(dgv_order, connStr);
                            HighlightOrderRowOnly();

                            foreach (DataGridViewRow row in dgv_order.Rows)
                            {
                                if (row.Cells["주문번호"].Value != null &&
                                    int.TryParse(row.Cells["주문번호"].Value.ToString(), out int orderId) &&
                                    orderId == currentVehicleNumber)
                                {
                                    row.Selected = true;
                                    dgv_order.FirstDisplayedScrollingRowIndex = row.Index;
                                    break;
                                }
                            }

                            return; 
                        }
                    }

                    if (payload.Trim() == "READY")
                    {
                        servoBusy = false;  
                        listBox1.Items.Add($"[{DateTime.Now:HH:mm:ss}] [SYSTEM] AGV 대기중");
                        listBox1.TopIndex = listBox1.Items.Count - 1;

                        if (pendingOrderId != -1 && pendingProductId != -1)
                        {
                            IncrementProcessedQuantity(pendingOrderId, pendingProductId);
                            pendingOrderId = -1;
                            pendingProductId = -1;
                            pendingLabel = null;
                        }
                        else
                        {
                            listBox1.Items.Add($"[{DateTime.Now:HH:mm:ss}] [SYSTEM] 처리 항목이 없습니다.");
                            listBox1.TopIndex = listBox1.Items.Count - 1;
                        }
                    }
                    else if (payload.Trim() == "STOP_ACTION")
                    {
                        SystemStopAction();
                    }
                    else
                    {
                        listBox1.Items.Add($"[{DateTime.Now:HH:mm:ss}][SYSTEM] " + payload.Trim());
                        listBox1.TopIndex = listBox1.Items.Count - 1;
                    }

                });

                await Task.CompletedTask;
            };


            mqttClient.ConnectedAsync += async e =>
            {
                Invoke(() =>
                {
                    listBox1.Items.Add($"[{DateTime.Now:HH:mm:ss}][SYSTEM] MQTT 연결 성공");
                    listBox1.TopIndex = listBox1.Items.Count - 1;
                });
                await mqttClient.SubscribeAsync("Arduino_To/PC");
                await mqttClient.SubscribeAsync("PiCar_To/PC");
                await mqttClient.SubscribeAsync("ArCAR_To/PC");
            };

            mqttClient.DisconnectedAsync += async e =>
            {
                Invoke(() =>
                {
                    listBox1.Items.Add($"[{DateTime.Now:HH:mm:ss}][SYSTEM] MQTT 연결 해제");
                    listBox1.TopIndex = listBox1.Items.Count - 1;
                });

                await Task.CompletedTask;
            };

            try
            {
                await mqttClient.ConnectAsync(options);
            }
            catch (Exception ex)
            {
                Invoke(() =>
                {
                    listBox1.Items.Add($"[{DateTime.Now:HH:mm:ss}][SYSTEM] MQTT 연결 실패: " + ex.Message);
                    listBox1.TopIndex = listBox1.Items.Count - 1;
                });

            }
        }
        // 시스템 비상 정지 액션 수행 및 로그 기록
        private void SystemStopAction()
        {
            listBox1.Items.Add($"[{DateTime.Now:HH:mm:ss}][SYSTEM] 문제가 발생했습니다");
            listBox1.TopIndex = listBox1.Items.Count - 1;
            pendingOrderId = -1;
            pendingProductId = -1;
            pendingLabel = null;
            bt_reset.Enabled = true;
            string username = "system";
            string description = "물체를 잡지 못했습니다";

            var frame = cam2.CaptureCurrentFrame();  
            if (frame != null && !frame.Empty())
            {
                EmergencyHelper.InsertEmergencyLogWithImage(username, description, connStr, frame);
            }
            else
            {
                MessageBox.Show("⚠ cam2 캡처 실패 – 로그만 저장됩니다.");
                EmergencyHelper.InsertEmergencyLog(username, description, connStr);
            }

            EmergencyHelper.SetEmergencyState(true, connStr);
            EmergencyHelper.LoadEmergencyLogs(dgv_emergency, connStr);

            bt_emergency.Enabled = false;

            new warning().ShowDialog();
        }


        // MQTT 메시지 전송
        private async void PublishMessage(string topic, string message)
        {
            if (mqttClient?.IsConnected == true)
            {
                var msg = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(message)
                    .Build();

                await mqttClient.PublishAsync(msg);
            }
            else
            {
                MessageBox.Show("MQTT client not connected.");
            }
        }

        // 카메라 노출값 증가 버튼
        private void bt_light_up_Click(object sender, EventArgs e)
        {
            double current = cam1.GetExposure();
            currentExposure += 1.0;
            cam1.SetExposure(currentExposure);
            lb_light.Text = $"현재 노출도: {currentExposure}";
        }

        // 카메라 노출값 감소 버튼
        private void bt_light_down_Click(object sender, EventArgs e)
        {
            double current = cam1.GetExposure();
            currentExposure -= 1.0;
            cam1.SetExposure(currentExposure);
            lb_light.Text = $"현재 노출도: {currentExposure}";
        }
        // 프로그램 종료 버튼
        private void bt_exit_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }


        static bool main_belt_flag = false;
        // 메인벨트 동작/정지 버튼
        private void bt_main_belt_Click(object sender, EventArgs e)
        {
            main_belt_flag = !main_belt_flag;
            if (main_belt_flag)
            {
                bt_main_belt.Text = "A-1구역 동작";
                PublishMessage("PC_To/Arduino", $"MAIN:{0}");
                listBox1.Items.Add($"[{DateTime.Now:HH:mm:ss}] [PC_To/Arduino] : 메인 벨트 동작");
                listBox1.TopIndex = listBox1.Items.Count - 1;
            }
            else
            {
                bt_main_belt.Text = "A-1구역 정지";
                PublishMessage("PC_To/Arduino", $"MAIN:{255}");
                listBox1.Items.Add($"[{DateTime.Now:HH:mm:ss}] [PC_To/Arduino] : A 벨트 정지");
                listBox1.TopIndex = listBox1.Items.Count - 1;
            }
        }


        // 세컨드벨트 동작/정지 버튼
        static bool second_belt_flag = false;
        private void bt_second_belt_Click(object sender, EventArgs e)
        {
            second_belt_flag = !second_belt_flag;
            if (second_belt_flag) 
            {
                bt_second_belt.Text = "A-2구역 동작";
                PublishMessage("PC_To/Arduino", $"SECOND:{0}");
                listBox1.Items.Add($"[{DateTime.Now:HH:mm:ss}][PC_To/Arduino] : A 벨트 동작");
                listBox1.TopIndex = listBox1.Items.Count - 1;
            }
            else
            {
                bt_second_belt.Text = "A-2구역 정지";
                PublishMessage("PC_To/Arduino", $"SECOND:{255}");
                listBox1.Items.Add($"[{DateTime.Now:HH:mm:ss}][PC_To/Arduino] : A 벨트 정지");
                listBox1.TopIndex = listBox1.Items.Count - 1;
            }
        }

        // 서보모터 동작/정지 버튼
        static bool servo_flag = false;
        private void bt_servo_Click(object sender, EventArgs e)
        {
            servo_flag = !servo_flag;
            if (servo_flag) 
            {
                bt_servo.Text = "A구역 분류 동작";
                PublishMessage("PC_To/Arduino", $"SERVO:{65}");
                listBox1.Items.Add($"[{DateTime.Now:HH:mm:ss}][PC_To/Arduino] : A 게이트 닫힘");
                listBox1.TopIndex = listBox1.Items.Count - 1;
            }
            else
            {
                bt_servo.Text = "A구역 분류 정지";
                PublishMessage("PC_To/Arduino", $"SERVO:{0}");
                listBox1.Items.Add($"[{DateTime.Now:HH:mm:ss}][PC_To/Arduino] : A 게이트 열림");
                listBox1.TopIndex = listBox1.Items.Count - 1;
            }
        }



        // YOLO 객체 탐지 이벤트 처리 및 분류 명령
        private async void HandleDetection(string detectedObject)
        {

            if (servoBusy) return;
            if ((DateTime.Now - lastDetectionTime).TotalSeconds < 2) return;
            lastDetectionTime = DateTime.Now;

            string cleanedLabel = detectedObject.Trim();
            if (cleanedLabel.Contains("'"))
            {
                int start = cleanedLabel.IndexOf("'") + 1;
                int end = cleanedLabel.LastIndexOf("'");
                if (start > 0 && end > start)
                    cleanedLabel = cleanedLabel.Substring(start, end - start);
            }

            int productId = GetProductIdFromLabel(cleanedLabel);
            if (productId == -1) return;

            int matchedProductId;
            int orderId = FindEarliestVehicleOrderMatchingProduct(currentVehicleNumber, out matchedProductId);
            if (orderId == -1 || matchedProductId != productId) return;

            pendingOrderId = orderId;
            pendingProductId = productId;
            pendingLabel = cleanedLabel;

            servoBusy = true;
            listBox1.Invoke(() =>
            {
                listBox1.Items.Add($"[{DateTime.Now:HH:mm:ss}] [PC_To/Arduino] {cleanedLabel} A 게이트 닫힘");
                listBox1.TopIndex = listBox1.Items.Count - 1;
            });
            PublishMessage("PC_To/Arduino", "SERVO:65");
            await Task.Delay(5000);

            PublishMessage("PC_To/Arduino", "SERVO:0");
            listBox1.Invoke(() =>
            {
                listBox1.Items.Add($"[{DateTime.Now:HH:mm:ss}] [PC_To/Arduino] A 게이트 열림");
                listBox1.TopIndex = listBox1.Items.Count - 1;
            });


        }

        // code_name으로 product_id 조회
        private int GetProductIdFromLabel(string label)         
        {
            using var conn = new MySqlConnection(connStr);
            conn.Open();
            var cmd = new MySqlCommand("SELECT product_id FROM products WHERE code_name = @label", conn);
            cmd.Parameters.AddWithValue("@label", label);
            var result = cmd.ExecuteScalar();
            return result != null ? Convert.ToInt32(result) : -1;
        }

        // 해당 차량번호의 처리 대기중인 주문/제품 조회
        private int FindEarliestVehicleOrderMatchingProduct(int vehicleNumber, out int productId)
        {
            using var conn = new MySqlConnection(connStr);
            conn.Open();

            string query = @"
        SELECT oi.product_id, o.order_id
        FROM orders o
        JOIN order_items oi ON o.order_id = oi.order_id
        WHERE o.status = '대기중'
          AND oi.quantity > oi.Pquantity
          AND o.order_id = @vehicleNumber
        ORDER BY oi.product_id ASC, o.order_time ASC
        LIMIT 1";

            var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@vehicleNumber", vehicleNumber);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                productId = reader.GetInt32("product_id");
                return reader.GetInt32("order_id");
            }
            else
            {
                productId = -1;
                return -1;
            }
        }


        // 주문 품목 처리수량(Pquantity) 1 증가 및 상태/차량 이동 처리
        private void IncrementProcessedQuantity(int orderId, int productId)     // 처리 수량 1 증가
        {
            using var conn = new MySqlConnection(connStr);
            conn.Open();

            var cmd = new MySqlCommand(@"
        UPDATE order_items 
        SET Pquantity = Pquantity + 1
        WHERE order_id = @oid AND product_id = @pid", conn);
            cmd.Parameters.AddWithValue("@oid", orderId);
            cmd.Parameters.AddWithValue("@pid", productId);
            cmd.ExecuteNonQuery();

            var checkCmd = new MySqlCommand(@"
        SELECT COUNT(*) FROM order_items 
        WHERE order_id = @oid AND Pquantity < quantity", conn);
            checkCmd.Parameters.AddWithValue("@oid", orderId);
            int remaining = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (remaining == 0)
            {
                var updateOrder = new MySqlCommand("UPDATE orders SET status = '처리됨' WHERE order_id = @oid", conn);
                updateOrder.Parameters.AddWithValue("@oid", orderId);
                updateOrder.ExecuteNonQuery();
                label_vehicle.Text = "차량 대기 중...";

                PublishMessage("PC_To/ArCAR", "GO"); 
                listBox1.Invoke(() =>
                {
                    listBox1.Items.Add($"[{DateTime.Now:HH:mm:ss}] [PC_To/Arduino] 주문 처리 완료");
                    listBox1.TopIndex = listBox1.Items.Count - 1;
                });


                DisplayCurrentChart();  
            }
            DBHelper.LoadOrderData(dgv_order, connStr);
        }


        // 시스템 재가동(리셋) 버튼
        private void bt_reset_Click(object sender, EventArgs e)
        {
            servoBusy = false;
            bt_reset.Enabled = false;
            listBox1.Items.Add($"[{DateTime.Now:HH:mm:ss}] [SYSTEM] A 구역 재가동");
            listBox1.TopIndex = listBox1.Items.Count - 1;
           

            PublishMessage("PC_To/Arduino", $"SECOND:{0}");
            Thread.Sleep(50);
            PublishMessage("PC_To/Robot", "RESET");
            Thread.Sleep(50);
            PublishMessage("PC_To/PiCar", "RESET");
            Thread.Sleep(50);
        }






        // 선택된 제품의 연도별 누적 판매량 차트 표시
        private void DisplayCurrentChart()
        {
            if (chartData == null || productKeys.Length == 0) return;

            string product = productKeys[currentChartIndex];
            var yearData = chartData[product];
            var yearRange = Enumerable.Range(2020, 5).ToList();

            panel_chart_single.Controls.Clear();



            var series = new ColumnSeries<int>
            {
                Values = yearRange.Select(y => yearData[y]).ToList(),
                Name = product,
                Fill = new SolidColorPaint(SKColors.Orange),  
                Stroke = null                                  
            };

            var chart = new CartesianChart
            {
                Series = new ISeries[] { series },
                XAxes = new[] { new Axis { Labels = yearRange.Select(y => y.ToString()).ToArray() } },
                Dock = DockStyle.Fill
            };

            panel_chart_single.Controls.Add(chart);
            label_chart_title.Text = $"▶ {product.ToUpper()} 누적 판매량";
            LoadCompletedOrders();
        }
        // 다음 제품 차트 표시
        private void btn_next_Click(object sender, EventArgs e)
        {
            currentChartIndex = (currentChartIndex + 1) % productKeys.Length;
            DisplayCurrentChart();

        }
        // 이전 제품 차트 표시
        private void btn_prev_Click(object sender, EventArgs e)
        {
            currentChartIndex = (currentChartIndex - 1 + productKeys.Length) % productKeys.Length;
            DisplayCurrentChart();

        }
        // 메인화면 시계 갱신 타이머
        private void tm_main_Tick(object sender, EventArgs e)
        {
            lb_main_time.Text = DateTime.Now.ToString("yyyy.MM.dd\n" + "시간" + " HH:mm:ss");
        }

        // 완료된 주문 내역 불러오기 및 표시
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

                dgv_completedorders.DataSource = dt;

                dgv_completedorders.DefaultCellStyle.Font = new Font("맑은 고딕", 10);
                dgv_completedorders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgv_completedorders.RowHeadersVisible = false;
                dgv_completedorders.AllowUserToAddRows = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("처리된 주문 데이터 불러오기 실패: " + ex.Message);
            }
        }


    }
}
