using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceWars;
using Newtonsoft.Json;

namespace Lec18
{

  [JsonObject(MemberSerialization.OptIn)]
  class Ship
  {
    [JsonProperty(PropertyName ="ID")]
    private int shipID;
    [JsonProperty]
    private Vector2D location;
    [JsonProperty]
    private Vector2D direction;

    public Ship(int id, double lX, double lY, double dX, double dY)
    {
      shipID = id;
      location = new Vector2D(lX, lY);
      direction = new Vector2D(dX, dY);
    }

  }


  class Program
  {
    static void Main(string[] args)
    {
      Ship s = new Ship(3, 122.4, -71.3, 0.0, -1.0);

      string asJson = JsonConvert.SerializeObject(s);
      Console.WriteLine(asJson);

      Console.Read();
    }
  }
}
