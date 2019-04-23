using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatServer
{
  /// <summary>
  /// A simple server for receiving simple text messages from multiple clients
  /// </summary>
  class ChatServer
  {
    static void Main(string[] args)
    {
      ChatServer server = new ChatServer();
      server.StartServer();

      // Sleep to prevent the program from closing,
      // since all the real work is done in separate threads
      // StartServer is non-blocking
      Thread.Sleep(10000000);
    }



    // A list of clients that are connected.
    private List<Socket> clients = new List<Socket>();
    
    private TcpListener listener;

    int port;

    public ChatServer()
    {
    }

    /// <summary>
    /// Start accepting Tcp sockets connections from clients
    /// </summary>
    public void StartServer()
    {
      Console.WriteLine("Enter port to listen on:");
      string portStr = Console.ReadLine();
      port = Int32.Parse(portStr);
      listener = new TcpListener(IPAddress.Any, port);
      Console.WriteLine("Server waiting for client");

      listener.Start();

      // This begins an "event loop".
      // ConnectionRequested will be invoked when the first connection arrives.
      listener.BeginAcceptSocket(ConnectionRequested, null);

      SendMessage();
    }

    /// <summary>
    /// A callback for invoking when a socket connection is accepted
    /// </summary>
    /// <param name="ar"></param>
    private void ConnectionRequested(IAsyncResult ar)
    {
      Console.WriteLine("Contact from client");
      
      // Get the socket
      clients.Add(listener.EndAcceptSocket(ar));

      listener.BeginAcceptSocket(ConnectionRequested, null);

    }

    
    /// <summary>
    /// Continuously ask the user for a message to send to the server
    /// </summary>
    private void SendMessage()
    {
      while (true)
      {
        Console.WriteLine("enter a message to send");
        string message = Console.ReadLine();
        if (message == "largemessage")
        {
          message = GenerateLargeMessage();
        }
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);
        // Begin sending the message
        List<Socket> toRemove = new List<Socket>();
        foreach (Socket s in clients)
        {
          try
          {
            s.BeginSend(messageBytes, 0, messageBytes.Length, SocketFlags.None, SendCallback, s);
          }
          catch (Exception) { toRemove.Add(s); }
        }
        foreach (Socket s in toRemove)
          clients.Remove(s);
      }
    }

    private string GenerateLargeMessage()
    {
      StringBuilder retval = new StringBuilder();

      for (int i = 0; i < 4999; i++)
        retval.Append("a");
      retval.Append(".");

      return retval.ToString();
      
    }


    /// <summary>
    /// A callback invoked when a send operation completes
    /// </summary>
    /// <param name="ar"></param>
    private void SendCallback(IAsyncResult ar)
    {
      // Nothing much to do here, just conclude the send operation so the socket is happy.
      Socket client = (Socket)ar.AsyncState;
      client.EndSend(ar);
    }


  }
}
