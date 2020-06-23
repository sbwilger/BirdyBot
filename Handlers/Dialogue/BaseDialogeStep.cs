using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Serana Wilger
/// 06/23/2020
/// BaseDialogueStep.cs
/// 
/// Abstract class for handling dialogue steps.
/// </summary>

namespace BirdyBot.Handlers.Dialogue
{
    public abstract class BaseDialogeStep : IDialogueStep
    {
        protected readonly string _content;

        // constructor
        public BaseDialogeStep(string content)
        {
            _content = content;
        }

        public Action<DiscordMessage> OnMessageAdded { get; set; } = delegate{};

        public abstract IDialogueStep NextStep { get; }

        public abstract Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user);

        //sends a "Try Again" message when something goes wrong
        protected async Task TryAgain(DiscordChannel channel, string problem)
        {
            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder
            {
                Title = "Please Try Again",
                Color = DiscordColor.Red
            };

            embedBuilder.AddField("There was a problem with your previous input", problem);

            DiscordMessage embed = await channel.SendMessageAsync(embed: embedBuilder).ConfigureAwait(false);

            OnMessageAdded(embed);
        }
    }
}
