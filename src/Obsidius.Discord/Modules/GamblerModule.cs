using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Obsidius.Discord.Models;
using Obsidius.Discord.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Obsidius.Discord.Modules
{
    public class GamblerModule : ModuleBase<SocketCommandContext>
    {
        readonly IServiceProvider services;
        readonly DiscordSocketClient client;
        readonly CommandService commands;
        readonly IGamblerService gambler;

        public GamblerModule(
            IServiceProvider services,
            DiscordSocketClient client,
            CommandService commands,
            IGamblerService gambler)
        {
            this.services = services;
            this.client = client;
            this.commands = commands;
            this.gambler = gambler;
            Random = new Random();
        }

        Random Random { get; }

        [Command("new", RunMode = RunMode.Async)]
        [Summary("all: create a new game" +
            "\n >new")]
        async Task NewAsync()
        {
            await RemoveCommandMessageAsync();
            if (!gambler.Gambler.Started)
            {
                gambler.Gambler = new Gambler()
                {
                    Started = true
                };
                await ReplyAsync("closing rolls in 30 seconds" +
                    "\ntype '>roll' to roll");
                await Task.Delay(30000);
                gambler.Gambler.Closed = true;
                await ReplyAsync("rolls are now closed");
                var winner = gambler.Gambler.Gamblers.OrderBy(x => x.Value).LastOrDefault();
                var loser = gambler.Gambler.Gamblers.OrderBy(x => x.Value).FirstOrDefault();
                while (winner.Value == loser.Value)
                {
                    var winnerReroll = Random.Next(1, 100);
                    var loserReroll = Random.Next(1, 100);
                    if (winnerReroll >= loserReroll)
                    {
                        winner = new KeyValuePair<SocketUser, int>(winner.Key, winnerReroll);
                        loser = new KeyValuePair<SocketUser, int>(loser.Key, loserReroll);
                    }
                    else if (loserReroll > winnerReroll)
                    {
                        winner = new KeyValuePair<SocketUser, int>(winner.Key, loserReroll);
                        loser = new KeyValuePair<SocketUser, int>(loser.Key, winnerReroll);
                    }
                    await ReplyAsync((((SocketGuildUser)winner.Key).Nickname ?? winner.Key.Username) + " and " + (((SocketGuildUser)loser.Key).Nickname ?? loser.Key.Username) + " tied" +
                        "\n" + (((SocketGuildUser)winner.Key).Nickname ?? winner.Key.Username) + " rolls a " + winner.Value +
                        "\n" + (((SocketGuildUser)loser.Key).Nickname ?? loser.Key.Username) + " rolls a " + loser.Value);
                }
                gambler.Gambler.Started = false;
                var embedBuilder = new EmbedBuilder();
                foreach (var gambler in gambler.Gambler.Gamblers)
                    embedBuilder.AddField(((SocketGuildUser)gambler.Key).Nickname ?? gambler.Key.Username, gambler.Value);
                await ReplyAsync("", false, embedBuilder.Build());
                await ReplyAsync((((SocketGuildUser)loser.Key).Nickname ?? loser.Key.Username) + " owes " + (((SocketGuildUser)winner.Key).Nickname ?? winner.Key.Username) + " " + (winner.Value - loser.Value) + " silver" +
                    "\nyou're welcome");
            }
            else
                await ReplyAsync("a gambler has already started");
        }

        [Command("roll", RunMode = RunMode.Async)]
        async Task RollAsync()
        {
            await RemoveCommandMessageAsync();
            if (gambler.Gambler.Started && !gambler.Gambler.Closed)
            {
                var roll = Random.Next(1, 100);
                if (!gambler.Gambler.Gamblers.ContainsKey(Context.User))
                {
                    gambler.Gambler.Gamblers.Add(Context.User, roll);
                    //await ReplyAsync((((SocketGuildUser)Context.User).Nickname ?? Context.User.Username) + " rolls a " + roll);
                }
                else
                    await ReplyAsync((((SocketGuildUser)Context.User).Nickname ?? Context.User.Username) + " already rolled");
            }
            else
                await ReplyAsync("a gambler has not started");
        }

        async Task RemoveCommandMessageAsync() =>
            await client.GetGuild(Context.Guild.Id).GetTextChannel(Context.Message.Channel.Id).DeleteMessageAsync(Context.Message);
    }
}
