using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading;
using System.Diagnostics;

namespace AppEvaluator
{
    internal class NetworkMethods
    {
        private static IPAddress McastIPAddress { get { return IPAddress.Parse("224.168.100.2"); } }
        private static int ClientPort { get { return 11001; } }
        private static IPAddress ServerIPAddress { get; set; }
        public static Socket McastSocket { get; set; }
        private static IPAddress LocalIPAddress
        {
            get
            {
                IPAddress address = IPAddress.Parse("0.0.0.0");
                NetworkInterface.GetAllNetworkInterfaces().ToList().ForEach(netwint =>
                {
                    if (netwint.OperationalStatus == OperationalStatus.Up && netwint.GetIPProperties().GatewayAddresses.Count != 0)
                    {
                        address = IPAddress.Parse(netwint.GetIPProperties().UnicastAddresses.ToList()
                            .Find(item => Regex.IsMatch(item.Address.ToString(), "^([0-9]{1,3}.){3}[0-9]{1,3}$"))
                            .Address.ToString());
                    }
                });
                return address;
            }
        }
        private static Thread receiveThread;
        public static string RecievedInformationLocation { get; set; }
        public static string SqlResponse { get; set; }

        #region MulticastFunctions

        /// <summary>
        /// Checks if this PC is the server. If not, joins a multicast group, calls the server and listens for the respond (which contains the needed server IP address)
        /// </summary>
        public static void GetServerAddress()
        {
            JoinMulticastGroup();
            SendRequestToMcast();
            receiveThread = new Thread(ReceiveMcastMessage);
            receiveThread.Start();
        }

        /// <summary>
        /// Joins a multicast group which will provide all the clients the necessary first step to acquire the server ip
        /// </summary>
        private static void JoinMulticastGroup()
        {
            McastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint IPendPoint = new IPEndPoint(LocalIPAddress, ClientPort);
            McastSocket.Bind(IPendPoint);
            MulticastOption McastOption = new MulticastOption(McastIPAddress, LocalIPAddress);
            McastSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, McastOption);
        }

        /// <summary>
        /// Monitors the Multicast group (waiting for the server's respond with the needed server IP address)
        /// </summary>
        private static void ReceiveMcastMessage()
        {
            bool done = false;
            byte[] bytes = new byte[100];
            EndPoint remoteEP = new IPEndPoint(IPAddress.Any, ClientPort - 1);
            try
            {
                while (!done)
                {
                    int length = McastSocket.ReceiveFrom(bytes, ref remoteEP);
                    if (Regex.IsMatch(Encoding.ASCII.GetString(bytes, 0, length), "^([0-9]{1,3}.){3}[0-9]{1,3}$"))
                    {
                        ServerIPAddress = IPAddress.Parse(Encoding.ASCII.GetString(bytes, 0, length));
                        done = true;
                    }
                }
                McastSocket.Close();
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Sends a request (with "Need server IP" message) to the multicast group
        /// </summary>
        private static void SendRequestToMcast()
        {
            IPEndPoint endPoint;
            try
            {
                endPoint = new IPEndPoint(McastIPAddress, ClientPort - 1);
                _ = McastSocket.SendTo(Encoding.ASCII.GetBytes("Need Server IP"), endPoint);
            }
            catch (Exception e)
            {
                //Console.WriteLine("\n" + e.ToString());
            }
        }
        #endregion

        #region TCPFunctions

        public static void SendTcpMessage(string username, string request, string optional = null)
        {
            TcpClient client = null;
            NetworkStream stream = null;
            try
            {
                byte[] container;
                char[] name = new char[50];
                char[] req = new char[30];
                username.ToCharArray().CopyTo(name, 0);
                request.ToCharArray().CopyTo(req, 0);
                name = FillWithSpace(name, username.Length);
                req = FillWithSpace(req, request.Length);

                if (!string.IsNullOrEmpty(optional))
                {
                    char[] opt = new char[100];
                    optional.ToCharArray().CopyTo(opt, 0);
                    opt = FillWithSpace(opt, optional.Length);
                    container = Encoding.ASCII.GetBytes(new string(name) + new string(req) + new string(opt));
                }
                else
                {
                    container = Encoding.ASCII.GetBytes(new string(name) + new string(req));
                }
                
                client = new TcpClient(ServerIPAddress.ToString(), ClientPort);
                stream = client.GetStream();
                
                stream.Write(container, 0, container.Length);

                /*After sending the message it have to wait for the response */
                
                switch (request)
                {
                    case "send descriptionfile":
                        ListenAndSaveDescription();
                        break;
                    case "send testfiles":
                        ListenAndSaveTestFiles();
                        break;
                    case "sql command":
                        ListenSqlResponse();
                        break;
                    default:
                        break;
                }

                stream.Dispose();
                client.Close();
            }
            catch (Exception)
            {
                stream.Dispose();
                client.Close();
            }
        }

        /// <summary>
        /// Fills the given character array up with dummy characters (SPACE) starting with the given position
        /// </summary>
        /// <param name="array"></param>
        /// <param name="startPosition"></param>
        /// <returns></returns>
        private static char[] FillWithSpace(char[] array, int startPosition)
        {
            for (int i = startPosition; i < array.Length; i++)
            {
                array[i] = ' ';
            }
            return array;
        }

        private static void ListenAndSaveDescription()
        {
            RecievedInformationLocation = "";
        }

        private static void ListenAndSaveTestFiles()
        {
            RecievedInformationLocation = "";
        }

        private static void ListenSqlResponse()
        {
            SqlResponse = "";
        }

        /* public static void TcpListener()
         {
             TcpServer = null;
             try
             {
                 TcpServer = new TcpListener(LocalIPAddress, Port);

                 // Start listening for client requests.
                 TcpServer.Start();

                 // Buffer for reading data
                 byte[] bytes = new byte[256];
                 string data = null;
                 int i;

                 // Enter the listening loop.
                 while (true)
                 {
                     data = null;
                     TcpClient client = TcpServer.AcceptTcpClient();

                     NetworkStream stream = client.GetStream();

                     // Loop to receive all the data sent by the client.
                     while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                     {
                         // Translate data bytes to a ASCII string.
                         data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                         Console.WriteLine("Received: {0}", data);

                         // Process the data sent by the client.
                         data = data.ToUpper();

                         byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                         // Send back a response.
                         stream.Write(msg, 0, msg.Length);
                         Console.WriteLine("Sent: {0}", data);
                     }

                     // Shutdown and end connection
                     client.Close();
                 }
             }
             catch (SocketException e)
             {
                 Console.WriteLine("SocketException: {0}", e);
             }
             finally
             {
                 // Stop listening for new clients.
                 TcpServer.Stop();
             }

         }*/

        #endregion


    }
}
