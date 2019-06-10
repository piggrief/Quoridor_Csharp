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
using GameTree;

namespace Quoridor_With_C
{
    public partial class DebugView : Skin_Metro
    {
        public DebugView()
        {
            InitializeComponent();
        }

        private void DebugView_Load(object sender, EventArgs e)
        {
            treeView1.Location = new Point(skinMenuStrip1.Location.X, skinMenuStrip1.Location.Y+skinMenuStrip1.Size.Height);
            treeView1.Size = new Size(skinMenuStrip1.Size.Width, this.Size.Height - treeView1.Location.Y);
        }
    }
}
