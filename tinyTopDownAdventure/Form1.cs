using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tinyTopDownAdventure
{
    public partial class Form1 : Form
        
    {
       static Image F1 = tinyTopDownAdventure.Properties.Resources.walkF1; // resource references are long so defining them as nice short variables makes tidy code
       static Image F2 = tinyTopDownAdventure.Properties.Resources.walkF2; // these are unecessary if you are happy to repeat the full reference to the resource each time
        static Image B1 = tinyTopDownAdventure.Properties.Resources.walkB1;
        static Image B2 = tinyTopDownAdventure.Properties.Resources.walkB2;
        static Image L1 = tinyTopDownAdventure.Properties.Resources.walkL1;
        static Image L2 = tinyTopDownAdventure.Properties.Resources.walkL2;
        static Image R1 = tinyTopDownAdventure.Properties.Resources.walkR1;
        static Image R2 = tinyTopDownAdventure.Properties.Resources.walkR2;
        static Image SF = tinyTopDownAdventure.Properties.Resources.f_still;
        static Image SR = tinyTopDownAdventure.Properties.Resources.r_still;
        static Image SL = tinyTopDownAdventure.Properties.Resources.l_still;
        static Image SB = tinyTopDownAdventure.Properties.Resources.b_still;
        static Image fence = tinyTopDownAdventure.Properties.Resources.fence;
        static Image coinFront = tinyTopDownAdventure.Properties.Resources.coin;   
        Image man = F1; // defines the start image for the man
        static int y = 20;  // start x and y position
        static int x = 20;
        static int newX=20;
        static int newY=20;
        static int manW =30;
        static int manH = 40;
        int coins;
        int hp = 100;
       
        struct obstacle
        {
           public Image imageName;
           public  int xLoc;
            public int yLoc;
            public int width;
            public int height;
            public Rectangle bounds;
        }

        obstacle[] obstacles = new obstacle[3]; // we have space for 3 objects in our array. These are fixed and don't change so an array is appropriate
        List<obstacle> coinList = new List<obstacle>(); // the collectables will reduce in number as they are collected - a list is appropriate
        


        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e) // the background image of grass is set in the picturebox properties as background image NOT image
        {                                                                // the background image layout is set to tile
            e.Graphics.DrawImage(man, x, y, manW , manH);       // paint the man onto the picture box (image, x_pos, y_pos, width, height)
            for (int i =0;i<3;i++)
            {
                e.Graphics.DrawImage(obstacles[i].imageName, obstacles[i].xLoc, obstacles[i].yLoc, obstacles[i].width, obstacles[i].height);
            }
            
            if (coinList.Count > 0)//check if the list is empty 
            {
                for (int i = 0; i < coinList.Count; i++)
                {

                    e.Graphics.DrawImage(coinList[i].imageName, coinList[i].xLoc, coinList[i].yLoc, coinList[i].width, coinList[i].height);
                }
            }
                        
             
        }

        

        private void Form1_KeyDown(object sender, KeyEventArgs e) // handlers for a pressed key. For this to work the form attribute KeyPreview MUST be set to true 
        {
            
                // this forces the form to handle to keypress and not any other control
           if (e.KeyCode==Keys.A) // if A pressed 
            {
                newX = x - 10;
                if (!checkCollision2())
                {
                    x = newX; //move left
                    if (man == L1)
                    {
                        man = L2;
                    }
                    else
                    { man = L1; }
                }
                x = x + 5;            
                checkCoins();
                 
                if (x<0) // if we go off the left then
                    {
                        x = pictureBox1.Width ; //come on the right --the pacman effect
                    }              
            }
           if (e.KeyCode == Keys.D) //if D pressed
            {
                newX = x + 10;
                if (!checkCollision2())
                {
                    x = newX; //move right
                    if (man == R1)
                    {
                        man = R2;
                    }
                    else
                    { man = R1; }
                }
                x=x-5;
                checkCoins();

                if (x > pictureBox1.Width) // if we go off the left then
                {
                    x =0 ; //come on the right --the pacman effect
                }
            }
            if (e.KeyCode == Keys.S) //if S pressed
            {
                newY = y + 10;
                if (!checkCollision2())
                {
                    y = newY; //move right
                    if (man == F1)
                    {
                        man = F2;
                    }
                    else
                    { man = F1; }
                }
                y = y - 5;
                checkCoins();

                if (y > pictureBox1.Height) // if we go off the bottom then
                {
                    y = 0; //come on the right --the pacman effect
                }
            }
            if (e.KeyCode == Keys.W) //if W pressed
            {
                newY = y - 10;
                if (!checkCollision2())
                {
                    y = newY; //move right
                    if (man == B1)
                    {
                        man = B2;
                    }
                    else
                    { man = B1; }
                }
                y = y + 5;//bounce back
                checkCoins();

                if (y < 0 ) // if we go off the bottom then
                {
                    y = pictureBox1.Height; //come on the right --the pacman effect
                }
            }
            pictureBox1.Refresh(); // refresh and update the picture box
            pictureBox1.Update();
            if (hp <= 0) { MessageBox.Show("Oops, You died"); }//death EOG required here
        }

        private Boolean checkCoins() //did we hit a coin?
        {
            
            RectangleF manBound = new RectangleF(x, y, manW, manH);
            if (coinList.Count > 0)//check if the list is empty 
            {
                for (int i = 0; i < coinList.Count; i++)//iterate through the coins list
                {
                    if (manBound.IntersectsWith(coinList[i].bounds))
                    {
                        coinList.RemoveAt(i);
                        coins++;
                        label4.Text = coins.ToString();
                      
                    }
                }
            }
            return true;
        }
        private Boolean checkCollision()// is an obstacle blocking the way
        { Boolean collision = false;
            RectangleF manBound = new RectangleF(x, y, 30, 40);            
                    for (int i = 0; i < 3; i++) //iterate through the obstacle array
            {
                if (manBound.IntersectsWith(obstacles[i].bounds))
                {
                    collision = true;
                }               
            }
                return collision;            
        }
        private Boolean checkCollision2()//more sophisticated collision check
        {// from https://www.jeffreythompson.org/collision-detection/rect-rect.php
            Boolean collision = false;
            for (int i =0;i<3;i++)
            {
                if (newX + manW - 10 >= obstacles[i].xLoc &&// 10 less than width to compensate for newX being 10 ahead of position
                    newX <= obstacles[i].xLoc + obstacles[i].width &&
                    newY +manH-10 >= obstacles[i].yLoc && //
                    newY <= obstacles[i].yLoc + obstacles[i].height)
                { 
                    collision = true;
                    hp--;
                    label2.Text = hp.ToString();
                }
                
            }
            return collision;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            coins = 0;
            obstacles[0] = new obstacle // building the obstacle - this is a very clunky way of doing this for 500 or so obstacles - maybe load from file using a loop
            {                           // this would allow each level to be loaded into the same framework using the same code
                imageName = fence,
                xLoc = 500,
                yLoc = 450,
                width = 48,
                height = 22,
                bounds = new Rectangle(500, 450, 48, 22)
            };            

            obstacles[1] = new obstacle
            {
                imageName = fence,
                xLoc = 350,
                yLoc = 400,
                width = 48,
                height = 22,
                bounds = new Rectangle(350, 400, 48, 22)
            };
           
            obstacles[2] = new obstacle
            {
                imageName = fence,
                xLoc = 580,
                yLoc = 300,
                width = 48,
                height = 22,
                bounds = new Rectangle(580, 300,48, 22)
            };
            


            //add some coins to the list - again better with a loop from a file
            int coinx = 400;
            for (int i = 0; i < 10; i++)
            {
                coinList.Add(new obstacle()
                {
                    imageName = coinFront,
                    xLoc = coinx,
                    yLoc = 400,
                    width = 20,
                    height = 20,
                    bounds = new Rectangle(coinx, 400, 20, 20)
                });
                coinx = coinx + 40;
            }
            
        }


        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void panel2_Click(object sender, EventArgs e)
        {

        }
    }
}
