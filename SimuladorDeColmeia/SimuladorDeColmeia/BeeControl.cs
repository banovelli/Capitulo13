using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimuladorDeColmeia
{
    public partial class BeeControl : UserControl
    {
        public BeeControl()
        {
            InitializeComponent();
            BackColor = System.Drawing.Color.Transparent;
            BackgroundImageLayout = ImageLayout.None;
            ResizeCells();
        }

        private int cell = 0;

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            cell++;
            switch (cell)
            {
                case 1: BackgroundImage = cells[0]; break;
                case 2: BackgroundImage = cells[1]; break;
                case 3: BackgroundImage = cells[2]; break;
                case 4: BackgroundImage = cells[3]; break;
                case 5: BackgroundImage = cells[2]; break;
                default: BackgroundImage = cells[1];
                    cell = 0; break;
            }
        }

        private Bitmap[] cells = new Bitmap[4];
        private void ResizeCells()
        {
            cells[0] = Renderer.ResizeImage(Properties.Resources.Bee_animation_1, Width, Height);
            cells[1] = Renderer.ResizeImage(Properties.Resources.Bee_animation_2, Width, Height);
            cells[2] = Renderer.ResizeImage(Properties.Resources.Bee_animation_3, Width, Height);
            cells[3] = Renderer.ResizeImage(Properties.Resources.Bee_animation_4, Width, Height);
        }
        
        private void BeeControl_Resize_1(object sender, EventArgs e)
        {
            ResizeCells();
        }
    }
}
