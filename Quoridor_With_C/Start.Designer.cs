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
            this.SinglePlayBTN = new CCWin.SkinControl.SkinButton();
            this.DoublePlayBTN = new CCWin.SkinControl.SkinButton();
            this.StartUI_PB = new CCWin.SkinControl.SkinPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.StartUI_PB)).BeginInit();
            this.SuspendLayout();
            // 
            // SinglePlayBTN
            // 
            this.SinglePlayBTN.BackColor = System.Drawing.Color.Transparent;
            this.SinglePlayBTN.BaseColor = System.Drawing.Color.AliceBlue;
            this.SinglePlayBTN.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.SinglePlayBTN.DownBack = null;
            this.SinglePlayBTN.Enabled = false;
            this.SinglePlayBTN.Font = new System.Drawing.Font("微软雅黑", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SinglePlayBTN.Location = new System.Drawing.Point(583, 75);
            this.SinglePlayBTN.MouseBack = null;
            this.SinglePlayBTN.Name = "SinglePlayBTN";
            this.SinglePlayBTN.NormlBack = null;
            this.SinglePlayBTN.Size = new System.Drawing.Size(191, 60);
            this.SinglePlayBTN.TabIndex = 2;
            this.SinglePlayBTN.Text = "单人游戏";
            this.SinglePlayBTN.UseVisualStyleBackColor = false;
            // 
            // DoublePlayBTN
            // 
            this.DoublePlayBTN.BackColor = System.Drawing.Color.Transparent;
            this.DoublePlayBTN.BaseColor = System.Drawing.Color.AliceBlue;
            this.DoublePlayBTN.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.DoublePlayBTN.DownBack = null;
            this.DoublePlayBTN.Font = new System.Drawing.Font("微软雅黑", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DoublePlayBTN.Location = new System.Drawing.Point(583, 164);
            this.DoublePlayBTN.MouseBack = null;
            this.DoublePlayBTN.Name = "DoublePlayBTN";
            this.DoublePlayBTN.NormlBack = null;
            this.DoublePlayBTN.Size = new System.Drawing.Size(191, 60);
            this.DoublePlayBTN.TabIndex = 3;
            this.DoublePlayBTN.Text = "双人游戏";
            this.DoublePlayBTN.UseVisualStyleBackColor = false;
            this.DoublePlayBTN.Click += new System.EventHandler(this.DoublePlayBTN_Click);
            // 
            // StartUI_PB
            // 
            this.StartUI_PB.BackColor = System.Drawing.Color.Transparent;
            this.StartUI_PB.Image = global::Quoridor_With_C.Resource1.封面;
            this.StartUI_PB.Location = new System.Drawing.Point(33, 49);
            this.StartUI_PB.Name = "StartUI_PB";
            this.StartUI_PB.Size = new System.Drawing.Size(400, 400);
            this.StartUI_PB.TabIndex = 0;
            this.StartUI_PB.TabStop = false;
            // 
            // Start
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 640);
            this.Controls.Add(this.DoublePlayBTN);
            this.Controls.Add(this.SinglePlayBTN);
            this.Controls.Add(this.StartUI_PB);
            this.IsMdiContainer = true;
            this.Name = "Start";
            this.Text = "Start";
            this.Load += new System.EventHandler(this.Start_Load);
            ((System.ComponentModel.ISupportInitialize)(this.StartUI_PB)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CCWin.SkinControl.SkinPictureBox StartUI_PB;
        private CCWin.SkinControl.SkinButton SinglePlayBTN;
        private CCWin.SkinControl.SkinButton DoublePlayBTN;
    }
}