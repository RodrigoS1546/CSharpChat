using System;
using System.IO;
using System.Threading;
using System.Net.Sockets;

namespace ClientWindows
{
    public static class Program
    {
        private static TcpClient _tcpClient;
        private static StreamReader _reader;
        private static StreamWriter _writer;
        public static void Main()
        {
            // Console Settings begin
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Title = "ChatClient";
            Console.Clear();
            // Console Settings end

            _tcpClient = new TcpClient();

            _tcpClient.Connect("localhost", 15246);

            _reader = new StreamReader(_tcpClient.GetStream());
            _writer = new StreamWriter(_tcpClient.GetStream()) { AutoFlush = true };

            Console.Write("Enter your nickname: ");
            _writer.WriteLine(Console.ReadLine());

            new Thread(ReceivingLoop).Start();

            while (true) _writer.WriteLine(Console.ReadLine());
        }
        private static void ReceivingLoop()
        {
            try
            {
                while (true)
                {
                    string data = _reader.ReadLine();
                    Console.WriteLine(data);
                }
            }
            catch
            {
                Console.WriteLine("The connection was closed by the host!");
                Console.ReadKey(true);
                Environment.Exit(0);
            }
        }
    }
}
