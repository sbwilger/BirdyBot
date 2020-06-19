using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Serana Wilger
/// 06/18/2020
/// RoleCommands.cs
/// 
/// This class defines the commands which allow the users to manage their roles.
/// </summary>

namespace BirdyBot.Commands
{
    class RoleCommands : BaseCommandModule
    {
        //work in progress
        [Command("pronouns")]
        [Description("Allows users to set their pronouns in the server")]
        public async Task SetPronouns(CommandContext ctx)
        {
            DiscordEmbedBuilder pronounEmbed = new DiscordEmbedBuilder
            {
                Title = "Set your pronouns",
                Color = DiscordColor.Cyan
            };

            DiscordMessage pronounMessage = await ctx.Channel.SendMessageAsync(embed: pronounEmbed).ConfigureAwait(false);
        }

        [Command("supportStreamers")]
        [Description("Toggles the stream supporter role, which notifies the user when a supported streamer goes live")]
        public async Task SupportStreamers(CommandContext ctx)
        {
            DiscordEmbedBuilder supportEmbed = new DiscordEmbedBuilder
            {
                Title = "Support streamers?",
                Color = DiscordColor.Cyan
            };

            DiscordMessage supportMessage = await ctx.Channel.SendMessageAsync(embed: supportEmbed).ConfigureAwait(false);

            DiscordEmoji thumbsUpEmoji = DiscordEmoji.FromName(ctx.Client, ":+1:");
            DiscordEmoji thumbsDownEmoji = DiscordEmoji.FromName(ctx.Client, ":-1:");

            await supportMessage.CreateReactionAsync(thumbsUpEmoji).ConfigureAwait(false);
            await supportMessage.CreateReactionAsync(thumbsDownEmoji).ConfigureAwait(false);

            InteractivityExtension interactivity = ctx.Client.GetInteractivity();

            var reactionResult = await interactivity.WaitForReactionAsync(
                x => x.Message == supportMessage &&
                x.User == ctx.User &&
                (x.Emoji == thumbsUpEmoji || x.Emoji == thumbsDownEmoji)).ConfigureAwait(false);

            DiscordRole role = ctx.Guild.GetRole(676505924520771595);

            if (reactionResult.Result.Emoji == thumbsUpEmoji)
            {
                await ctx.Member.GrantRoleAsync(role).ConfigureAwait(false);
            }
            else if(reactionResult.Result.Emoji == thumbsDownEmoji)
            {
                await ctx.Member.RevokeRoleAsync(role).ConfigureAwait(false);
            }
            else
            {
                //Something went wrong
            }

            await supportMessage.DeleteAsync().ConfigureAwait(false);
        }
    }
}
