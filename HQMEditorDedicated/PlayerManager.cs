using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HQMEditorDedicated
{
    public class PlayerManager
    {    

        /// <summary>
        /// An array of Player objects for every player in the server
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
        /// Attempts to find a player by slot
        /// </summary>
        /// <param name="slot">the slot the player is in</param>
        /// <returns>A Player class or null if invalid slot given</returns>
        public static Player GetPlayerFromSlot(int slot)
        {
            if (slot > ServerInfo.MaxPlayerCount || slot < 0)
            {
                return null;
            }
            return new Player(slot);
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
    }
}
