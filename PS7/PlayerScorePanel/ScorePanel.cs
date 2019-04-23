using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceWars
{
    /// <summary>
    /// Draws the player name and score using Graphics.Draw and DrawString
    /// It is appears to update the panels based on calls to invalidate
    /// </summary>
    public class ScorePanel : FlowLayoutPanel
    {
        private World theWorld;
        public int PlayerHP { get; set; }


        /// <summary>
        /// Created with the world so that we have a list of ships
        /// Anchored so that it will stay an appropriate size when resizing
        /// </summary>
        /// <param name="w"></param>
        public ScorePanel(World w)
        {
            this.Anchor = AnchorStyles.Right;
            this.Anchor = AnchorStyles.Bottom;
            DoubleBuffered = true;
            theWorld = w;
            this.BackColor = Color.White;
        }

        private void PlayerDrawer(Ship ship, int offset, PaintEventArgs e)
        {
            int padding = 55 * offset;

            using (SolidBrush greenBrush = new SolidBrush(Color.Green))
            using (SolidBrush blackBrush = new SolidBrush(Color.Black))
            using (SolidBrush redBrush   = new SolidBrush(Color.DarkRed))
            {
                e.Graphics.DrawString($"{ship.Name} : {ship.Score}",
                                    new Font("Times new Roman", 10, FontStyle.Bold),
                                    new SolidBrush(Color.Black), 10, 2 + padding);

                Rectangle border = new Rectangle(6, (22 + padding), 180, 24);
                Rectangle lifebar = new Rectangle(8, (24 + padding), 35 * ship.HP, 20);

                e.Graphics.DrawRectangle(new Pen(blackBrush, 2), border);
                if (ship.HP == 1)
                {
                    e.Graphics.FillRectangle(redBrush, lifebar);
                }
                else
                    e.Graphics.FillRectangle(greenBrush, lifebar);
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            lock (theWorld)
            {
                int numOfPlayers = 0;

                foreach (Ship ship in theWorld.Ships.Values.OrderByDescending(ship => ship.Score))
                {
                    PlayerDrawer(ship, numOfPlayers, e);
                    numOfPlayers++;
                }
            }
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle,
                                    Color.Black, 2, ButtonBorderStyle.Outset,
                                    Color.Black, 2, ButtonBorderStyle.Outset,
                                    Color.Black, 2, ButtonBorderStyle.Outset,
                                    Color.Black, 2, ButtonBorderStyle.Outset);
            base.OnPaint(e);
        }
    }
}
