using ServerContracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AppEvaluator.NetworkingAndWCF
{
    internal static class WcfService
    {
        private static string _selectionUriData;
        private static string _fileUriData;
        public static ISelectionService MainProxy { get; set; }
        public static IFileService FileProxy { get; set; }
        public static ChannelFactory<ISelectionService> MainCommunicationChannel { get; set; }
        public static ChannelFactory<IFileService> FileChannel { get; set; }

        public static void ConnectToSelectionService(IPAddress ip, int serverPort)
        {
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            MainCommunicationChannel = new ChannelFactory<ISelectionService>(binding);
            _selectionUriData = $"net.tcp://{ip}:{serverPort}/MainService";
            EndpointAddress endpoint = new EndpointAddress(_selectionUriData);
            MainProxy = MainCommunicationChannel.CreateChannel(endpoint);

            binding.TransferMode = TransferMode.Streamed;
            FileChannel = new ChannelFactory<IFileService>(binding);
            _fileUriData = $"net.tcp://{ip}:{serverPort}/FileService";
            endpoint = new EndpointAddress(_fileUriData);
            FileProxy = FileChannel.CreateChannel(endpoint);
        }
    }
    
}
