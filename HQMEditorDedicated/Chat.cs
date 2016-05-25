using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HQMEditorDedicated
{
    public class Chat
    {
        const int PLAYER_SLOT_OFFSET = -0x20;
        const int LAST_MESSAGE = 0x00CFA430;
        const int MESSAGE_COUNT = 0x00CFA534;
        const int MESSAGES = 0x0106B620;
        const int MESSAGE_SIZE = 0x80;

        const int COMMAND_SOURCE = 0x00438000;
        const int COMMAND = 0x00CFA430;

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
        /// The last command sent to the server. Must call RecordCommandSource() or this won't work.
        /// </summary>
        public static ChatMessage LastCommand
        {
            get
            {
                int ipIndex = MemoryEditor.ReadInt(COMMAND_SOURCE);
                int playerIndex = MemoryEditor.ReadInt(0x004138C0 + (ipIndex * 0x4C) + 0x08);
                if (ipIndex == -1)
                    return null;
                return new ChatMessage(COMMAND, playerIndex);
            }
        }

        /// <summary>
        /// Removes the last command from game memory. Usefull when checking if there is a new command
        /// </summary>
        public static void FlushLastCommand()
        {
            MemoryEditor.WriteInt(-1, COMMAND_SOURCE);
            MemoryEditor.WriteString(" \0", COMMAND);
        }

        /// <summary>
        /// Messages sent in the server
        /// </summary>
        public static List<ChatMessage> Messages
        {
            get
            {
                List<ChatMessage> messages = new List<ChatMessage>();
                for (int i = 0; i <= MessageCount; i++)
                {
                    int address = MESSAGES + MESSAGE_SIZE * i;
                    messages.Add(new ChatMessage(address, MemoryEditor.ReadInt(address + PLAYER_SLOT_OFFSET)));
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

        /// <summary>
        /// Forces the server to save the player index for commands starting with '/'
        /// Thanks to u/tema2
        /// </summary>
        public static void RecordCommandSource()
        {
            byte[] code_original = new byte[6] { 0xE9, 0x09, 0x0B, 0x12, 0x00, 0x90 };
            byte[] code_inject = new byte[18] { 0x8B, 0x55, 0x8C, 0x89, 0x15, 0x00, 0x80, 0x43, 0x00, 0x6B, 0xD2, 0x4C, 0xE9, 0xE7, 0xF4, 0xED, 0xFF, 0x90 };

            int addr_original = 0x0040F4F2;
            int addr_inject = 0x00530000;

            MemoryEditor.WriteBytes(code_inject, addr_inject);
            MemoryEditor.WriteBytes(code_original, addr_original);
        }


        public class ChatMessage
        {
            public Player Sender;
            public string Message;

            internal ChatMessage(int address, int playerSlot)
            {
                Message = MemoryEditor.ReadString(address, 63);
                if (playerSlot >= 0 && playerSlot < PlayerManager.Players.Length)
                    Sender = PlayerManager.Players[playerSlot];
            }
        }
    }
}
