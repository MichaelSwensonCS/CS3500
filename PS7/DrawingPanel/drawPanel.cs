/*
 * 
 * Authors: Mike Swenson, Seth Jackson
 * 
 * This class is a panel that is the visual aspect of the game "Space Wars" it draws all the objects sent by the server.
 * Much of this code is a derivative of Lab10.  There are 8 unique ship colors and projectile colors which will cycle after 8
 * players have joined.  This classes primary job is to draw the game when the form calls invalidate.
 *
 */

using DrawingPanel;
using SpaceWars;
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
    /// The "Game" part of the screen. It extends panel so that we can paint it
    /// Contains the world at the current given frame.
    /// </summary>
    public class DrawPanel : Panel
    {
        private Dictionary<int, Image> shipImages = new Dictionary<int, Image>() { };
        private World theWorld;

        public DrawPanel(World w)
        {
            DoubleBuffered = true;
            theWorld = w;
        }

        /// <summary>
        /// Helper method for DrawObjectWithTransform
        /// </summary>
        /// Kopta
        /// <param name="size">The world (and image) size</param>
        /// <param name="w">The worldspace coordinate</param>
        /// <returns></returns>
        private static int WorldSpaceToImageSpace(int size, double w)
        {
            return (int)w + size / 2;
        }

        //Kopta
        // A delegate for DrawObjectWithTransform
        // Methods matching this delegate can draw whatever they want using e  
        public delegate void ObjectDrawer(object o, PaintEventArgs e);


        /// <summary>
        /// Kopta
        /// This method performs a translation and rotation to drawn an object in the world.
        /// </summary>
        /// <param name="e">PaintEventArgs to access the graphics (for drawing)</param>
        /// <param name="o">The object to draw</param>
        /// <param name="worldSize">The size of one edge of the world (assuming the world is square)</param>
        /// <param name="worldX">The X coordinate of the object in world space</param>
        /// <param name="worldY">The Y coordinate of the object in world space</param>
        /// <param name="angle">The orientation of the objec, measured in degrees clockwise from "up"</param>
        /// <param name="drawer">The drawer delegate. After the transformation is applied, the delegate is invoked to draw whatever it wants</param>
        private void DrawObjectWithTransform(PaintEventArgs e, object o, int worldSize, double worldX, double worldY, double angle, ObjectDrawer drawer)
        {
            // Perform the transformation
            int x = WorldSpaceToImageSpace(worldSize, worldX);
            int y = WorldSpaceToImageSpace(worldSize, worldY);
            e.Graphics.TranslateTransform(x, y);
            e.Graphics.RotateTransform((float)angle);
            // Draw the object 
            drawer(o, e);
            // Then undo the transformation
            e.Graphics.ResetTransform();
        }

        /// <summary>
        /// Acts as a drawing delegate for DrawObjectWithTransform
        /// After performing the necessary transformation (translate/rotate)
        /// DrawObjectWithTransform will invoke this method
        /// </summary>
        /// <param name="obj">The object to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void ShipDrawer(object obj, PaintEventArgs e)
        {
            Ship shipToDraw = obj as Ship;

            int width = 40;
            int height = 40;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Rectangle shipRectangle = new Rectangle(-(width / 2), -(height / 2), width, height);

            //Helper method gets the appropriate image based on ship id
            e.Graphics.DrawImage(getShipImageFromID(shipToDraw), shipRectangle);
        }


        /// <summary>
        /// Acts as a drawing delegate for DrawObjectWithTransform
        /// After performing the necessary transformation (translate/rotate)
        /// DrawObjectWithTransform will invoke this method
        /// </summary>
        /// <param name="o">The object to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void ProjectileDrawer(object o, PaintEventArgs e)
        {
            Projectile p = o as Projectile;

            int width = 10;
            int height = 20;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            {
                // Rectangles are drawn starting from the top-left corner.
                // So if we want the circle centered on the powerup's location, we have to offset it
                // by half its size to the left (-width/2) and up (-height/2)
                Rectangle r = new Rectangle(-(width / 2), -(height / 2), width, height);

                e.Graphics.DrawImage(getImageFromIDProj(p), r);
            }
        }

        /// <summary>
        /// Acts as a drawing delegate for DrawObjectWithTransform
        /// After performing the necessary transformation (translate/rotate)
        /// DrawObjectWithTransform will invoke this method
        /// </summary>
        /// <param name="o">The object to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void StarDrawer(object o, PaintEventArgs e)
        {
            Star p = o as Star;

            int width = 55;
            int height = 55;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            {
                //Kopta
                // Rectangles are drawn starting from the top-left corner.
                // So if we want the circle centered on the powerup's location, we have to offset it
                // by half its size to the left (-width/2) and up (-height/2)
                Rectangle r = new Rectangle(-(width / 2), -(height / 2), width, height);

                e.Graphics.DrawImage(Resource1.star, r);
            }
        }

        /// <summary>
        /// This method essentially updates the visual world, this method is called
        /// by invoking invalidate either specifically on this component or one of its
        /// parents
        /// </summary>
        /// <param name="e">Paint Event</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            lock (theWorld)
            {
                // Draw the Ships
                foreach (Ship ship in theWorld.Ships.Values)
                {
                    DrawObjectWithTransform(e, ship, this.Size.Width, ship.Location.GetX(), ship.Location.GetY(), ship.Direction.ToAngle(), ShipDrawer);
                }

                // Draw the Projectiles
                foreach (Projectile bullet in theWorld.Projectiles.Values)
                {
                    if (theWorld.Ships.ContainsKey(bullet.Owner))
                    {
                        Vector2D angle = theWorld.Ships[bullet.Owner].Direction;
                        DrawObjectWithTransform(e, bullet, this.Size.Width, bullet.Location.GetX(), bullet.Location.GetY(), angle.ToAngle(), ProjectileDrawer);

                    }
                }
                //Draw the Stars
                foreach (Star star in theWorld.Stars.Values)
                {
                    DrawObjectWithTransform(e, star, this.Size.Width, star.Location.GetX(), star.Location.GetY(), 0, StarDrawer);
                }
            }
            // Do anything that Panel (from which we inherit) needs to do
            base.OnPaint(e);
        }


        /************************     REFACTORED HELPER METHODS ****************************************/


        /// <summary>
        /// Helper method that checks a shipsID to decide which color ship it should be
        /// and also is used to draw an explosion if the ship is killed
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private Image getShipImageFromID(Ship p)
        {
            Image findImage = Resource1.ship_coast_blue;
            if (p.HP < 1)
            {
                findImage = Resource1.explosion;
                return findImage;
            }

            switch (p.p_ID % 8)
            {
                //Blue
                case 0:
                    if (!p.Thrust)
                        findImage = Resource1.ship_coast_blue;
                    else
                        findImage = Resource1.ship_thrust_blue;
                    break;
                //Brown
                case 1:
                    if (!p.Thrust)
                        findImage = Resource1.ship_coast_brown;
                    else
                        findImage = Resource1.ship_thrust_brown;
                    break;
                //Green
                case 2:
                    if (!p.Thrust)
                        findImage = Resource1.ship_coast_green;
                    else
                        findImage = Resource1.ship_thrust_green;
                    break;
                //Grey
                case 3:
                    if (!p.Thrust)
                        findImage = Resource1.ship_coast_grey;
                    else
                        findImage = Resource1.ship_thrust_grey;
                    break;
                //Red
                case 4:
                     if (!p.Thrust)
                        findImage = Resource1.ship_coast_red;
                    else
                        findImage = Resource1.ship_thrust_red;
                    break;
                //Violet
                case 5:
                    if (!p.Thrust)
                        findImage = Resource1.ship_coast_violet;
                    else
                        findImage = Resource1.ship_thrust_violet;
                    break;
                //White
                case 6:
                    if (!p.Thrust)
                        findImage = Resource1.ship_coast_white;
                    else
                        findImage = Resource1.ship_thrust_white;
                    break;
                //Yellow
                case 7:
                    if (!p.Thrust)
                        findImage = Resource1.ship_coast_yellow;
                    else
                        findImage = Resource1.ship_thrust_yellow;
                    break;
                default:
                    Console.WriteLine("FIND IMAGE DEFAULT CASE");
                    break;
            }

            return findImage;
        }

        /// </summary
        /// Helper Method that assigns the correct projectile image resource to the same color ship
        /// <param name="p"></param>
        /// <returns></returns>
        private Image getImageFromIDProj(Projectile p)
        {
            Image findImage = Resource1.ship_coast_blue;
            switch (p.Owner % 8)
            {
                case 0:
                    findImage = Resource1.shot_blue;
                    break;
                case 1:
                    findImage = Resource1.shot_brown;
                    break;
                case 2:
                    findImage = Resource1.shot_green;
                    break;
                case 3:
                    findImage = Resource1.shot_grey;
                    break;
                case 4:
                    findImage = Resource1.shot_red;
                    break;
                case 5:
                    findImage = Resource1.shot_violet;
                    break;
                case 6:
                    findImage = Resource1.shot_white;
                    break;
                case 7:
                    findImage = Resource1.shot_yellow;
                    break;
                default:
                    Console.WriteLine("FIND Projectile DEFAULT CASE");
                    break;
            }
            return findImage;
        }
    }
}
