using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HQMEditorDedicated
{
    public class PlayerManager
    {
        const int BANNED_IP_COUNT = 0x01C142A8;
        const int BANNED_IP_LIST = 0x005BA2C0;

        /// <summary>
        /// An array of Player objects for every player slot
        /// </summary>
        public static Player[] Players
        {
            get
            {
                List<Player> playerList = new List<Player>();
                for (int i = 0; i < ServerInfo.MaxPlayerCount; i++)
                {
                    Player player = new Player(i);
                    playerList.Add(player);
                }

                return playerList.ToArray();
            }
        }

        /// <summary>
        /// An array of Player objects for every player in the server
        /// </summary>
        public static Player[] PlayersInServer
        {
            get
            {
                List<Player> playerList = new List<Player>();
                for (int i = 0; i < ServerInfo.MaxPlayerCount; i++)
                {
                    Player player = new Player(i);
                    if(player.InServer)
                        playerList.Add(player);
                }

                return playerList.ToArray();
            }
        }

        /// <summary>
        /// Attempts to find a player by name
        /// </summary>
        /// <param name="name">The name of the player to search for</param>
        /// <returns>A Player class or null if no player with that name found</returns>
        public static Player GetPlayerFromName(string name)
        {
            foreach (Player player in Players)
            {
                if (player.Name == name)
                    return player;
            }

            // No player by that name
            return null;
        }

        /// <summary>
        /// A list of banned IPs. They are reversed for some reason.
        /// </summary>
        public static List<byte[]> BannedIPs
        {
            get
            {
                List<byte[]> bannedIPs = new List<byte[]>();
                for(int i = 0; i < MemoryEditor.ReadInt(BANNED_IP_COUNT); i++)
                {
                    byte[] ip = MemoryEditor.ReadBytes(BANNED_IP_LIST + i * 0x04, 4);
                    bannedIPs.Add(ip);
                }
                return bannedIPs;
            }
        }

        /// <summary>
        /// Ban an ip. Best to use in conjunction with Player.IPAddress
        /// </summary>
        /// <param name="ip">the ip to ban</param>
        public static void BanIP(byte[] ip)
        {
            int count = MemoryEditor.ReadInt(BANNED_IP_COUNT);
            MemoryEditor.WriteInt(count + 1, BANNED_IP_COUNT);
            MemoryEditor.WriteBytes(ip, BANNED_IP_LIST + count * 0x04);
        }
    }
}
