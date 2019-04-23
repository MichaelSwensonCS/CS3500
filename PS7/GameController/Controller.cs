using NetworkController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SpaceWars;

namespace GameController
{
    public delegate void MessageReceived(List<Ship> ships, List<Projectile> projectiles, List<Star> stars);
    /// <summary>
    /// This class uses the NetworkController to receive data about the 
    /// world and passes information to all the other related classes.
    /// </summary>
    public class Controller
    {
        private int playerID;
        public int PlayerID { get => playerID; }
        private int worldSize = 750;
        private int index = 0;
        private string Name = "";

        public int WorldSize { get => worldSize; }

        Socket socket;

        List<Ship> Ships = new List<Ship>();
        List<Projectile> Projectiles = new List<Projectile>();
        List<Star> Stars = new List<Star>();

         // Note: Form1 uses MessageReceived as UpdateWorld()
        public MessageReceived worldObjects;


        /// <summary>
        /// This is used to start the game session by using the StartGame method
        /// and passing the ip to the server.  This also initializes the world.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="host"></param>
        public void Connect(MessageReceived message, string host)
        {
            socket = Networking.ConnectToServer(StartGame, host);
            World theWorld = new World();
            worldObjects = message;
        }

        /// <summary>
        /// Easy method used to assign the player name given from user input
        /// </summary>
        /// <param name="name"></param>
        public void sendName(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Tells the server our name then,
        /// Gets the initial state data of the world(size, id) so that we can start drawing
        /// </summary>
        /// <param name="ss"></param>
        private void StartGame(SocketState ss)
        {
            //First we send our name and then ask for more data expecting a world size and player id
            if (index == 0)
            {
                Networking.Send(socket, Name);
                index = 1;
                Networking.GetData(ss);
            }
            else if (index == 1)
            {
                StringBuilder startData = ss.sb;
                string[] parts = Regex.Split(startData.ToString(), @"(?<=[\n])");
                //If there isn't 2 things sent or if it doesn't have a terminator stop
                if (parts.Length < 2 || startData[startData.Length - 1] != '\n')
                    return;
                
                //We have our ID now remove it
                Int32.TryParse(parts[0], out playerID);
                startData.Remove(0, parts[0].Length);

                //We have our world size now remove it
                Int32.TryParse(parts[1], out worldSize);
                startData.Remove(0, parts[1].Length);

                //Set the delegate to ProcessMessage in anticipation of receiving world data
                ss.callMe = ProcessMessage;

                //Give me mah bits
                Networking.GetData(ss);
            }
        }

        /// <summary>
        /// Simple method that wraps a string in the expected format
        /// that the server expects commands
        /// </summary>
        /// <param name="keys"></param>
        public void SendCommand(string keys)
        {
            string command = "(" + keys + ")\n";
            Networking.Send(socket, command);
        }

        /// <summary>
        /// This is the worker method that processes a message when received.
        /// For proper MVC, this should go in its own controller
        /// </summary>
        /// <param name="ss">The SocketState on which the message was received</param>
        private void ProcessMessage(SocketState ss)
        {
            Ships.Clear();
            Stars.Clear();
            string totalData = ss.sb.ToString();
            string[] parts = Regex.Split(totalData, @"(?<=[\n])");

            // Loop until we have processed all messages.
            // We may have received more than one.

            foreach (string p in parts)
            {
                // Ignore empty strings added by the regex splitter
                if (p.Length == 0)
                    continue;
                if (p.Length == 1)
                    continue;
                // The regex splitter will include the last string even if it doesn't end with a '\n',
                // So we need to ignore it if this happens. 
                if (p[p.Length - 1] != '\n')
                    break;

                JObject obj = JObject.Parse(p);

                JToken token = obj["ship"];
                JToken token2 = obj["proj"];
                JToken token3 = obj["star"];

                //Receive ships, projectiles, and stars so that we can build our world
                if (token != null)
                {
                    Ship rebuilt = JsonConvert.DeserializeObject<Ship>(p);
                    Ships.Add(rebuilt);
                }
                else if (token2 != null)
                {
                    Projectile rebuilt = JsonConvert.DeserializeObject<Projectile>(p);
                    Projectiles.Add(rebuilt);
                }
                else if (token3 != null)
                {
                    Star rebuilt = JsonConvert.DeserializeObject<Star>(p);
                    Stars.Add(rebuilt);
                }

                // Then remove it from the SocketState's growable buffer
                ss.sb.Remove(0, p.Length);
            }

            worldObjects(Ships, Projectiles, Stars);

            //Keep receiving the world
            ss.callMe = this.ProcessMessage;
            Networking.GetData(ss);
        }
    }
}
