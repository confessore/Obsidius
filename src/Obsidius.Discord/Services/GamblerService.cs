using Obsidius.Discord.Models;
using Obsidius.Discord.Services.Interfaces;

namespace Obsidius.Discord.Services
{
    public class GamblerService : IGamblerService
    {
        public Gambler Gambler { get; set; } = new Gambler();
    }
}
