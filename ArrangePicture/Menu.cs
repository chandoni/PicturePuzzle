/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;


THIS IS NOT USED IN THE PROJECT

namespace ArrangePicture
{
    class Menu : Panel
    {
        private Panel canvas;
        private Timer timer;
        private int dy, dh;

        public Menu() { }
        public Menu(int x, int y, int w, int h)
        {
            timer = new Timer();
            this.Location = new Point(x, y); dy = 100 - 20;
            this.Size = new Size(w, h); dh = 20;
            canvas = new Panel();
            canvas.Location = new Point(0, 0);
            canvas.Size = new Size(w, h);
            canvas.Paint += new PaintEventHandler(canvas_OnPaint);
            canvas.MouseClick += new MouseEventHandler(canvas_MouseClick);

            this.Controls.Add(canvas);
            canvas.Invalidate();
            OnLoad();
            timer.Interval = 15;
            timer.Tick += new EventHandler(timer_UpdateCanvas);
            timer.Start();
        }

        private void canvas_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (timer.Tag.Equals("open"))
                {
                    timer.Tag = "down";
                    timer.Enabled = true;
                    timer.Start();
                }
                else if (timer.Tag.Equals("close"))
                {
                    timer.Tag = "up";
                    timer.Enabled = true;
                    timer.Start();
                }
                else
                    return;
            }
        }

        private void canvas_OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            //Pen pen = new Pen(Color.Red);
            SolidBrush brush = new SolidBrush(Color.DarkOrange);
            g.FillRectangle(brush, 0, dy, canvas.Width, dh);
       
            if (dh >= 100)
            {
                 //canvas.BorderStyle = BorderStyle.Fixed3D;
                 //ControlPaint.DrawBorder3D(g, canvas.ClientRectangle, Border3DStyle.Raised, Border3DSide.All);
                
                ControlPaint.DrawBorder(g, canvas.ClientRectangle,
                    SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
                    SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
                    SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
                    SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset);
            }
        }

        private void OnLoad()
        {
            timer.Tag = "up";
            timer.Enabled = true;
            timer.Start();
        }

        private void timer_UpdateCanvas(object myObject, EventArgs myEventArgs)
        {
            if (timer.Tag.Equals("up"))
            {
                if (dh >= 100)
                {
                    timer.Tag = "open";
                    timer.Enabled = false;
                    timer.Stop();
                    canvas.Invalidate();
                    
                    return;
                }
                else
                {
                    dy -= 2;
                    dh += 2;
                    canvas.Invalidate();
                    return;
                }
            }

            if (timer.Tag.Equals("down"))
            {
                if (dh <= 20)
                {
                    timer.Tag = "close";
                    timer.Enabled = false;
                    timer.Stop();
                    canvas.Invalidate();
                    return;
                }
                else
                {
                    dy += 2;
                    dh -= 2;
                    canvas.Invalidate();
                    return;
                }
            }
        }
    }
}*/
