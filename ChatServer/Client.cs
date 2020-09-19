using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;

namespace ChatServer
{
    public class Client
    {
        private readonly static List<TcpClient> _clients = new List<TcpClient>();
        private readonly TcpClient _client;
        private string _nickname;

        public Client(TcpClient clientSock)
        {
            _client = clientSock;
        }

        public void Start()
        {
            new Thread(HandleClient).Start(); // Starting separate thread
        }

        protected void HandleClient()
        {
            StreamReader reader = new StreamReader(_client.GetStream());
            // Creating reader object for receiving any information from the client

            try
            {
                // Gets the address and port of the connection
                string address = ((IPEndPoint)_client.Client.RemoteEndPoint).Address.ToString();
                int port = ((IPEndPoint)_client.Client.RemoteEndPoint).Port;

                Console.WriteLine($"Connection Received from {address}:{port}");
                // Logs the Address and Port

                _nickname = reader.ReadLine(); // reading the client's nickname
                _clients.Add(_client); // adding the client to the server's list

                BroadcastInfo($"{_nickname} joined the chat!"); // join message broadcast
                Console.WriteLine($"{_nickname} is connected from {address}:{port}");
                // Logs the Nickname, Address and Port

                // Emits Join Event
                Program.PluginManager.EmitEvent("OnJoin", new object[] { _nickname, address, port });

                // Displays the IP and port of the Client for statistics or whatever

                // While loop receiving and broadcasting messages
                while (true) BroadcastMessage(this, reader.ReadLine());
            }
            catch {}

            _clients.Remove(_client); // removing the client from the server's list
            BroadcastInfo($"{_nickname} left the chat!"); // quit message broadcast

            // Emits Quit Event
            Program.PluginManager.EmitEvent("OnQuit", new object[] { _nickname });
        }

        public static void BroadcastInfo(string message)
        {
            Console.WriteLine(message); // Displaying the message on screen

            // For each client online creating a Writer Object and writing the message
            foreach (TcpClient client in _clients)
                new StreamWriter(client.GetStream()) { AutoFlush = true }.WriteLine(message);
        }

        public static void BroadcastMessage(Client sender, string message)
        {
            string finalmessage = $"{sender._nickname} >> {message}";
            // Mounting the Message

            Console.WriteLine(finalmessage); // Displaying it on screen

            // For each client online creating a Writer Object and writing the message
            // Unless that client is the sender
            foreach (TcpClient tcpclient in _clients)
            {
                if (tcpclient.Equals(sender._client))
                    continue;
                new StreamWriter(tcpclient.GetStream()) { AutoFlush = true }.WriteLine(finalmessage);
            }
        }
    }
}
