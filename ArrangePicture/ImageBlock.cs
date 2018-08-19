using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace ArrangePicture
{
    public class ImageBlock : Control
    {
        // initial position
        private int initialX;
        private int initialY;

        public ImageBlock(Rectangle source, Rectangle destination, Image img)
        {
            image = img;
            srcRec = source;
            destRec = destination;
        }

        public Rectangle SrcRectangle { get { return srcRec; } set { srcRec = value; } }
        public Rectangle DestRectangle { get { return destRec; } set { destRec = value; } }
        public bool IsSelected { get { return selected; } set { selected = value; this.Invalidate(); } }
        public int InitialX { get { return initialX; } set { initialX = value; } }
        public int InitialY { get { return initialY; } set { initialY = value; } }
        public bool IsCorrect { get { return correct; } set { correct = value; this.Invalidate(); } }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (correct == false)
            {
                base.OnMouseEnter(e);
                hover = true;
                this.Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (correct == false)
            {
                base.OnMouseLeave(e);
                hover = false;
                this.Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && !selected)
            {
                selected = true;
                Invalidate();
            }
            else
            {
                selected = false;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // draw the image 
            DrawImage(e.Graphics);
            if (selected)
            {
                hover = false;
                // set colors and border for when control is selected
                selectionColor = Color.FromArgb(50, 245, 245, 220);
                selectionBorderColor = SystemColors.ControlDark;
                DrawSelectionRectangle(e.Graphics);
            }
            if (!selected)
            {
                // set hover colors and border
                selectionColor = Color.FromArgb(50, 0, 0, 150);
                selectionBorderColor = SystemColors.Highlight;
                DrawSelectionRectangle(e.Graphics);
            }
            if (correct)
            {
                hover = false;
                selectionColor = Color.FromArgb(50, 0, 255, 0);
                selectionBorderColor = SystemColors.Highlight;
                DrawSelectionRectangle(e.Graphics);
            }
        }

        private void DrawSelectionRectangle(Graphics graphics)
        {
            if (hover || selected || correct)
            {
                using (var brush = new SolidBrush(selectionColor))
                {
                    graphics.FillRectangle(brush, this.ClientRectangle);
                }
                using (var pen = new Pen(selectionBorderColor))
                {
                    pen.Width = 1;
                    graphics.DrawRectangle(pen,
                        new Rectangle()
                        {
                            Width = (this.Width - 1),
                            Height = (this.Height - 1)
                        });
                }
            }
        }

        private void DrawImage(Graphics graphics)
        {
            GraphicsUnit units = GraphicsUnit.Pixel;
            if (image != null)
                graphics.DrawImage(image, destRec, srcRec, units);
        }


        // states
        private bool hover = false;
        private bool selected = false;
        private bool correct = false;

        Image image;

        private Rectangle destRec;
        private Rectangle srcRec;

        private Color selectionColor = Color.FromArgb(50, 0, 0, 150);
        private Color selectionBorderColor = SystemColors.Highlight;
    }
}
