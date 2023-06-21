using System;
using HiLoGameClient.Classes;

namespace HiLoGameClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hi-Lo Game Client");

            HiLoClient client = new HiLoClient();
            client.Start();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}