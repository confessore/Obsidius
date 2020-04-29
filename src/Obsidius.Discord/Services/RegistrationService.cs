using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using Obsidius.Discord.Services.Interfaces;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Obsidius.Discord.Services
{
    public class RegistrationService : IRegistrationService
    {
        readonly IServiceProvider services;
        readonly CommandService commands;

        public RegistrationService(
            IServiceProvider services,
            CommandService commands)
        {
            this.services = services;
            this.commands = commands;
        }

        public async Task IntializeRegistrationsAsync()
        {
            await RegisterServices();
            await RegisterModulesAsync();
            Console.WriteLine("registration completed!");
        }

        async Task RegisterModulesAsync()
        {
            Console.WriteLine("registering modules...");
            await commands.AddModulesAsync(
                Assembly.GetEntryAssembly(),
                services);
        }

        Task RegisterServices()
        {
            Console.WriteLine("registering services...");
            services.GetRequiredService<IEventService>();
            services.GetRequiredService<IBaseService>();
            services.GetRequiredService<IHtmlService>();
            return Task.CompletedTask;
        }
    }
}
