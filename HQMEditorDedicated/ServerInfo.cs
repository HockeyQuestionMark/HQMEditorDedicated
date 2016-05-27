using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HQMEditorDedicated
{
    public class ServerInfo
    {
        const int SERVER_NAME = 0x01C1400E;
        const int SERVER_PASSWORD = 0x00438704;
        const int PLAYER_COUNT = 0x00CFA400;
        const int MAX_PLAYERS = 0x00438748;
        const int UPTIME = 0x018931F0;

        /// <summary>
        /// The number of players in the server
        /// </summary>
        public static int PlayerCount
        {
            get 
            {
                int playerCount = 0;
                foreach(Player p in PlayerManager.Players)
                {
                    if (p.InServer) playerCount++;
                }
                return playerCount;
            }           
        }

        /// <summary>
        /// The admin password of the server
        /// </summary>
        public static string Password
        {
            get { return MemoryEditor.ReadString(SERVER_PASSWORD, 100); }
            set { MemoryEditor.WriteString(value, SERVER_PASSWORD); }
        }

        /// <summary>
        /// The name of the server
        /// </summary>
        public static string Name
        {
            get { return MemoryEditor.ReadString(SERVER_NAME, 100); }
            set { MemoryEditor.WriteString(value, SERVER_NAME); }
        }

        /// <summary>
        /// The max number of players allowed on the server
        /// </summary>
        public static int MaxPlayerCount
        {
            get { return MemoryEditor.ReadInt(MAX_PLAYERS); }
        }

        /// <summary>
        /// The amount of time the server has been active
        /// </summary>
        public static TimeSpan Uptime
        {
            get { return new TimeSpan(0, 0, MemoryEditor.ReadInt(UPTIME) / 100); }
            set { MemoryEditor.WriteInt((int)value.TotalSeconds * 100, UPTIME); }
        }
    }
}
