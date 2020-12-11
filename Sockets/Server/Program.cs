using System;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main()
        {
            try
            {
                Console.WriteLine("Укажите кол-во серверов");
                var scaleService = new ScaleService(Convert.ToInt32(Console.ReadLine()));

                scaleService.CreateScale();

                Console.WriteLine("Сокеты запущены. Ожидание подключений...");
                scaleService.Listen();

                Console.WriteLine("Получение информации от клиента");
                Parallel.ForEach(scaleService.Sockets, i => i.RecieveData());
                Parallel.ForEach(scaleService.Sockets, i => i.FilterProcess());
                Parallel.ForEach(scaleService.Sockets, i => i.Send());
                //не должен закрывать коннекшны, пока клиент не завершит получение обработанных данных
                scaleService.CloseConnections();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}