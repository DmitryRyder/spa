//using System;
//using System.Net.Sockets;
//using System.Text;

//namespace ConsoleClient
//{
//    public class NetworkClient : IConnection
//    {
//        private readonly TcpClient _client;
//        private static NetworkStream _stream;

//        public NetworkClient(TcpClient client)
//        {
//            _client = client;
//        }

//        public void Connect(string ipAdress, int port, string nameOfClient)
//        {
//            try
//            {
//                _client.Connect(ipAdress, port);
//                _stream = _client.GetStream();
//                string message = nameOfClient;
//                byte[] data = Encoding.Unicode.GetBytes(message);
//                _stream.Write(data, 0, data.Length);
//            }
//            catch (Exception ex)
//            {
//                var exception = ex;
//            }
//        }

//        //public void SendData(Direction direction)
//        //{
//        //    var t = direction.ToString();
//        //    byte[] data = { (byte)direction };
//        //    _stream.Write(data, 0, data.Length);
//        //}

//        //public Direction RecieveData()
//        //{
//        //    byte[] data = new byte[64]; // буфер для получаемых данных
//        //    int dataValue = 0;
//        //    do
//        //    {
//        //        _stream.Read(data, 0, data.Length);
//        //    }
//        //    while (_stream.DataAvailable);
//        //    dataValue = BitConverter.ToInt32(data, 0);
//        //    return (Direction)dataValue;
//        //}

//        private void Disconnect()
//        {
//            if (_client != null)
//                _client.Close();//отключение клиента
//            Environment.Exit(0); //завершение процесса
//        }
//    }
//}
