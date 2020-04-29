#!/bin/sh

sudo systemctl stop obsidius.discord.service

sudo systemctl disable obsidius.discord.service

read -p "Obsidius Discord Application Token: " discordToken

sudo cp ./services/obsidius.discord.service ./services/obsidius.discord.service.backup

sudo sed -i '/ObsidiusDiscordToken=/s/$/'"$discordToken"'/' ./services/obsidius.discord.service.backup

sudo mv ./services/obsidius.discord.service.backup /etc/systemd/system/obsidius.discord.service

sudo systemctl enable obsidius.discord.service

sudo systemctl start obsidius.discord.service
