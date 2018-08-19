/*
 * CSCI473  Assignment 6
 * 
 * Charles Andoni, Aleena Ayaz
 * 
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;


namespace ArrangePicture
{
    public partial class Form1 : Form
    {
        /*
         * Assumption: all images in the pics array will have size 
         *              pixels width x pixels height represented as
         *              
         *              (100 * n) x (100 * n)   where n >= 0
         *              
         * All puzzles will have to obey this rule about the width and height in pixels 
         * of the images. 
         * 
         * Integer n is allowed to differ for each instance of Bitmap
         * 
         */
        public static Bitmap[] pics = { Properties.Resources.moby300x300, Properties.Resources.apples };
        static int cur_pic = 0; // current index in pics array
        public static Bitmap src_img = pics[cur_pic]; // image to be displayed as puzzle
        static int M_ROWS = pics[cur_pic].Size.Width / 100; // number of rows in the puzzle
        static int M_COLS = pics[cur_pic].Size.Height / 100; // number of cols in the puzzle
        static ImageBlock[,] Positions; // Represents the puzzle by 2d array of ImageBlock
        static Viewer viewer; // picture in top of corner than can be CLICKED to view how puzzle looks if complete
            
        static int OFFSET_X = 200;
        static int OFFSET_Y = 100;
        public static int timeLeft = 5;

        public Form1()
        {
            InitializeComponent();
        }

        /* Loads a new Puzzle and sets up the window 
         *
         */
        private void WindowInit()
        {
            // set window size according to the size of the Bitmap
            this.ClientSize = new Size((M_COLS * 100) + OFFSET_X, (M_ROWS * 100) + OFFSET_Y);
            foreach (Control ctrl in this.Controls)
            {
                // shift all ImageBlock objects 100 pixels to the right
                if (ctrl.GetType() == typeof(ImageBlock))
                {
                    ctrl.Location = new Point(ctrl.Location.X + (OFFSET_X / 2), ctrl.Location.Y);
                }
            }

            // positions the buttons
            btnClose.Size = new Size(75, 50);
            btnClose.Location = new Point(100, ClientSize.Height - 75);
            btnClose.Text = "Exit";
            btnNext.Size = new Size(75, 50);
            btnNext.Location = new Point((ClientSize.Width / 2) - 75 / 2, ClientSize.Height - 75);
            btnNext.Text = "Next Puzzle";
            button1.Size = new Size(75, 50);  // this is the Hint button
            button1.Location = new Point(btnNext.Location.X + (btnNext.Location.X - btnClose.Location.X), ClientSize.Height - 75);

            // create a "Viewer" in the corner the user can click on 
            // to see a Form that shows what the completed puzzle should look like
            if (cur_pic == 0)
            {
                viewer = new Viewer(src_img);
                this.Controls.Add(viewer.PictureBoxControl);
            }
            else // if viewer already exists then reset the image
            {
                viewer.CurrentImage = src_img;
            }


            // set up the countdown in the beginning, user will wait 5 seconds
            // and then the puzzle will scramble up
            timeLeft = 5;
            pictureBox1.Visible = true;
            pictureBox1.Paint += new PaintEventHandler(Paint_CountDown);
            pictureBox1.Size = new Size(src_img.Width, src_img.Height);
            pictureBox1.Location = new Point(OFFSET_X / 2, 0);
            pictureBox1.Image = src_img;
            pictureBox1.BringToFront();
            timer1.Tag = "countdown";
            timer1.Enabled = true;
        }

        /* Initialize the board uusing the ImageBlock[,] Positions 2d array
         * 
         */
        private void BoardInit()
        {
            Positions = new ImageBlock[M_ROWS, M_COLS];
            int y = 0, IncrementValue = 100;
            for (int row = 0; row < M_ROWS; row++)
            {
                for (int col = 0; col < M_COLS; col++)
                {
                    Positions[row, col] = new ImageBlock(
                        new Rectangle(IncrementValue * col, y, 100, 100),
                        new Rectangle(0, 0, 100, 100),
                        src_img
                       );
                    Positions[row, col].Location = new Point(
                        IncrementValue * col, y);
                    Positions[row, col].Size = new Size(100, 100);

                    // set initial x and y values NO OFFSET VALUES
                    // this is important because of how another method will
                    // test for a compelete puzzle
                    Positions[row, col].InitialX = IncrementValue * col;
                    Positions[row, col].InitialY = y;

                    this.Controls.Add(Positions[row, col]);
                    Positions[row, col].MouseClick += mouseClick;
                }

                y += IncrementValue;
            }
        }

        /* Scramble the puzzle
         * Basically re-arranges the instances of ImageBlock inside
         * the Positions 2d array
         */
        private void Scramble()
        {
            Random rand = new Random();
            bool[,] marked = new bool[M_ROWS, M_COLS];
            int x, y, xpos, ypos;

            foreach (ImageBlock block in Positions)
            {
                x = rand.Next(0, M_COLS);
                y = rand.Next(0, M_ROWS);
                xpos = block.Location.X / 100;
                ypos = block.Location.Y / 100;

                // Keep in mind that this "swaps" by swapping the
                // Location properties of the two ImageBlocks.
                // The ImageBlock instance in the 2D Array Positions[,]
                // remains the same.
                Point pos1 = block.Location;
                Point pos2 = Positions[x, y].Location;
                block.Location = pos2;
                Positions[x, y].Location = pos1;
            }
        }

        /* Swaps the locations of the two 
         * selected instances of ImageBlock 
         * within the Positions 2d array
         * 
         */
        public void Swap(ImageBlock selected1, ImageBlock selected2)
        {
            // if blocks too far
            if (selected1.Location.X > selected2.Location.X + 100
                || selected1.Location.X < selected2.Location.X - 100
                || selected1.Location.Y > selected2.Location.Y + 100
                || selected1.Location.Y < selected2.Location.Y - 100)
            {
                // too far
                return;
            }

            // confirm that there is no diagonal, if there is then return
            // from the method immediately
            if ((selected1.Location.X + 100 == selected2.Location.X
                || selected1.Location.X - 100 == selected2.Location.X)
                && (selected1.Location.Y + 100 == selected2.Location.Y
                || selected1.Location.Y - 100 == selected2.Location.Y))
            {
                // diagonal so return from method
                return;
            }

            // first check if blocks are adjacent
            if (selected1.Location.X + 100 == selected2.Location.X
                || selected1.Location.X - 100 == selected2.Location.X
                || selected1.Location.Y + 100 == selected2.Location.Y
                || selected1.Location.Y - 100 == selected2.Location.Y)
            {
                // is adjacent 
                // so swap
                selected1.IsSelected = false;
                selected2.IsSelected = false;
                Point pos1 = selected1.Location;
                Point pos2 = selected2.Location;
                selected1.Location = pos2;
                selected2.Location = pos1;

                // if upon swap you completed the puzzle then tell the user that
                // and then load the next puzzle with NextPicture()
                if (PuzzleComplete())
                {
                    MessageBox.Show("Good job! Picture is sorted!");
                    NextPicture();
                }
            }
            else
            {
                return; // is not adjacent 
            }
        }

        /* check if puzzle is sorted correctly */
        private bool PuzzleComplete()
        {
            int initX, initY, curX, curY;
            for (int i = 0; i < M_ROWS; i++)
            {
                for (int j = 0; j < M_COLS; j++)
                {
                    // try to see if current x and y values match
                    // the ImageBlock's initial x and y values
                    initX = Positions[i, j].InitialX;
                    initY = Positions[i, j].InitialY;
                    curX = Positions[i, j].Location.X - (OFFSET_X / 2); // remove amount OFFSET_X that WindowInit method had added to X
                    curY = Positions[i, j].Location.Y;

                    // test if location is at starting place
                    if (initX != curX || initY != curY)
                        return false; // Puzzle incomplete
                }
            }
            // all the Blocks are in correct position!
            return true;
        }

        /* Loads the next image 
         * 
         * Case 1: loads the next image and initializes the new window settings
         * Case 2: if this was the last image in the pics array then you're done 
         *          and tell the user that
         */
        private void NextPicture()
        {
            cur_pic++;
            if (cur_pic != pics.Length)
            {
                // loading next image

                // remove the previous ImageBlocks from Positions
                // they wil be replaced when BoardInit() is called
                foreach (ImageBlock block in Positions)
                {
                    block.Dispose();
                }

                src_img = pics[cur_pic];
                M_ROWS = pics[cur_pic].Size.Width / 100;
                M_COLS = pics[cur_pic].Size.Height / 100;
                BoardInit(); // replace the ImageBlocks
                WindowInit(); // set up window to accomodate the image size
            }
            else
            {
                MessageBox.Show("No images left in the array.");
            }
        }

        /*
         * FORM1 ON LOAD METHOD
         * 
         * set up the puzzle and window settings
         */
        private void Form1_Load(object sender, EventArgs e)
        {
            BoardInit();
            WindowInit();
        }

        /*
         * Draws a number that will decrement in value and timer will
         * stop calling it once integer timeLeft == 0
         */ 
        private void Paint_CountDown(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (timeLeft > 0)
            {
                using (Font font = new Font("Arial", 75, FontStyle.Bold))
                {
                    g.DrawString(timeLeft.ToString(), font, Brushes.Black,
                        new Point(100, 100)); //pictureBox1.Width / M_COLS, pictureBox1.Height / M_ROWS);
                }
            }
        }

        /* when one of the ImageBlock's are clicked on the screen
         * selected that block
         * 
         * user must deselect block manually
         * ONLY ADJACENT BLOCKS SWAP! 
         */
        private void mouseClick(object sender, MouseEventArgs e)
        {
            List<ImageBlock> Elements = new List<ImageBlock>();
            foreach (ImageBlock block in Positions)
                Elements.Add(block);
            var SelectedBlocks = Elements.Where(a => a.IsSelected).ToList();
            if (SelectedBlocks.Count == 2)
            {
                // exactly two blocks selected
                Swap(SelectedBlocks[0], SelectedBlocks[1]);
            }
            else if (SelectedBlocks.Count > 2)
            {
                ImageBlock block = (ImageBlock)sender;
                block.IsSelected = false;
            }
        }

        /* Represents a "Hint button".
         * 
         * When clicked this will highlight green all those ImageBlock's currently
         * in the correct location of the 2d array
         * 
         * the green highlight will only last for a few seconds and is 
         * not drawn permanently
         * 
         */
        private void button1_Click(object sender, EventArgs e)
        {
            if (PuzzleComplete())
            {
                MessageBox.Show("Picture is correct!");
                return;
            }
            else
            {
                // show blocks in correct position

                int initX, initY, curX, curY;
                for (int i = 0; i < M_ROWS; i++)
                {
                    for (int j = 0; j < M_COLS; j++)
                    {
                        // try to see if current x and y values match
                        // the ImageBlock's initial x and y values
                        initX = Positions[i, j].InitialX;
                        initY = Positions[i, j].InitialY;
                        curX = Positions[i, j].Location.X - (OFFSET_X / 2); // remove amount OFFSET_X that WindowInit method had added to X
                        curY = Positions[i, j].Location.Y;

                        // test if location is at starting place
                        if (initX == curX && initY == curY)
                        {
                            // peak at correct positions
                            Positions[i, j].IsSelected = false;
                            Positions[i, j].IsCorrect = true;
                        }
                    }
                }

                timer1.Interval = 2500;
                timer1.Tag = "searching";
                timer1.Start();
            }
        }

        /*
         * 
         */
        private void timer1_Tick(object sender, EventArgs e)
        {
            
            // changes the colors of the ImageBlock's back to normal after Hint 
            // button is pressed
            if (timer1.Tag.Equals("searching"))
            {
                //timer1.Tag = null;
                timer1.Stop();
                foreach (ImageBlock block in Positions)
                    block.IsCorrect = false;
            }
            // for drawing the countdown
            else if (timeLeft > 1)
            {
                // display new time left
                // by updating the time left label
                btnNext.Enabled = false;
                button1.Enabled = false;
                timeLeft = timeLeft - 1;
                pictureBox1.Invalidate();
            }
            else
            {
                // timeLeft is <= 0 so now the timer stops
                timer1.Stop();
                btnNext.Enabled = true;
                button1.Enabled = true;
                pictureBox1.Visible = false;
                Scramble();

            }
        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
                //
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            NextPicture();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
