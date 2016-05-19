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

        /// <summary>
        /// The number of players in the server
        /// </summary>
        public static int PlayerCount
        {
            get { return MemoryEditor.ReadInt(PLAYER_COUNT); }           
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
    }
}
