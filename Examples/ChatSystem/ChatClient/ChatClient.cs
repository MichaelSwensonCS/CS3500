using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatClient
{

  /// <summary>
  /// A SocketState represents all of the information needed
  /// to handle one connection.
  /// </summary>
  class SocketState
  {
    // It would be better to make these private
    public Socket theSocket;
    public byte[] messageBuffer = new byte[1024];

    public SocketState(Socket s)
    {
      theSocket = s;
    }


    //public Socket GetSocket()
    //{
    //  return theSocket;
    //}

    //public byte[] GetBuffer()
    //{
    //  return messageBuffer;
    //}

  }


  /// <summary>
  /// A client for receiving simple text messages from a chat server
  /// </summary>
  class ChatClient
  {
    private const int port = 11000;


    public ChatClient()
    {
    }



    static void Main(string[] args)
    {
      ChatClient client = new ChatClient();

      Console.WriteLine("enter server address:");
      string serverAddr = Console.ReadLine();

      client.ConnectToServer(serverAddr);


      Console.Read();
    }



    /// <summary>
    /// Start attempting to connect to a server
    /// </summary>
    /// <param name="ip">The address of the server</param>
    public void ConnectToServer(string ip)
    {
      // Parse the IP
      IPAddress addr = IPAddress.Parse(ip);

      // Create a socket
      Socket s = new Socket(addr.AddressFamily, SocketType.Stream,
        ProtocolType.Tcp);

      // Put the socket into a SocketState that also contains the buffer 
      // where data will be received
      SocketState ss = new SocketState(s);

      // Connect
      // We pass the state to the callback. It will be contained in the IAsyncResult
      ss.theSocket.BeginConnect(addr, 11000, ConnectedCallback, ss);

    }

    /// <summary>
    /// This method is automatically invoked on its own thread when a connection is made.
    /// </summary>
    /// <param name="ar"></param>
    private void ConnectedCallback(IAsyncResult ar)
    {
      Console.WriteLine("contact from server");

      // Get the SocketState associated with this connection 
      SocketState ss = (SocketState)ar.AsyncState;

      // This is required to complete the "handshake" with the server. Both parties agree a connection is made.
      ss.theSocket.EndConnect(ar);

      // Wait for data from the server
      // We pass the last argument (socket state) so that the callback knows which connection data was received on
      ss.theSocket.BeginReceive(ss.messageBuffer, 0, ss.messageBuffer.Length,
        SocketFlags.None, ReceiveCallback, ss);

    }

    /// <summary>
    /// This method is invoked on its own thread when data arrives from the server.
    /// </summary>
    /// <param name="ar"></param>
    private void ReceiveCallback(IAsyncResult ar)
    {
      Console.WriteLine("message received");

      // Get the SocketState representing the connection on which data was received
      SocketState ss = (SocketState)ar.AsyncState;

      int numBytes = ss.theSocket.EndReceive(ar);

      // Convert the raw bytes to a string
      string message = Encoding.UTF8.GetString(ss.messageBuffer, 0, numBytes);

      // "process" the data
      Console.WriteLine(message);

      // Wait for more data from the server. This creates an "event loop".
      // ReceiveCallback will be invoked every time new data is available on the socket.
      ss.theSocket.BeginReceive(ss.messageBuffer, 0, ss.messageBuffer.Length,
        SocketFlags.None, ReceiveCallback, ss);

    }
  }

}
