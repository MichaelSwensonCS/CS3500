using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab10
{
  public class World
  {
    // In reality, these should not be public,
    // but for the purposes of this lab, the "World" 
    // class is just a wrapper around these two fields.
    public Dictionary<int, Player> Players;
    public Dictionary<int, Powerup> Powerups;

    public World()
    {
      Players = new Dictionary<int, Player>();
      Powerups = new Dictionary<int, Powerup>();
    }

  }
}
