namespace XSao
{
    partial class HomePage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HomePage));
            this.uiGroupBox1 = new Sunny.UI.UIGroupBox();
            this.H_User = new Sunny.UI.UIHeaderButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.H_Output = new Sunny.UI.UIHeaderButton();
            this.H_Camare = new Sunny.UI.UIHeaderButton();
            this.H_Log = new Sunny.UI.UIHeaderButton();
            this.H_IO = new Sunny.UI.UIHeaderButton();
            this.uiPanel1 = new Sunny.UI.UIPanel();
            this.uiGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.Controls.Add(this.H_User);
            this.uiGroupBox1.Controls.Add(this.pictureBox1);
            this.uiGroupBox1.Controls.Add(this.H_Output);
            this.uiGroupBox1.Controls.Add(this.H_Camare);
            this.uiGroupBox1.Controls.Add(this.H_Log);
            this.uiGroupBox1.Controls.Add(this.H_IO);
            this.uiGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.uiGroupBox1.FillColor = System.Drawing.Color.CornflowerBlue;
            this.uiGroupBox1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiGroupBox1.Location = new System.Drawing.Point(0, 35);
            this.uiGroupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiGroupBox1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiGroupBox1.Size = new System.Drawing.Size(1411, 229);
            this.uiGroupBox1.TabIndex = 0;
            this.uiGroupBox1.Text = null;
            this.uiGroupBox1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // H_User
            // 
            this.H_User.FillColor = System.Drawing.Color.CornflowerBlue;
            this.H_User.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.H_User.Image = ((System.Drawing.Image)(resources.GetObject("H_User.Image")));
            this.H_User.Location = new System.Drawing.Point(1215, 56);
            this.H_User.MinimumSize = new System.Drawing.Size(1, 1);
            this.H_User.Name = "H_User";
            this.H_User.Padding = new System.Windows.Forms.Padding(0, 8, 0, 3);
            this.H_User.Radius = 0;
            this.H_User.RadiusSides = Sunny.UI.UICornerRadiusSides.None;
            this.H_User.RectSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.None;
            this.H_User.Size = new System.Drawing.Size(135, 130);
            this.H_User.TabIndex = 3;
            this.H_User.Text = "用户管理";
            this.H_User.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.H_User.Click += new System.EventHandler(this.H_User_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.CornflowerBlue;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 32);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(308, 197);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // H_Output
            // 
            this.H_Output.FillColor = System.Drawing.Color.CornflowerBlue;
            this.H_Output.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.H_Output.Image = ((System.Drawing.Image)(resources.GetObject("H_Output.Image")));
            this.H_Output.Location = new System.Drawing.Point(1019, 56);
            this.H_Output.MinimumSize = new System.Drawing.Size(1, 1);
            this.H_Output.Name = "H_Output";
            this.H_Output.Padding = new System.Windows.Forms.Padding(0, 8, 0, 3);
            this.H_Output.Radius = 0;
            this.H_Output.RadiusSides = Sunny.UI.UICornerRadiusSides.None;
            this.H_Output.RectSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.None;
            this.H_Output.Size = new System.Drawing.Size(135, 130);
            this.H_Output.TabIndex = 3;
            this.H_Output.Text = "产量管理";
            this.H_Output.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.H_Output.Click += new System.EventHandler(this.H_Output_Click);
            // 
            // H_Camare
            // 
            this.H_Camare.FillColor = System.Drawing.Color.CornflowerBlue;
            this.H_Camare.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.H_Camare.Image = ((System.Drawing.Image)(resources.GetObject("H_Camare.Image")));
            this.H_Camare.Location = new System.Drawing.Point(809, 56);
            this.H_Camare.MinimumSize = new System.Drawing.Size(1, 1);
            this.H_Camare.Name = "H_Camare";
            this.H_Camare.Padding = new System.Windows.Forms.Padding(0, 8, 0, 3);
            this.H_Camare.Radius = 0;
            this.H_Camare.RadiusSides = Sunny.UI.UICornerRadiusSides.None;
            this.H_Camare.RectSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.None;
            this.H_Camare.Size = new System.Drawing.Size(135, 130);
            this.H_Camare.TabIndex = 3;
            this.H_Camare.Text = "相机管理";
            this.H_Camare.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.H_Camare.Click += new System.EventHandler(this.H_Camare_Click);
            // 
            // H_Log
            // 
            this.H_Log.FillColor = System.Drawing.Color.CornflowerBlue;
            this.H_Log.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.H_Log.Image = ((System.Drawing.Image)(resources.GetObject("H_Log.Image")));
            this.H_Log.Location = new System.Drawing.Point(422, 56);
            this.H_Log.MinimumSize = new System.Drawing.Size(1, 1);
            this.H_Log.Name = "H_Log";
            this.H_Log.Padding = new System.Windows.Forms.Padding(0, 8, 0, 3);
            this.H_Log.Radius = 0;
            this.H_Log.RadiusSides = Sunny.UI.UICornerRadiusSides.None;
            this.H_Log.RectSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.None;
            this.H_Log.Size = new System.Drawing.Size(135, 130);
            this.H_Log.TabIndex = 3;
            this.H_Log.Text = "日志操作";
            this.H_Log.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.H_Log.Click += new System.EventHandler(this.H_Log_Click);
            // 
            // H_IO
            // 
            this.H_IO.FillColor = System.Drawing.Color.CornflowerBlue;
            this.H_IO.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.H_IO.Image = ((System.Drawing.Image)(resources.GetObject("H_IO.Image")));
            this.H_IO.Location = new System.Drawing.Point(610, 56);
            this.H_IO.MinimumSize = new System.Drawing.Size(1, 1);
            this.H_IO.Name = "H_IO";
            this.H_IO.Padding = new System.Windows.Forms.Padding(0, 8, 0, 3);
            this.H_IO.Radius = 0;
            this.H_IO.RadiusSides = Sunny.UI.UICornerRadiusSides.None;
            this.H_IO.RectSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.None;
            this.H_IO.Size = new System.Drawing.Size(135, 130);
            this.H_IO.TabIndex = 3;
            this.H_IO.Text = "IO管理";
            this.H_IO.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.H_IO.Click += new System.EventHandler(this.H_IO_Click);
            // 
            // uiPanel1
            // 
            this.uiPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.uiPanel1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiPanel1.Location = new System.Drawing.Point(0, 742);
            this.uiPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(1411, 180);
            this.uiPanel1.TabIndex = 1;
            this.uiPanel1.Text = null;
            this.uiPanel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // HomePage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1411, 922);
            this.Controls.Add(this.uiPanel1);
            this.Controls.Add(this.uiGroupBox1);
            this.Name = "HomePage";
            this.Text = "上位机实训";
            this.ZoomScaleRect = new System.Drawing.Rectangle(22, 22, 800, 450);
            this.uiGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UIGroupBox uiGroupBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Sunny.UI.UIPanel uiPanel1;
        private Sunny.UI.UIHeaderButton H_Log;
        private Sunny.UI.UIHeaderButton H_IO;
        private Sunny.UI.UIHeaderButton H_Camare;
        private Sunny.UI.UIHeaderButton H_Output;
        private Sunny.UI.UIHeaderButton H_User;
    }
}