using Discord.WebSocket;
using System.Threading.Tasks;

namespace Obsidius.Discord.Services.Interfaces
{
    public interface IBaseService
    {
        Task<SocketRole> GetGuildRoleAsync(SocketGuild guild, string name);
        Task ModifyRoleAsync(SocketGuild guild, SocketGuildUser user, string name);
    }
}
