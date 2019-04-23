/*
 * 
 * Author: Michael Swenson & Seth Jackson
 * 
 * This class is used to create a projectile and that projectile has information
 * about its ID, its owner's ID, and geometric data used in drawing it and calculating where it is going.
 */


using SpaceWars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SpaceWars
{
    /// <summary>
    /// This class represents a bullet.  It knows its location, direction, 
    /// if it is alive and also which player shot it.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Projectile
    {
        [JsonProperty(PropertyName = "proj")]
        private int ID;

        public int Projectile_ID { get => ID; }

        [JsonProperty]
        private Vector2D loc;

        public Vector2D Location { get => loc; set => loc = value; }

        [JsonProperty]
        private Vector2D dir;

        public Vector2D Direction { get => dir; }

        [JsonProperty]
        private bool alive;

        public bool Alive { get => alive; set => alive = value; }

        [JsonProperty]
        private int owner;

        public int Owner { get => owner; }
        public int deadCount { get; set; }

        /// <summary>
        /// Default constructor sets all properties to default values
        /// </summary>
        public Projectile()
        {
            this.ID = 0;
            this.loc = new Vector2D(0, 0);
            this.dir = new Vector2D(0, 0);
            this.alive = true;
            this.owner = 0;
            this.deadCount = 0;
        }

        /// <summary>
        /// Creates a projectile at a certain location and facing a certain direction, 
        /// it also gives it a ID and an owner.  Since it is created it must be alive.
        /// </summary>
        /// <param name="_ID"></param>
        /// <param name="location"></param>
        /// <param name="direction"></param>
        /// <param name="alive"></param>
        /// <param name="owner"></param>
        public Projectile(int _ID, Vector2D location, Vector2D direction, bool alive, int owner)
        {
            this.ID = _ID;
            this.loc = location;
            this.dir = direction;
            this.alive = true;
            this.owner = owner;
            this.deadCount = 0;
        }

        /// <summary>
        /// Simple Getter returning the value of alive and
        /// helps in creating self documenting code
        /// </summary>
        /// <returns></returns>
        public bool GetActive()
        {
            return alive;
        }
    }
}
