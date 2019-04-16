namespace Quoridor_With_C
{
    partial class DebugTool
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebugTool));
            this.skinTabControl1 = new CCWin.SkinControl.SkinTabControl();
            this.PortTabPage = new CCWin.SkinControl.SkinTabPage();
            this.SendGB = new CCWin.SkinControl.SkinGroupBox();
            this.ReceiveGB = new CCWin.SkinControl.SkinGroupBox();
            this.skinMenuStrip1 = new CCWin.SkinControl.SkinMenuStrip();
            this.串口设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.PortCB = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.BaudCB = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.DataBitsCB = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.StopBitsCB = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.ParityCB = new System.Windows.Forms.ToolStripComboBox();
            this.ReceiveTB = new CCWin.SkinControl.SkinTextBox();
            this.SwitchReceiveBTN = new CCWin.SkinControl.SkinButton();
            this.SaveDataBTN = new CCWin.SkinControl.SkinButton();
            this.ReceiveClearBTN = new CCWin.SkinControl.SkinButton();
            this.HexCB = new CCWin.SkinControl.SkinCheckBox();
            this.skinTabControl1.SuspendLayout();
            this.PortTabPage.SuspendLayout();
            this.ReceiveGB.SuspendLayout();
            this.skinMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // skinTabControl1
            // 
            this.skinTabControl1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.skinTabControl1.AnimatorType = CCWin.SkinControl.AnimationType.HorizSlide;
            this.skinTabControl1.CloseRect = new System.Drawing.Rectangle(2, 2, 12, 12);
            this.skinTabControl1.Controls.Add(this.PortTabPage);
            this.skinTabControl1.HeadBack = null;
            this.skinTabControl1.ImgTxtOffset = new System.Drawing.Point(0, 0);
            this.skinTabControl1.ItemSize = new System.Drawing.Size(70, 36);
            this.skinTabControl1.Location = new System.Drawing.Point(8, 61);
            this.skinTabControl1.Name = "skinTabControl1";
            this.skinTabControl1.PageArrowDown = ((System.Drawing.Image)(resources.GetObject("skinTabControl1.PageArrowDown")));
            this.skinTabControl1.PageArrowHover = ((System.Drawing.Image)(resources.GetObject("skinTabControl1.PageArrowHover")));
            this.skinTabControl1.PageCloseHover = ((System.Drawing.Image)(resources.GetObject("skinTabControl1.PageCloseHover")));
            this.skinTabControl1.PageCloseNormal = ((System.Drawing.Image)(resources.GetObject("skinTabControl1.PageCloseNormal")));
            this.skinTabControl1.PageDown = ((System.Drawing.Image)(resources.GetObject("skinTabControl1.PageDown")));
            this.skinTabControl1.PageHover = ((System.Drawing.Image)(resources.GetObject("skinTabControl1.PageHover")));
            this.skinTabControl1.PageImagePosition = CCWin.SkinControl.SkinTabControl.ePageImagePosition.Left;
            this.skinTabControl1.PageNorml = null;
            this.skinTabControl1.SelectedIndex = 0;
            this.skinTabControl1.Size = new System.Drawing.Size(621, 408);
            this.skinTabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.skinTabControl1.TabIndex = 0;
            this.skinTabControl1.SizeChanged += new System.EventHandler(this.skinTabControl1_SizeChanged);
            // 
            // PortTabPage
            // 
            this.PortTabPage.BackColor = System.Drawing.Color.White;
            this.PortTabPage.Controls.Add(this.SendGB);
            this.PortTabPage.Controls.Add(this.ReceiveGB);
            this.PortTabPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PortTabPage.Location = new System.Drawing.Point(0, 36);
            this.PortTabPage.Name = "PortTabPage";
            this.PortTabPage.Size = new System.Drawing.Size(621, 372);
            this.PortTabPage.TabIndex = 0;
            this.PortTabPage.TabItemImage = null;
            this.PortTabPage.Text = "串口助手";
            // 
            // SendGB
            // 
            this.SendGB.BackColor = System.Drawing.Color.Transparent;
            this.SendGB.BorderColor = System.Drawing.Color.SteelBlue;
            this.SendGB.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SendGB.ForeColor = System.Drawing.Color.Black;
            this.SendGB.Location = new System.Drawing.Point(3, 258);
            this.SendGB.Name = "SendGB";
            this.SendGB.RectBackColor = System.Drawing.Color.White;
            this.SendGB.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.SendGB.Size = new System.Drawing.Size(615, 111);
            this.SendGB.TabIndex = 1;
            this.SendGB.TabStop = false;
            this.SendGB.Text = "发送助手";
            this.SendGB.TitleBorderColor = System.Drawing.Color.Transparent;
            this.SendGB.TitleRectBackColor = System.Drawing.Color.White;
            this.SendGB.TitleRoundStyle = CCWin.SkinClass.RoundStyle.All;
            // 
            // ReceiveGB
            // 
            this.ReceiveGB.BackColor = System.Drawing.Color.Transparent;
            this.ReceiveGB.BorderColor = System.Drawing.Color.SteelBlue;
            this.ReceiveGB.Controls.Add(this.HexCB);
            this.ReceiveGB.Controls.Add(this.ReceiveClearBTN);
            this.ReceiveGB.Controls.Add(this.SaveDataBTN);
            this.ReceiveGB.Controls.Add(this.SwitchReceiveBTN);
            this.ReceiveGB.Controls.Add(this.ReceiveTB);
            this.ReceiveGB.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ReceiveGB.ForeColor = System.Drawing.Color.Black;
            this.ReceiveGB.Location = new System.Drawing.Point(3, 3);
            this.ReceiveGB.Name = "ReceiveGB";
            this.ReceiveGB.RectBackColor = System.Drawing.Color.White;
            this.ReceiveGB.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.ReceiveGB.Size = new System.Drawing.Size(615, 249);
            this.ReceiveGB.TabIndex = 0;
            this.ReceiveGB.TabStop = false;
            this.ReceiveGB.Text = "接收助手";
            this.ReceiveGB.TitleBorderColor = System.Drawing.Color.Transparent;
            this.ReceiveGB.TitleRectBackColor = System.Drawing.Color.White;
            this.ReceiveGB.TitleRoundStyle = CCWin.SkinClass.RoundStyle.All;
            // 
            // skinMenuStrip1
            // 
            this.skinMenuStrip1.Arrow = System.Drawing.Color.Black;
            this.skinMenuStrip1.Back = System.Drawing.Color.White;
            this.skinMenuStrip1.BackRadius = 4;
            this.skinMenuStrip1.BackRectangle = new System.Drawing.Rectangle(10, 10, 10, 10);
            this.skinMenuStrip1.Base = System.Drawing.Color.Silver;
            this.skinMenuStrip1.BaseFore = System.Drawing.Color.Black;
            this.skinMenuStrip1.BaseForeAnamorphosis = false;
            this.skinMenuStrip1.BaseForeAnamorphosisBorder = 4;
            this.skinMenuStrip1.BaseForeAnamorphosisColor = System.Drawing.Color.White;
            this.skinMenuStrip1.BaseHoverFore = System.Drawing.Color.White;
            this.skinMenuStrip1.BaseItemAnamorphosis = true;
            this.skinMenuStrip1.BaseItemBorder = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinMenuStrip1.BaseItemBorderShow = true;
            this.skinMenuStrip1.BaseItemDown = ((System.Drawing.Image)(resources.GetObject("skinMenuStrip1.BaseItemDown")));
            this.skinMenuStrip1.BaseItemHover = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinMenuStrip1.BaseItemMouse = ((System.Drawing.Image)(resources.GetObject("skinMenuStrip1.BaseItemMouse")));
            this.skinMenuStrip1.BaseItemPressed = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinMenuStrip1.BaseItemRadius = 4;
            this.skinMenuStrip1.BaseItemRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinMenuStrip1.BaseItemSplitter = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinMenuStrip1.DropDownImageSeparator = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            this.skinMenuStrip1.Fore = System.Drawing.Color.Black;
            this.skinMenuStrip1.HoverFore = System.Drawing.Color.White;
            this.skinMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.skinMenuStrip1.ItemAnamorphosis = true;
            this.skinMenuStrip1.ItemBorder = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinMenuStrip1.ItemBorderShow = true;
            this.skinMenuStrip1.ItemHover = System.Drawing.Color.LightCoral;
            this.skinMenuStrip1.ItemPressed = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinMenuStrip1.ItemRadius = 4;
            this.skinMenuStrip1.ItemRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.串口设置ToolStripMenuItem});
            this.skinMenuStrip1.Location = new System.Drawing.Point(8, 30);
            this.skinMenuStrip1.Name = "skinMenuStrip1";
            this.skinMenuStrip1.RadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinMenuStrip1.Size = new System.Drawing.Size(624, 28);
            this.skinMenuStrip1.SkinAllColor = true;
            this.skinMenuStrip1.TabIndex = 1;
            this.skinMenuStrip1.Text = "skinMenuStrip1";
            this.skinMenuStrip1.TitleAnamorphosis = true;
            this.skinMenuStrip1.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(228)))), ((int)(((byte)(236)))));
            this.skinMenuStrip1.TitleRadius = 4;
            this.skinMenuStrip1.TitleRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            // 
            // 串口设置ToolStripMenuItem
            // 
            this.串口设置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5});
            this.串口设置ToolStripMenuItem.Name = "串口设置ToolStripMenuItem";
            this.串口设置ToolStripMenuItem.Size = new System.Drawing.Size(100, 24);
            this.串口设置ToolStripMenuItem.Text = "串口设置(&S)";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PortCB});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(181, 26);
            this.toolStripMenuItem1.Text = "端口号";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // PortCB
            // 
            this.PortCB.Name = "PortCB";
            this.PortCB.Size = new System.Drawing.Size(121, 28);
            this.PortCB.Click += new System.EventHandler(this.PortCB_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BaudCB});
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(181, 26);
            this.toolStripMenuItem2.Text = "波特率";
            // 
            // BaudCB
            // 
            this.BaudCB.Items.AddRange(new object[] {
            "256000",
            "128000",
            "115200",
            "57600",
            "56000",
            "38400",
            "19200",
            "14400",
            "9600",
            "4800",
            "2400"});
            this.BaudCB.Name = "BaudCB";
            this.BaudCB.Size = new System.Drawing.Size(121, 28);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DataBitsCB});
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(181, 26);
            this.toolStripMenuItem3.Text = "数据位";
            // 
            // DataBitsCB
            // 
            this.DataBitsCB.Items.AddRange(new object[] {
            "8",
            "7",
            "6",
            "5"});
            this.DataBitsCB.Name = "DataBitsCB";
            this.DataBitsCB.Size = new System.Drawing.Size(121, 28);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StopBitsCB});
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(181, 26);
            this.toolStripMenuItem4.Text = "停止位";
            // 
            // StopBitsCB
            // 
            this.StopBitsCB.Items.AddRange(new object[] {
            "1"});
            this.StopBitsCB.Name = "StopBitsCB";
            this.StopBitsCB.Size = new System.Drawing.Size(121, 28);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ParityCB});
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(181, 26);
            this.toolStripMenuItem5.Text = "校验位";
            // 
            // ParityCB
            // 
            this.ParityCB.Items.AddRange(new object[] {
            "无"});
            this.ParityCB.Name = "ParityCB";
            this.ParityCB.Size = new System.Drawing.Size(121, 28);
            // 
            // ReceiveTB
            // 
            this.ReceiveTB.BackColor = System.Drawing.Color.Transparent;
            this.ReceiveTB.DownBack = null;
            this.ReceiveTB.Icon = null;
            this.ReceiveTB.IconIsButton = false;
            this.ReceiveTB.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.ReceiveTB.IsPasswordChat = '\0';
            this.ReceiveTB.IsSystemPasswordChar = false;
            this.ReceiveTB.Lines = new string[0];
            this.ReceiveTB.Location = new System.Drawing.Point(3, 30);
            this.ReceiveTB.Margin = new System.Windows.Forms.Padding(0);
            this.ReceiveTB.MaxLength = 32767;
            this.ReceiveTB.MinimumSize = new System.Drawing.Size(28, 28);
            this.ReceiveTB.MouseBack = null;
            this.ReceiveTB.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.ReceiveTB.Multiline = true;
            this.ReceiveTB.Name = "ReceiveTB";
            this.ReceiveTB.NormlBack = null;
            this.ReceiveTB.Padding = new System.Windows.Forms.Padding(5);
            this.ReceiveTB.ReadOnly = false;
            this.ReceiveTB.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.ReceiveTB.Size = new System.Drawing.Size(479, 219);
            // 
            // 
            // 
            this.ReceiveTB.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ReceiveTB.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReceiveTB.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.ReceiveTB.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.ReceiveTB.SkinTxt.Multiline = true;
            this.ReceiveTB.SkinTxt.Name = "BaseText";
            this.ReceiveTB.SkinTxt.Size = new System.Drawing.Size(469, 209);
            this.ReceiveTB.SkinTxt.TabIndex = 0;
            this.ReceiveTB.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.ReceiveTB.SkinTxt.WaterText = "";
            this.ReceiveTB.TabIndex = 0;
            this.ReceiveTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.ReceiveTB.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.ReceiveTB.WaterText = "";
            this.ReceiveTB.WordWrap = true;
            // 
            // SwitchReceiveBTN
            // 
            this.SwitchReceiveBTN.BackColor = System.Drawing.Color.Transparent;
            this.SwitchReceiveBTN.BaseColor = System.Drawing.Color.Silver;
            this.SwitchReceiveBTN.BorderColor = System.Drawing.Color.Transparent;
            this.SwitchReceiveBTN.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.SwitchReceiveBTN.DownBack = null;
            this.SwitchReceiveBTN.Location = new System.Drawing.Point(485, 129);
            this.SwitchReceiveBTN.MouseBack = null;
            this.SwitchReceiveBTN.Name = "SwitchReceiveBTN";
            this.SwitchReceiveBTN.NormlBack = null;
            this.SwitchReceiveBTN.Size = new System.Drawing.Size(124, 36);
            this.SwitchReceiveBTN.TabIndex = 1;
            this.SwitchReceiveBTN.Text = "开始接收";
            this.SwitchReceiveBTN.UseVisualStyleBackColor = false;
            this.SwitchReceiveBTN.Click += new System.EventHandler(this.SwitchReceiveBTN_Click);
            // 
            // SaveDataBTN
            // 
            this.SaveDataBTN.BackColor = System.Drawing.Color.Transparent;
            this.SaveDataBTN.BaseColor = System.Drawing.Color.Silver;
            this.SaveDataBTN.BorderColor = System.Drawing.Color.Transparent;
            this.SaveDataBTN.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.SaveDataBTN.DownBack = null;
            this.SaveDataBTN.Location = new System.Drawing.Point(485, 171);
            this.SaveDataBTN.MouseBack = null;
            this.SaveDataBTN.Name = "SaveDataBTN";
            this.SaveDataBTN.NormlBack = null;
            this.SaveDataBTN.Size = new System.Drawing.Size(124, 36);
            this.SaveDataBTN.TabIndex = 2;
            this.SaveDataBTN.Text = "保存数据";
            this.SaveDataBTN.UseVisualStyleBackColor = false;
            // 
            // ReceiveClearBTN
            // 
            this.ReceiveClearBTN.BackColor = System.Drawing.Color.Transparent;
            this.ReceiveClearBTN.BaseColor = System.Drawing.Color.Silver;
            this.ReceiveClearBTN.BorderColor = System.Drawing.Color.Transparent;
            this.ReceiveClearBTN.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.ReceiveClearBTN.DownBack = null;
            this.ReceiveClearBTN.Location = new System.Drawing.Point(485, 213);
            this.ReceiveClearBTN.MouseBack = null;
            this.ReceiveClearBTN.Name = "ReceiveClearBTN";
            this.ReceiveClearBTN.NormlBack = null;
            this.ReceiveClearBTN.Size = new System.Drawing.Size(124, 36);
            this.ReceiveClearBTN.TabIndex = 3;
            this.ReceiveClearBTN.Text = "清除数据";
            this.ReceiveClearBTN.UseVisualStyleBackColor = false;
            // 
            // HexCB
            // 
            this.HexCB.AutoSize = true;
            this.HexCB.BackColor = System.Drawing.Color.Transparent;
            this.HexCB.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.HexCB.DownBack = null;
            this.HexCB.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HexCB.Location = new System.Drawing.Point(485, 99);
            this.HexCB.MouseBack = null;
            this.HexCB.Name = "HexCB";
            this.HexCB.NormlBack = null;
            this.HexCB.SelectedDownBack = null;
            this.HexCB.SelectedMouseBack = null;
            this.HexCB.SelectedNormlBack = null;
            this.HexCB.Size = new System.Drawing.Size(127, 29);
            this.HexCB.TabIndex = 4;
            this.HexCB.Text = "以Hex显示";
            this.HexCB.UseVisualStyleBackColor = false;
            // 
            // DebugTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 480);
            this.Controls.Add(this.skinTabControl1);
            this.Controls.Add(this.skinMenuStrip1);
            this.MainMenuStrip = this.skinMenuStrip1;
            this.Name = "DebugTool";
            this.Text = "DebugTool";
            this.Load += new System.EventHandler(this.DebugTool_Load);
            this.SizeChanged += new System.EventHandler(this.DebugTool_SizeChanged);
            this.Resize += new System.EventHandler(this.DebugTool_Resize);
            this.skinTabControl1.ResumeLayout(false);
            this.PortTabPage.ResumeLayout(false);
            this.ReceiveGB.ResumeLayout(false);
            this.ReceiveGB.PerformLayout();
            this.skinMenuStrip1.ResumeLayout(false);
            this.skinMenuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CCWin.SkinControl.SkinTabControl skinTabControl1;
        private CCWin.SkinControl.SkinMenuStrip skinMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 串口设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripComboBox PortCB;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripComboBox BaudCB;
        private System.Windows.Forms.ToolStripComboBox DataBitsCB;
        private System.Windows.Forms.ToolStripComboBox StopBitsCB;
        private System.Windows.Forms.ToolStripComboBox ParityCB;
        private CCWin.SkinControl.SkinTabPage PortTabPage;
        private CCWin.SkinControl.SkinGroupBox ReceiveGB;
        private CCWin.SkinControl.SkinGroupBox SendGB;
        private CCWin.SkinControl.SkinButton ReceiveClearBTN;
        private CCWin.SkinControl.SkinButton SaveDataBTN;
        private CCWin.SkinControl.SkinButton SwitchReceiveBTN;
        private CCWin.SkinControl.SkinTextBox ReceiveTB;
        private CCWin.SkinControl.SkinCheckBox HexCB;
    }
}