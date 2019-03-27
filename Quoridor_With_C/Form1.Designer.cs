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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.ChessBoardPB = new CCWin.SkinControl.SkinPictureBox();
            this.PlaceVerticalBoardBTN = new CCWin.SkinControl.SkinButton();
            this.TestTB = new CCWin.SkinControl.SkinTextBox();
            this.ChessWhitePB = new CCWin.SkinControl.SkinPictureBox();
            this.ChessBlackPB = new CCWin.SkinControl.SkinPictureBox();
            this.PlaceHorizontalBoardBTN = new CCWin.SkinControl.SkinButton();
            ((System.ComponentModel.ISupportInitialize)(this.ChessBoardPB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChessWhitePB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChessBlackPB)).BeginInit();
            this.SuspendLayout();
            // 
            // ChessBoardPB
            // 
            this.ChessBoardPB.BackColor = System.Drawing.Color.Transparent;
            this.ChessBoardPB.Image = ((System.Drawing.Image)(resources.GetObject("ChessBoardPB.Image")));
            this.ChessBoardPB.Location = new System.Drawing.Point(11, 33);
            this.ChessBoardPB.Name = "ChessBoardPB";
            this.ChessBoardPB.Size = new System.Drawing.Size(630, 630);
            this.ChessBoardPB.TabIndex = 0;
            this.ChessBoardPB.TabStop = false;
            this.ChessBoardPB.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ChessBoardPB_MouseClick);
            this.ChessBoardPB.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ChessBoardPB_MouseDown);
            // 
            // PlaceVerticalBoardBTN
            // 
            this.PlaceVerticalBoardBTN.BackColor = System.Drawing.Color.Transparent;
            this.PlaceVerticalBoardBTN.BaseColor = System.Drawing.Color.AliceBlue;
            this.PlaceVerticalBoardBTN.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.PlaceVerticalBoardBTN.DownBack = null;
            this.PlaceVerticalBoardBTN.Font = new System.Drawing.Font("微软雅黑", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PlaceVerticalBoardBTN.Location = new System.Drawing.Point(898, 119);
            this.PlaceVerticalBoardBTN.MouseBack = null;
            this.PlaceVerticalBoardBTN.Name = "PlaceVerticalBoardBTN";
            this.PlaceVerticalBoardBTN.NormlBack = null;
            this.PlaceVerticalBoardBTN.Size = new System.Drawing.Size(194, 54);
            this.PlaceVerticalBoardBTN.TabIndex = 1;
            this.PlaceVerticalBoardBTN.Text = "放置竖挡板";
            this.PlaceVerticalBoardBTN.UseVisualStyleBackColor = false;
            this.PlaceVerticalBoardBTN.Click += new System.EventHandler(this.PlaceBoardBTN_Click);
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
            // ChessWhitePB
            // 
            this.ChessWhitePB.BackColor = System.Drawing.Color.Transparent;
            this.ChessWhitePB.Image = global::Quoridor_With_C.Resource1.ChessWhite;
            this.ChessWhitePB.Location = new System.Drawing.Point(297, 66);
            this.ChessWhitePB.Name = "ChessWhitePB";
            this.ChessWhitePB.Size = new System.Drawing.Size(58, 58);
            this.ChessWhitePB.TabIndex = 4;
            this.ChessWhitePB.TabStop = false;
            this.ChessWhitePB.Click += new System.EventHandler(this.ChessWhitePB_Click);
            // 
            // ChessBlackPB
            // 
            this.ChessBlackPB.BackColor = System.Drawing.Color.Transparent;
            this.ChessBlackPB.Image = global::Quoridor_With_C.Resource1.ChessBlack;
            this.ChessBlackPB.Location = new System.Drawing.Point(297, 569);
            this.ChessBlackPB.Name = "ChessBlackPB";
            this.ChessBlackPB.Size = new System.Drawing.Size(58, 58);
            this.ChessBlackPB.TabIndex = 5;
            this.ChessBlackPB.TabStop = false;
            // 
            // PlaceHorizontalBoardBTN
            // 
            this.PlaceHorizontalBoardBTN.BackColor = System.Drawing.Color.Transparent;
            this.PlaceHorizontalBoardBTN.BaseColor = System.Drawing.Color.AliceBlue;
            this.PlaceHorizontalBoardBTN.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.PlaceHorizontalBoardBTN.DownBack = null;
            this.PlaceHorizontalBoardBTN.Font = new System.Drawing.Font("微软雅黑", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PlaceHorizontalBoardBTN.Location = new System.Drawing.Point(898, 190);
            this.PlaceHorizontalBoardBTN.MouseBack = null;
            this.PlaceHorizontalBoardBTN.Name = "PlaceHorizontalBoardBTN";
            this.PlaceHorizontalBoardBTN.NormlBack = null;
            this.PlaceHorizontalBoardBTN.Size = new System.Drawing.Size(194, 54);
            this.PlaceHorizontalBoardBTN.TabIndex = 6;
            this.PlaceHorizontalBoardBTN.Text = "放置横挡板";
            this.PlaceHorizontalBoardBTN.UseVisualStyleBackColor = false;
            this.PlaceHorizontalBoardBTN.Click += new System.EventHandler(this.PlaceHorizontalBoardBTN_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 800);
            this.Controls.Add(this.PlaceHorizontalBoardBTN);
            this.Controls.Add(this.ChessBlackPB);
            this.Controls.Add(this.ChessWhitePB);
            this.Controls.Add(this.TestTB);
            this.Controls.Add(this.PlaceVerticalBoardBTN);
            this.Controls.Add(this.ChessBoardPB);
            this.Name = "Form1";
            this.Radius = 35;
            this.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.Text = "步步为营游戏仿真环境";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.ChessBoardPB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChessWhitePB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChessBlackPB)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public CCWin.SkinControl.SkinPictureBox ChessBoardPB;
        private CCWin.SkinControl.SkinButton PlaceVerticalBoardBTN;
        private CCWin.SkinControl.SkinTextBox TestTB;
        private CCWin.SkinControl.SkinPictureBox ChessWhitePB;
        private CCWin.SkinControl.SkinPictureBox ChessBlackPB;
        private CCWin.SkinControl.SkinButton PlaceHorizontalBoardBTN;
    }
}

