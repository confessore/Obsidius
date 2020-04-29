using Discord.WebSocket;
using Obsidius.Discord.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Obsidius.Discord.Services
{
    public class BaseService : IBaseService
    {
        readonly DiscordSocketClient client;

        public BaseService(DiscordSocketClient client)
        {
            this.client = client;
        }

        public Task<SocketRole> GetGuildRoleAsync(SocketGuild guild, string name) =>
            Task.FromResult(client.GetGuild(guild.Id).Roles.FirstOrDefault(x => x.Name.ToLower() == name.ToLower()));

        public async Task ModifyRoleAsync(SocketGuild guild, SocketGuildUser user, string name) =>
            await client.GetGuild(guild.Id).GetUser(user.Id).AddRoleAsync(await GetGuildRoleAsync(guild, name));
    }
}
