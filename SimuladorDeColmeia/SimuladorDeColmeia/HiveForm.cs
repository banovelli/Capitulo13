using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimuladorDeColmeia
{
    public partial class HiveForm : Form
    {
        public HiveForm()
        {
            InitializeComponent();
            BackgroundImage = Renderer.ResizeImage(
                    Properties.Resources.HiveInside,
                    ClientRectangle.Width, ClientRectangle.Height);
        }

        public Renderer renderer;
        private void HiveForm_Paint(object sender, PaintEventArgs e)
        {
            renderer.PaintHive(e.Graphics);
        }
    }
}
