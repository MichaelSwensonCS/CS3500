using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkController
{
  /// <summary>
  /// This class holds all the necessary state to represent a socket connection
  /// Note that all of its fields are public because we are using it like a "struct"
  /// It is a simple collection of fields
  /// </summary>
  public class SocketState
  {
    public Socket theSocket;
    public int ID;
    
    // This is the buffer where we will receive data from the socket
    public byte[] messageBuffer = new byte[1024];

    // This is a larger (growable) buffer, in case a single receive does not contain the full message.
    public StringBuilder sb = new StringBuilder();

    public SocketState(Socket s, int id)
    {
      theSocket = s;
      ID = id;
    }
  }

  public class Networking
  {

    public const int DEFAULT_PORT = 11000;


    /// <summary>
    /// Creates a Socket object for the given host string
    /// </summary>
    /// <param name="hostName">The host name or IP address</param>
    /// <param name="socket">The created Socket</param>
    /// <param name="ipAddress">The created IPAddress</param>
    public static void MakeSocket(string hostName, out Socket socket, out IPAddress ipAddress)
    {
      ipAddress = IPAddress.None;
      socket = null;
      try
      {
        // Establish the remote endpoint for the socket.
        IPHostEntry ipHostInfo;

        // Determine if the server address is a URL or an IP
        try
        {
          ipHostInfo = Dns.GetHostEntry(hostName);
          bool foundIPV4 = false;
          foreach (IPAddress addr in ipHostInfo.AddressList)
            if (addr.AddressFamily != AddressFamily.InterNetworkV6)
            {
              foundIPV4 = true;
              ipAddress = addr;
              break;
            }
          // Didn't find any IPV4 addresses
          if (!foundIPV4)
          {
            System.Diagnostics.Debug.WriteLine("Invalid addres: " + hostName);
            throw new ArgumentException("Invalid address");
          }
        }
        catch (Exception)
        {
          // see if host name is actually an ipaddress, i.e., 155.99.123.456
          System.Diagnostics.Debug.WriteLine("using IP");
          ipAddress = IPAddress.Parse(hostName);
        }

        // Create a TCP/IP socket.
        socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        socket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
        
        // Disable Nagle's algorithm - can speed things up for tiny messages, 
        // such as for a game
        socket.NoDelay = true;

      }
      catch (Exception e)
      {
        System.Diagnostics.Debug.WriteLine("Unable to create socket. Error occured: " + e);
        throw new ArgumentException("Invalid address");
      }
    }

    
    // TODO: Move all networking code to this class. Left as an exercise.
    // Networking code should be completely general-purpose, and useable by any other application.
    // It should contain no references to a specific project.
  }

}
