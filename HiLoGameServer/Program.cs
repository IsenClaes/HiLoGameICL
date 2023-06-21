using HiLoGameServer.Classes;

namespace HiLoGameServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hi-Lo Game Server");

            HiLoServer server = new HiLoServer();
            server.Start();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}