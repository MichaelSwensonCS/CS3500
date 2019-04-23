/*
 * 
 * Authors: Michael Swenson and Seth Jackson
 * 
 * This class allows people to connect via a TCP listener to a game server and the server sends them
 * world objects that are updated by the server according to the commands sent by
 * the clients.  The server settings are located in a file called "settings.xml" that
 * is located in the bin/debug folder.
 * 
 */

using NetworkController;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

/// <summary>
/// Receives players, generates ships for those players, and provides a world to those
/// players that calculates thrust, gravity, location, projectiles and scores.  It then
/// updates the world and sends an updated world via JSON serializing
/// </summary>
namespace SpaceWars
{
    /// <summary>
    /// Class that provides readability for error handling in the xml reader
    /// </summary>
    public class SettingsReaderException : Exception
    {
        public SettingsReaderException(string msg) : base(msg)
        {

        }
    }

    /// <summary>
    /// Provides information about a spacewars world to clients that connect to it.  It also
    /// reads the initial settings for the world from an xml file called settings.xml
    /// This file also allows for a different game mode to be enabled requiring no change 
    /// on the clients end
    /// </summary>
    public class Server
    {
        //These dictionaries are a way of storing commands sent by the clients, it prevents multiple commands from being 
        //updated when the world updates
        private Dictionary<int, string> thrustCommands;
        private Dictionary<int, string> rightCommands;
        private Dictionary<int, string> leftCommands;
        private Dictionary<int, string> shootCommands;

        private Random rand = new Random();

        //Fields set by the XML on server creation
        private int worldSize = 0;
        private int MSPerFrame = 0;
        private int FramesPerShot = 0;
        private int RespawnRate = 0;
        private bool movingStar = false;

        private double x = 0;
        private double y = 0;
        private double mass = 0;

        private double EngineStrength = .02;

        private static int starCounter = 0;

        //Used to accurately time world updates based on MSperFrame
        private Stopwatch sendWorldWatch = new Stopwatch();

        private World theWorld;
        public World World { get => theWorld; }

        private List<SocketState> clients;
        private TcpListener listener;

        private bool canFire = true;
        private int bulletID;

        private bool movingRight = true;
        private bool movingUp = false;


        /// <summary>
        /// Entry point for the server,
        /// starts the server and lets the user know it has started.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            Server server = new Server();
            server.StartServer();
            Console.Read();
        }


