namespace XSao
{
    partial class DateLog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DateLog));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.uiCheckBoxGroup1 = new Sunny.UI.UICheckBoxGroup();
            this.uiGroupBox1 = new Sunny.UI.UIGroupBox();
            this.DataGridView_Log = new Sunny.UI.UIDataGridView();
            this.CB_LogLevel = new Sunny.UI.UIComboBox();
            this.End_time = new Sunny.UI.UIDatetimePicker();
            this.Start_time = new Sunny.UI.UIDatetimePicker();
            this.But_CZ = new Sunny.UI.UIButton();
            this.But_Select = new Sunny.UI.UIButton();
            this.uiGroupBox2 = new Sunny.UI.UIGroupBox();
            this.But_UpPage = new Sunny.UI.UIButton();
            this.But_NextPage = new Sunny.UI.UIButton();
            this.But_JumpPage = new Sunny.UI.UIButton();
            this.TextBox_Page = new Sunny.UI.UITextBox();
            this.Label_Page = new Sunny.UI.UILabel();
            this.uiLabel4 = new Sunny.UI.UILabel();
            this.uiLabel3 = new Sunny.UI.UILabel();
            this.uiLabel5 = new Sunny.UI.UILabel();
            this.uiLabel2 = new Sunny.UI.UILabel();
            this.TextBox_UserID = new Sunny.UI.UITextBox();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.userName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MakeTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LogLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Facility = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Log = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uiCheckBoxGroup1.SuspendLayout();
            this.uiGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_Log)).BeginInit();
            this.uiGroupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiCheckBoxGroup1
            // 
            this.uiCheckBoxGroup1.Controls.Add(this.DataGridView_Log);
            this.uiCheckBoxGroup1.Controls.Add(this.uiGroupBox1);
            this.uiCheckBoxGroup1.Controls.Add(this.uiGroupBox2);
            this.uiCheckBoxGroup1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiCheckBoxGroup1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiCheckBoxGroup1.Location = new System.Drawing.Point(0, 35);
            this.uiCheckBoxGroup1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiCheckBoxGroup1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiCheckBoxGroup1.Name = "uiCheckBoxGroup1";
            this.uiCheckBoxGroup1.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiCheckBoxGroup1.SelectedIndexes = ((System.Collections.Generic.List<int>)(resources.GetObject("uiCheckBoxGroup1.SelectedIndexes")));
            this.uiCheckBoxGroup1.Size = new System.Drawing.Size(1411, 887);
            this.uiCheckBoxGroup1.TabIndex = 0;
            this.uiCheckBoxGroup1.Text = "日志管理";
            this.uiCheckBoxGroup1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.Controls.Add(this.CB_LogLevel);
            this.uiGroupBox1.Controls.Add(this.End_time);
            this.uiGroupBox1.Controls.Add(this.Start_time);
            this.uiGroupBox1.Controls.Add(this.But_CZ);
            this.uiGroupBox1.Controls.Add(this.But_Select);
            this.uiGroupBox1.Controls.Add(this.uiLabel3);
            this.uiGroupBox1.Controls.Add(this.uiLabel5);
            this.uiGroupBox1.Controls.Add(this.uiLabel2);
            this.uiGroupBox1.Controls.Add(this.TextBox_UserID);
            this.uiGroupBox1.Controls.Add(this.uiLabel1);
            this.uiGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.uiGroupBox1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox1.Location = new System.Drawing.Point(0, 32);
            this.uiGroupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox1.Size = new System.Drawing.Size(1411, 154);
            this.uiGroupBox1.TabIndex = 0;
            this.uiGroupBox1.Text = "查询条件";
            this.uiGroupBox1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DataGridView_Log
            // 
            this.DataGridView_Log.AllowUserToAddRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.DataGridView_Log.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.DataGridView_Log.BackgroundColor = System.Drawing.Color.White;
            this.DataGridView_Log.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataGridView_Log.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.DataGridView_Log.ColumnHeadersHeight = 32;
            this.DataGridView_Log.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.DataGridView_Log.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.userName,
            this.MakeTime,
            this.LogLevel,
            this.Facility,
            this.Log});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DataGridView_Log.DefaultCellStyle = dataGridViewCellStyle3;
            this.DataGridView_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGridView_Log.EnableHeadersVisualStyles = false;
            this.DataGridView_Log.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DataGridView_Log.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            this.DataGridView_Log.Location = new System.Drawing.Point(0, 186);
            this.DataGridView_Log.Name = "DataGridView_Log";
            this.DataGridView_Log.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataGridView_Log.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.DataGridView_Log.RowHeadersVisible = false;
            this.DataGridView_Log.RowHeadersWidth = 62;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DataGridView_Log.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.DataGridView_Log.RowTemplate.Height = 30;
            this.DataGridView_Log.SelectedIndex = -1;
            this.DataGridView_Log.Size = new System.Drawing.Size(1411, 583);
            this.DataGridView_Log.StripeOddColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.DataGridView_Log.TabIndex = 5;
            // 
            // CB_LogLevel
            // 
            this.CB_LogLevel.DataSource = null;
            this.CB_LogLevel.FillColor = System.Drawing.Color.White;
            this.CB_LogLevel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CB_LogLevel.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.CB_LogLevel.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.CB_LogLevel.Location = new System.Drawing.Point(864, 60);
            this.CB_LogLevel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CB_LogLevel.MinimumSize = new System.Drawing.Size(63, 0);
            this.CB_LogLevel.Name = "CB_LogLevel";
            this.CB_LogLevel.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.CB_LogLevel.Size = new System.Drawing.Size(161, 34);
            this.CB_LogLevel.SymbolSize = 24;
            this.CB_LogLevel.TabIndex = 4;
            this.CB_LogLevel.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.CB_LogLevel.Watermark = "";
            // 
            // End_time
            // 
            this.End_time.DateCultureInfo = new System.Globalization.CultureInfo("zh-CN");
            this.End_time.DateFormat = "yyyy-MM-dd";
            this.End_time.FillColor = System.Drawing.Color.White;
            this.End_time.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.End_time.Location = new System.Drawing.Point(515, 102);
            this.End_time.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.End_time.MaxLength = 10;
            this.End_time.MinimumSize = new System.Drawing.Size(63, 0);
            this.End_time.Name = "End_time";
            this.End_time.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.End_time.Size = new System.Drawing.Size(187, 34);
            this.End_time.SymbolDropDown = 61555;
            this.End_time.SymbolNormal = 61555;
            this.End_time.SymbolSize = 24;
            this.End_time.TabIndex = 3;
            this.End_time.Text = "2026-11-03";
            this.End_time.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.End_time.Value = new System.DateTime(2026, 11, 3, 0, 0, 0, 0);
            this.End_time.Watermark = "";
            // 
            // Start_time
            // 
            this.Start_time.DateCultureInfo = new System.Globalization.CultureInfo("zh-CN");
            this.Start_time.DateFormat = "yyyy-MM-dd";
            this.Start_time.FillColor = System.Drawing.Color.White;
            this.Start_time.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Start_time.Location = new System.Drawing.Point(515, 34);
            this.Start_time.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Start_time.MaxLength = 10;
            this.Start_time.MinimumSize = new System.Drawing.Size(63, 0);
            this.Start_time.Name = "Start_time";
            this.Start_time.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.Start_time.Size = new System.Drawing.Size(187, 34);
            this.Start_time.SymbolDropDown = 61555;
            this.Start_time.SymbolNormal = 61555;
            this.Start_time.SymbolSize = 24;
            this.Start_time.TabIndex = 3;
            this.Start_time.Text = "2026-01-03";
            this.Start_time.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.Start_time.Value = new System.DateTime(2026, 1, 3, 0, 0, 0, 0);
            this.Start_time.Watermark = "";
            // 
            // But_CZ
            // 
            this.But_CZ.Cursor = System.Windows.Forms.Cursors.Hand;
            this.But_CZ.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.But_CZ.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.But_CZ.ForeColor = System.Drawing.Color.Black;
            this.But_CZ.Location = new System.Drawing.Point(1213, 60);
            this.But_CZ.MinimumSize = new System.Drawing.Size(1, 1);
            this.But_CZ.Name = "But_CZ";
            this.But_CZ.Size = new System.Drawing.Size(115, 49);
            this.But_CZ.TabIndex = 2;
            this.But_CZ.Text = "重置";
            this.But_CZ.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.But_CZ.Click += new System.EventHandler(this.But_CZ_Click);
            // 
            // But_Select
            // 
            this.But_Select.Cursor = System.Windows.Forms.Cursors.Hand;
            this.But_Select.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.But_Select.Location = new System.Drawing.Point(1070, 60);
            this.But_Select.MinimumSize = new System.Drawing.Size(1, 1);
            this.But_Select.Name = "But_Select";
            this.But_Select.Size = new System.Drawing.Size(115, 49);
            this.But_Select.TabIndex = 2;
            this.But_Select.Text = "查询";
            this.But_Select.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.But_Select.Click += new System.EventHandler(this.But_Select_Click);
            // 
            // uiGroupBox2
            // 
            this.uiGroupBox2.Controls.Add(this.But_UpPage);
            this.uiGroupBox2.Controls.Add(this.But_NextPage);
            this.uiGroupBox2.Controls.Add(this.uiLabel4);
            this.uiGroupBox2.Controls.Add(this.Label_Page);
            this.uiGroupBox2.Controls.Add(this.But_JumpPage);
            this.uiGroupBox2.Controls.Add(this.TextBox_Page);
            this.uiGroupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.uiGroupBox2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox2.Location = new System.Drawing.Point(0, 769);
            this.uiGroupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox2.Name = "uiGroupBox2";
            this.uiGroupBox2.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox2.Size = new System.Drawing.Size(1411, 118);
            this.uiGroupBox2.TabIndex = 2;
            this.uiGroupBox2.Text = null;
            this.uiGroupBox2.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // But_UpPage
            // 
            this.But_UpPage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.But_UpPage.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.But_UpPage.Location = new System.Drawing.Point(455, 52);
            this.But_UpPage.MinimumSize = new System.Drawing.Size(1, 1);
            this.But_UpPage.Name = "But_UpPage";
            this.But_UpPage.Size = new System.Drawing.Size(110, 38);
            this.But_UpPage.TabIndex = 2;
            this.But_UpPage.Text = "上一页";
            this.But_UpPage.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.But_UpPage.Click += new System.EventHandler(this.But_UpPage_Click);
            // 
            // But_NextPage
            // 
            this.But_NextPage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.But_NextPage.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.But_NextPage.Location = new System.Drawing.Point(636, 52);
            this.But_NextPage.MinimumSize = new System.Drawing.Size(1, 1);
            this.But_NextPage.Name = "But_NextPage";
            this.But_NextPage.Size = new System.Drawing.Size(110, 38);
            this.But_NextPage.TabIndex = 2;
            this.But_NextPage.Text = "下一页";
            this.But_NextPage.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.But_NextPage.Click += new System.EventHandler(this.But_NextPage_Click);
            // 
            // But_JumpPage
            // 
            this.But_JumpPage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.But_JumpPage.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.But_JumpPage.Location = new System.Drawing.Point(1201, 52);
            this.But_JumpPage.MinimumSize = new System.Drawing.Size(1, 1);
            this.But_JumpPage.Name = "But_JumpPage";
            this.But_JumpPage.Size = new System.Drawing.Size(110, 38);
            this.But_JumpPage.TabIndex = 2;
            this.But_JumpPage.Text = "页面跳转";
            this.But_JumpPage.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.But_JumpPage.Click += new System.EventHandler(this.But_JumpPage_Click);
            // 
            // TextBox_Page
            // 
            this.TextBox_Page.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.TextBox_Page.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TextBox_Page.Location = new System.Drawing.Point(1047, 44);
            this.TextBox_Page.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TextBox_Page.MinimumSize = new System.Drawing.Size(1, 16);
            this.TextBox_Page.Name = "TextBox_Page";
            this.TextBox_Page.Padding = new System.Windows.Forms.Padding(5);
            this.TextBox_Page.ShowText = false;
            this.TextBox_Page.Size = new System.Drawing.Size(126, 57);
            this.TextBox_Page.TabIndex = 1;
            this.TextBox_Page.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.TextBox_Page.Watermark = "";
            // 
            // Label_Page
            // 
            this.Label_Page.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_Page.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.Label_Page.Location = new System.Drawing.Point(92, 56);
            this.Label_Page.Name = "Label_Page";
            this.Label_Page.Size = new System.Drawing.Size(341, 34);
            this.Label_Page.TabIndex = 0;
            this.Label_Page.Text = "第：X页/共：Y页";
            // 
            // uiLabel4
            // 
            this.uiLabel4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel4.Location = new System.Drawing.Point(881, 62);
            this.uiLabel4.Name = "uiLabel4";
            this.uiLabel4.Size = new System.Drawing.Size(159, 34);
            this.uiLabel4.TabIndex = 0;
            this.uiLabel4.Text = "请输入页码：";
            // 
            // uiLabel3
            // 
            this.uiLabel3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel3.Location = new System.Drawing.Point(734, 60);
            this.uiLabel3.Name = "uiLabel3";
            this.uiLabel3.Size = new System.Drawing.Size(132, 34);
            this.uiLabel3.TabIndex = 0;
            this.uiLabel3.Text = "日志等级：";
            // 
            // uiLabel5
            // 
            this.uiLabel5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel5.Location = new System.Drawing.Point(376, 100);
            this.uiLabel5.Name = "uiLabel5";
            this.uiLabel5.Size = new System.Drawing.Size(132, 34);
            this.uiLabel5.TabIndex = 0;
            this.uiLabel5.Text = "结束日期：";
            // 
            // uiLabel2
            // 
            this.uiLabel2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel2.Location = new System.Drawing.Point(376, 32);
            this.uiLabel2.Name = "uiLabel2";
            this.uiLabel2.Size = new System.Drawing.Size(132, 34);
            this.uiLabel2.TabIndex = 0;
            this.uiLabel2.Text = "开始日期：";
            // 
            // TextBox_UserID
            // 
            this.TextBox_UserID.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.TextBox_UserID.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TextBox_UserID.Location = new System.Drawing.Point(152, 60);
            this.TextBox_UserID.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TextBox_UserID.MinimumSize = new System.Drawing.Size(1, 16);
            this.TextBox_UserID.Name = "TextBox_UserID";
            this.TextBox_UserID.Padding = new System.Windows.Forms.Padding(5);
            this.TextBox_UserID.ShowText = false;
            this.TextBox_UserID.Size = new System.Drawing.Size(184, 34);
            this.TextBox_UserID.TabIndex = 1;
            this.TextBox_UserID.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.TextBox_UserID.Watermark = "";
            // 
            // uiLabel1
            // 
            this.uiLabel1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel1.Location = new System.Drawing.Point(24, 60);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(132, 34);
            this.uiLabel1.TabIndex = 0;
            this.uiLabel1.Text = "用户账户：";
            // 
            // userName
            // 
            this.userName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.userName.DataPropertyName = "UserName";
            this.userName.FillWeight = 30F;
            this.userName.HeaderText = "用户名称";
            this.userName.MinimumWidth = 8;
            this.userName.Name = "userName";
            // 
            // MakeTime
            // 
            this.MakeTime.DataPropertyName = "CreateTime";
            this.MakeTime.HeaderText = "操作时间";
            this.MakeTime.MinimumWidth = 8;
            this.MakeTime.Name = "MakeTime";
            this.MakeTime.Width = 150;
            // 
            // LogLevel
            // 
            this.LogLevel.DataPropertyName = "LogLevel";
            this.LogLevel.HeaderText = "日志等级";
            this.LogLevel.MinimumWidth = 8;
            this.LogLevel.Name = "LogLevel";
            this.LogLevel.Width = 150;
            // 
            // Facility
            // 
            this.Facility.DataPropertyName = "SourceDevice";
            this.Facility.HeaderText = "来源设备";
            this.Facility.MinimumWidth = 8;
            this.Facility.Name = "Facility";
            this.Facility.Width = 150;
            // 
            // Log
            // 
            this.Log.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Log.DataPropertyName = "LogContent";
            this.Log.HeaderText = "操作内容";
            this.Log.MinimumWidth = 8;
            this.Log.Name = "Log";
            // 
            // DateLog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1420, 700);
            this.Controls.Add(this.uiCheckBoxGroup1);
            this.Name = "DateLog";
            this.Text = "DateLog";
            this.ZoomScaleRect = new System.Drawing.Rectangle(22, 22, 800, 450);
            this.uiCheckBoxGroup1.ResumeLayout(false);
            this.uiGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_Log)).EndInit();
            this.uiGroupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UICheckBoxGroup uiCheckBoxGroup1;
        private Sunny.UI.UIGroupBox uiGroupBox1;
        private Sunny.UI.UILabel uiLabel3;
        private Sunny.UI.UILabel uiLabel2;
        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UIGroupBox uiGroupBox2;
        private Sunny.UI.UITextBox TextBox_Page;
        private Sunny.UI.UILabel uiLabel4;
        private Sunny.UI.UIButton But_JumpPage;
        private Sunny.UI.UIButton But_UpPage;
        private Sunny.UI.UIButton But_NextPage;
        private Sunny.UI.UILabel Label_Page;
        private Sunny.UI.UITextBox TextBox_UserID;
        private Sunny.UI.UIDatetimePicker Start_time;
        private Sunny.UI.UIComboBox CB_LogLevel;
        private Sunny.UI.UIButton But_CZ;
        private Sunny.UI.UIButton But_Select;
        private Sunny.UI.UIDataGridView DataGridView_Log;
        private Sunny.UI.UIDatetimePicker End_time;
        private Sunny.UI.UILabel uiLabel5;
        private System.Windows.Forms.DataGridViewTextBoxColumn userName;
        private System.Windows.Forms.DataGridViewTextBoxColumn MakeTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn LogLevel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Facility;
        private System.Windows.Forms.DataGridViewTextBoxColumn Log;
    }
}