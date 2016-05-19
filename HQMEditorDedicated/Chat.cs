using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HQMRules
{
    class Chat
    {
        const int LAST_MESSAGE = 0x00CFA430;
        const int MESSAGE_COUNT = 0x00CFA534;
        const int MESSAGES = 0x0106B620;
        const int MESSAGE_SIZE = 0x80;

        /// <summary>
        /// Number of messages sent to server
        /// </summary>
        public static int MessageCount
        {
            get { return MemoryEditor.ReadInt(MESSAGE_COUNT); }
            set { MemoryEditor.WriteInt(value, MESSAGE_COUNT); }
        }

        /// <summary>
        /// The last message sent to the server
        /// </summary>
        public static string LastMessage
        {
            get { return MemoryEditor.ReadString(LAST_MESSAGE, 128); }
        }

        /// <summary>
        /// Messages sent in the server
        /// </summary>
        public static List<string> Messages
        {
            get 
            {
                List<string> messages = new List<string>();
                for(int i = 0; i < MessageCount; i++)
                {
                    messages.Add(MemoryEditor.ReadString(MESSAGES + MESSAGE_SIZE * i, 128));
                }
                return messages;
            }
        }

        /// <summary>
        /// Send a message to chat
        /// </summary>
        /// <param name="message">the message to send, only the first 63 characters will be shown</param>
        public static void SendMessage(string message)
        {
            int count = MessageCount + 1;

            if (message.Length > 63)
                message = message.Substring(0, 63);

            int msgOffset = MESSAGES + count * MESSAGE_SIZE;
            int offset2 = msgOffset - 0x40;
            int msgIndexOffset = msgOffset - 0x20;
            int msgLengthOffset = msgOffset - 0x1C;
            MemoryEditor.WriteInt(2, offset2);
            MemoryEditor.WriteInt(-1, msgIndexOffset);
            MemoryEditor.WriteInt(message.Length, msgLengthOffset);
            MemoryEditor.WriteString(message, msgOffset);
           
            
            MessageCount = count;
        }
    }
}
