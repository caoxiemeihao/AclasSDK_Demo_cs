namespace AclasSDK_Demo
{
    partial class frmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblDeviceIP = new System.Windows.Forms.Label();
            this.tbDeviceIP = new System.Windows.Forms.TextBox();
            this.lblFileName = new System.Windows.Forms.Label();
            this.tbFileName = new System.Windows.Forms.TextBox();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnDownLoad = new System.Windows.Forms.Button();
            this.pbProgress = new System.Windows.Forms.ProgressBar();
            this.lblDataType = new System.Windows.Forms.Label();
            this.lblReadMe = new System.Windows.Forms.Label();
            this.lblReadMe2 = new System.Windows.Forms.Label();
            this.tbDataType = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblDeviceIP
            // 
            this.lblDeviceIP.AutoSize = true;
            this.lblDeviceIP.Location = new System.Drawing.Point(35, 40);
            this.lblDeviceIP.Name = "lblDeviceIP";
            this.lblDeviceIP.Size = new System.Drawing.Size(109, 20);
            this.lblDeviceIP.TabIndex = 0;
            this.lblDeviceIP.Text = "Device IP:";
            // 
            // tbDeviceIP
            // 
            this.tbDeviceIP.Location = new System.Drawing.Point(150, 37);
            this.tbDeviceIP.Name = "tbDeviceIP";
            this.tbDeviceIP.Size = new System.Drawing.Size(320, 30);
            this.tbDeviceIP.TabIndex = 1;
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Location = new System.Drawing.Point(35, 81);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(109, 20);
            this.lblFileName.TabIndex = 2;
            this.lblFileName.Text = "File Path:";
            // 
            // tbFileName
            // 
            this.tbFileName.Location = new System.Drawing.Point(150, 78);
            this.tbFileName.Name = "tbFileName";
            this.tbFileName.Size = new System.Drawing.Size(320, 30);
            this.tbFileName.TabIndex = 3;
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Location = new System.Drawing.Point(476, 78);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(51, 30);
            this.btnOpenFile.TabIndex = 4;
            this.btnOpenFile.Text = "...";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // OpenFileDialog
            // 
            this.OpenFileDialog.FileName = "openFileDialog";
            this.OpenFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.OpenFileDialog_FileOk);
            // 
            // btnDownLoad
            // 
            this.btnDownLoad.Location = new System.Drawing.Point(39, 214);
            this.btnDownLoad.Name = "btnDownLoad";
            this.btnDownLoad.Size = new System.Drawing.Size(488, 40);
            this.btnDownLoad.TabIndex = 5;
            this.btnDownLoad.Text = "DownLoad";
            this.btnDownLoad.UseVisualStyleBackColor = true;
            this.btnDownLoad.Click += new System.EventHandler(this.btnDownLoad_Click);
            // 
            // pbProgress
            // 
            this.pbProgress.Location = new System.Drawing.Point(39, 269);
            this.pbProgress.Name = "pbProgress";
            this.pbProgress.Size = new System.Drawing.Size(488, 25);
            this.pbProgress.TabIndex = 6;
            // 
            // lblDataType
            // 
            this.lblDataType.AutoSize = true;
            this.lblDataType.Location = new System.Drawing.Point(35, 119);
            this.lblDataType.Name = "lblDataType";
            this.lblDataType.Size = new System.Drawing.Size(109, 20);
            this.lblDataType.TabIndex = 7;
            this.lblDataType.Text = "Data Type:";
            // 
            // lblReadMe
            // 
            this.lblReadMe.AutoSize = true;
            this.lblReadMe.Location = new System.Drawing.Point(35, 152);
            this.lblReadMe.Name = "lblReadMe";
            this.lblReadMe.Size = new System.Drawing.Size(409, 20);
            this.lblReadMe.TabIndex = 8;
            this.lblReadMe.Text = "DataType 请参考ReadMe文档或者Demo Source";
            // 
            // lblReadMe2
            // 
            this.lblReadMe2.AutoSize = true;
            this.lblReadMe2.Location = new System.Drawing.Point(35, 181);
            this.lblReadMe2.Name = "lblReadMe2";
            this.lblReadMe2.Size = new System.Drawing.Size(199, 20);
            this.lblReadMe2.TabIndex = 9;
            this.lblReadMe2.Text = "默认值0000是PLU类型";
            // 
            // tbDataType
            // 
            this.tbDataType.Location = new System.Drawing.Point(150, 116);
            this.tbDataType.Name = "tbDataType";
            this.tbDataType.Size = new System.Drawing.Size(320, 30);
            this.tbDataType.TabIndex = 10;
            this.tbDataType.Text = "0000";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 311);
            this.Controls.Add(this.tbDataType);
            this.Controls.Add(this.lblReadMe2);
            this.Controls.Add(this.lblReadMe);
            this.Controls.Add(this.lblDataType);
            this.Controls.Add(this.pbProgress);
            this.Controls.Add(this.btnDownLoad);
            this.Controls.Add(this.btnOpenFile);
            this.Controls.Add(this.tbFileName);
            this.Controls.Add(this.lblFileName);
            this.Controls.Add(this.tbDeviceIP);
            this.Controls.Add(this.lblDeviceIP);
            this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "main";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDeviceIP;
        private System.Windows.Forms.TextBox tbDeviceIP;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.TextBox tbFileName;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.OpenFileDialog OpenFileDialog;
        private System.Windows.Forms.Label lblDataType;
        private System.Windows.Forms.Label lblReadMe;
        private System.Windows.Forms.Label lblReadMe2;
        private System.Windows.Forms.TextBox tbDataType;
        public System.Windows.Forms.ProgressBar pbProgress;
        public System.Windows.Forms.Button btnDownLoad;
    }
}

