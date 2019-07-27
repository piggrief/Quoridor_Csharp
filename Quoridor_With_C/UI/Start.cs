using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CCWin;

namespace Quoridor_With_C
{
    public partial class Start : Skin_Metro
    {
        public Start()
        {
            InitializeComponent();    
        }

        private void DoublePlayBTN_Click(object sender, EventArgs e)
        {
            //Close();
            this.Hide();
            Form1 f1 = new Form1(Form1.GameModeStatus.DoublePlay);
            //f1.MdiParent = this;
            f1.Show();
        }

        private void Start_Load(object sender, EventArgs e)
        {
            //SinglePlayBTN.FlatStyle = FlatStyle.Flat;
            //SinglePlayBTN.ForeColor = Color.Transparent;
            //SinglePlayBTN.BaseColor = Color.Transparent;
            //SinglePlayBTN.BackColor = Color.Transparent;
            //SinglePlayBTN.GlowColor = Color.Transparent;
            this.Size = new Size(Resource1.步步为营.Size.Width + 20, Resource1.步步为营.Size.Height + 40);
        }

        private void SinglePlayBTN_Click(object sender, EventArgs e)
        {
            //Close();
            this.Hide();
            Form1 f1 = new Form1(Form1.GameModeStatus.SinglePlay);
            //f1.MdiParent = this;
            f1.Show();

        }

        private void Queen8BTN_Click(object sender, EventArgs e)
        {
            //Close();
            this.Hide();
            Form1 f1 = new Form1(Form1.GameModeStatus.Queen8);
            //f1.MdiParent = this;
            f1.Show();
        }
    }
}
