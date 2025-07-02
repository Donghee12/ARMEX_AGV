namespace _ARMEX
{
    partial class Main
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            tab = new TabControl();
            tabPage1 = new TabPage();
            panel4 = new Panel();
            label2 = new Label();
            label_vehicle = new Label();
            dgv_order = new DataGridView();
            panel1 = new Panel();
            lb_video = new Label();
            pb_video1 = new PictureBox();
            tabPage2 = new TabPage();
            dgv_emergency = new DataGridView();
            tabPage3 = new TabPage();
            label1 = new Label();
            label3 = new Label();
            dgv_completedorders = new DataGridView();
            btn_next = new Button();
            btn_prev = new Button();
            label_chart_title = new Label();
            panel_chart_single = new Panel();
            tabPage4 = new TabPage();
            panel2 = new Panel();
            panel3 = new Panel();
            lb_light = new Label();
            bt_light_up = new Button();
            bt_light_down = new Button();
            lb_sub_camera = new Label();
            pb_video3 = new PictureBox();
            lb_main_camera = new Label();
            pb_video2 = new PictureBox();
            lb_communication_log = new Label();
            bt_exit = new Button();
            bt_reset = new Button();
            listBox1 = new ListBox();
            bt_servo = new Button();
            bt_second_belt = new Button();
            bt_main_belt = new Button();
            bt_restart = new Button();
            bt_emergency = new Button();
            bt_logout = new Button();
            tm_main = new System.Windows.Forms.Timer(components);
            lb_main_time = new Label();
            pictureBox1 = new PictureBox();
            tab.SuspendLayout();
            tabPage1.SuspendLayout();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgv_order).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pb_video1).BeginInit();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgv_emergency).BeginInit();
            tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgv_completedorders).BeginInit();
            tabPage4.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pb_video3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pb_video2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // tab
            // 
            tab.Controls.Add(tabPage1);
            tab.Controls.Add(tabPage2);
            tab.Controls.Add(tabPage3);
            tab.Controls.Add(tabPage4);
            tab.Font = new Font("맑은 고딕", 12F, FontStyle.Regular, GraphicsUnit.Point, 129);
            tab.Location = new Point(0, 100);
            tab.Name = "tab";
            tab.SelectedIndex = 0;
            tab.Size = new Size(1264, 585);
            tab.TabIndex = 0;
            tab.Tag = "전체 카메라 영상 예정";
            // 
            // tabPage1
            // 
            tabPage1.BackColor = Color.FromArgb(50, 50, 50);
            tabPage1.Controls.Add(panel4);
            tabPage1.Controls.Add(panel1);
            tabPage1.ForeColor = SystemColors.ControlText;
            tabPage1.Location = new Point(4, 30);
            tabPage1.Margin = new Padding(0);
            tabPage1.Name = "tabPage1";
            tabPage1.Size = new Size(1256, 551);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "작업 현황 보기";
            // 
            // panel4
            // 
            panel4.BorderStyle = BorderStyle.FixedSingle;
            panel4.Controls.Add(label2);
            panel4.Controls.Add(label_vehicle);
            panel4.Controls.Add(dgv_order);
            panel4.Location = new Point(646, 14);
            panel4.Name = "panel4";
            panel4.Size = new Size(585, 485);
            panel4.TabIndex = 17;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("맑은 고딕", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
            label2.ForeColor = Color.White;
            label2.Location = new Point(-4, 10);
            label2.Name = "label2";
            label2.Size = new Size(256, 30);
            label2.TabIndex = 17;
            label2.Text = "▶　현재 주문 처리 내역 :";
            // 
            // label_vehicle
            // 
            label_vehicle.AutoSize = true;
            label_vehicle.Font = new Font("맑은 고딕", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
            label_vehicle.ForeColor = Color.White;
            label_vehicle.Location = new Point(253, 10);
            label_vehicle.Name = "label_vehicle";
            label_vehicle.Size = new Size(91, 30);
            label_vehicle.TabIndex = 16;
            label_vehicle.Text = "대기중...";
            // 
            // dgv_order
            // 
            dgv_order.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv_order.Location = new Point(-1, 47);
            dgv_order.Name = "dgv_order";
            dgv_order.Size = new Size(585, 438);
            dgv_order.TabIndex = 6;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Transparent;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(lb_video);
            panel1.Controls.Add(pb_video1);
            panel1.Location = new Point(27, 14);
            panel1.Name = "panel1";
            panel1.Size = new Size(585, 485);
            panel1.TabIndex = 10;
            // 
            // lb_video
            // 
            lb_video.AutoSize = true;
            lb_video.Font = new Font("맑은 고딕", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 129);
            lb_video.ForeColor = Color.Transparent;
            lb_video.Location = new Point(-3, 10);
            lb_video.Name = "lb_video";
            lb_video.Size = new Size(184, 25);
            lb_video.TabIndex = 1;
            lb_video.Text = "▶ A구역 관제 CAM";
            // 
            // pb_video1
            // 
            pb_video1.Location = new Point(2, 47);
            pb_video1.Name = "pb_video1";
            pb_video1.Size = new Size(580, 435);
            pb_video1.SizeMode = PictureBoxSizeMode.StretchImage;
            pb_video1.TabIndex = 0;
            pb_video1.TabStop = false;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(dgv_emergency);
            tabPage2.Font = new Font("맑은 고딕", 12F, FontStyle.Regular, GraphicsUnit.Point, 129);
            tabPage2.Location = new Point(4, 30);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1256, 551);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "이벤트 로그";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgv_emergency
            // 
            dgv_emergency.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv_emergency.Dock = DockStyle.Fill;
            dgv_emergency.Location = new Point(3, 3);
            dgv_emergency.Name = "dgv_emergency";
            dgv_emergency.Size = new Size(1250, 545);
            dgv_emergency.TabIndex = 0;
            dgv_emergency.CellDoubleClick += dgv_emergency_CellDoubleClick;
            // 
            // tabPage3
            // 
            tabPage3.BackColor = Color.FromArgb(50, 50, 50);
            tabPage3.Controls.Add(label1);
            tabPage3.Controls.Add(label3);
            tabPage3.Controls.Add(dgv_completedorders);
            tabPage3.Controls.Add(btn_next);
            tabPage3.Controls.Add(btn_prev);
            tabPage3.Controls.Add(label_chart_title);
            tabPage3.Controls.Add(panel_chart_single);
            tabPage3.Location = new Point(4, 30);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(1256, 551);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "처리/통계 내역";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("맑은 고딕", 12F, FontStyle.Bold, GraphicsUnit.Point, 129);
            label1.ForeColor = Color.White;
            label1.Location = new Point(321, 441);
            label1.Name = "label1";
            label1.Size = new Size(93, 21);
            label1.TabIndex = 18;
            label1.Text = "수량 / 년도";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("맑은 고딕", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
            label3.ForeColor = Color.White;
            label3.Location = new Point(463, 11);
            label3.Name = "label3";
            label3.Size = new Size(181, 30);
            label3.TabIndex = 17;
            label3.Text = "▶ 누적 처리 기록";
            // 
            // dgv_completedorders
            // 
            dgv_completedorders.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv_completedorders.Location = new Point(466, 53);
            dgv_completedorders.Name = "dgv_completedorders";
            dgv_completedorders.Size = new Size(773, 460);
            dgv_completedorders.TabIndex = 16;
            // 
            // btn_next
            // 
            btn_next.BackColor = Color.Gray;
            btn_next.FlatAppearance.BorderSize = 0;
            btn_next.FlatStyle = FlatStyle.Flat;
            btn_next.ForeColor = Color.White;
            btn_next.Location = new Point(305, 476);
            btn_next.Name = "btn_next";
            btn_next.Size = new Size(109, 38);
            btn_next.TabIndex = 0;
            btn_next.Text = "▶";
            btn_next.UseVisualStyleBackColor = false;
            btn_next.Click += btn_next_Click;
            // 
            // btn_prev
            // 
            btn_prev.BackColor = Color.Gray;
            btn_prev.FlatAppearance.BorderSize = 0;
            btn_prev.FlatStyle = FlatStyle.Flat;
            btn_prev.ForeColor = Color.White;
            btn_prev.Location = new Point(35, 476);
            btn_prev.Name = "btn_prev";
            btn_prev.Size = new Size(109, 38);
            btn_prev.TabIndex = 15;
            btn_prev.Text = "◀";
            btn_prev.UseVisualStyleBackColor = false;
            btn_prev.Click += btn_prev_Click;
            // 
            // label_chart_title
            // 
            label_chart_title.AutoSize = true;
            label_chart_title.Font = new Font("맑은 고딕", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
            label_chart_title.ForeColor = Color.White;
            label_chart_title.Location = new Point(31, 11);
            label_chart_title.Name = "label_chart_title";
            label_chart_title.Size = new Size(202, 30);
            label_chart_title.TabIndex = 12;
            label_chart_title.Text = "▶ 누적 의자 판매량";
            // 
            // panel_chart_single
            // 
            panel_chart_single.BackColor = Color.White;
            panel_chart_single.ForeColor = Color.White;
            panel_chart_single.Location = new Point(34, 53);
            panel_chart_single.Name = "panel_chart_single";
            panel_chart_single.Size = new Size(380, 385);
            panel_chart_single.TabIndex = 10;
            // 
            // tabPage4
            // 
            tabPage4.BackColor = Color.FromArgb(50, 50, 50);
            tabPage4.Controls.Add(panel2);
            tabPage4.Controls.Add(lb_communication_log);
            tabPage4.Controls.Add(bt_exit);
            tabPage4.Controls.Add(bt_reset);
            tabPage4.Controls.Add(listBox1);
            tabPage4.Controls.Add(bt_servo);
            tabPage4.Controls.Add(bt_second_belt);
            tabPage4.Controls.Add(bt_main_belt);
            tabPage4.Location = new Point(4, 30);
            tabPage4.Name = "tabPage4";
            tabPage4.Size = new Size(1256, 551);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "시스템 점검";
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(50, 50, 50);
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(panel3);
            panel2.Controls.Add(lb_sub_camera);
            panel2.Controls.Add(pb_video3);
            panel2.Controls.Add(lb_main_camera);
            panel2.Controls.Add(pb_video2);
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(630, 551);
            panel2.TabIndex = 16;
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(50, 50, 50);
            panel3.Controls.Add(lb_light);
            panel3.Controls.Add(bt_light_up);
            panel3.Controls.Add(bt_light_down);
            panel3.Location = new Point(482, 2);
            panel3.Name = "panel3";
            panel3.Size = new Size(147, 267);
            panel3.TabIndex = 16;
            // 
            // lb_light
            // 
            lb_light.AutoSize = true;
            lb_light.Font = new Font("맑은 고딕", 12F, FontStyle.Bold, GraphicsUnit.Point, 129);
            lb_light.ForeColor = Color.White;
            lb_light.Location = new Point(13, 124);
            lb_light.Name = "lb_light";
            lb_light.Size = new Size(58, 21);
            lb_light.TabIndex = 15;
            lb_light.Text = "노출도";
            // 
            // bt_light_up
            // 
            bt_light_up.FlatAppearance.BorderSize = 0;
            bt_light_up.FlatStyle = FlatStyle.Flat;
            bt_light_up.Font = new Font("맑은 고딕", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 129);
            bt_light_up.ForeColor = Color.White;
            bt_light_up.Location = new Point(24, 58);
            bt_light_up.Name = "bt_light_up";
            bt_light_up.Size = new Size(100, 50);
            bt_light_up.TabIndex = 0;
            bt_light_up.Text = "▲";
            bt_light_up.UseVisualStyleBackColor = true;
            bt_light_up.Click += bt_light_up_Click;
            // 
            // bt_light_down
            // 
            bt_light_down.FlatAppearance.BorderSize = 0;
            bt_light_down.FlatStyle = FlatStyle.Flat;
            bt_light_down.Font = new Font("맑은 고딕", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
            bt_light_down.ForeColor = Color.White;
            bt_light_down.Location = new Point(24, 155);
            bt_light_down.Name = "bt_light_down";
            bt_light_down.Size = new Size(100, 50);
            bt_light_down.TabIndex = 1;
            bt_light_down.Text = "▼";
            bt_light_down.UseVisualStyleBackColor = true;
            bt_light_down.Click += bt_light_down_Click;
            // 
            // lb_sub_camera
            // 
            lb_sub_camera.AutoSize = true;
            lb_sub_camera.BackColor = Color.Transparent;
            lb_sub_camera.Font = new Font("맑은 고딕", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
            lb_sub_camera.ForeColor = Color.White;
            lb_sub_camera.Location = new Point(-1, 2);
            lb_sub_camera.Name = "lb_sub_camera";
            lb_sub_camera.Size = new Size(202, 30);
            lb_sub_camera.TabIndex = 13;
            lb_sub_camera.Text = "▶ A구역 관제 CAM";
            // 
            // pb_video3
            // 
            pb_video3.BackColor = Color.White;
            pb_video3.Location = new Point(4, 2);
            pb_video3.Name = "pb_video3";
            pb_video3.Size = new Size(475, 267);
            pb_video3.SizeMode = PictureBoxSizeMode.StretchImage;
            pb_video3.TabIndex = 2;
            pb_video3.TabStop = false;
            // 
            // lb_main_camera
            // 
            lb_main_camera.AutoSize = true;
            lb_main_camera.BackColor = Color.Transparent;
            lb_main_camera.Font = new Font("맑은 고딕", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
            lb_main_camera.ForeColor = Color.White;
            lb_main_camera.Location = new Point(-1, 272);
            lb_main_camera.Name = "lb_main_camera";
            lb_main_camera.Size = new Size(153, 30);
            lb_main_camera.TabIndex = 14;
            lb_main_camera.Text = "▶ A구역 CAM";
            // 
            // pb_video2
            // 
            pb_video2.BackColor = Color.White;
            pb_video2.Location = new Point(4, 272);
            pb_video2.Name = "pb_video2";
            pb_video2.Size = new Size(475, 267);
            pb_video2.SizeMode = PictureBoxSizeMode.StretchImage;
            pb_video2.TabIndex = 5;
            pb_video2.TabStop = false;
            // 
            // lb_communication_log
            // 
            lb_communication_log.AutoSize = true;
            lb_communication_log.Font = new Font("맑은 고딕", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
            lb_communication_log.ForeColor = Color.White;
            lb_communication_log.Location = new Point(628, 7);
            lb_communication_log.Name = "lb_communication_log";
            lb_communication_log.Size = new Size(170, 30);
            lb_communication_log.TabIndex = 12;
            lb_communication_log.Text = "▶ SYSTEM 로그";
            // 
            // bt_exit
            // 
            bt_exit.BackColor = Color.Transparent;
            bt_exit.FlatStyle = FlatStyle.Flat;
            bt_exit.Font = new Font("맑은 고딕", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
            bt_exit.ForeColor = Color.White;
            bt_exit.Location = new Point(1066, 468);
            bt_exit.Name = "bt_exit";
            bt_exit.Size = new Size(179, 39);
            bt_exit.TabIndex = 11;
            bt_exit.Text = "종료";
            bt_exit.UseVisualStyleBackColor = false;
            bt_exit.Click += bt_exit_Click;
            // 
            // bt_reset
            // 
            bt_reset.BackColor = Color.Transparent;
            bt_reset.FlatStyle = FlatStyle.Flat;
            bt_reset.Font = new Font("맑은 고딕", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
            bt_reset.ForeColor = Color.White;
            bt_reset.Location = new Point(1066, 287);
            bt_reset.Name = "bt_reset";
            bt_reset.Size = new Size(180, 70);
            bt_reset.TabIndex = 7;
            bt_reset.Text = "A 구역 재가동";
            bt_reset.UseVisualStyleBackColor = false;
            bt_reset.Click += bt_reset_Click;
            // 
            // listBox1
            // 
            listBox1.BackColor = Color.FromArgb(50, 50, 50);
            listBox1.Font = new Font("맑은 고딕", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 129);
            listBox1.ForeColor = Color.White;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 20;
            listBox1.Location = new Point(635, 44);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(425, 464);
            listBox1.TabIndex = 6;
            // 
            // bt_servo
            // 
            bt_servo.BackColor = Color.Transparent;
            bt_servo.FlatStyle = FlatStyle.Flat;
            bt_servo.Font = new Font("맑은 고딕", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
            bt_servo.ForeColor = Color.White;
            bt_servo.Location = new Point(1066, 206);
            bt_servo.Name = "bt_servo";
            bt_servo.Size = new Size(180, 70);
            bt_servo.TabIndex = 2;
            bt_servo.Text = "A 게이트 작동";
            bt_servo.UseVisualStyleBackColor = false;
            bt_servo.Click += bt_servo_Click;
            // 
            // bt_second_belt
            // 
            bt_second_belt.BackColor = Color.Transparent;
            bt_second_belt.FlatStyle = FlatStyle.Flat;
            bt_second_belt.Font = new Font("맑은 고딕", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
            bt_second_belt.ForeColor = Color.White;
            bt_second_belt.Location = new Point(1066, 125);
            bt_second_belt.Name = "bt_second_belt";
            bt_second_belt.Size = new Size(180, 70);
            bt_second_belt.TabIndex = 1;
            bt_second_belt.Text = "A 벨트 작동";
            bt_second_belt.UseVisualStyleBackColor = false;
            bt_second_belt.Click += bt_second_belt_Click;
            // 
            // bt_main_belt
            // 
            bt_main_belt.BackColor = Color.Transparent;
            bt_main_belt.FlatStyle = FlatStyle.Flat;
            bt_main_belt.Font = new Font("맑은 고딕", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
            bt_main_belt.ForeColor = Color.White;
            bt_main_belt.Location = new Point(1066, 44);
            bt_main_belt.Name = "bt_main_belt";
            bt_main_belt.Size = new Size(180, 70);
            bt_main_belt.TabIndex = 0;
            bt_main_belt.Text = "메인 벨트 작동";
            bt_main_belt.UseVisualStyleBackColor = false;
            bt_main_belt.Click += bt_main_belt_Click;
            // 
            // bt_restart
            // 
            bt_restart.BackColor = Color.FromArgb(255, 128, 0);
            bt_restart.FlatStyle = FlatStyle.Flat;
            bt_restart.Font = new Font("맑은 고딕", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
            bt_restart.ForeColor = Color.FromArgb(50, 50, 50);
            bt_restart.Location = new Point(907, 17);
            bt_restart.Name = "bt_restart";
            bt_restart.Size = new Size(157, 72);
            bt_restart.TabIndex = 4;
            bt_restart.Text = "시스템 가동";
            bt_restart.UseVisualStyleBackColor = false;
            bt_restart.Click += bt_restart_Click;
            // 
            // bt_emergency
            // 
            bt_emergency.BackColor = Color.FromArgb(255, 128, 0);
            bt_emergency.FlatAppearance.BorderSize = 0;
            bt_emergency.FlatStyle = FlatStyle.Flat;
            bt_emergency.Font = new Font("맑은 고딕", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 129);
            bt_emergency.ForeColor = Color.FromArgb(50, 50, 50);
            bt_emergency.Location = new Point(1090, 17);
            bt_emergency.Name = "bt_emergency";
            bt_emergency.Size = new Size(153, 33);
            bt_emergency.TabIndex = 10;
            bt_emergency.Text = "긴급 정지";
            bt_emergency.UseVisualStyleBackColor = false;
            bt_emergency.Click += bt_emergency_Click;
            // 
            // bt_logout
            // 
            bt_logout.BackColor = Color.FromArgb(255, 128, 0);
            bt_logout.FlatAppearance.BorderSize = 0;
            bt_logout.FlatStyle = FlatStyle.Flat;
            bt_logout.Font = new Font("맑은 고딕", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 129);
            bt_logout.ForeColor = Color.FromArgb(50, 50, 50);
            bt_logout.Location = new Point(1090, 56);
            bt_logout.Name = "bt_logout";
            bt_logout.Size = new Size(153, 33);
            bt_logout.TabIndex = 5;
            bt_logout.Text = "로그아웃";
            bt_logout.UseVisualStyleBackColor = false;
            bt_logout.Click += bt_logout_Click;
            // 
            // tm_main
            // 
            tm_main.Enabled = true;
            tm_main.Interval = 1000;
            tm_main.Tick += tm_main_Tick;
            // 
            // lb_main_time
            // 
            lb_main_time.AutoSize = true;
            lb_main_time.BackColor = Color.FromArgb(50, 50, 50);
            lb_main_time.Font = new Font("맑은 고딕", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 129);
            lb_main_time.ForeColor = Color.White;
            lb_main_time.Location = new Point(700, 15);
            lb_main_time.Name = "lb_main_time";
            lb_main_time.Size = new Size(190, 74);
            lb_main_time.TabIndex = 13;
            lb_main_time.Text = "2000.00.00\r\n시간 00:00:00";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(8, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(220, 95);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 14;
            pictureBox1.TabStop = false;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(50, 50, 50);
            ClientSize = new Size(1264, 685);
            ControlBox = false;
            Controls.Add(pictureBox1);
            Controls.Add(lb_main_time);
            Controls.Add(tab);
            Controls.Add(bt_emergency);
            Controls.Add(bt_logout);
            Controls.Add(bt_restart);
            Name = "Main";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Main";
            FormClosing += Main_FormClosing;
            Load += Main_Load;
            tab.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgv_order).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pb_video1).EndInit();
            tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgv_emergency).EndInit();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgv_completedorders).EndInit();
            tabPage4.ResumeLayout(false);
            tabPage4.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pb_video3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pb_video2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TabControl tab;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private PictureBox pb_video1;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private Label lb_video;
        private Button bt_logout;
        private DataGridView dgv_order;
        private Button bt_emergency;
        private DataGridView dgv_emergency;
        private PictureBox pb_video2;
        private Button bt_restart;
        private Button bt_servo;
        private Button bt_second_belt;
        private Button bt_main_belt;
        private Panel panel1;
        private Button bt_light_down;
        private Button bt_light_up;
        private PictureBox pb_video3;
        private Button bt_exit;
        private ListBox listBox1;
        private Button bt_reset;
        private Panel panel_chart_single;
        private Label lb_main_camera;
        private Label lb_sub_camera;
        private Label lb_communication_log;
        private Label lb_light;
        private Label label_chart_title;
        private Label label_vehicle;
        private Panel panel2;
        private Panel panel3;
        private Panel panel4;
        private Label label2;
        private Button btn_next;
        private Button btn_prev;
        private System.Windows.Forms.Timer tm_main;
        private Label lb_main_time;
        private DataGridView dgv_completedorders;
        private Label label3;
        private PictureBox pictureBox1;
        private Label label1;
    }
}