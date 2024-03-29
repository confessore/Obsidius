﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace Obsidius.Discord.Modules
{
    public class LogModule : ModuleBase
    {
        public LogModule(DiscordSocketClient client)
        {
            client.Log += Log;
        }

        Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
