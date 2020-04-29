using Discord.WebSocket;
using System.Threading.Tasks;

namespace Obsidius.Discord.Services.Interfaces
{
    public interface IHtmlService
    {
        Task VerifyGuildMemberAsync(SocketGuild guild, SocketGuildUser user, string name);
    }
}
