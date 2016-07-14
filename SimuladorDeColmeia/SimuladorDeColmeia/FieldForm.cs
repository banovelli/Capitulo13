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
    public partial class FieldForm : Form
    {
        public FieldForm()
        {
            InitializeComponent();
        }

        public Renderer renderer;
        private void FieldForm_Paint(object sender, PaintEventArgs e)
        {
            renderer.PaintField(e.Graphics);
        }
    }
}
