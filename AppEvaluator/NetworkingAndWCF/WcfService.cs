using ServerContracts.Interfaces;
using System.Net;
using System.ServiceModel;

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

        public static void ConnectToServices(IPAddress ip, int serverPort)
        {
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None)
            {
                OpenTimeout = new System.TimeSpan(0, 0, 0, 1),
                ReceiveTimeout = new System.TimeSpan(0, 0, 1),
                SendTimeout = new System.TimeSpan(0, 0, 0, 1),
                CloseTimeout = new System.TimeSpan(0, 0, 1)
            };
            MainCommunicationChannel = new ChannelFactory<ISelectionService>(binding);
            _selectionUriData = $"net.tcp://{ip}:{serverPort}/MainService";
            EndpointAddress endpoint = new EndpointAddress(_selectionUriData);
            MainProxy = MainCommunicationChannel.CreateChannel(endpoint);
            try
            {
                Stores.ConnectionStore.ConnectionStatus = MainProxy?.ConnectionTest();
            }
            catch (System.Exception)
            {
                Stores.ConnectionStore.ConnectionStatus = false;
                MainCommunicationChannel.Abort();
            }

            if (Stores.ConnectionStore.ConnectionStatus == true)
            {
                binding.TransferMode = TransferMode.Streamed;
                FileChannel = new ChannelFactory<IFileService>(binding);
                _fileUriData = $"net.tcp://{ip}:{serverPort}/FileService";
                endpoint = new EndpointAddress(_fileUriData);
                FileProxy = FileChannel.CreateChannel(endpoint);
            }
        }
    }
    
}
