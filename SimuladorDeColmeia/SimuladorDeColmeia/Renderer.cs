using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimuladorDeColmeia
{
    public class Renderer
    {
        private World world;
        private HiveForm hiveForm;
        private FieldForm fieldForm;

        private Dictionary<Flower, PictureBox> flowerLookup =
             new Dictionary<Flower, PictureBox>();
        private List<Flower> deadFlowers = new List<Flower>();

        private Dictionary<Bee, BeeControl> beeLookup =
             new Dictionary<Bee, BeeControl>();
        private List<Bee> retiredBees = new List<Bee>();

        public Renderer(World world, HiveForm hiveForm, FieldForm fieldForm)
        {
            this.world = world;
            this.hiveForm = hiveForm;
            this.fieldForm = fieldForm;
        }

        public void Render()
        {
            DrawBees();
            DrawFlowers();
            RemoveRetiredBeesAndDeadFlowers();
        }

        public void Reset()
        {
            foreach (PictureBox flower in flowerLookup.Values)
            {
                fieldForm.Controls.Remove(flower);
                flower.Dispose();
            }
            foreach (BeeControl bee in beeLookup.Values)
            {
                hiveForm.Controls.Remove(bee);
                fieldForm.Controls.Remove(bee);
                bee.Dispose();
            }
            flowerLookup.Clear();
            beeLookup.Clear();
        }

        private void DrawFlowers()
        {
            foreach (Flower flower in world.Flowers)
                if (!flowerLookup.ContainsKey(flower))
                {
                    PictureBox flowerControl = new PictureBox()
                    {
                        Width = 45,
                        Height = 55,
                        Image = Properties.Resources.Flower,
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Location = flower.Location
                    };
                    flowerLookup.Add(flower, flowerControl);
                    fieldForm.Controls.Add(flowerControl);
                }

            foreach (Flower flower in flowerLookup.Keys)
            {
                if (!world.Flowers.Contains(flower))
                {
                    PictureBox flowerControlToRemove = flowerLookup[flower];
                    fieldForm.Controls.Remove(flowerControlToRemove);
                    flowerControlToRemove.Dispose();
                    deadFlowers.Add(flower);
                }
            }
        }

        private void DrawBees()
        {
            BeeControl beeControl;
            foreach (Bee bee in world.Bees)
            {
                beeControl = GetBeeControl(bee);
                if (bee.InsideHive)
                {
                    if (fieldForm.Controls.Contains(beeControl))
                        MoveBeeFromFieldToHive(beeControl);
                }
                else if (hiveForm.Controls.Contains(beeControl))
                    MoveBeeFromHiveToField(beeControl, bee);
                beeControl.Location = bee.Location;
            }

            foreach (Bee bee in beeLookup.Keys)
            {
                if (!world.Bees.Contains(bee))
                {
                    beeControl = beeLookup[bee];
                    if (fieldForm.Controls.Contains(beeControl))
                        fieldForm.Controls.Remove(beeControl);
                    if (hiveForm.Controls.Contains(beeControl))
                        hiveForm.Controls.Remove(beeControl);
                    beeControl.Dispose();
                    retiredBees.Add(bee);
                }
            }
        }

        private BeeControl GetBeeControl(Bee bee)
        {
            BeeControl beeControl;
            if (!beeLookup.ContainsKey(bee))
            {
                beeControl = new BeeControl() { Width = 40, Height = 40 };
                beeLookup.Add(bee, beeControl);
                hiveForm.Controls.Add(beeControl);
                beeControl.BringToFront();
            }
            else
                beeControl = beeLookup[bee];
            return beeControl;
        }


        private void MoveBeeFromHiveToField(BeeControl beeControl, Bee bee)
        {
            hiveForm.Controls.Remove(beeControl);
            beeControl.Size = new Size(20, 20);
            fieldForm.Controls.Add(beeControl);
            beeControl.BringToFront();
        }


        private void MoveBeeFromFieldToHive(BeeControl beeControl)
        {
            fieldForm.Controls.Remove(beeControl);
            beeControl.Size = new Size(40, 40);
            hiveForm.Controls.Add(beeControl);
            beeControl.BringToFront();
        }


        private void RemoveRetiredBeesAndDeadFlowers()
        {
            foreach (Bee bee in retiredBees)
                beeLookup.Remove(bee);
            retiredBees.Clear();
            foreach (Flower flower in deadFlowers)
                flowerLookup.Remove(flower);
            deadFlowers.Clear();
        }

        public static Bitmap ResizeImage(Bitmap picture, int width, int height)
        {
            Bitmap resizedPicture = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(resizedPicture))
            {
                graphics.DrawImage(picture, 0, 0, width, height);
            }
            return resizedPicture;
        }


        Bitmap HiveInside;
        Bitmap HiveOutside;
        Bitmap Flower;
        Bitmap[] BeeAnimationSmall;
        Bitmap[] BeeAnimationLarge;
        private void InitializeImages()
        {
            HiveOutside = ResizeImage(Properties.Resources.HiveOutside, 85, 100);
            Flower = ResizeImage(Properties.Resources.Flower, 75, 75);
            HiveInside = ResizeImage(Properties.Resources.HiveInside,
                hiveForm.ClientRectangle.Width, hiveForm.ClientRectangle.Height);
            BeeAnimationLarge = new Bitmap[4];
            BeeAnimationLarge[0] = ResizeImage(Properties.Resources.Bee_animation_1, 40, 40);
            BeeAnimationLarge[1] = ResizeImage(Properties.Resources.Bee_animation_2, 40, 40);
            BeeAnimationLarge[2] = ResizeImage(Properties.Resources.Bee_animation_3, 40, 40);
            BeeAnimationLarge[3] = ResizeImage(Properties.Resources.Bee_animation_4, 40, 40);
            BeeAnimationSmall = new Bitmap[4];
            BeeAnimationSmall[0] = ResizeImage(Properties.Resources.Bee_animation_1, 20, 20);
            BeeAnimationSmall[1] = ResizeImage(Properties.Resources.Bee_animation_2, 20, 20);
            BeeAnimationSmall[2] = ResizeImage(Properties.Resources.Bee_animation_3, 20, 20);
            BeeAnimationSmall[3] = ResizeImage(Properties.Resources.Bee_animation_4, 20, 20);
        }

        public void PaintHive(Graphics g)
        {
            g.FillRectangle(Brushes.SkyBlue, hiveForm.ClientRectangle);
            g.DrawImageUnscaled(HiveInside, 0, 0);
            foreach (Bee bee in world.Bees)
            {
                if (bee.InsideHive)
                    g.DrawImageUnscaled(BeeAnimationLarge[Cell],
                                        bee.Location.X, bee.Location.Y);
            }
        }

        public void PaintField(Graphics g)
        {
            using (Pen brownPen = new Pen(Color.Brown, 6.0F))
            {
                g.FillRectangle(Brushes.SkyBlue, 0, 0,
                         fieldForm.ClientSize.Width, fieldForm.ClientSize.Height / 2);
                g.FillEllipse(Brushes.Yellow, new RectangleF(50, 15, 70, 70));
                g.FillRectangle(Brushes.Green, 0, fieldForm.ClientSize.Height / 2,
                         fieldForm.ClientSize.Width, fieldForm.ClientSize.Height / 2);
                g.DrawLine(brownPen, new Point(643, 0), new Point(643, 30));
                g.DrawImageUnscaled(HiveOutside, 600, 20);
                foreach (Flower flower in world.Flowers)
                {
                    g.DrawImageUnscaled(Flower, flower.Location.X, flower.Location.Y);
                }
                foreach (Bee bee in world.Bees)
                {
                    if (!bee.InsideHive)
                        g.DrawImageUnscaled(BeeAnimationSmall[Cell],
                                            bee.Location.X, bee.Location.Y);
                }
            }
        }

        private int Cell = 0;
        private int Frame = 0;
        public void AnimateBees()
        {
            Frame++;
            if (Frame >= 6)
                Frame = 0;
            switch (Frame)
            {
                case 0: Cell = 0; break;
                case 1: Cell = 1; break;
                case 2: Cell = 2; break;
                case 3: Cell = 3; break;
                case 4: Cell = 2; break;
                case 5: Cell = 1; break;
                default: Cell = 0; break;
            }
            hiveForm.Invalidate();
            fieldForm.Invalidate();
        }
    }
}
