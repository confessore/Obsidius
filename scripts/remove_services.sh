#!/bin/sh

sudo systemctl stop obsidius.discord.service

sudo systemctl disable obsidius.discord.service

sudo rm /etc/systemd/system/obsidius.discord.service
