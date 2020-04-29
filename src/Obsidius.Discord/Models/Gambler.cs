using Discord.WebSocket;
using System.Collections.Generic;

namespace Obsidius.Discord.Models
{
    public class Gambler
    {
        public bool Started { get; set; }
        public bool Closed { get; set; }
        public Dictionary<SocketUser, int> Gamblers { get; set; } = new Dictionary<SocketUser, int>();
    }
}
