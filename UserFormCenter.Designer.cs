namespace XSao
{
    partial class UserFormCenter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserFormCenter));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.uiGroupBox1 = new Sunny.UI.UIGroupBox();
            this.GroupBox3 = new Sunny.UI.UIGroupBox();
            this.Refresh = new Sunny.UI.UIButton();
            this.But_DelUser = new Sunny.UI.UIButton();
            this.But_EditUser = new Sunny.UI.UIButton();
            this.But_AddUser = new Sunny.UI.UIButton();
            this.uiGroupBox2 = new Sunny.UI.UIGroupBox();
            this.TB_PassWord = new Sunny.UI.UITextBox();
            this.TB_UserName = new Sunny.UI.UITextBox();
            this.uiLabel2 = new Sunny.UI.UILabel();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.uiCheckBoxGroup1 = new Sunny.UI.UICheckBoxGroup();
            this.DataGridView_User = new Sunny.UI.UIDataGridView();
            this.Choices = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.PassWord = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AccountNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IOManage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CameraManage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LogManage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserManage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CountManage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uiGroupBox1.SuspendLayout();
            this.GroupBox3.SuspendLayout();
            this.uiGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.uiCheckBoxGroup1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_User)).BeginInit();
            this.SuspendLayout();
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.Controls.Add(this.GroupBox3);
            this.uiGroupBox1.Controls.Add(this.uiGroupBox2);
            this.uiGroupBox1.Controls.Add(this.uiCheckBoxGroup1);
            this.uiGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox1.Location = new System.Drawing.Point(0, 35);
            this.uiGroupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox1.Size = new System.Drawing.Size(1411, 887);
            this.uiGroupBox1.TabIndex = 0;
            this.uiGroupBox1.Text = "用户管理";
            this.uiGroupBox1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // GroupBox3
            // 
            this.GroupBox3.Controls.Add(this.Refresh);
            this.GroupBox3.Controls.Add(this.But_DelUser);
            this.GroupBox3.Controls.Add(this.But_EditUser);
            this.GroupBox3.Controls.Add(this.But_AddUser);
            this.GroupBox3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.GroupBox3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.GroupBox3.Location = new System.Drawing.Point(0, 469);
            this.GroupBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.GroupBox3.MinimumSize = new System.Drawing.Size(1, 1);
            this.GroupBox3.Name = "GroupBox3";
            this.GroupBox3.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.GroupBox3.Size = new System.Drawing.Size(415, 283);
            this.GroupBox3.TabIndex = 2;
            this.GroupBox3.Text = "用户操作";
            this.GroupBox3.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Refresh
            // 
            this.Refresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Refresh.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Refresh.Location = new System.Drawing.Point(42, 216);
            this.Refresh.MinimumSize = new System.Drawing.Size(1, 1);
            this.Refresh.Name = "Refresh";
            this.Refresh.Size = new System.Drawing.Size(322, 54);
            this.Refresh.TabIndex = 0;
            this.Refresh.Text = "刷新";
            this.Refresh.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Refresh.Click += new System.EventHandler(this.Refresh_Click);
            // 
            // But_DelUser
            // 
            this.But_DelUser.Cursor = System.Windows.Forms.Cursors.Hand;
            this.But_DelUser.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.But_DelUser.Location = new System.Drawing.Point(42, 156);
            this.But_DelUser.MinimumSize = new System.Drawing.Size(1, 1);
            this.But_DelUser.Name = "But_DelUser";
            this.But_DelUser.Size = new System.Drawing.Size(322, 54);
            this.But_DelUser.TabIndex = 0;
            this.But_DelUser.Text = "删除用户";
            this.But_DelUser.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.But_DelUser.Click += new System.EventHandler(this.But_DelUser_Click);
            // 
            // But_EditUser
            // 
            this.But_EditUser.Cursor = System.Windows.Forms.Cursors.Hand;
            this.But_EditUser.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.But_EditUser.Location = new System.Drawing.Point(42, 96);
            this.But_EditUser.MinimumSize = new System.Drawing.Size(1, 1);
            this.But_EditUser.Name = "But_EditUser";
            this.But_EditUser.Size = new System.Drawing.Size(322, 54);
            this.But_EditUser.TabIndex = 0;
            this.But_EditUser.Text = "修改用户";
            this.But_EditUser.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.But_EditUser.Click += new System.EventHandler(this.But_EditUser_Click);
            // 
            // But_AddUser
            // 
            this.But_AddUser.Cursor = System.Windows.Forms.Cursors.Hand;
            this.But_AddUser.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.But_AddUser.Location = new System.Drawing.Point(42, 36);
            this.But_AddUser.MinimumSize = new System.Drawing.Size(1, 1);
            this.But_AddUser.Name = "But_AddUser";
            this.But_AddUser.Size = new System.Drawing.Size(322, 54);
            this.But_AddUser.TabIndex = 0;
            this.But_AddUser.Text = "添加用户";
            this.But_AddUser.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.But_AddUser.Click += new System.EventHandler(this.But_AddUser_Click);
            // 
            // uiGroupBox2
            // 
            this.uiGroupBox2.Controls.Add(this.TB_PassWord);
            this.uiGroupBox2.Controls.Add(this.TB_UserName);
            this.uiGroupBox2.Controls.Add(this.uiLabel2);
            this.uiGroupBox2.Controls.Add(this.uiLabel1);
            this.uiGroupBox2.Controls.Add(this.pictureBox1);
            this.uiGroupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.uiGroupBox2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox2.Location = new System.Drawing.Point(0, 32);
            this.uiGroupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox2.Name = "uiGroupBox2";
            this.uiGroupBox2.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox2.Size = new System.Drawing.Size(415, 385);
            this.uiGroupBox2.TabIndex = 1;
            this.uiGroupBox2.Text = "用户信息";
            this.uiGroupBox2.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TB_PassWord
            // 
            this.TB_PassWord.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.TB_PassWord.Enabled = false;
            this.TB_PassWord.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TB_PassWord.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.TB_PassWord.Location = new System.Drawing.Point(157, 290);
            this.TB_PassWord.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TB_PassWord.MinimumSize = new System.Drawing.Size(1, 16);
            this.TB_PassWord.Name = "TB_PassWord";
            this.TB_PassWord.Padding = new System.Windows.Forms.Padding(5);
            this.TB_PassWord.PasswordChar = '*';
            this.TB_PassWord.ShowText = false;
            this.TB_PassWord.Size = new System.Drawing.Size(223, 40);
            this.TB_PassWord.TabIndex = 2;
            this.TB_PassWord.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.TB_PassWord.Watermark = "";
            // 
            // TB_UserName
            // 
            this.TB_UserName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.TB_UserName.Enabled = false;
            this.TB_UserName.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TB_UserName.Location = new System.Drawing.Point(157, 215);
            this.TB_UserName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TB_UserName.MinimumSize = new System.Drawing.Size(1, 16);
            this.TB_UserName.Name = "TB_UserName";
            this.TB_UserName.Padding = new System.Windows.Forms.Padding(5);
            this.TB_UserName.ShowText = false;
            this.TB_UserName.Size = new System.Drawing.Size(223, 38);
            this.TB_UserName.TabIndex = 2;
            this.TB_UserName.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.TB_UserName.Watermark = "";
            // 
            // uiLabel2
            // 
            this.uiLabel2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel2.Location = new System.Drawing.Point(38, 290);
            this.uiLabel2.Name = "uiLabel2";
            this.uiLabel2.Size = new System.Drawing.Size(146, 40);
            this.uiLabel2.TabIndex = 1;
            this.uiLabel2.Text = "用户密码：";
            // 
            // uiLabel1
            // 
            this.uiLabel1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel1.Location = new System.Drawing.Point(38, 215);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(146, 38);
            this.uiLabel1.TabIndex = 1;
            this.uiLabel1.Text = "用户姓名：";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(157, 70);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(94, 91);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // uiCheckBoxGroup1
            // 
            this.uiCheckBoxGroup1.Controls.Add(this.DataGridView_User);
            this.uiCheckBoxGroup1.Dock = System.Windows.Forms.DockStyle.Right;
            this.uiCheckBoxGroup1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiCheckBoxGroup1.Location = new System.Drawing.Point(415, 32);
            this.uiCheckBoxGroup1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiCheckBoxGroup1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiCheckBoxGroup1.Name = "uiCheckBoxGroup1";
            this.uiCheckBoxGroup1.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiCheckBoxGroup1.SelectedIndexes = ((System.Collections.Generic.List<int>)(resources.GetObject("uiCheckBoxGroup1.SelectedIndexes")));
            this.uiCheckBoxGroup1.Size = new System.Drawing.Size(996, 855);
            this.uiCheckBoxGroup1.TabIndex = 0;
            this.uiCheckBoxGroup1.Text = "用户信息管理";
            this.uiCheckBoxGroup1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DataGridView_User
            // 
            this.DataGridView_User.AllowUserToAddRows = false;
            this.DataGridView_User.AllowUserToDeleteRows = false;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.DataGridView_User.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.DataGridView_User.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DataGridView_User.BackgroundColor = System.Drawing.Color.White;
            this.DataGridView_User.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataGridView_User.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.DataGridView_User.ColumnHeadersHeight = 32;
            this.DataGridView_User.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.DataGridView_User.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Choices,
            this.PassWord,
            this.MId,
            this.UId,
            this.AId,
            this.UserName,
            this.AccountNum,
            this.IOManage,
            this.CameraManage,
            this.LogManage,
            this.UserManage,
            this.CountManage});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DataGridView_User.DefaultCellStyle = dataGridViewCellStyle8;
            this.DataGridView_User.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGridView_User.EnableHeadersVisualStyles = false;
            this.DataGridView_User.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DataGridView_User.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            this.DataGridView_User.Location = new System.Drawing.Point(0, 32);
            this.DataGridView_User.Name = "DataGridView_User";
            this.DataGridView_User.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle9.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataGridView_User.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.DataGridView_User.RowHeadersVisible = false;
            this.DataGridView_User.RowHeadersWidth = 62;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DataGridView_User.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.DataGridView_User.RowTemplate.Height = 30;
            this.DataGridView_User.SelectedIndex = -1;
            this.DataGridView_User.Size = new System.Drawing.Size(996, 823);
            this.DataGridView_User.StripeOddColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.DataGridView_User.TabIndex = 0;
            // 
            // Choices
            // 
            this.Choices.HeaderText = "选择";
            this.Choices.MinimumWidth = 8;
            this.Choices.Name = "Choices";
            this.Choices.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Choices.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // PassWord
            // 
            this.PassWord.DataPropertyName = "PassWord";
            this.PassWord.HeaderText = "密码";
            this.PassWord.MinimumWidth = 8;
            this.PassWord.Name = "PassWord";
            this.PassWord.Visible = false;
            // 
            // MId
            // 
            this.MId.DataPropertyName = "MId";
            this.MId.HeaderText = "MId";
            this.MId.MinimumWidth = 8;
            this.MId.Name = "MId";
            this.MId.Visible = false;
            // 
            // UId
            // 
            this.UId.DataPropertyName = "UId";
            this.UId.HeaderText = "UId";
            this.UId.MinimumWidth = 8;
            this.UId.Name = "UId";
            this.UId.Visible = false;
            // 
            // AId
            // 
            this.AId.DataPropertyName = "AId";
            this.AId.HeaderText = "AId";
            this.AId.MinimumWidth = 8;
            this.AId.Name = "AId";
            this.AId.Visible = false;
            // 
            // UserName
            // 
            this.UserName.DataPropertyName = "UserName";
            this.UserName.HeaderText = "姓名";
            this.UserName.MinimumWidth = 8;
            this.UserName.Name = "UserName";
            // 
            // AccountNum
            // 
            this.AccountNum.DataPropertyName = "AccountNum";
            this.AccountNum.HeaderText = "账号";
            this.AccountNum.MinimumWidth = 8;
            this.AccountNum.Name = "AccountNum";
            // 
            // IOManage
            // 
            this.IOManage.DataPropertyName = "IOManage";
            this.IOManage.HeaderText = "IO管理";
            this.IOManage.MinimumWidth = 8;
            this.IOManage.Name = "IOManage";
            // 
            // CameraManage
            // 
            this.CameraManage.DataPropertyName = "CameraManage";
            this.CameraManage.HeaderText = "相机管理";
            this.CameraManage.MinimumWidth = 8;
            this.CameraManage.Name = "CameraManage";
            // 
            // LogManage
            // 
            this.LogManage.DataPropertyName = "LogManage";
            this.LogManage.HeaderText = "日志管理";
            this.LogManage.MinimumWidth = 8;
            this.LogManage.Name = "LogManage";
            // 
            // UserManage
            // 
            this.UserManage.DataPropertyName = "UserManage";
            this.UserManage.HeaderText = "用户管理";
            this.UserManage.MinimumWidth = 8;
            this.UserManage.Name = "UserManage";
            // 
            // CountManage
            // 
            this.CountManage.DataPropertyName = "CountManage";
            this.CountManage.HeaderText = "产量管理";
            this.CountManage.MinimumWidth = 8;
            this.CountManage.Name = "CountManage";
            // 
            // UserFormCenter
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1420, 700);
            this.Controls.Add(this.uiGroupBox1);
            this.Name = "UserFormCenter";
            this.Text = "用户管理界面";
            this.ZoomScaleRect = new System.Drawing.Rectangle(22, 22, 800, 450);
            this.uiGroupBox1.ResumeLayout(false);
            this.GroupBox3.ResumeLayout(false);
            this.uiGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.uiCheckBoxGroup1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_User)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UIGroupBox uiGroupBox1;
        private Sunny.UI.UICheckBoxGroup uiCheckBoxGroup1;
        private Sunny.UI.UIGroupBox GroupBox3;
        private Sunny.UI.UIGroupBox uiGroupBox2;
        private Sunny.UI.UIDataGridView DataGridView_User;
        private Sunny.UI.UIButton Refresh;
        private Sunny.UI.UIButton But_DelUser;
        private Sunny.UI.UIButton But_EditUser;
        private Sunny.UI.UIButton But_AddUser;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Sunny.UI.UILabel uiLabel2;
        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UITextBox TB_PassWord;
        private Sunny.UI.UITextBox TB_UserName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Choices;
        private System.Windows.Forms.DataGridViewTextBoxColumn PassWord;
        private System.Windows.Forms.DataGridViewTextBoxColumn MId;
        private System.Windows.Forms.DataGridViewTextBoxColumn UId;
        private System.Windows.Forms.DataGridViewTextBoxColumn AId;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn AccountNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn IOManage;
        private System.Windows.Forms.DataGridViewTextBoxColumn CameraManage;
        private System.Windows.Forms.DataGridViewTextBoxColumn LogManage;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserManage;
        private System.Windows.Forms.DataGridViewTextBoxColumn CountManage;
    }
}