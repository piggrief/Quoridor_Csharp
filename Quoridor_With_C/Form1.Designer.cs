namespace Quoridor_With_C
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.ChessBoardPB = new CCWin.SkinControl.SkinPictureBox();
            this.PlaceBoardBTN = new CCWin.SkinControl.SkinButton();
            this.MoveBTN = new CCWin.SkinControl.SkinButton();
            this.TestTB = new CCWin.SkinControl.SkinTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.ChessBoardPB)).BeginInit();
            this.SuspendLayout();
            // 
            // ChessBoardPB
            // 
            this.ChessBoardPB.BackColor = System.Drawing.Color.Transparent;
            this.ChessBoardPB.Location = new System.Drawing.Point(1, 42);
            this.ChessBoardPB.Name = "ChessBoardPB";
            this.ChessBoardPB.Size = new System.Drawing.Size(630, 630);
            this.ChessBoardPB.TabIndex = 0;
            this.ChessBoardPB.TabStop = false;
            this.ChessBoardPB.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ChessBoardPB_MouseClick);
            // 
            // PlaceBoardBTN
            // 
            this.PlaceBoardBTN.BackColor = System.Drawing.Color.Transparent;
            this.PlaceBoardBTN.BaseColor = System.Drawing.Color.AliceBlue;
            this.PlaceBoardBTN.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.PlaceBoardBTN.DownBack = null;
            this.PlaceBoardBTN.Font = new System.Drawing.Font("微软雅黑", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PlaceBoardBTN.Location = new System.Drawing.Point(898, 119);
            this.PlaceBoardBTN.MouseBack = null;
            this.PlaceBoardBTN.Name = "PlaceBoardBTN";
            this.PlaceBoardBTN.NormlBack = null;
            this.PlaceBoardBTN.Size = new System.Drawing.Size(194, 54);
            this.PlaceBoardBTN.TabIndex = 1;
            this.PlaceBoardBTN.Text = "放置挡板";
            this.PlaceBoardBTN.UseVisualStyleBackColor = false;
            this.PlaceBoardBTN.Click += new System.EventHandler(this.PlaceBoardBTN_Click);
            // 
            // MoveBTN
            // 
            this.MoveBTN.BackColor = System.Drawing.Color.Transparent;
            this.MoveBTN.BaseColor = System.Drawing.Color.AliceBlue;
            this.MoveBTN.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.MoveBTN.DownBack = null;
            this.MoveBTN.Font = new System.Drawing.Font("微软雅黑", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MoveBTN.Location = new System.Drawing.Point(898, 227);
            this.MoveBTN.MouseBack = null;
            this.MoveBTN.Name = "MoveBTN";
            this.MoveBTN.NormlBack = null;
            this.MoveBTN.Size = new System.Drawing.Size(194, 54);
            this.MoveBTN.TabIndex = 2;
            this.MoveBTN.Text = "移动棋子";
            this.MoveBTN.UseVisualStyleBackColor = false;
            this.MoveBTN.Click += new System.EventHandler(this.MoveBTN_Click);
            // 
            // TestTB
            // 
            this.TestTB.BackColor = System.Drawing.Color.Transparent;
            this.TestTB.DownBack = null;
            this.TestTB.Icon = null;
            this.TestTB.IconIsButton = false;
            this.TestTB.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.TestTB.IsPasswordChat = '\0';
            this.TestTB.IsSystemPasswordChar = false;
            this.TestTB.Lines = new string[0];
            this.TestTB.Location = new System.Drawing.Point(874, 446);
            this.TestTB.Margin = new System.Windows.Forms.Padding(0);
            this.TestTB.MaxLength = 32767;
            this.TestTB.MinimumSize = new System.Drawing.Size(28, 28);
            this.TestTB.MouseBack = null;
            this.TestTB.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.TestTB.Multiline = true;
            this.TestTB.Name = "TestTB";
            this.TestTB.NormlBack = null;
            this.TestTB.Padding = new System.Windows.Forms.Padding(5);
            this.TestTB.ReadOnly = false;
            this.TestTB.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.TestTB.Size = new System.Drawing.Size(250, 209);
            // 
            // 
            // 
            this.TestTB.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TestTB.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TestTB.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.TestTB.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.TestTB.SkinTxt.Multiline = true;
            this.TestTB.SkinTxt.Name = "BaseText";
            this.TestTB.SkinTxt.Size = new System.Drawing.Size(240, 199);
            this.TestTB.SkinTxt.TabIndex = 0;
            this.TestTB.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.TestTB.SkinTxt.WaterText = "";
            this.TestTB.TabIndex = 3;
            this.TestTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.TestTB.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.TestTB.WaterText = "";
            this.TestTB.WordWrap = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 800);
            this.Controls.Add(this.TestTB);
            this.Controls.Add(this.MoveBTN);
            this.Controls.Add(this.PlaceBoardBTN);
            this.Controls.Add(this.ChessBoardPB);
            this.Name = "Form1";
            this.Radius = 35;
            this.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.Text = "步步为营游戏仿真环境";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
            ((System.ComponentModel.ISupportInitialize)(this.ChessBoardPB)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public CCWin.SkinControl.SkinPictureBox ChessBoardPB;
        private CCWin.SkinControl.SkinButton PlaceBoardBTN;
        private CCWin.SkinControl.SkinButton MoveBTN;
        private CCWin.SkinControl.SkinTextBox TestTB;
    }
}

