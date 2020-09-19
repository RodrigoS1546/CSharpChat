using System;
using System.IO;

using ChatServer;

namespace ExamplePlugin
{
    public class ExamplePlugin : IPlugin
    {
        [ChatServerEventHandler("OnLoad")]
        public void OnLoad()
        {
            Console.WriteLine("ExamplePlugin Loaded Successfully!");
        }

        [ChatServerEventHandler("OnJoin")]
        public void ClientOnJoinEvent(string nickname, string address, int port)
        {
            File.AppendAllText($"plugins{Path.DirectorySeparatorChar}users.txt", $"{nickname} at {address}:{port}{Environment.NewLine}");
            Client.BroadcastInfo($"Welcome {nickname}! See '!commands' to see the commands!");
        }

        [ChatServerEventHandler("OnQuit")]
        public void ClientOnQuitEvent(string nickname)
        {
            Console.WriteLine($"OnQuit Event Called with {nickname}");
        }
        [ChatServerEventHandler("OnQuit")]
        public void SecondOnQuitEvent(string nickname)
        {
            Console.WriteLine("Nice!");
        }
    }
}
