/*
 * 
 * Authors: Michael Swenson & Seth Jackson
 * This class is used to create a ship that contains information about where it is geometrically along with
 * a unique ID used to identify a ship. It also has hit points and the ability to see if the engine is "thrusting"
 * 
 */

using Newtonsoft.Json;
using SpaceWars;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
///  Represents a unique spacecraft that can move, rotate, shoot, and die
/// </summary>
namespace SpaceWars
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Ship
    {

        public Vector2D Velocity { get; set; } = new Vector2D(0, 0);

        private static int ShipIDHolder;

        [JsonProperty(PropertyName = "ship")]
        private int ID;

        public int p_ID { get => ID; }

        [JsonProperty]
        private Vector2D loc;

        public Vector2D Location { get => loc; set => loc = value; }

        [JsonProperty]
        private Vector2D dir;

        public Vector2D Direction { get => dir; set => dir = value; }

        [JsonProperty]
        private bool thrust;
        
        public bool Thrust { get => thrust; set => thrust = value; }

        [JsonProperty]
        private string name;

        public string Name { get => name; }

        [JsonProperty]
        private int hp;

        public int HP { get => hp; set => hp = value; }

        [JsonProperty]
        private int score;

        public int Score { get => score; set => score = value; }

        private bool active = true;

        public int fireCounter = 0;
        public int respawnCounter = 0;

        public Ship()
        {
            this.ID = 0;
            this.loc = new Vector2D(0, 0);
            this.dir = new Vector2D(0, 0);
            this.thrust = false;
            this.score = 0;
            this.name = "";
            this.hp = 5;
        }
        /// <summary>
        /// Constructor that assigns ID, name, location(Vector2D) and direction(Vector2D)
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="name"></param>
        /// <param name="loc"></param>
        /// <param name="dir"></param>
        public Ship(int ID, string name, Vector2D loc, Vector2D dir)
        {
            this.ID = ShipIDHolder++;
            this.name = name;
            this.loc = new Vector2D(loc);
            this.dir = new Vector2D(dir);
            this.hp = 5;
            this.score = 0;
        }
        /// <summary>
        /// This is a method that provides a way for us to "kill" a ship and deactivate it
        /// </summary>
        /// <returns></returns>
        public bool GetActive()
        {
            if (HP < 0 || HP == 0)
            {
                active = false;
            }
            else
            {
                active = true;
            }
            return active;
        }
    }
}
