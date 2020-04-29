using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Obsidius.Discord.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Obsidius.Discord.Modules
{
    public class CommandModule : ModuleBase<SocketCommandContext>
    {
        readonly IServiceProvider services;
        readonly DiscordSocketClient client;
        readonly CommandService commands;
        readonly IHtmlService htmlService;

        public CommandModule(
            IServiceProvider services,
            DiscordSocketClient client,
            CommandService commands,
            IHtmlService htmlService)
        {
            this.services = services;
            this.client = client;
            this.commands = commands;
            this.htmlService = htmlService;
        }

        readonly Random random = new Random();

        [Command("help", RunMode = RunMode.Async)]
        [Summary("all: displays available commands" +
            "\n >help")]
        async Task HelpAsync()
        {
            await RemoveCommandMessageAsync();
            var embedBuilder = new EmbedBuilder();
            foreach (var command in await commands.GetExecutableCommandsAsync(Context, services))
                embedBuilder.AddField(command.Name, command.Summary ?? "no summary available");
            await ReplyAsync("here's a list of commands and their summaries: ", false, embedBuilder.Build());
        }

        [Command("insult", RunMode = RunMode.Async)]
        [Summary("all: got 'em" +
            "\n >insult")]
        async Task InsultAsync()
        {
            await RemoveCommandMessageAsync();
            await ReplyAsync("your mother");
        }

        [Command("nick", RunMode = RunMode.Async)]
        [Summary("all: change your nick" +
            "\n >nick 'your nick here'")]
        async Task NickAsync([Remainder] string name)
        {
            await RemoveCommandMessageAsync();
            await client.GetGuild(Context.Guild.Id).GetUser(Context.User.Id).ModifyAsync(x => x.Nickname = name);
        }

        [Command("verify", RunMode = RunMode.Async)]
        [Summary("all: updates a single guild user's membership role according to their nickname (character name)" +
            "\n >verify" +
            "\n >verify")]
        async Task VerifyAsync()
        {
            await RemoveCommandMessageAsync();
            var user = client.GetGuild(Context.Guild.Id).GetUser(Context.User.Id);
            await htmlService.VerifyGuildMemberAsync(Context.Guild, user, user.Nickname ?? user.Username);
        }

        async Task RemoveCommandMessageAsync() =>
            await client.GetGuild(Context.Guild.Id).GetTextChannel(Context.Message.Channel.Id).DeleteMessageAsync(Context.Message);
    }
}
