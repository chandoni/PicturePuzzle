using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace ArrangePicture
{
    public class Form2 : System.Windows.Forms.Form
    {
        // this will hold the image
        public Bitmap image;

        public Form2(Image theImg) // I will always pass in a Properties.Resources
        {
            InitializeComponent();

            image = new Bitmap(theImg);
            Text = "HINT";

            AutoScroll = true;
            AutoScrollMinSize = image.Size;

            ClientSize = new Size(image.Width + 5,
                image.Height + 5);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(image, this.ClientRectangle);
        }



        private void InitializeComponent()
        {
            // any controls to add?
        }
    }
}
