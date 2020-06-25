using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Serana Wilger
/// 06/25/2020
/// ReactionStep.cs
/// 
/// A dialogue step that uses reaction emojis
/// </summary>

namespace BirdyBot.Handlers.Dialogue.Steps
{
    public class ReactionStep : BaseDialogueStep
    {
        private readonly Dictionary<DiscordEmoji, ReactionStepData> _options;

        private DiscordEmoji _selectedEmoji;

        public ReactionStep(string content, Dictionary<DiscordEmoji, ReactionStepData> options) : base(content)
        {
            _options = options;
        }

        public override IDialogueStep NextStep => _options[_selectedEmoji].NextStep;

        public Action<DiscordEmoji> OnValidResult { get; set; } = delegate { };

        public override async Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user)
        {
            DiscordEmoji cancelEmoji = DiscordEmoji.FromName(client, ":x:");

            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder
            {
                Title = $"Please React To This Embed",
                Description = $"{user.Mention}, {_content}"
            };

            embedBuilder.AddField("To Stop The Dialogue", "React with the :x: emoji");

            InteractivityExtension interactivity = client.GetInteractivity();

            while (true)
            {
                DiscordMessage embed = await channel.SendMessageAsync(embed: embedBuilder).ConfigureAwait(false);

                OnMessageAdded(embed);

                foreach(DiscordEmoji emoji in _options.Keys)
                {
                    await embed.CreateReactionAsync(emoji).ConfigureAwait(false);
                }

                await embed.CreateReactionAsync(cancelEmoji).ConfigureAwait(false);

                var reactionResult = await interactivity.WaitForReactionAsync(x => _options.ContainsKey(x.Emoji) || x.Emoji == cancelEmoji, embed, user).ConfigureAwait(false);

                if(reactionResult.Result.Emoji == cancelEmoji)
                {
                    return true;
                }

                _selectedEmoji = reactionResult.Result.Emoji;

                OnValidResult(_selectedEmoji);

                return false;
            }
        }
    }

    public class ReactionStepData
    {
        public string Content { get; set; }
        public IDialogueStep NextStep { get; set; }
    }
}
