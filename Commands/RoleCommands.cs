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

            //adds fields to the embed
            pronounEmbed.AddField(":one: :", "She / Her");
            pronounEmbed.AddField(":two: :", "He / Him");
            pronounEmbed.AddField(":three: :", "They / Them");
            pronounEmbed.AddField(":four: :", "He / She / They");
            pronounEmbed.AddField(":five: :", "They / He");
            pronounEmbed.AddField(":six: :", "They / She");
            pronounEmbed.AddField(":seven: :", "Call Me Whatever (No Preference)");
            pronounEmbed.AddField(":eight: :", "Call Me By My Name (No Pronouns)");

            DiscordMessage pronounMessage = await ctx.Channel.SendMessageAsync(embed: pronounEmbed).ConfigureAwait(false);

            DiscordEmoji one = DiscordEmoji.FromName(ctx.Client, ":one:");
            DiscordEmoji two = DiscordEmoji.FromName(ctx.Client, ":two:");
            DiscordEmoji three = DiscordEmoji.FromName(ctx.Client, ":three:");
            DiscordEmoji four = DiscordEmoji.FromName(ctx.Client, ":four:");
            DiscordEmoji five = DiscordEmoji.FromName(ctx.Client, ":five:");
            DiscordEmoji six = DiscordEmoji.FromName(ctx.Client, ":six:");
            DiscordEmoji seven = DiscordEmoji.FromName(ctx.Client, ":seven:");
            DiscordEmoji eight = DiscordEmoji.FromName(ctx.Client, ":eight:");

            DiscordRole sheHer = ctx.Guild.GetRole(724251338375954432);
            DiscordRole heHim = ctx.Guild.GetRole(724251720326053928);
            DiscordRole theyThem = ctx.Guild.GetRole(724251807274238022);
            DiscordRole heSheThey = ctx.Guild.GetRole(724251911858946049);
            DiscordRole theyHe = ctx.Guild.GetRole(724252024903827508);
            DiscordRole theyShe = ctx.Guild.GetRole(724252107066048615);
            DiscordRole callMeWhatever = ctx.Guild.GetRole(724252197990432862);
            DiscordRole noPronouns = ctx.Guild.GetRole(724252313681920061);

            await pronounMessage.CreateReactionAsync(one).ConfigureAwait(false);
            await pronounMessage.CreateReactionAsync(two).ConfigureAwait(false);
            await pronounMessage.CreateReactionAsync(three).ConfigureAwait(false);
            await pronounMessage.CreateReactionAsync(four).ConfigureAwait(false);
            await pronounMessage.CreateReactionAsync(five).ConfigureAwait(false);
            await pronounMessage.CreateReactionAsync(six).ConfigureAwait(false);
            await pronounMessage.CreateReactionAsync(seven).ConfigureAwait(false);
            await pronounMessage.CreateReactionAsync(eight).ConfigureAwait(false);

            DiscordMessage pingReminder = await ctx.Channel.SendMessageAsync("If your preferred pronouns are missing, please feel free to tag Serana to add them.").ConfigureAwait(false);

            InteractivityExtension interactivity = ctx.Client.GetInteractivity();

            var reactionResult = await interactivity.WaitForReactionAsync(
                x => x.Message == pronounMessage &&
                x.User == ctx.User &&
                (x.Emoji == one || x.Emoji == two || x.Emoji == three || x.Emoji == four || x.Emoji == five || x.Emoji == six || x.Emoji == seven || x.Emoji == eight)).ConfigureAwait(false);

            if (reactionResult.Result.Emoji == one)
            {
                await ctx.Member.GrantRoleAsync(sheHer).ConfigureAwait(false);
            }
            else if (reactionResult.Result.Emoji == two)
            {
                await ctx.Member.GrantRoleAsync(heHim).ConfigureAwait(false);
            }
            else if (reactionResult.Result.Emoji == three)
            {
                await ctx.Member.GrantRoleAsync(theyThem).ConfigureAwait(false);
            }
            else if (reactionResult.Result.Emoji == four)
            {
                await ctx.Member.GrantRoleAsync(heSheThey).ConfigureAwait(false); 
            }
            else
            {
                //Something went wrong
            }

            await pronounMessage.DeleteAsync().ConfigureAwait(false);
            await pingReminder.DeleteAsync().ConfigureAwait(false);
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
