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
            this.PlaceVerticalBoardBTN = new CCWin.SkinControl.SkinButton();
            this.TestTB = new CCWin.SkinControl.SkinTextBox();
            this.ChessWhitePB = new CCWin.SkinControl.SkinPictureBox();
            this.ChessBlackPB = new CCWin.SkinControl.SkinPictureBox();
            this.PlaceHorizontalBoardBTN = new CCWin.SkinControl.SkinButton();
            this.TestBTN = new CCWin.SkinControl.SkinButton();
            this.RandomPlaceBTN = new CCWin.SkinControl.SkinButton();
            this.CustomPlaceBTN = new CCWin.SkinControl.SkinButton();
            this.Test2BTN = new CCWin.SkinControl.SkinButton();
            this.HBoardPB = new CCWin.SkinControl.SkinPictureBox();
            this.VBoardPB = new CCWin.SkinControl.SkinPictureBox();
            this.QueenChess1PB = new CCWin.SkinControl.SkinPictureBox();
            this.QueenChess2PB = new CCWin.SkinControl.SkinPictureBox();
            this.QueenChess3PB = new CCWin.SkinControl.SkinPictureBox();
            this.QueenChess4PB = new CCWin.SkinControl.SkinPictureBox();
            this.QueenChess8PB = new CCWin.SkinControl.SkinPictureBox();
            this.QueenChess7PB = new CCWin.SkinControl.SkinPictureBox();
            this.QueenChess6PB = new CCWin.SkinControl.SkinPictureBox();
            this.QueenChess5PB = new CCWin.SkinControl.SkinPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.ChessBoardPB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChessWhitePB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChessBlackPB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HBoardPB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VBoardPB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.QueenChess1PB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.QueenChess2PB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.QueenChess3PB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.QueenChess4PB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.QueenChess8PB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.QueenChess7PB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.QueenChess6PB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.QueenChess5PB)).BeginInit();
            this.SuspendLayout();
            // 
            // ChessBoardPB
            // 
            this.ChessBoardPB.BackColor = System.Drawing.Color.Transparent;
            this.ChessBoardPB.Image = global::Quoridor_With_C.Resource1.qipan2019;
            this.ChessBoardPB.Location = new System.Drawing.Point(11, 33);
            this.ChessBoardPB.Name = "ChessBoardPB";
            this.ChessBoardPB.Size = new System.Drawing.Size(630, 630);
            this.ChessBoardPB.TabIndex = 0;
            this.ChessBoardPB.TabStop = false;
            this.ChessBoardPB.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ChessBoardPB_MouseClick);
            this.ChessBoardPB.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ChessBoardPB_MouseDown);
            this.ChessBoardPB.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ChessBoardPB_MouseMove);
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
            // TestBTN
            // 
            this.TestBTN.BackColor = System.Drawing.Color.Transparent;
            this.TestBTN.BaseColor = System.Drawing.Color.AliceBlue;
            this.TestBTN.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.TestBTN.DownBack = null;
            this.TestBTN.Font = new System.Drawing.Font("微软雅黑", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TestBTN.Location = new System.Drawing.Point(898, 262);
            this.TestBTN.MouseBack = null;
            this.TestBTN.Name = "TestBTN";
            this.TestBTN.NormlBack = null;
            this.TestBTN.Size = new System.Drawing.Size(194, 54);
            this.TestBTN.TabIndex = 7;
            this.TestBTN.Text = "测试";
            this.TestBTN.UseVisualStyleBackColor = false;
            this.TestBTN.Click += new System.EventHandler(this.TestBTN_Click);
            // 
            // RandomPlaceBTN
            // 
            this.RandomPlaceBTN.BackColor = System.Drawing.Color.Transparent;
            this.RandomPlaceBTN.BaseColor = System.Drawing.Color.AliceBlue;
            this.RandomPlaceBTN.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.RandomPlaceBTN.DownBack = null;
            this.RandomPlaceBTN.Font = new System.Drawing.Font("微软雅黑", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.RandomPlaceBTN.Location = new System.Drawing.Point(981, 119);
            this.RandomPlaceBTN.MouseBack = null;
            this.RandomPlaceBTN.Name = "RandomPlaceBTN";
            this.RandomPlaceBTN.NormlBack = null;
            this.RandomPlaceBTN.Size = new System.Drawing.Size(194, 54);
            this.RandomPlaceBTN.TabIndex = 8;
            this.RandomPlaceBTN.Text = "随机放置";
            this.RandomPlaceBTN.UseVisualStyleBackColor = false;
            // 
            // CustomPlaceBTN
            // 
            this.CustomPlaceBTN.BackColor = System.Drawing.Color.Transparent;
            this.CustomPlaceBTN.BaseColor = System.Drawing.Color.AliceBlue;
            this.CustomPlaceBTN.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.CustomPlaceBTN.DownBack = null;
            this.CustomPlaceBTN.Font = new System.Drawing.Font("微软雅黑", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CustomPlaceBTN.Location = new System.Drawing.Point(981, 190);
            this.CustomPlaceBTN.MouseBack = null;
            this.CustomPlaceBTN.Name = "CustomPlaceBTN";
            this.CustomPlaceBTN.NormlBack = null;
            this.CustomPlaceBTN.Size = new System.Drawing.Size(194, 54);
            this.CustomPlaceBTN.TabIndex = 9;
            this.CustomPlaceBTN.Text = "自由放置";
            this.CustomPlaceBTN.UseVisualStyleBackColor = false;
            this.CustomPlaceBTN.Click += new System.EventHandler(this.CustomPlaceBTN_Click);
            // 
            // Test2BTN
            // 
            this.Test2BTN.BackColor = System.Drawing.Color.Transparent;
            this.Test2BTN.BaseColor = System.Drawing.Color.AliceBlue;
            this.Test2BTN.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.Test2BTN.DownBack = null;
            this.Test2BTN.Font = new System.Drawing.Font("微软雅黑", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Test2BTN.Location = new System.Drawing.Point(981, 262);
            this.Test2BTN.MouseBack = null;
            this.Test2BTN.Name = "Test2BTN";
            this.Test2BTN.NormlBack = null;
            this.Test2BTN.Size = new System.Drawing.Size(194, 54);
            this.Test2BTN.TabIndex = 10;
            this.Test2BTN.Text = "测试";
            this.Test2BTN.UseVisualStyleBackColor = false;
            // 
            // HBoardPB
            // 
            this.HBoardPB.BackColor = System.Drawing.Color.Red;
            this.HBoardPB.Location = new System.Drawing.Point(640, 728);
            this.HBoardPB.Name = "HBoardPB";
            this.HBoardPB.Size = new System.Drawing.Size(167, 10);
            this.HBoardPB.TabIndex = 11;
            this.HBoardPB.TabStop = false;
            this.HBoardPB.Visible = false;
            // 
            // VBoardPB
            // 
            this.VBoardPB.BackColor = System.Drawing.Color.Red;
            this.VBoardPB.Location = new System.Drawing.Point(821, 518);
            this.VBoardPB.Name = "VBoardPB";
            this.VBoardPB.Size = new System.Drawing.Size(10, 169);
            this.VBoardPB.TabIndex = 12;
            this.VBoardPB.TabStop = false;
            this.VBoardPB.Visible = false;
            this.VBoardPB.Click += new System.EventHandler(this.VBoardPB_Click);
            // 
            // QueenChess1PB
            // 
            this.QueenChess1PB.BackColor = System.Drawing.Color.Transparent;
            this.QueenChess1PB.Image = global::Quoridor_With_C.Resource1.ChessWhite;
            this.QueenChess1PB.Location = new System.Drawing.Point(808, 348);
            this.QueenChess1PB.Name = "QueenChess1PB";
            this.QueenChess1PB.Size = new System.Drawing.Size(58, 58);
            this.QueenChess1PB.TabIndex = 13;
            this.QueenChess1PB.TabStop = false;
            // 
            // QueenChess2PB
            // 
            this.QueenChess2PB.BackColor = System.Drawing.Color.Transparent;
            this.QueenChess2PB.Image = global::Quoridor_With_C.Resource1.ChessWhite;
            this.QueenChess2PB.Location = new System.Drawing.Point(808, 348);
            this.QueenChess2PB.Name = "QueenChess2PB";
            this.QueenChess2PB.Size = new System.Drawing.Size(58, 58);
            this.QueenChess2PB.TabIndex = 14;
            this.QueenChess2PB.TabStop = false;
            // 
            // QueenChess3PB
            // 
            this.QueenChess3PB.BackColor = System.Drawing.Color.Transparent;
            this.QueenChess3PB.Image = global::Quoridor_With_C.Resource1.ChessWhite;
            this.QueenChess3PB.Location = new System.Drawing.Point(808, 348);
            this.QueenChess3PB.Name = "QueenChess3PB";
            this.QueenChess3PB.Size = new System.Drawing.Size(58, 58);
            this.QueenChess3PB.TabIndex = 15;
            this.QueenChess3PB.TabStop = false;
            // 
            // QueenChess4PB
            // 
            this.QueenChess4PB.BackColor = System.Drawing.Color.Transparent;
            this.QueenChess4PB.Image = global::Quoridor_With_C.Resource1.ChessWhite;
            this.QueenChess4PB.Location = new System.Drawing.Point(808, 348);
            this.QueenChess4PB.Name = "QueenChess4PB";
            this.QueenChess4PB.Size = new System.Drawing.Size(58, 58);
            this.QueenChess4PB.TabIndex = 16;
            this.QueenChess4PB.TabStop = false;
            // 
            // QueenChess8PB
            // 
            this.QueenChess8PB.BackColor = System.Drawing.Color.Transparent;
            this.QueenChess8PB.Image = global::Quoridor_With_C.Resource1.ChessWhite;
            this.QueenChess8PB.Location = new System.Drawing.Point(808, 348);
            this.QueenChess8PB.Name = "QueenChess8PB";
            this.QueenChess8PB.Size = new System.Drawing.Size(58, 58);
            this.QueenChess8PB.TabIndex = 20;
            this.QueenChess8PB.TabStop = false;
            // 
            // QueenChess7PB
            // 
            this.QueenChess7PB.BackColor = System.Drawing.Color.Transparent;
            this.QueenChess7PB.Image = global::Quoridor_With_C.Resource1.ChessWhite;
            this.QueenChess7PB.Location = new System.Drawing.Point(808, 348);
            this.QueenChess7PB.Name = "QueenChess7PB";
            this.QueenChess7PB.Size = new System.Drawing.Size(58, 58);
            this.QueenChess7PB.TabIndex = 19;
            this.QueenChess7PB.TabStop = false;
            // 
            // QueenChess6PB
            // 
            this.QueenChess6PB.BackColor = System.Drawing.Color.Transparent;
            this.QueenChess6PB.Image = global::Quoridor_With_C.Resource1.ChessWhite;
            this.QueenChess6PB.Location = new System.Drawing.Point(808, 348);
            this.QueenChess6PB.Name = "QueenChess6PB";
            this.QueenChess6PB.Size = new System.Drawing.Size(58, 58);
            this.QueenChess6PB.TabIndex = 18;
            this.QueenChess6PB.TabStop = false;
            // 
            // QueenChess5PB
            // 
            this.QueenChess5PB.BackColor = System.Drawing.Color.Transparent;
            this.QueenChess5PB.Image = global::Quoridor_With_C.Resource1.ChessWhite;
            this.QueenChess5PB.Location = new System.Drawing.Point(808, 348);
            this.QueenChess5PB.Name = "QueenChess5PB";
            this.QueenChess5PB.Size = new System.Drawing.Size(58, 58);
            this.QueenChess5PB.TabIndex = 17;
            this.QueenChess5PB.TabStop = false;
            this.QueenChess5PB.Click += new System.EventHandler(this.skinPictureBox4_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 840);
            this.Controls.Add(this.QueenChess8PB);
            this.Controls.Add(this.QueenChess7PB);
            this.Controls.Add(this.QueenChess6PB);
            this.Controls.Add(this.QueenChess5PB);
            this.Controls.Add(this.QueenChess4PB);
            this.Controls.Add(this.QueenChess3PB);
            this.Controls.Add(this.QueenChess2PB);
            this.Controls.Add(this.QueenChess1PB);
            this.Controls.Add(this.HBoardPB);
            this.Controls.Add(this.VBoardPB);
            this.Controls.Add(this.Test2BTN);
            this.Controls.Add(this.CustomPlaceBTN);
            this.Controls.Add(this.RandomPlaceBTN);
            this.Controls.Add(this.TestBTN);
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
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.ChessBoardPB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChessWhitePB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChessBlackPB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HBoardPB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VBoardPB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.QueenChess1PB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.QueenChess2PB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.QueenChess3PB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.QueenChess4PB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.QueenChess8PB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.QueenChess7PB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.QueenChess6PB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.QueenChess5PB)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private CCWin.SkinControl.SkinPictureBox HBoardPB;
        private CCWin.SkinControl.SkinPictureBox VBoardPB;
        public CCWin.SkinControl.SkinPictureBox ChessBoardPB;
        private CCWin.SkinControl.SkinButton PlaceVerticalBoardBTN;
        private CCWin.SkinControl.SkinTextBox TestTB;
        private CCWin.SkinControl.SkinPictureBox ChessWhitePB;
        private CCWin.SkinControl.SkinPictureBox ChessBlackPB;
        private CCWin.SkinControl.SkinButton PlaceHorizontalBoardBTN;
        private CCWin.SkinControl.SkinButton TestBTN;
        private CCWin.SkinControl.SkinButton RandomPlaceBTN;
        private CCWin.SkinControl.SkinButton CustomPlaceBTN;
        private CCWin.SkinControl.SkinButton Test2BTN;
        public CCWin.SkinControl.SkinPictureBox QueenChess1PB;
        private CCWin.SkinControl.SkinPictureBox QueenChess2PB;
        private CCWin.SkinControl.SkinPictureBox QueenChess3PB;
        private CCWin.SkinControl.SkinPictureBox QueenChess4PB;
        private CCWin.SkinControl.SkinPictureBox QueenChess8PB;
        private CCWin.SkinControl.SkinPictureBox QueenChess7PB;
        private CCWin.SkinControl.SkinPictureBox QueenChess6PB;
        private CCWin.SkinControl.SkinPictureBox QueenChess5PB;

    }
}

