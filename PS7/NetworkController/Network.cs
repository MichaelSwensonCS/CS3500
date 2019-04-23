/*
 * Authors: Mike Swenson and Seth Jackson
 * Many comments and implementations were provided by Professor Daniel Kopta
 * 
 * Handles communication between server and client
 * 
 * Specifically for PS8 it creates an event loop that allows clients to connect to a TCP listener
 * Personal Note: This class will be used in the 3505 class with Peter Jensen.
 */

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
/// <summary>
/// Acts as the client side of a TCP connection.  It provides a delegate allowing outside classes to use 
/// the information that is received in a way specific to that classes needs so that this controller can 
/// satisfy the separation of concerns.
/// </summary>
namespace NetworkController
{
    public delegate void NetworkAction(SocketState ss);

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
        public byte[] messageBuffer = new byte[4096];

        // This is a larger (growable) buffer, in case a single receive does not contain the full message.
        public StringBuilder sb = new StringBuilder();

        public NetworkAction callMe;

        public SocketState(Socket s, int id)
        {
            theSocket = s;
            ID = id;
        }
    }

    public class Client
    {
        private NetworkAction callMe;
        private TcpListener listener;

        public NetworkAction CallMe {get => callMe;}

        public TcpListener Listener { get => listener; }

        public Client(NetworkAction callMe, TcpListener listener)
        {
            this.callMe = callMe;
            this.listener = listener;
        }
    }


    /// <summary>
    /// Provides static methods for accessing and sending server data.
    /// </summary>
    public class Networking
    {
        private static int clientCounter = 0;
        //public int ServerClientCounter { get => serverClientCounter; }

        public const int DEFAULT_PORT = 11000;


        /// <summary>
        /// Creates a Socket object for the given host string
        /// Kopta
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



        /// <summary>
        /// Connects to the server and allows for a delegate to be
        /// passed in via the Network action that allows outside 
        /// classes to process the connection data in a way specific to them
        /// </summary>
        /// <param name="hostName"> server to connect to </param>
        /// <returns></returns>
        public static Socket ConnectToServer(NetworkAction call, string hostName)
        {
            System.Diagnostics.Debug.WriteLine("connecting  to " + hostName);

            // Create a TCP/IP socket.
            Socket socket;
            IPAddress ipAddress;

            Networking.MakeSocket(hostName, out socket, out ipAddress);

            SocketState ss = new SocketState(socket, -1);
            ss.callMe = call;

            socket.BeginConnect(ipAddress, Networking.DEFAULT_PORT, ConnectedCallback, ss);

            return socket;

        }

        /// <summary>
        /// This function is "called" by the operating system when the remote site acknowledges the
        /// connect request, Part of the event loop that keeps asking will continually update
        /// the client.
        /// </summary>
        /// <param name="ar"></param>
        private static void ConnectedCallback(IAsyncResult ar)
        {
            SocketState ss = (SocketState)ar.AsyncState;

            try
            {
                // Complete the connection.
                ss.theSocket.EndConnect(ar);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Unable to connect to server. Error occured: " + e);
                return;
            }
            ss.callMe(ss);
        }

        /// <summary>
        /// This is a small helper function that the client code will call whenever it wants more data.
        /// Note: the client will probably want more data every time it gets data, and has finished processing
        /// it in its callMe.
        /// </summary>
        /// <param name="state"></param>
        public static void GetData(SocketState ss)
        {
            ss.theSocket.BeginReceive(ss.messageBuffer, 0, 4096, SocketFlags.None, new AsyncCallback(Networking.ReceiveCallback), (object)ss);
        }

        /// <summary>
        /// This function is "called" by the operating system when data arrives on the socket
        /// Move this function to a standalone networking library. 
        /// </summary>
        /// <param name="ar"></param>
        private static void ReceiveCallback(IAsyncResult ar)
        {
            SocketState ss = (SocketState)ar.AsyncState;

            try
            {
                int bytesRead = ss.theSocket.EndReceive(ar);

                // If the socket is still open
                if (bytesRead > 0)
                {
                    string theMessage = Encoding.UTF8.GetString(ss.messageBuffer, 0, bytesRead);
                    // Append the received data to the growable buffer.
                    // It may be an incomplete message, so we need to start building it up piece by piece
                    ss.sb.Append(theMessage);

                    //ProcessMessage(ss);

                    ss.callMe(ss);
                }

                // Continue the "event loop" that was started on line 100.
                // Start listening for more parts of a message, or more new messages
            }
            catch (SocketException e)
            {
                System.Diagnostics.Debug.WriteLine("Unable to connect to server. Error occured: " + e);
                return;
            }
            
        }

        /// <summary>
        /// This fuction converts data into bytes and sends them using the socket.BeginSend.
        /// </summary>
        /// <param name="ss"></param>
        /// <param name="data"></param>
        public static void Send(Socket server, string data)
        {
            // Append a newline, since that is our protocol's terminating character for a message.
            byte[] messageBytes = Encoding.UTF8.GetBytes(data);
            server.BeginSend(messageBytes, 0, messageBytes.Length, SocketFlags.None, SendCallback, server);
        }

        /// <summary>
        /// A callback invoked when a send operation completes
        /// Move this function to a standalone networking library. 
        /// </summary>
        /// <param name="ar"></param>
        private static void SendCallback(IAsyncResult ar)
        {
            Socket s = (Socket)ar.AsyncState;
            // Nothing much to do here, just conclude the send operation so the socket is happy.
            //trycatch forcibly closed from client side?
            s.EndSend(ar);
        }


        //PS8
        
        /// <summary>
        /// The first part of the event loop that generates a new client and aTcpListener.
        /// It then calls the AcceptNewClient that will be the actual loop that continues
        /// "listening" for new clients
        /// </summary>
        /// <param name="callMe"></param>
        public static void ServerAwaitingClientLoop(NetworkAction callMe)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, DEFAULT_PORT);
            try
            {
                //Wrapped in a try catch to prevent the listener from being started more than once.
                listener.Start();
                Client newClient = new Client(callMe, listener);
                listener.BeginAcceptSocket(AcceptNewClient, (object)newClient);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        /// <summary>
        /// Creates a client based on the IAsyncResult sets creates a client
        /// and a socketstate based on the number of clients that have already 
        /// been created.
        /// </summary>
        /// <param name="ar"></param>
        public static void AcceptNewClient(IAsyncResult ar)
        {
            Console.WriteLine("A new client has connected");
            Client client = (Client)ar.AsyncState;
            Socket socket = client.Listener.EndAcceptSocket(ar);
        
            SocketState ss = new SocketState(socket, clientCounter++);
            ss.callMe = client.CallMe;
            ss.callMe(ss);
            client.Listener.BeginAcceptSocket(AcceptNewClient, (object)client);
        }
    }

}