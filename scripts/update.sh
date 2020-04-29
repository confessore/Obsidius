#!/bin/sh

sudo service obsidius.discord stop

cd /home/$USER/obsidius
sudo git pull origin master

cd /home/$USER/obsidius/src/Obsidius.Discord
sudo dotnet publish -c Release -o /var/dotnetcore/Obsidius.Discord

sudo service obsidius.discord start
