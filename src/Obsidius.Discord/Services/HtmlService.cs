using Discord.WebSocket;
using HtmlAgilityPack;
using Obsidius.Discord.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Obsidius.Discord.Services
{
    public class HtmlService : IHtmlService
    {
        readonly IBaseService baseService;
        readonly HttpClient httpClient;
        public HtmlService(
            IBaseService baseService)
        {
            this.baseService = baseService;
            httpClient = new HttpClient();
        }

        async Task<HtmlDocument> LookupGuildMembersAsync() =>

            await GetHtmlDocumentFromStringAsync($"https://endless.gg/armory/guild/obsidius");

        async Task<string> GetStringFromUrlAsync(string url)
        {
            var response = await httpClient.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        async Task<HtmlDocument> GetHtmlDocumentFromStringAsync(string url)
        {
            var tmp = new HtmlDocument();
            tmp.LoadHtml(await GetStringFromUrlAsync(url));
            return tmp;
        }

        async Task<IEnumerable<string>> GetGuildMembersAsync()
        {
            var document = await LookupGuildMembersAsync();
            var tmp = new List<string>();
            foreach (var node in document.DocumentNode.SelectNodes(@"//tbody//tr//td//a"))
                tmp.Add(node.InnerText);
            return tmp;
        }

        public async Task VerifyGuildMemberAsync(SocketGuild guild, SocketGuildUser user, string name)
        {
            var role = await baseService.GetGuildRoleAsync(guild, "member");
            if (!user.Roles.Contains(role))
            {
                var members = await GetGuildMembersAsync();
                if (members.Any(x => x == name))
                    await baseService.ModifyRoleAsync(guild, user, role.Name);
            }

        }
    }
}
