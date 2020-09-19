using System;
using System.Net;
using System.Net.Sockets;

namespace ChatServer
{
    public static class Program
    {
        public static PluginManager PluginManager { get; private set; }
        private static TcpListener _serverListener;
        public static void Main()
        {
            // Console Settings begin
            Console.ForegroundColor = ConsoleColor.White; // Set's the Console Foreground to White
            Console.BackgroundColor = ConsoleColor.Black; // Set's the Console Background to Black
            Console.Title = "ChatServer"; // Set's the Console title to ChatServer
            Console.Clear(); // Clears the screen so that those changes are applied
            // Console Settings end

            _serverListener = new TcpListener(IPAddress.Any, 15246);
            // Creates the listener object at any ip address with the port 15246

            _serverListener.Start(); // Starts listening for connections

            Console.WriteLine($"Server is listening for connections at {IPAddress.Any}:15246");
            // Displaying in screen the IP Address and the port

            PluginManager = new PluginManager(); // Instanciating Plugin Manager
            PluginManager.LoadPlugins(); // Loading Plugins
            PluginManager.EmitEvent("OnLoad", null); // Emiting on load event

            // While loop accepting connection and handling them on separate thread
            while (true)
            {
                TcpClient clientSock = _serverListener.AcceptTcpClient();

                new Client(clientSock).Start();
            }
        }
    }
}