        /// <summary>
        /// Creates a new TcpListener and and new list to store clients as socketstates.
        /// Additionally it reads the state of the game world from an xml file and creates
        /// the initial world for the client(s)
        /// </summary>
        public Server()
        {
            thrustCommands = new Dictionary<int, string>();
            rightCommands = new Dictionary<int, string>();
            leftCommands = new Dictionary<int, string>();
            shootCommands = new Dictionary<int, string>();

            listener = new TcpListener(IPAddress.Any, Networking.DEFAULT_PORT);

            clients = new List<SocketState>();

            //START XML READER HERE  read in settings.xml Our Extra game mode is controlled by an attribute called Moving Star"
            try
            {
                using (XmlReader xReader = XmlReader.Create("settings.xml"))
                {
                    while (xReader.Read())
                    {
                        if (xReader.IsStartElement())
                        {
                            switch (xReader.Name)
                            {
                                //After we get the world size we can create the basic world
                                //Should be the first attribute in the file
                                case "UniverseSize":
                                    xReader.Read();
                                    Int32.TryParse(xReader.Value, out worldSize);
                                    theWorld = new World(worldSize);
                                    break;
                                case "MSPerFrame":
                                    xReader.Read();
                                    Int32.TryParse(xReader.Value, out MSPerFrame);
                                    break;
                                case "FramesPerShot":
                                    xReader.Read();
                                    Int32.TryParse(xReader.Value, out FramesPerShot);
                                    break;
                                case "RespawnRate":
                                    xReader.Read();
                                    Int32.TryParse(xReader.Value, out RespawnRate);
                                    break;
                                case "MovingStar":
                                    xReader.Read();
                                    if (xReader.Value == "True" || xReader.Value == "true" || xReader.Value == "T")
                                    {
                                        movingStar = true;
                                    }
                                    break;
                                case "Star":
                                    break;
                                case "x":
                                    xReader.Read();
                                    Double.TryParse(xReader.Value, out x);
                                    break;
                                case "y":
                                    xReader.Read();
                                    Double.TryParse(xReader.Value, out y);
                                    break;
                                //Now we have enough information to create a star and add it to the world
                                //that should've been created after the first attribute
                                case "mass":
                                    xReader.Read();
                                    Double.TryParse(xReader.Value, out mass);
                                    Star star = new Star(starCounter++, new Vector2D(x, y), mass);
                                    theWorld.Stars.Add(star.p_ID, star);
                                    break;
                            }
                        }
                    }

                }

            }
            //Our custom class that makes catching multiple errors more readable
            catch (SettingsReaderException e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Lets the user know that the client has started and starts the event loop for
        /// connecting clients
        /// </summary>
        public void StartServer()
        {
            Console.WriteLine("Server Awaiting first Client");

            Networking.ServerAwaitingClientLoop(HandleNewClient);
            while (true) updateWorld();
        }

        /// <summary>
        /// Switch the delegate to Recieve name to get the player name
        /// </summary>
        /// <param name="ss"></param>
        private void HandleNewClient(SocketState ss)
        {
            ss.callMe = ReceiveName;
            Networking.GetData(ss);
        }
        /// <summary>
        /// Receive the player name, create a ship with their name
        /// and assign them an ID, send them their ID and the world size.
        /// Change their socketstate delegate to ProcessMessage (which will handle incoming commands)
        /// </summary>
        /// <param name="ss"></param>
        private void ReceiveName(SocketState ss)
        {
            string totalData = ss.sb.ToString();

            string[] parts = Regex.Split(totalData, @"(?<=[\n])");

            if (parts[0].Length == 0)
            {
                throw new Exception("You need to type a name.");
            }
            // The regex splitter will include the last string even if it doesn't end with a '\n',
            // So we need to ignore it if this happens. 
            if (parts[0][parts[0].Length - 1] != '\n')
            {
                return;
            }

            //Set the location of the ship to a random spot inside the world
            Vector2D location = new Vector2D(rand.Next(-worldSize / 2, worldSize / 2), rand.Next(-worldSize / 2, worldSize / 2));

            //This is how the given client behaves, on creating/respawn a ship is pointing upwards
            Vector2D direction = new Vector2D(0, -1);

            //All direction vectors must be normalized
            direction.Normalize();

            //Remove the newline from name so it appears normal on the scoreboard (This is one issue we fixed)
            parts[0] = parts[0].Remove(parts[0].Length - 1);

            lock (theWorld)
            {
                Ship newShip = new Ship(ss.ID, parts[0], location, direction);
                theWorld.Ships.Add(newShip.p_ID, newShip);
            }

            //Get Ready to start handling user input
            ss.callMe = ProcessMessage;
            Networking.Send(ss.theSocket, ss.ID + "\n" + theWorld.WorldSize + "\n");

            //Add the new player to the list of clients, it must be locked because we don't know when players might be joining
            lock (clients)
            {
                clients.Add(ss);
            }

            Networking.GetData(ss);
        }

        /// <summary>
        /// This is handles all game mechanics based on user input and sends them
        /// an updated version of the world once every MSPerFrame. It also restricts
        /// how often a ship can fire and when and where a ship will respawn.
        /// </summary>
        private void updateWorld()
        {
            sendWorldWatch.Start();
            while (sendWorldWatch.ElapsedMilliseconds < MSPerFrame)
            {

            }
            sendWorldWatch.Reset();
            StringBuilder worldUpdatesSB = new StringBuilder();

            //We must lock because we are enumerating over every object in the world below
            lock (theWorld)
            {
                foreach (Ship ship in theWorld.Ships.Values)
                {
                    //Ship can only thrust if it is alive
                    if (ship.HP > 0)
                    {
                        //Used to calculate final position after thrust and gravity has been applied
                        Vector2D totalAcceleration = new Vector2D(0, 0);

                        //It has been another frame since the ship has fired
                        ship.fireCounter++;


                        /*********************************** THRUST COMMAND START ************************/

                        if (thrustCommands.ContainsKey(ship.p_ID))
                        {
                            if (thrustCommands[ship.p_ID] != "")
                            {
                                totalAcceleration = handleThrustCalculations(ship, totalAcceleration);
                            }
                            //Turn thrust off if it isn't a command
                            else ship.Thrust = false;

                            //Clear all thrust commands of the current ship ID
                            thrustCommands[ship.p_ID] = "";
                        }
                        /****************************  END THRUST COMMANDS *************************************/





                        /***************************** TURNING COMMANDS (not Alan) ******************/

                        if (rightCommands.ContainsKey(ship.p_ID) && leftCommands.ContainsKey(ship.p_ID))
                            HandleAmbigousLeftAndRightRotation(ship);

                        if (rightCommands.ContainsKey(ship.p_ID))
                        {
                            if (rightCommands[ship.p_ID] != "")
                            {
                                ship.Direction.Rotate(2); //Default value, and it isn't included in the xml 

                                //Clear commands after the commands logic has been applied
                                rightCommands[ship.p_ID] = "";
                            }
                        }

                        if (leftCommands.ContainsKey(ship.p_ID))
                        {
                            if (leftCommands[ship.p_ID] != "")
                            {
                                ship.Direction.Rotate(-2); //Default value, and it isn't included in the xml 

                                //Clear commands after the commands logic has been applied
                                rightCommands[ship.p_ID] = "";
                            }
                        }
                        /*************** END TURNING ***********************************************/






                        /************************ START SHOOTING COMMANDS ****************************/

                        if (shootCommands.ContainsKey(ship.p_ID) && (ship.fireCounter >= FramesPerShot))
                        {
                            if (shootCommands[ship.p_ID] != "")
                            {
                                bulletID++;
                                Vector2D bulletDirection = ship.Direction * 1; //*1 prevents bullet direction from equaling ship direction (no curving bullets)

                                theWorld.Projectiles.Add(bulletID, new Projectile(bulletID, ship.Location, bulletDirection, true, ship.p_ID));

                                shootCommands[ship.p_ID] = "";
                            }
                            //Reset the ship fire counter so it can't immediately shoot again
                            ship.fireCounter = 0;
                        }
                        /************************* END SHOOTING COMMANDS **********************/





                        /************* STAR COLLISION AND GRAVITY *****************************/
                        foreach (Star star in World.Stars.Values)
                        {
                            totalAcceleration = AddGravityToTotal(ship, totalAcceleration, star);

                            StarCollisionCheck(ship, star);

                            //Extra Mode Logic
                            //Just one star will wrap and behaves fairly linearly, multiple stars all share the same bool so the movement will be much harder for players to predict movement and you can play it
                            //like a "dodgeball" kind of game, if there are no ships alive the stars will not move; additionally the stars move faster and faster every time they
                            //wrap until a ship dies either from other players or star collision (Recommended 2 stars)
                            if (clients.Count > 0)
                            {
                                if (movingStar && movingRight)
                                {
                                    star.Location = star.Location + new Vector2D(2, 0);
                                }
                                if (movingStar && movingUp)
                                {
                                    star.Location = star.Location + new Vector2D(0, -2);
                                }
                                checkForReflection(star);
                                worldUpdatesSB.Append(JsonConvert.SerializeObject(star) + "\n");
                            }
                        }
                        checkForReflection(ship);
                        //Final update of ships new calculated location
                        ship.Location = ship.Location + ship.Velocity;




                        /***********************  PROJECTILE COLLISIONS ***************************/

                        foreach (Projectile bullet in World.Projectiles.Values)
                        {
                            ProjectileCollisionHandler(ship, bullet);

                            //Nested for loops shouldn't be an issue because we will never have a ton of projectiles or stars (Game play mechanics)
                            foreach (Star star in World.Stars.Values)
                            {
                                BulletCollisionWithStar(bullet, star);
                            }
                        }
                        /************************ END PROJECTILE COLLISIONS ************************/
                    } 
                    

                    //Ships have been handled add them to the world
                    worldUpdatesSB.Append(JsonConvert.SerializeObject(ship) + "\n");
                } //End ship updates



                /********************** PROJECTILE MOVEMENT AND BULLET LIFE *************************/
                Dictionary<int, Projectile> bullets = new Dictionary<int, Projectile>(theWorld.Projectiles);
                foreach (Projectile bullet in bullets.Values)
                {
                    if (!CheckBulletAtEdgeOfWorld(bullet))
                    {
                        continue;
                    }
                    if (!bullet.Alive && bullet.deadCount >= 100) theWorld.Projectiles.Remove(bullet.Projectile_ID); // remove bullets
                    bullet.deadCount++;

                    //Bullet Movement
                    bullet.Location = bullet.Location + new Vector2D(bullet.Direction * 15);

                    //Add projectiles to world
                    worldUpdatesSB.Append(JsonConvert.SerializeObject(bullet) + "\n");
                }
                /********************** END PROJECTILE MOVEMENT AND BULLET LIFE ****************************/

                //Console.WriteLine(worldUpdatesSB.ToString()); //TODO FOR TESTING ONLY


                List<SocketState> Clients = new List<SocketState>(clients);
                
                //Send the world to all of our players
                foreach (SocketState ss in Clients)
                {
                    if (World.Ships[ss.ID].HP <= 0)
                    {
                        if (World.Ships[ss.ID].respawnCounter >= 300)  shipRespawnHandler(ss);

                        else World.Ships[ss.ID].respawnCounter++;
                    }

                    //Handle Clients disconnecting
                    try
                    {
                        Networking.Send(ss.theSocket, worldUpdatesSB.ToString());
                    }
                    catch (SocketException)
                    {
                        World.Ships[ss.ID].HP = 0;
                        clients.Remove(ss);
                    }
                }
            }
        }
        //END UPDATE WORLD

        /// <summary>
        /// Given the data that has arrived so far, 
        /// potentially from multiple receive operations, 
        /// determine if we have enough to make a complete message,
        /// and process it into player commands
        /// </summary>
        /// <param name="sender">The SocketState that represents the client</param>
        private void ProcessMessage(SocketState sender)
        {

            string totalData = sender.sb.ToString();

            //Check each character in the string to see if it is a valid command
            foreach (char p in totalData)
            {
                if (totalData.Length <= 0)
                {
                    clients.Remove(sender);
                    theWorld.Ships.Remove(sender.ID);
                }
                // So we need to ignore it if this happens. 
                if (totalData[totalData.Length - 1] != '\n')
                    break;

                //Thrust
                if (p == 'T')
                {
                    if (thrustCommands.ContainsKey(sender.ID))
                    {
                        if (thrustCommands[sender.ID] == "")
                        {
                            thrustCommands[sender.ID] += p;
                        }
                    }
                    else
                    {
                        thrustCommands.Add(sender.ID, p + "");
                    }
                }

                //Rotate Right
                if (p == 'R')
                {
                    if (rightCommands.ContainsKey(sender.ID))
                        rightCommands[sender.ID] += p;

                    else
                        rightCommands.Add(sender.ID, p + "");

                }
                //Rotate Left
                if (p == 'L')
                {
                    if (leftCommands.ContainsKey(sender.ID))
                        leftCommands[sender.ID] += p;

                    else
                        leftCommands.Add(sender.ID, p + "");
                }

                //Shoot
                if (p == 'F')
                {
                    if (shootCommands.ContainsKey(sender.ID))
                        shootCommands[sender.ID] += p;

                    else
                        shootCommands.Add(sender.ID, p + "");
                }

            }
            // Remove it from the SocketState's growable buffer
            sender.sb.Clear();
            Networking.GetData(sender);
        }

        /******************************************** REFACTORED HELPER METHODS **********************************************/

        /// <summary>
        /// Reset ships location, direction, velocity and HP
        /// </summary>
        /// <param name="ss"></param>
        private void shipRespawnHandler(SocketState ss)
        {
            Vector2D location = new Vector2D(rand.Next(-worldSize / 2, worldSize / 2), rand.Next(-worldSize / 2, worldSize / 2));

            //This is how the given client behaves, on creating/respawn a ship is pointing upwards
            Vector2D direction = new Vector2D(0, -1);
            direction.Normalize();
            World.Ships[ss.ID].Location = location;
            World.Ships[ss.ID].Direction = direction;
            World.Ships[ss.ID].Velocity = new Vector2D(0, 0);
            World.Ships[ss.ID].HP = 5;
        }

        /// <summary>
        /// Check for bullet collision with a star
        /// </summary>
        /// <param name="bullet"></param>
        /// <param name="star"></param>
        private static void BulletCollisionWithStar(Projectile bullet, Star star)
        {
            Vector2D distance2 = bullet.Location - star.Location;
            double length2 = distance2.Length();
            if (length2 < 35)
            {
                bullet.Alive = false;
            }
        }

        /// <summary>
        /// Checks to see if the bullet has collided with a ship
        /// </summary>
        /// <param name="ship"></param>
        /// <param name="bullet"></param>
        private void ProjectileCollisionHandler(Ship ship, Projectile bullet)
        {
            Vector2D distance = bullet.Location - ship.Location;
            double length = distance.Length();
            if (length < 20 && (bullet.Owner != ship.p_ID))
            {
                ship.HP--;
                bullet.Alive = false;
                if (ship.HP <= 0)
                {
                    World.Ships[bullet.Owner].Score++;
                    ship.respawnCounter = 0;
                }
            }
        }

        /// <summary>
        /// Adds the gravity acceleration from the passed star to the total 
        /// acceleration vector
        /// </summary>
        /// <param name="ship"></param>
        /// <param name="totalAcceleration"></param>
        /// <param name="star"></param>
        /// <returns></returns>
        private static Vector2D AddGravityToTotal(Ship ship, Vector2D totalAcceleration, Star star)
        {
            Vector2D g = star.Location - ship.Location;
            g.Normalize();
            g = g * star.Mass;
            ship.Velocity = ship.Velocity + g;
            totalAcceleration += g;
            return totalAcceleration;
        }

        /// <summary>
        /// Checks to see if a ship has come close enough to a
        /// star to see if it collided and the ship died
        /// </summary>
        /// <param name="ship"></param>
        /// <param name="star"></param>
        private static void StarCollisionCheck(Ship ship, Star star)
        {
            Vector2D distance = ship.Location - star.Location;
            double length = distance.Length();
            if (length < 35)
            {
                ship.HP = 0;
                ship.respawnCounter = 0;
            }
        }

        /// <summary>
        /// Add Thrust acceleration to totalacceleration and
        /// turn the ships thrust bool to true
        /// </summary>
        /// <param name="ship"></param>
        /// <param name="totalAcceleration"></param>
        /// <returns></returns>
        private Vector2D handleThrustCalculations(Ship ship, Vector2D totalAcceleration)
        {
            Vector2D t = new Vector2D(ship.Direction);
            t = t * EngineStrength;
            totalAcceleration += t;
            ship.Velocity = ship.Velocity + t;
            ship.Thrust = true;
            return totalAcceleration;
        }

        /// <summary>
        /// Turns the ship left or right based off of which is held down longer.
        /// </summary>
        /// <param name="ship">The ship turning left or right.</param>
        private void HandleAmbigousLeftAndRightRotation(Ship ship)
        {
            if (rightCommands[ship.p_ID].Length > leftCommands[ship.p_ID].Length)
            {
                if (rightCommands.ContainsKey(ship.p_ID))
                    if (rightCommands[ship.p_ID] != "")
                    {
                        ship.Direction.Rotate(2);
                        rightCommands[ship.p_ID] = "";
                        leftCommands[ship.p_ID] = "";
                    }
            }
            if (rightCommands[ship.p_ID].Length < leftCommands[ship.p_ID].Length)
            {
                if (leftCommands.ContainsKey(ship.p_ID))
                    if (leftCommands[ship.p_ID] != "")
                    {
                        ship.Direction.Rotate(-2);
                        leftCommands[ship.p_ID] = "";
                        rightCommands[ship.p_ID] = "";
                    }
            }
        }

        /// <summary>
        /// Send the ship to the other side of the screen if you move into the edge.
        /// </summary>
        /// <param name="ship">The ship moving into the edge.</param>
        private void checkForReflection(Ship ship)
        {
            if (!(ship.Location.GetX() >= -1 * (theWorld.WorldSize / 2) - 20) || !(ship.Location.GetX() <= (theWorld.WorldSize / 2) + 20))
            {
                Vector2D currentLocation = ship.Location;
                Vector2D reflectedLocation = new Vector2D(currentLocation.GetX() * -1, currentLocation.GetY());
                ship.Location = reflectedLocation;
            }

            if (!(ship.Location.GetY() >= -1 * (theWorld.WorldSize / 2) - 20) || !(ship.Location.GetY() <= (theWorld.WorldSize / 2) + 20))
            {
                Vector2D currentLocation = ship.Location;
                Vector2D reflectedLocation = new Vector2D(currentLocation.GetX(), currentLocation.GetY() * -1);
                ship.Location = reflectedLocation;
            }
        }

        /// <summary>
        /// Star moves right until the edge of the world and is then placed randomly
        /// at the bottom of the world, it then moves up until it hits the top
        /// It then is placed randomly at the left of the world and moves right
        /// 
        /// NOTE: If there is more than one star any star hitting an edge will change the
        /// movement direction for ALL Stars
        /// </summary>
        /// <param name="star"></param>
        private void checkForReflection(Star star)
        {
            if (!(star.Location.GetX() >= -1 * (theWorld.WorldSize / 2) - 30) || !(star.Location.GetX() <= (theWorld.WorldSize / 2) + 30))
            {
                Vector2D currentLocation = star.Location;
                Vector2D reflectedLocation = new Vector2D(rand.Next(-worldSize / 2, worldSize / 2), (theWorld.WorldSize / 2) + 30);
                star.Location = reflectedLocation;
                movingRight = false;
                movingUp = true;
            }

            if (!(star.Location.GetY() >= -1 * (theWorld.WorldSize / 2) - 30) || !(star.Location.GetY() <= (theWorld.WorldSize / 2) + 30))
            {
                Vector2D currentLocation = star.Location;
                Vector2D reflectedLocation = new Vector2D(-(theWorld.WorldSize / 2) + 30, rand.Next(-worldSize / 2, worldSize / 2));
                star.Location = reflectedLocation;
                movingRight = true;
                movingUp = false;
            }

        }

        /// <summary>
        /// Kills the projectile if it collides with our defined edge of the world.
        /// </summary>
        /// <param name="projectile">projectile moving into the edge.</param>
        /// <returns></returns>
        private bool CheckBulletAtEdgeOfWorld(Projectile projectile)
        {
            if (!(projectile.Location.GetX() >= -1.5 * (theWorld.WorldSize / 2)) || !(projectile.Location.GetX() <= 1.5 * (theWorld.WorldSize / 2)))
            {
                projectile.Alive = false;
                return false;
            }

            if (!(projectile.Location.GetY() >= -1.5 * (theWorld.WorldSize / 2)) || !(projectile.Location.GetY() <= 1.5 * (theWorld.WorldSize / 2)))
            {
                projectile.Alive = false;
                return false;
            }
            return true;

        }
    }
}

