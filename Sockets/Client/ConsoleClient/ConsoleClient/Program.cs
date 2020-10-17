using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Linq;

namespace ConsoleClient
{
    class Program
    {
        Action[] actionArray = new Action[10];
        
        // адрес и порт сервера, к которому будем подключаться
        static int port1 = 8005; // порт сервера
        static string address1 = "127.0.0.1"; // адрес сервера
        static int port2 = 8006; // порт сервера
        static string address2 = "127.0.0.2"; // адрес сервера
        static void Main(string[] args)
        {
            try
            {
                IPEndPoint ipPoint1 = new IPEndPoint(IPAddress.Parse(address1), port1);
                IPEndPoint ipPoint2 = new IPEndPoint(IPAddress.Parse(address2), port2);

                Socket socket1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Socket socket2 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // подключаемся к удаленному хосту
                socket1.Connect(ipPoint1);
                socket2.Connect(ipPoint2);
                Console.Write("Введите сообщение:");
                string message = Console.ReadLine();
                byte[] data = Encoding.Unicode.GetBytes(message);
                socket1.Send(data);
                socket2.Send(data);

                // получаем ответ
                data = new byte[256]; // буфер для ответа
                StringBuilder builder1 = new StringBuilder();
                StringBuilder builder2 = new StringBuilder();
                int bytes1 = 0; // количество полученных байт
                int bytes2 = 0; // количество полученных байт

                do
                {
                    bytes1 = socket1.Receive(data, data.Length, 0);
                    builder1.Append(Encoding.Unicode.GetString(data, 0, bytes1));

                    bytes2 = socket2.Receive(data, data.Length, 0);
                    builder2.Append(Encoding.Unicode.GetString(data, 0, bytes2));
                }
                while (socket1.Available > 0 || socket2.Available > 0);
                Console.WriteLine("ответ сервера 1: " + builder1.ToString());
                Console.WriteLine("ответ сервера 2: " + builder2.ToString());

                // закрываем сокет
                socket1.Shutdown(SocketShutdown.Both);
                socket1.Close();
                socket2.Shutdown(SocketShutdown.Both);
                socket2.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.Read();
        }

        private IPEndPoint[] SocketsScale(int numberOfServers)
        {
            var defaultIp = "127.0.0.1";
            var defaultPort = 8001;
            var ipPoints = new IPEndPoint[numberOfServers];

            for (var i = 0; i < numberOfServers; i++)
            {
                var ip = defaultIp.Replace(defaultIp.Split('.').LastOrDefault(), i.ToString());
                var port = defaultPort + i;
                ipPoints[i] = new IPEndPoint(IPAddress.Parse(ip), port);
            }

            return ipPoints;
        }
    }
}