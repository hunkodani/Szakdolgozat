﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading;

namespace Server_WPF_IPC
{
    internal static class NetworkMethods
    {
        private static IPAddress McastIPAddress { get { return IPAddress.Parse("224.168.100.2"); } }
        private static int ServerPort { get { return 11000; } }
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
        //public static TcpListener TcpServer { get; set; }
        public static bool TcpServerShutdown { get; set; }

        #region MulticastFunctions

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
                while (true)
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
                //Console.WriteLine(e.ToString());
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
                //Console.WriteLine("\n" + e.ToString());
            }
        }
        #endregion

        #region TCPFunctions

        /// <summary>
        /// Starts a new TCP listener thread that handles the clients requests
        /// </summary>
        public static void ListenTcpRequests()
        {
            Thread listenThread = new Thread(StartServerTcpListener);
            listenThread.Start();
        }

        /// <summary>
        /// Starts the TCP listener that handles the clients requests
        /// If there is a request, first reads the first 80 bytes: 50 for the username, 30 for the request type
        /// Additional information follows i.e: test name(100 bytes) or sql codes or file
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

                string username = null;
                string request = null;

                while (!TcpServerShutdown)
                {
                    client = TcpServerListener.AcceptTcpClient();
                    stream = client.GetStream();
                    
                    //Reads the first 80 bytes
                    if ((i = stream.Read(bytes, 0, 80)) != 0)
                    {
                        username = Encoding.ASCII.GetString(bytes, 0, 50).Trim();
                        request = Encoding.ASCII.GetString(bytes, 50, i - 50).Trim();
                        switch (request.ToLower())
                        {
                            case "send testlist":
                                SendTestsListToMessage(stream, username);
                                break;
                            case "send descriptionfile":
                                i = stream.Read(bytes, 0, 100);
                                SendFilesToMessage(stream, username, Encoding.ASCII.GetString(bytes, 0, i).Trim(), true);
                                break;
                            case "send testfiles"://test name
                                i = stream.Read(bytes, 0, 100);
                                SendFilesToMessage(stream, username, Encoding.ASCII.GetString(bytes, 0, i).Trim(), false);
                                break;
                            case "save evaulation":
                                i = stream.Read(bytes, 0, 100);
                                SaveTestEvaulation(stream, username, Encoding.ASCII.GetString(bytes, 0, i).Trim());
                                break;
                            case "save testfile":
                                i = stream.Read(bytes, 0, 100);
                                SaveTestFile(stream, username, Encoding.ASCII.GetString(bytes, 0, i).Trim());
                                break;
                            case "sql command":
                                break;
                            default:
                                break;
                        }
                        /*// Loop to receive all the data sent by the client.
                        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            data = Encoding.ASCII.GetString(bytes, 0, i);
                            
                        }*/
                    }
                    stream.Dispose();
                    client.Close();
                }
            }
            catch (Exception)
            {
                //Console.WriteLine("SocketException: {0}", e);
                stream.Dispose();
                client.Close();
            }
            finally
            {
                TcpServerListener.Stop();
                TcpServerShutdown = true;
            }
        }

        /// <summary>
        /// Sends all the available testNames to the user
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="username"></param>
        private static void SendTestsListToMessage(NetworkStream stream, string username)
        {
            string data = null;
            byte[] msg = Encoding.ASCII.GetBytes(data);
           // stream.Write(msg, 0, msg.Length);
        }

        /// <summary>
        /// Sends all the test files available for the test to the user
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="username"></param>
        /// <param name="testName"></param>
        /// <param name="isDescriptionRequested"></param>
        private static void SendFilesToMessage(NetworkStream stream, string username, string testName, bool isDescriptionRequested)
        {
            if (isDescriptionRequested)
            {

            }
            else
            {

            }
            string data = null;
            //byte[] msg = Encoding.ASCII.GetBytes(data);
            //stream.Write(msg, 0, msg.Length);
        }

        /// <summary>
        /// Gets and saves the testfile in the corresponding test folder
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="username"></param>
        /// <param name="testName"></param>
        private static void SaveTestFile(NetworkStream stream, string username, string testName)
        {
            //save the testfile in the correct test folder
        }

        /// <summary>
        /// Gets and saves the evaulation in the username/testName/
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="username"></param>
        /// <param name="testName"></param>
        private static void SaveTestEvaulation(NetworkStream stream, string username, string testName)
        {
            //save the evaulation in the username/testName/
        }

        #endregion
    }
}