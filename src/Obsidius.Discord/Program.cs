using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Obsidius.Discord.Services;
using Obsidius.Discord.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Obsidius.Discord
{
    class Program
    {
        readonly IServiceProvider services;
        readonly DiscordSocketClient client;

        Program()
        {
            client = new DiscordSocketClient();
            services = ConfigureServices();
        }

        static void Main(string[] args) =>
            new Program().MainAsync().GetAwaiter().GetResult();

        async Task MainAsync()
        {
            await services.GetRequiredService<IRegistrationService>().IntializeRegistrationsAsync();
            await client.LoginAsync(
                TokenType.Bot,
                Environment.GetEnvironmentVariable("ObsidiusDiscordToken"));
            await client.StartAsync();
            await client.SetGameAsync("'>help' for commands");
            await Task.Delay(-1);
        }

        IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(client)
                .AddSingleton<CommandService>()
                .AddSingleton<IEventService, EventService>()
                .AddSingleton<IBaseService, BaseService>()
                .AddSingleton<IHtmlService, HtmlService>()
                .AddSingleton<IGamblerService, GamblerService>()
                .AddSingleton<IRegistrationService, RegistrationService>()
                .BuildServiceProvider();
        }
    }
}
