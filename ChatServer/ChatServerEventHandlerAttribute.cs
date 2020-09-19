using System;

namespace ChatServer
{
    public class ChatServerEventHandlerAttribute : Attribute
    {
        public readonly string EventName;
        public ChatServerEventHandlerAttribute(string eventName)
        {
            EventName = eventName;
        }
    }
}
