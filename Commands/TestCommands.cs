using BirdyBot.Handlers.Dialogue;
using BirdyBot.Handlers.Dialogue.Steps;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Serana Wilger
/// 06/24/2020
/// TestCommands.cs
/// 
/// Testing new things on the server
/// </summary>

namespace BirdyBot.Commands
{
    class TestCommands : BaseCommandModule
    {
        [Command("dialogue")]
        public async Task Dialogue(CommandContext ctx)
        {
            TextStep inputStep = new TextStep("Enter something interesting", null);
            TextStep funnyStep = new TextStep("Haha, funny", null);

            string input = string.Empty;

            inputStep.OnValidResult += (result) =>
            {
                input = result;

                if(result == "something interesting")
                {
                    inputStep.SetNextStep(funnyStep);
                }
            };

            DiscordDmChannel userChannel = await ctx.Member.CreateDmChannelAsync().ConfigureAwait(false);

            DialogueHandler inputDialogueHandler = new DialogueHandler(ctx.Client, userChannel, ctx.User, inputStep);

            bool succeeded = await inputDialogueHandler.ProcessDialogue().ConfigureAwait(false);

            if (!succeeded) { return; }

            await ctx.Channel.SendMessageAsync(input).ConfigureAwait(false);
        }

        [Command("emojiDialogue")]
        public async Task EmojiDialogue(CommandContext ctx)
        {
            ReactionStep emojiStep = new ReactionStep("Yes or No?", new Dictionary<DiscordEmoji, ReactionStepData>
            {
                { DiscordEmoji.FromName(ctx.Client, ":+1:"), new ReactionStepData{Content = "This means yes", NextStep = null} },
                { DiscordEmoji.FromName(ctx.Client, ":-1:"), new ReactionStepData{Content = "This means no", NextStep = null} }
            });

            DiscordDmChannel userChannel = await ctx.Member.CreateDmChannelAsync().ConfigureAwait(false);

            DialogueHandler inputDialogueHandler = new DialogueHandler(ctx.Client, userChannel, ctx.User, emojiStep);

            bool succeeded = await inputDialogueHandler.ProcessDialogue().ConfigureAwait(false);

            if(!succeeded)
            {
                return;
            }
        }
    }
}
