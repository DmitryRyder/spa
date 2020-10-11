﻿using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class Program
    {
        static int port1 = 8005; // порт для приема входящих запросов
        static int port2 = 8006; // порт для приема входящих запросов
        static void Main(string[] args)
        {
            // получаем адреса для запуска сокета
            IPEndPoint ipPoint1 = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port1);
            IPEndPoint ipPoint2 = new IPEndPoint(IPAddress.Parse("127.0.0.2"), port2);


            // создаем сокет
            Socket listenSocket1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket listenSocket2 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
                listenSocket1.Bind(ipPoint1);
                listenSocket2.Bind(ipPoint2);
                // начинаем прослушивание
                listenSocket1.Listen(10);
                listenSocket2.Listen(10);

                Console.WriteLine("Сокеты запущены. Ожидание подключений...");

                while (true)
                {
                    Socket handler1 = listenSocket1.Accept();
                    Socket handler2 = listenSocket2.Accept();

                    // получаем сообщение
                    StringBuilder builder = new StringBuilder();
                    int bytes1 = 0; // количество полученных байтов
                    int bytes2 = 0; // количество полученных байтов
                    byte[] data = new byte[256]; // буфер для получаемых данных

                    do
                    {
                        bytes1 = handler1.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes1));
                        bytes2 = handler2.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes2));
                    }
                    while (handler1.Available > 0 || handler2.Available > 0);

                    Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder.ToString());

                    // отправляем ответ
                    string message = "ваше сообщение доставлено";
                    data = Encoding.Unicode.GetBytes(message);
                    handler1.Send(data);
                    handler2.Send(data);
                    // закрываем сокет
                    handler1.Shutdown(SocketShutdown.Both);
                    handler1.Close();
                    handler2.Shutdown(SocketShutdown.Both);
                    handler2.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}