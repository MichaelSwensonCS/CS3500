using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Lab10
{
  public class Controller
  {
    // World is a simple container for Players and Powerups
    private World theWorld;

    public const int worldSize = 500;

    // Although we aren't actually doing any networking,
    // this FakeServer object will simulate game updates coming 
    // from an asynchronous server.
    private FakeServer server;

    public delegate void ServerUpdateHandler(List<Player> newPlayers, List<Powerup> newPowerups);

    private event ServerUpdateHandler UpdateArrived;

    public Controller()
    {
      theWorld = new World();
      server = new FakeServer(worldSize);
    }

    public World GetWorld()
    {
      return theWorld;
    }

    public void RegisterServerUpdateHandler(ServerUpdateHandler h)
    {
      UpdateArrived += h;
    }

    public void ProcessUpdatesFromServer()
    {
      // Start a new timer that will simulate updates coming from the server
      // every 15 millisecoonds
      System.Timers.Timer serverTimer = new System.Timers.Timer();
      serverTimer.Interval = 15;
      serverTimer.Elapsed += UpdateCameFromServer;
      serverTimer.Start();
    }


    // Simulate a game update from a fake server
    // This method is invoked every time the "serverTimer", above, ticks
    private void UpdateCameFromServer(object sender, ElapsedEventArgs e)
    {
      List<Player> newPlayers;
      List<Powerup> newPowerups;

      // Get the update from the "server"
      server.GenerateFakeServerUpdate(out newPlayers, out newPowerups);

      // Notify any listeners (the view) that a new game world has arrived from the server
      if (UpdateArrived != null)
        UpdateArrived(newPlayers, newPowerups);
    }

  }
}
