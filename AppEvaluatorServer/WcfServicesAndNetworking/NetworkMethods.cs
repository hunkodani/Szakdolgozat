using System;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading;
using System.ServiceModel;
using AppEvaluatorServer.FileManupulationAndSQL;

namespace AppEvaluatorServer.WcfServicesAndNetworking
{
    internal static class NetworkMethods
    {
        internal static ServiceHost SelectionHost { get; set; }
        internal static ServiceHost FileHost { get; set; }
        internal static int ServerPort { get { return 11000; } }
        internal static IPAddress LocalIPAddress
        {
            get
            {
                IPAddress address = IPAddress.Parse("127.0.0.1");
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
        //public static TcpListener TcpServer { get; set; }
        public static bool TcpServerShutdown { get; set; }
        
        #region MulticastFunctions

        internal static IPAddress McastIPAddress { get { return IPAddress.Parse("224.168.100.2"); } }
        public static Socket McastSocket { get; set; }

        public static bool IsMulticasting { get; set; } = true;

        /// <summary>
        /// Starts a multicast group, listens, and if the server called sends the local IPv4 address
        /// </summary>
        public static void ListenMulticastGroup()
        {
            StartMulticastGroup();
            Thread receiveThread = new Thread(ReceiveMcastMessage);
            receiveThread.Start();
        }

        /// <summary>
        /// Starts a multicast group which will provide all the clients the necessary first step to acquire the server ip
        /// </summary>
        private static void StartMulticastGroup()
        {
            McastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            EndPoint endPoint = new IPEndPoint(LocalIPAddress, ServerPort);
            McastSocket.Bind(endPoint);
            MulticastOption McastOption = new MulticastOption(McastIPAddress, LocalIPAddress);
            McastSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, McastOption);
        }

        /// <summary>
        /// Listens the Multicast group for a "Need Server IP" message and after hearing, it sends the server's IP address
        /// </summary>
        private static void ReceiveMcastMessage()
        {
            byte[] bytes = new byte[100];
            EndPoint remoteEP = new IPEndPoint(IPAddress.Any, ServerPort + 1);
            try
            {
                while (IsMulticasting)
                {
                    int length = McastSocket.ReceiveFrom(bytes, ref remoteEP);

                    if (Encoding.ASCII.GetString(bytes, 0, length) == "Need Server IP")
                    {
                        SendIPAddressToMcast();
                    }
                }
            }
            catch (Exception e)
            {
                Logging.WriteToLog(LogTypes.Error, e.Message);
            }
        }

        /// <summary>
        /// Sends the server's IP address to the multicast group
        /// </summary>
        private static void SendIPAddressToMcast()
        {
            IPEndPoint endPoint;
            try
            {
                endPoint = new IPEndPoint(McastIPAddress, ServerPort + 1);
                _ = McastSocket.SendTo(Encoding.ASCII.GetBytes(LocalIPAddress.ToString()), endPoint);
            }
            catch (Exception e)
            {
                Logging.WriteToLog(LogTypes.Error, e.Message);
            }
        }
        #endregion

        #region TCPFunctions

        /// <summary>
        /// Starts a new TCP listener thread that handles the clients requests
        /// </summary>
        public static void ListenTcpRequests()
        {
            Thread listenThread = new Thread(StartServerTcpListener)
            {
                IsBackground = true
            };
            listenThread.Start();
        }

        /// <summary>
        /// Starts the TCP listener that handles the clients requests
        /// If there is a request, first reads the first byte: it tells what type of request was sent
        /// If the first byte is 2 then reads the next 30 bytes, that contains the request and accordingly reads the rest which contains the actual data
        /// </summary>
        private static void StartServerTcpListener()
        {
            TcpClient client = null;
            NetworkStream stream = null;
            TcpListener TcpServerListener = null;
            try
            {
                TcpServerListener = new TcpListener(LocalIPAddress, ServerPort + 1);
                TcpServerListener.Start();

                byte[] bytes = new byte[256];
                int i;

                string request = null;
                int requestmode = 0;

                while (!TcpServerShutdown)
                {
                    client = TcpServerListener.AcceptTcpClient();
                    stream = client.GetStream();

                    ///It reads the first byte that tells what type of request was sent
                    if ((i = stream.Read(bytes, 0, 1)) != 0)
                    {
                        requestmode = int.Parse(Encoding.ASCII.GetString(bytes, 0, 1));
                        ///Database insert requests
                        if (requestmode == 2)
                        {
                            i = stream.Read(bytes, 0, 30);
                            request = Encoding.ASCII.GetString(bytes, 0, 30).Trim();
                            switch (request.ToLower())
                            {
                                case "save subject":
                                    i = stream.Read(bytes, 0, 200);
                                    SQLiteMethods.InsertSubject(Encoding.ASCII.GetString(bytes, 0, 50).Trim(),
                                                                Encoding.ASCII.GetString(bytes, 50, i - 50).Trim());
                                    break;
                                case "save test":
                                    i = stream.Read(bytes, 0, 150);
                                    SQLiteMethods.InsertTest(Encoding.ASCII.GetString(bytes, 0, 100).Trim(),
                                                             Encoding.ASCII.GetString(bytes, 100, i - 100).Trim());
                                    break;
                                case "save assignment":
                                    i = stream.Read(bytes, 0, 2);
                                    SQLiteMethods.InsertAssignment(int.Parse(Encoding.ASCII.GetString(bytes, 0, 1)),
                                                                   int.Parse(Encoding.ASCII.GetString(bytes, 1, 1)));
                                    break;
                                case "save user":
                                    i = stream.Read(bytes, 0, 111);
                                    string name = Encoding.ASCII.GetString(bytes, 0, 100).Trim();
                                    string code = Encoding.ASCII.GetString(bytes, 100, 10).Trim();
                                    int role = int.Parse(Encoding.ASCII.GetString(bytes, 110, 1));
                                    i = stream.Read(bytes, 0, 255);
                                    SQLiteMethods.InsertUser(name, Encoding.ASCII.GetString(bytes, 0, 255).Trim(), code, role);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    stream.Dispose();
                    client.Close();
                }
            }
            catch (Exception e)
            {
                Logging.WriteToLog(LogTypes.Error, e.Message);
                stream.Dispose();
                client.Close();
            }
            finally
            {
                TcpServerListener.Stop();
                TcpServerShutdown = true;
            }
        }

        #endregion
    }
}
