﻿using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
using sbwilger.BirdyBot.Handlers.Dialogue;
using sbwilger.BirdyBot.Handlers.Dialogue.Steps;
using sbwilger.Core.Services.Items;
using sbwilger.DAL;
using sbwilger.DAL.Models.Items;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace sbwilger.BirdyBot.Commands
{
    class RPGCommands : BaseCommandModule
    {
        private readonly IItemService _itemService;

        public RPGCommands(IItemService itemService)
        {
            _itemService = itemService;
        }

        [Command("iteminfo")]
        public async Task ItemInfo(CommandContext ctx)
        {
            TextStep itemNameStep = new TextStep("Enter the name of the item you want info on.", null);

            string itemName = string.Empty;

            itemNameStep.OnValidResult += (result) => itemName = result;

            DialogueHandler inputDialogueHandler = new DialogueHandler(ctx.Client, ctx.Channel, ctx.User, itemNameStep);

            bool succeeded = await inputDialogueHandler.ProcessDialogue().ConfigureAwait(false);

            if(!succeeded)
            {
                return;
            }

            Item item = await _itemService.GetItemByName(itemName).ConfigureAwait(false);

            if(item == null)
            {
                await ctx.Channel.SendMessageAsync($"There is no item called {itemName}.");
                return;
            }

            await ctx.Channel.SendMessageAsync($"Name: {item.Name}, Description: {item.Description}");
        }
    }
}