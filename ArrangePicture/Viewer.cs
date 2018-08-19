using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

// so I can access Resources where images are stored
using ArrangePicture.Properties;

namespace ArrangePicture
{
    public class Viewer
    {
        private PictureBox pictureBox;
        private Image image;
        private bool hover;

        public Viewer() { }
        public Viewer(Image img)
        {
            image = img;
            hover = false;
            pictureBox = new PictureBox();
            pictureBox.Location = new Point(0, 0);
            pictureBox.Size = new Size(50, 50);
            pictureBox.Image = image;
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.Click += new EventHandler(this.pictureBox_Click);
            pictureBox.MouseEnter += new EventHandler(this.pictureBox_MouseEnter);
            pictureBox.MouseLeave += new EventHandler(this.pictureBox_MouseLeave);
            pictureBox.Paint += new PaintEventHandler(this.pictureBox_OnPaint);
        }

        public PictureBox PictureBoxControl { get { return pictureBox; } }
        public Image CurrentImage { set { image = value; pictureBox.Image = image; pictureBox.Invalidate(); } }

        protected void pictureBox_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2(image);
            f2.Show();
        }
        
        protected void pictureBox_MouseEnter(object sender, EventArgs e)
        {
            PictureBox tempReference = (PictureBox)sender;
            hover = true;
            tempReference.Invalidate();
        }

        protected void pictureBox_MouseLeave(object sender, EventArgs e)
        {
            PictureBox tempReference = (PictureBox)sender;
            hover = false;
            tempReference.Invalidate();
        }
        
        protected void pictureBox_OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (hover)
            {
                Color selectionColor = Color.FromArgb(50, 200, 50, 50);
                Color selectionBorderColor = SystemColors.Highlight;

                using (var brush = new SolidBrush(selectionColor))
                {
                    g.FillRectangle(brush, pictureBox.ClientRectangle);
                }
                using (var pen = new Pen(selectionBorderColor))
                {
                    g.DrawRectangle(pen,
                        new Rectangle()
                        {
                            Width = (pictureBox.Width - 1),
                            Height = (pictureBox.Height - 1)
                        });
                }
            }

        }
    }
}
