using System;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading;

namespace AppEvaluator.NetworkingAndWCF
{
    internal class NetworkMethods
    {
        private static IPAddress McastIPAddress { get { return IPAddress.Parse("224.168.100.2"); } }
        internal static IPAddress ServerIPAddress { get; set; }
        public static Socket McastSocket { get; set; }
        public static IPAddress LocalIPAddress
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
        public static Thread receiveThread;

        #region MulticastFunctions

        /// <summary>
        /// Checks if this PC is the server. If not, joins a multicast group, calls the server and listens for the respond (which contains the needed server IP address)
        /// </summary>
        public static void GetServerAddress()
        {
            JoinMulticastGroup();
            receiveThread = new Thread(ReceiveMcastMessage);
            receiveThread.Start();
            SendRequestToMcast();
        }

        /// <summary>
        /// Joins a multicast group which will provide all the clients the necessary first step to acquire the server ip
        /// </summary>
        private static void JoinMulticastGroup()
        {
            McastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint IPendPoint = new IPEndPoint(LocalIPAddress, Properties.Settings.Default.ClientPort);
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
            EndPoint remoteEP = new IPEndPoint(IPAddress.Any, Properties.Settings.Default.ClientPort - 1);
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
                WcfService.ConnectToServices(ServerIPAddress, Properties.Settings.Default.ClientPort - 1);
            }
            catch (Exception e)
            {
                Logging.WriteToLog(LogTypes.Error, e.Message);
                //Console.WriteLine(e.ToString());
            }
            McastSocket.Close();
            receiveThread.Abort();
        }

        /// <summary>
        /// Sends a request (with "Need server IP" message) to the multicast group
        /// </summary>
        private static void SendRequestToMcast()
        {
            IPEndPoint endPoint;
            try
            {
                endPoint = new IPEndPoint(McastIPAddress, Properties.Settings.Default.ClientPort - 1);
                _ = McastSocket.SendTo(Encoding.ASCII.GetBytes("Need Server IP"), endPoint);
            }
            catch (Exception e)
            {
                Logging.WriteToLog(LogTypes.Error, e.Message);
                //Console.WriteLine("\n" + e.ToString());
            }
        }
        #endregion

        #region TCPInserts and FillWithSpace function

        /// <summary>
        /// Fills the given character array up with dummy characters (SPACE) starting with the given position
        /// </summary>
        /// <param name="array"></param>
        /// <param name="startPosition"></param>
        /// <returns></returns>
        internal static char[] FillWithSpace(char[] array, int startPosition)
        {
            for (int i = startPosition; i < array.Length; i++)
            {
                array[i] = ' ';
            }
            return array;
        }

        /// <summary>
        /// Sends an insert tcp message with the specified data to insert a new subject
        /// </summary>
        /// <param name="subjectCode"></param>
        /// <param name="subjectName"></param>
        public static void SendInsertSubject(string subjectCode, string subjectName)
        {
            TcpClient client = null;
            NetworkStream stream = null;
            string request = "save subject";
            try
            {
                byte[] container;
                char[] req = new char[30];
                char[] code = new char[50];
                char[] name = new char[150];
                request.ToCharArray().CopyTo(req, 0);
                subjectCode.ToCharArray().CopyTo(code, 0);
                subjectName.ToCharArray().CopyTo(name, 0);
                req = FillWithSpace(req, request.Length);
                code = FillWithSpace(code, subjectCode.Length);
                name = FillWithSpace(name, subjectName.Length);

                container = Encoding.ASCII.GetBytes(2 + new string(req) + new string(code) + new string(name));

                client = new TcpClient(ServerIPAddress.ToString(), Properties.Settings.Default.ClientPort);
                stream = client.GetStream();

                stream.Write(container, 0, container.Length);

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
        /// Sends an insert tcp message with the specified data to insert a new test
        /// </summary>
        /// <param name="testName"></param>
        /// <param name="subjectCode"></param>
        public static void SendInsertTest(string testName, string subjectCode)//minimum req: sql row data, 1 testfile, 1 description
        {
            TcpClient client = null;
            NetworkStream stream = null;
            string request = "save test";
            try
            {
                byte[] container;
                char[] req = new char[30];
                char[] name = new char[100];
                char[] code = new char[50];
                request.ToCharArray().CopyTo(req, 0);
                testName.ToCharArray().CopyTo(name, 0);
                subjectCode.ToCharArray().CopyTo(code, 0);
                req = FillWithSpace(req, request.Length);
                name = FillWithSpace(name, testName.Length);
                code = FillWithSpace(code, subjectCode.Length);

                container = Encoding.ASCII.GetBytes(2 + new string(req) + new string(name) + new string(code));

                client = new TcpClient(ServerIPAddress.ToString(), Properties.Settings.Default.ClientPort);
                stream = client.GetStream();

                stream.Write(container, 0, container.Length);

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
        /// Sends an insert tcp message with the specified data to insert a new user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="code"></param>
        /// <param name="role"></param>
        public static void SendInsertUser(string username, string password, string code, int role)
        {
            TcpClient client = null;
            NetworkStream stream = null;
            string request = "save user";
            try
            {
                byte[] container;
                char[] req = new char[30];
                char[] name = new char[100];
                char[] pass = new char[255];
                char[] codechar = new char[10];
                request.ToCharArray().CopyTo(req, 0);
                username.ToCharArray().CopyTo(name, 0);
                password.ToCharArray().CopyTo(pass, 0);
                code.ToCharArray().CopyTo(codechar, 0);
                req = FillWithSpace(req, request.Length);
                name = FillWithSpace(name, username.Length);
                pass = FillWithSpace(pass, password.Length);
                codechar = FillWithSpace(codechar, code.Length);

                container = Encoding.ASCII.GetBytes(2 + new string(req) + new string(name) + new string(codechar) + role);

                client = new TcpClient(ServerIPAddress.ToString(), Properties.Settings.Default.ClientPort);
                stream = client.GetStream();
                stream.Write(container, 0, container.Length);

                container = Encoding.ASCII.GetBytes(new string(pass));
                stream.Write(container, 0, container.Length);

                stream.Dispose();
                client.Close();
            }
            catch (Exception)
            {
                stream?.Dispose();
                client?.Close();
                throw;
            }
        }

        /// <summary>
        /// Sends an insert tcp message with the specified data to insert a new assignment --> not working above 10 (int number) on the other side or this one
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="testId"></param>
        public static void SendInsertAssignment(int userId, int testId)
        {
            TcpClient client = null;
            NetworkStream stream = null;
            string request = "save assignment";
            try
            {
                byte[] container;
                char[] req = new char[30];
                request.ToCharArray().CopyTo(req, 0);
                req = FillWithSpace(req, request.Length);

                container = Encoding.ASCII.GetBytes(2 + new string(req) + userId + testId);

                client = new TcpClient(ServerIPAddress.ToString(), Properties.Settings.Default.ClientPort);
                stream = client.GetStream();
                stream.Write(container, 0, container.Length);
                
                stream.Dispose();
                client.Close();
            }
            catch (Exception)
            {
                stream.Dispose();
                client.Close();
            }
        }

        #endregion

    }
}
