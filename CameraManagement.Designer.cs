namespace XSao
{
    partial class CameraManagement
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.uiGroupBox1 = new Sunny.UI.UIGroupBox();
            this.uiGroupBox4 = new Sunny.UI.UIGroupBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.uiGroupBox3 = new Sunny.UI.UIGroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.uiGroupBox2 = new Sunny.UI.UIGroupBox();
            this.Camera_Log = new Sunny.UI.UIDataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LogTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LogMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uiGroupBox5 = new Sunny.UI.UIGroupBox();
            this.TextBox_Deep = new Sunny.UI.UITextBox();
            this.TextBox_Top = new Sunny.UI.UITextBox();
            this.uiLabel2 = new Sunny.UI.UILabel();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.But_HalconRun = new Sunny.UI.UIButton();
            this.But_CloseGet = new Sunny.UI.UIButton();
            this.But_GetPhoto = new Sunny.UI.UIButton();
            this.But_ChoicePhoton = new Sunny.UI.UIButton();
            this.But_OpenCam = new Sunny.UI.UIButton();
            this.But_RunTake = new Sunny.UI.UIButton();
            this.uiGroupBox1.SuspendLayout();
            this.uiGroupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.uiGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.uiGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Camera_Log)).BeginInit();
            this.uiGroupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.Controls.Add(this.uiGroupBox4);
            this.uiGroupBox1.Controls.Add(this.uiGroupBox3);
            this.uiGroupBox1.Controls.Add(this.uiGroupBox2);
            this.uiGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox1.Location = new System.Drawing.Point(2, 36);
            this.uiGroupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox1.Size = new System.Drawing.Size(1416, 662);
            this.uiGroupBox1.TabIndex = 0;
            this.uiGroupBox1.Text = "相机管理";
            this.uiGroupBox1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiGroupBox4
            // 
            this.uiGroupBox4.Controls.Add(this.pictureBox2);
            this.uiGroupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox4.Location = new System.Drawing.Point(0, 356);
            this.uiGroupBox4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox4.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox4.Name = "uiGroupBox4";
            this.uiGroupBox4.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox4.Size = new System.Drawing.Size(565, 306);
            this.uiGroupBox4.TabIndex = 2;
            this.uiGroupBox4.Text = "作业图像";
            this.uiGroupBox4.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Gray;
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Location = new System.Drawing.Point(0, 32);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(565, 274);
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            // 
            // uiGroupBox3
            // 
            this.uiGroupBox3.Controls.Add(this.pictureBox1);
            this.uiGroupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.uiGroupBox3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox3.Location = new System.Drawing.Point(0, 32);
            this.uiGroupBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox3.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox3.Name = "uiGroupBox3";
            this.uiGroupBox3.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox3.Size = new System.Drawing.Size(565, 324);
            this.uiGroupBox3.TabIndex = 1;
            this.uiGroupBox3.Text = "实时图像";
            this.uiGroupBox3.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Gray;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 32);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(565, 292);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // uiGroupBox2
            // 
            this.uiGroupBox2.Controls.Add(this.Camera_Log);
            this.uiGroupBox2.Controls.Add(this.uiGroupBox5);
            this.uiGroupBox2.Dock = System.Windows.Forms.DockStyle.Right;
            this.uiGroupBox2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox2.Location = new System.Drawing.Point(565, 32);
            this.uiGroupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox2.Name = "uiGroupBox2";
            this.uiGroupBox2.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox2.Size = new System.Drawing.Size(851, 630);
            this.uiGroupBox2.TabIndex = 0;
            this.uiGroupBox2.Text = "相机操作";
            this.uiGroupBox2.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Camera_Log
            // 
            this.Camera_Log.AllowUserToAddRows = false;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.Camera_Log.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.Camera_Log.BackgroundColor = System.Drawing.Color.White;
            this.Camera_Log.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Camera_Log.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.Camera_Log.ColumnHeadersHeight = 32;
            this.Camera_Log.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.Camera_Log.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.LogTime,
            this.LogMessage});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Camera_Log.DefaultCellStyle = dataGridViewCellStyle8;
            this.Camera_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Camera_Log.EnableHeadersVisualStyles = false;
            this.Camera_Log.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Camera_Log.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            this.Camera_Log.Location = new System.Drawing.Point(0, 152);
            this.Camera_Log.Name = "Camera_Log";
            this.Camera_Log.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle9.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Camera_Log.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.Camera_Log.RowHeadersVisible = false;
            this.Camera_Log.RowHeadersWidth = 62;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Camera_Log.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.Camera_Log.RowTemplate.Height = 30;
            this.Camera_Log.SelectedIndex = -1;
            this.Camera_Log.Size = new System.Drawing.Size(851, 478);
            this.Camera_Log.StripeOddColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.Camera_Log.TabIndex = 1;
            // 
            // id
            // 
            this.id.HeaderText = "...";
            this.id.MinimumWidth = 8;
            this.id.Name = "id";
            this.id.Width = 150;
            // 
            // LogTime
            // 
            this.LogTime.DataPropertyName = "MakeTime";
            this.LogTime.HeaderText = "日志时间";
            this.LogTime.MinimumWidth = 8;
            this.LogTime.Name = "LogTime";
            this.LogTime.Width = 150;
            // 
            // LogMessage
            // 
            this.LogMessage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.LogMessage.DataPropertyName = "MakeLog";
            this.LogMessage.HeaderText = "日志信息";
            this.LogMessage.MinimumWidth = 8;
            this.LogMessage.Name = "LogMessage";
            // 
            // uiGroupBox5
            // 
            this.uiGroupBox5.Controls.Add(this.TextBox_Deep);
            this.uiGroupBox5.Controls.Add(this.TextBox_Top);
            this.uiGroupBox5.Controls.Add(this.uiLabel2);
            this.uiGroupBox5.Controls.Add(this.uiLabel1);
            this.uiGroupBox5.Controls.Add(this.But_HalconRun);
            this.uiGroupBox5.Controls.Add(this.But_CloseGet);
            this.uiGroupBox5.Controls.Add(this.But_GetPhoto);
            this.uiGroupBox5.Controls.Add(this.But_ChoicePhoton);
            this.uiGroupBox5.Controls.Add(this.But_OpenCam);
            this.uiGroupBox5.Controls.Add(this.But_RunTake);
            this.uiGroupBox5.Dock = System.Windows.Forms.DockStyle.Top;
            this.uiGroupBox5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox5.Location = new System.Drawing.Point(0, 32);
            this.uiGroupBox5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox5.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox5.Name = "uiGroupBox5";
            this.uiGroupBox5.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox5.Size = new System.Drawing.Size(851, 160);
            this.uiGroupBox5.TabIndex = 0;
            this.uiGroupBox5.Text = null;
            this.uiGroupBox5.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBox_Deep
            // 
            this.TextBox_Deep.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.TextBox_Deep.DoubleValue = 200D;
            this.TextBox_Deep.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TextBox_Deep.IntValue = 200;
            this.TextBox_Deep.Location = new System.Drawing.Point(387, 76);
            this.TextBox_Deep.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TextBox_Deep.MinimumSize = new System.Drawing.Size(1, 16);
            this.TextBox_Deep.Name = "TextBox_Deep";
            this.TextBox_Deep.Padding = new System.Windows.Forms.Padding(5);
            this.TextBox_Deep.ShowText = false;
            this.TextBox_Deep.Size = new System.Drawing.Size(118, 39);
            this.TextBox_Deep.TabIndex = 2;
            this.TextBox_Deep.Text = "200";
            this.TextBox_Deep.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.TextBox_Deep.Watermark = "";
            // 
            // TextBox_Top
            // 
            this.TextBox_Top.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.TextBox_Top.DoubleValue = 255D;
            this.TextBox_Top.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TextBox_Top.IntValue = 255;
            this.TextBox_Top.Location = new System.Drawing.Point(387, 24);
            this.TextBox_Top.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TextBox_Top.MinimumSize = new System.Drawing.Size(1, 16);
            this.TextBox_Top.Name = "TextBox_Top";
            this.TextBox_Top.Padding = new System.Windows.Forms.Padding(5);
            this.TextBox_Top.ShowText = false;
            this.TextBox_Top.Size = new System.Drawing.Size(118, 41);
            this.TextBox_Top.TabIndex = 2;
            this.TextBox_Top.Text = "255";
            this.TextBox_Top.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.TextBox_Top.Watermark = "";
            // 
            // uiLabel2
            // 
            this.uiLabel2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel2.Location = new System.Drawing.Point(308, 81);
            this.uiLabel2.Name = "uiLabel2";
            this.uiLabel2.Size = new System.Drawing.Size(89, 27);
            this.uiLabel2.TabIndex = 1;
            this.uiLabel2.Text = "下限：";
            // 
            // uiLabel1
            // 
            this.uiLabel1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel1.Location = new System.Drawing.Point(308, 26);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(89, 27);
            this.uiLabel1.TabIndex = 1;
            this.uiLabel1.Text = "上限：";
            // 
            // But_HalconRun
            // 
            this.But_HalconRun.Cursor = System.Windows.Forms.Cursors.Hand;
            this.But_HalconRun.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.But_HalconRun.Location = new System.Drawing.Point(706, 41);
            this.But_HalconRun.MinimumSize = new System.Drawing.Size(1, 1);
            this.But_HalconRun.Name = "But_HalconRun";
            this.But_HalconRun.Size = new System.Drawing.Size(127, 54);
            this.But_HalconRun.TabIndex = 0;
            this.But_HalconRun.Text = "HalconRun";
            this.But_HalconRun.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.But_HalconRun.Click += new System.EventHandler(this.But_HalconRun_Click);
            // 
            // But_CloseGet
            // 
            this.But_CloseGet.Cursor = System.Windows.Forms.Cursors.Hand;
            this.But_CloseGet.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.But_CloseGet.Location = new System.Drawing.Point(512, 74);
            this.But_CloseGet.MinimumSize = new System.Drawing.Size(1, 1);
            this.But_CloseGet.Name = "But_CloseGet";
            this.But_CloseGet.Size = new System.Drawing.Size(177, 41);
            this.But_CloseGet.TabIndex = 0;
            this.But_CloseGet.Text = "关闭采集";
            this.But_CloseGet.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.But_CloseGet.Click += new System.EventHandler(this.But_CloseGet_Click);
            // 
            // But_GetPhoto
            // 
            this.But_GetPhoto.Cursor = System.Windows.Forms.Cursors.Hand;
            this.But_GetPhoto.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.But_GetPhoto.Location = new System.Drawing.Point(512, 24);
            this.But_GetPhoto.MinimumSize = new System.Drawing.Size(1, 1);
            this.But_GetPhoto.Name = "But_GetPhoto";
            this.But_GetPhoto.Size = new System.Drawing.Size(177, 41);
            this.But_GetPhoto.TabIndex = 0;
            this.But_GetPhoto.Text = "获取当前图片";
            this.But_GetPhoto.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.But_GetPhoto.Click += new System.EventHandler(this.But_GetPhoto_Click);
            // 
            // But_ChoicePhoton
            // 
            this.But_ChoicePhoton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.But_ChoicePhoton.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.But_ChoicePhoton.Location = new System.Drawing.Point(160, 48);
            this.But_ChoicePhoton.MinimumSize = new System.Drawing.Size(1, 1);
            this.But_ChoicePhoton.Name = "But_ChoicePhoton";
            this.But_ChoicePhoton.Size = new System.Drawing.Size(142, 47);
            this.But_ChoicePhoton.TabIndex = 0;
            this.But_ChoicePhoton.Text = "选择照片";
            this.But_ChoicePhoton.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.But_ChoicePhoton.Click += new System.EventHandler(this.But_ChoicePhoton_Click);
            // 
            // But_OpenCam
            // 
            this.But_OpenCam.Cursor = System.Windows.Forms.Cursors.Hand;
            this.But_OpenCam.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.But_OpenCam.Location = new System.Drawing.Point(6, 48);
            this.But_OpenCam.MinimumSize = new System.Drawing.Size(1, 1);
            this.But_OpenCam.Name = "But_OpenCam";
            this.But_OpenCam.Size = new System.Drawing.Size(142, 47);
            this.But_OpenCam.TabIndex = 0;
            this.But_OpenCam.Text = "打开相机";
            this.But_OpenCam.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.But_OpenCam.Click += new System.EventHandler(this.But_OpenCam_Click);
            // 
            // But_RunTake
            // 
            this.But_RunTake.Cursor = System.Windows.Forms.Cursors.Hand;
            this.But_RunTake.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.But_RunTake.Location = new System.Drawing.Point(160, 104);
            this.But_RunTake.MinimumSize = new System.Drawing.Size(1, 1);
            this.But_RunTake.Name = "But_RunTake";
            this.But_RunTake.Size = new System.Drawing.Size(142, 47);
            this.But_RunTake.TabIndex = 0;
            this.But_RunTake.Text = "启动一次";
            this.But_RunTake.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.But_RunTake.Click += new System.EventHandler(this.Run_Take_Click);
            // 
            // CameraManagement
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1420, 700);
            this.Controls.Add(this.uiGroupBox1);
            this.Name = "CameraManagement";
            this.Padding = new System.Windows.Forms.Padding(2, 36, 2, 2);
            this.Resizable = true;
            this.Text = "CameraManagement";
            this.ZoomScaleRect = new System.Drawing.Rectangle(22, 22, 800, 450);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CameraManagement_FormClosing);
            this.uiGroupBox1.ResumeLayout(false);
            this.uiGroupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.uiGroupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.uiGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Camera_Log)).EndInit();
            this.uiGroupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UIGroupBox uiGroupBox1;
        private Sunny.UI.UIGroupBox uiGroupBox2;
        private Sunny.UI.UIGroupBox uiGroupBox4;
        private Sunny.UI.UIGroupBox uiGroupBox3;
        private Sunny.UI.UIGroupBox uiGroupBox5;
        private Sunny.UI.UIButton But_ChoicePhoton;
        private Sunny.UI.UIButton But_OpenCam;
        private Sunny.UI.UIButton But_RunTake;
        private Sunny.UI.UILabel uiLabel2;
        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UIButton But_GetPhoto;
        private Sunny.UI.UITextBox TextBox_Deep;
        private Sunny.UI.UITextBox TextBox_Top;
        private Sunny.UI.UIDataGridView Camera_Log;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Sunny.UI.UIButton But_CloseGet;
        private Sunny.UI.UIButton But_HalconRun;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn LogTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn LogMessage;
    }
}