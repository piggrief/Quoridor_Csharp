namespace Quoridor_With_C
{
    partial class Start
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Start));
            this.SinglePlayBTN = new CCWin.SkinControl.SkinButton();
            this.DoublePlayBTN = new CCWin.SkinControl.SkinButton();
            this.Queen8BTN = new CCWin.SkinControl.SkinButton();
            this.SuspendLayout();
            // 
            // SinglePlayBTN
            // 
            this.SinglePlayBTN.BackColor = System.Drawing.Color.Transparent;
            this.SinglePlayBTN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("SinglePlayBTN.BackgroundImage")));
            this.SinglePlayBTN.BaseColor = System.Drawing.Color.DarkGoldenrod;
            this.SinglePlayBTN.BorderColor = System.Drawing.Color.Transparent;
            this.SinglePlayBTN.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.SinglePlayBTN.DownBack = null;
            this.SinglePlayBTN.DrawType = CCWin.SkinControl.DrawStyle.Img;
            this.SinglePlayBTN.Font = new System.Drawing.Font("迷你简毡笔黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SinglePlayBTN.ForeColor = System.Drawing.SystemColors.Desktop;
            this.SinglePlayBTN.Location = new System.Drawing.Point(62, 360);
            this.SinglePlayBTN.MouseBack = ((System.Drawing.Image)(resources.GetObject("SinglePlayBTN.MouseBack")));
            this.SinglePlayBTN.Name = "SinglePlayBTN";
            this.SinglePlayBTN.NormlBack = null;
            this.SinglePlayBTN.Size = new System.Drawing.Size(191, 60);
            this.SinglePlayBTN.TabIndex = 2;
            this.SinglePlayBTN.Text = "单人游戏";
            this.SinglePlayBTN.UseVisualStyleBackColor = false;
            this.SinglePlayBTN.Click += new System.EventHandler(this.SinglePlayBTN_Click);
            // 
            // DoublePlayBTN
            // 
            this.DoublePlayBTN.BackColor = System.Drawing.Color.Transparent;
            this.DoublePlayBTN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("DoublePlayBTN.BackgroundImage")));
            this.DoublePlayBTN.BaseColor = System.Drawing.Color.AliceBlue;
            this.DoublePlayBTN.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.DoublePlayBTN.DownBack = null;
            this.DoublePlayBTN.DrawType = CCWin.SkinControl.DrawStyle.Img;
            this.DoublePlayBTN.Font = new System.Drawing.Font("迷你简毡笔黑", 18F);
            this.DoublePlayBTN.Location = new System.Drawing.Point(62, 426);
            this.DoublePlayBTN.MouseBack = ((System.Drawing.Image)(resources.GetObject("DoublePlayBTN.MouseBack")));
            this.DoublePlayBTN.Name = "DoublePlayBTN";
            this.DoublePlayBTN.NormlBack = null;
            this.DoublePlayBTN.Size = new System.Drawing.Size(191, 60);
            this.DoublePlayBTN.TabIndex = 3;
            this.DoublePlayBTN.Text = "双人游戏";
            this.DoublePlayBTN.UseVisualStyleBackColor = false;
            this.DoublePlayBTN.Click += new System.EventHandler(this.DoublePlayBTN_Click);
            // 
            // Queen8BTN
            // 
            this.Queen8BTN.BackColor = System.Drawing.Color.Transparent;
            this.Queen8BTN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Queen8BTN.BackgroundImage")));
            this.Queen8BTN.BaseColor = System.Drawing.Color.AliceBlue;
            this.Queen8BTN.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.Queen8BTN.DownBack = null;
            this.Queen8BTN.DrawType = CCWin.SkinControl.DrawStyle.Img;
            this.Queen8BTN.Font = new System.Drawing.Font("迷你简毡笔黑", 18F);
            this.Queen8BTN.Location = new System.Drawing.Point(62, 294);
            this.Queen8BTN.MouseBack = ((System.Drawing.Image)(resources.GetObject("Queen8BTN.MouseBack")));
            this.Queen8BTN.Name = "Queen8BTN";
            this.Queen8BTN.NormlBack = null;
            this.Queen8BTN.Size = new System.Drawing.Size(191, 60);
            this.Queen8BTN.TabIndex = 7;
            this.Queen8BTN.Text = "八皇后仿真";
            this.Queen8BTN.UseVisualStyleBackColor = false;
            this.Queen8BTN.Click += new System.EventHandler(this.Queen8BTN_Click);
            // 
            // Start
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(800, 640);
            this.Controls.Add(this.Queen8BTN);
            this.Controls.Add(this.DoublePlayBTN);
            this.Controls.Add(this.SinglePlayBTN);
            this.IsMdiContainer = true;
            this.MdiImage = ((System.Drawing.Image)(resources.GetObject("$this.MdiImage")));
            this.Name = "Start";
            this.ShadowColor = System.Drawing.Color.White;
            this.ShadowWidth = 50;
            this.Text = "对弈创意组测试平台_上海海事大学";
            this.Load += new System.EventHandler(this.Start_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CCWin.SkinControl.SkinButton SinglePlayBTN;
        private CCWin.SkinControl.SkinButton DoublePlayBTN;
        private CCWin.SkinControl.SkinButton Queen8BTN;
    }
}