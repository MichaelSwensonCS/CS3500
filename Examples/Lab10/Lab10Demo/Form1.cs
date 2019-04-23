using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace Lab10
{
  public partial class Form1 : Form
  {
    // The controller handles updates from the "server"
    // and notifies us via an event
    private Controller theController;

    // World is a simple container for Players and Powerups
    // The controller owns the world, but we have a reference to it
    private World theWorld;

    DrawingPanel drawingPanel;

    public Form1(Controller ctl)
    {
      InitializeComponent();
      theController = ctl;
      theWorld = new World();
      theController.RegisterServerUpdateHandler(UpdateWorld);

      // Set up the windows Form.
      // This stuff is usually handled by the drag and drop designer,
      // but it's simple enough for this lab.
      ClientSize = new Size(Controller.worldSize, Controller.worldSize);
      drawingPanel = new DrawingPanel(theWorld);
      drawingPanel.Location = new Point(0, 0);
      drawingPanel.Size = new Size(this.ClientSize.Width, this.ClientSize.Height);
      this.Controls.Add(drawingPanel);

    }


    // Redraw the game. This method is invoked every time the "frameTimer"
    // above ticks.
    private void UpdateWorld(List<Player> newPlayers, List<Powerup> newPowerups)
    {
      // Don't try to redraw if the window doesn't exist yet.
      // This might happen if the controller sends an update
      // before the Form has started.
      if (!IsHandleCreated)
        return;

      //Random r = new Random(); // ignore this for now

      // The server is not required to send updates about every object,
      // so we update our local copy of the world only for the objects that 
      // the server gave us an update for.
      foreach (Player play in newPlayers)
      {
        //while (r.Next() % 1000 != 0) ; // ignore this loop for now
        if (!play.GetActive())
          theWorld.Players.Remove(play.GetID());
        else
          theWorld.Players[play.GetID()] = play;
      }

      foreach (Powerup pow in newPowerups)
      {
        if (!pow.GetActive())
          theWorld.Powerups.Remove(pow.GetID());
        else
          theWorld.Powerups[pow.GetID()] = pow;
      }

      // Invalidate this form and all its children
      // This will cause the form to redraw as soon as it can
      this.Invalidate(true);
    }

  }
}
