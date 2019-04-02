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
            StartUI_PB.Size = new Size(400,400);      
        }

        private void DoublePlayBTN_Click(object sender, EventArgs e)
        {
            //Close();
            this.Hide();
            Form1 f1 = new Form1();
            //f1.MdiParent = this;
            f1.Show();
            f1.GameMode = "DoublePlay";
        }

        private void Start_Load(object sender, EventArgs e)
        {

        }

        private void SinglePlayBTN_Click(object sender, EventArgs e)
        {
            //Close();
            this.Hide();
            Form1 f1 = new Form1();
            //f1.MdiParent = this;
            f1.Show();
            f1.GameMode = "SinglePlay";
        }
    }
}
