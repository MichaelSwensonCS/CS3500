using DrawingPanel;
using GameController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Timers;
using System.Runtime.InteropServices;
using SpaceWars;

namespace Form1
{
    public partial class Form1 : Form
    {
        // The controller handles updates from the "server"
        // and notifies us via an event
        private Controller theController;
        private string passedName;
        private string typedAddress;
        
        bool leftKey = false;
        bool rightKey = false;
        bool upKey = false;
        bool fireKey = false;

        // World is a simple container for Players and Powerups
        // The controller owns the world, but we have a reference to it
        private World theWorld;

        DrawPanel drawingPanel;
        private ScorePanel scorePanel;

        public Form1(Controller ctl)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);

            theController = new Controller();
            theWorld = new World();

            // Set up the windows Form.
            // This stuff is usually handled by the drag and drop designer,
            // but it's simple enough for this lab.
            ClientSize = new Size(theController.WorldSize, theController.WorldSize);
            drawingPanel = new DrawPanel(theWorld);
            drawingPanel.Location = new Point(0, connectControlsPnl.Height + 15);
            drawingPanel.Size = new Size(this.ClientSize.Width, this.ClientSize.Height);
            drawingPanel.BackColor = Color.Black;
            panelGameHolder.Controls.Add(drawingPanel);

        }

        /********      VISUAL  COMPONENTS **********************************************************************/

        /// <summary>
        /// This is a Delegate that matches the signature of messagereceived.
        /// Every time a full message is received from the server this will be called as a delegate and the lists will be populated with 
        /// the correct objects
        /// </summary>
        /// <param name="newShips"></param>
        /// <param name="projectiles"></param>
        /// <param name="stars"></param>
        private void UpdateWorld(List<Ship> newShips, List<Projectile> projectiles, List<Star> stars)
        {
            // Don't try to redraw if the window doesn't exist yet.
            // This might happen if the controller sends an update
            // before the Form has started.
            if (!IsHandleCreated)
                return;

            lock (theWorld)
            {
                foreach (Ship ship in newShips)
                {
                        theWorld.Ships[ship.p_ID] = ship;
                }

                foreach (Projectile bullet in projectiles)
                {
                    if (!bullet.GetActive())
                        theWorld.Projectiles.Remove(bullet.Projectile_ID);
                    else
                        theWorld.Projectiles[bullet.Projectile_ID] = bullet;
                }

                foreach (Star star in stars)
                {
                    theWorld.Stars[star.p_ID] = star;
                }
            }
            // Invalidate this form and all its children
            try
            {
                MethodInvoker formInvoker = new MethodInvoker(() => this.Invalidate(true));
                this.Invoke(formInvoker);

                MethodInvoker invoker = new MethodInvoker(() => drawingPanel.Invalidate());
                this.Invoke(invoker);
            }
            catch (Exception) { }

            UpdateServer();
        }
        /// <summary>
        /// Calls controller commands and is used initiate giving the IP info and player name info
        /// to the server. Also if it connects we know we are going to have at least one player 
        /// so create the score panel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectBtn_Click(object sender, EventArgs e)
        {
            try
            {
                //Don't let them try to connect or change data while the connection is being handled
                playerNameTxtBx.Enabled = false;
                ipInfoTxtBx.Enabled = false;
                connectBtn.Enabled = false;

                passedName = playerNameTxtBx.Text;
                typedAddress = ipInfoTxtBx.Text;
                theController.Connect(UpdateWorld, typedAddress);
                theController.sendName(passedName);
            }
            catch (ArgumentException E)
            {
                MessageBox.Show(E.Message);
                Close();
            }

            panelGameHolder.Size = new Size(theWorld.WorldSize, theWorld.WorldSize);
            drawingPanel.Size = new Size(theWorld.WorldSize, theWorld.WorldSize);
            scorePanel = new ScorePanel(theWorld)
            {
                Location = new Point(drawingPanel.Width + 25, connectControlsPnl.Height + 25),
                Size = new Size(this.Width - drawingPanel.Width - 300, this.Height - 200)
            };
            this.Controls.Add(scorePanel);
        }

        /***********************************     END NONTRIVIAL VISUAL COMPONENTS **************************/




        /************************************** KEY DOWN / UP USER INPUT LOGIC AND SEND COMMANDS *******************/
        /// <summary>
        /// Based on booleans set by Key Events on this form it will send
        /// strings that correspond to commands expected by the server, this
        /// also takes into account multiple key presses.
        /// </summary>
        private void UpdateServer()
        {
            if (leftKey && !rightKey && !upKey && !fireKey)
            {
                theController.SendCommand("L");
            }
            else if (!leftKey && rightKey && !upKey && !fireKey)
            {
                theController.SendCommand("R");
            }
            else if (!leftKey && !rightKey && upKey && !fireKey)
            {
                theController.SendCommand("T");
            }
            else if (!leftKey && !rightKey && !upKey && fireKey)
            {
                theController.SendCommand("F");
            }
            else if (leftKey && rightKey && !upKey && !fireKey)
            {
                theController.SendCommand("LR");
            }
            else if (leftKey && !rightKey && upKey && !fireKey)
            {
                theController.SendCommand("LT");
            }
            else if (leftKey && !rightKey && !upKey && fireKey)
            {
                theController.SendCommand("LF");
            }
            else if (!leftKey && rightKey && upKey && !fireKey)
            {
                theController.SendCommand("RT");
            }
            else if (!leftKey && rightKey && !upKey && fireKey)
            {
                theController.SendCommand("RF");
            }
            else if (!leftKey && !rightKey && upKey && fireKey)
            {
                theController.SendCommand("TF");
            }
            else if (leftKey && rightKey && upKey && !fireKey)
            {
                theController.SendCommand("LRT");
            }
            else if (leftKey && rightKey && !upKey && fireKey)
            {
                  theController.SendCommand("LRF");
            }
            else if (!leftKey && rightKey && upKey && fireKey)
            {
                theController.SendCommand("RTF");
            }
            else if (leftKey && !rightKey && upKey && fireKey)
            {
                theController.SendCommand("LTF");
            }
        }
        /// <summary>
        /// This is used in conjunction with the <see cref="UpdateServer"/> 
        /// and <seealso cref="Form1_KeyUp(object, System.Windows.Forms.KeyEventArgs)"/>
        /// to manage key presses and convert them into server commands.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Up))
            {
                upKey = true;
            }
            if (Keyboard.IsKeyDown(Key.Right))
            {
                rightKey = true;
            }
            if (Keyboard.IsKeyDown(Key.Left))
            {
                leftKey = true;
            }
            if (Keyboard.IsKeyDown(Key.Space))
            {
                fireKey = true;
            }
        }
        /// <summary>
        /// This is used in conjunction with the <see cref="UpdateServer"/> 
        /// and <seealso cref="Form1_KeyDown(object, System.Windows.Forms.KeyEventArgs)"/>
        /// to manage key presses and convert them into server commands.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Form1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (Keyboard.IsKeyUp(Key.Up))
            {
                upKey = false;
            }
            if (Keyboard.IsKeyUp(Key.Right))
            {
                rightKey = false;
            }
            if (Keyboard.IsKeyUp(Key.Left))
            {
                leftKey = false;
            }
            if (Keyboard.IsKeyUp(Key.Space))
            {
                fireKey = false;
            }
        }

        /*****************************************   END USER INPUT **************************************/ 

        //Message boxes to match the given client.exe
        private void controlsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("UP:  Fire Thrusters\n LEFT:  Rotate left\nRIGHT: Rotate Right\nSPACE:Fire");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Michael Swenson and Seth Jackson CS3500 \n SpaceWars");
        }
    }
}
