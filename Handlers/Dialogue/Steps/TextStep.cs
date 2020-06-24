﻿using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Serana Wilger
/// 06/23/2020
/// TextStep.cs
/// 
/// Dialogue steps that take a text result
/// </summary>

namespace BirdyBot.Handlers.Dialogue.Steps
{
    public class TextStep : BaseDialogueStep
    {
        private readonly IDialogueStep _nextStep;
        private readonly int? _minLength;
        private readonly int? _maxLength;

        public TextStep(
            string content,
            IDialogueStep nextStep,
            int? minLength = null,
            int? maxlength = null) : base(content)
        {
            _nextStep = nextStep;
            _minLength = minLength;
            _maxLength = maxlength;
        }

        public Action<string> OnValidResult { get; set; } = delegate { };

        public override IDialogueStep NextStep => _nextStep;

        public override async Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user)
        {
            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder
            {
                Title = $"Please Respond Below",
                Description = $"{user.Mention}, {_content}"
            };

            embedBuilder.AddField("To Stop The Dialogue", "Use the ?cancel command");

            if(_minLength.HasValue)
            {
                embedBuilder.AddField("Min Length", $"{_minLength.Value} characters");
            }
            if(_maxLength.HasValue)
            {
                embedBuilder.AddField("Max Length", $"{_maxLength.Value} characters");
            }

            InteractivityExtension interactivity = client.GetInteractivity();

            while(true)
            {
                DiscordMessage embed = await channel.SendMessageAsync(embed: embedBuilder).ConfigureAwait(false);

                OnMessageAdded(embed);

                var messageResult = await interactivity.WaitForMessageAsync(x => x.ChannelId == channel.Id && x.Author.Id == user.Id).ConfigureAwait(false);

                OnMessageAdded(messageResult.Result);

                if(messageResult.Result.Content.Equals("?cancel", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                if(_minLength.HasValue)
                {
                    if(messageResult.Result.Content.Length < _minLength.Value)
                    {
                        await TryAgain(channel, $"Your input is {_minLength.Value - messageResult.Result.Content.Length} characters too short").ConfigureAwait(false);
                        continue;
                    }
                }

                if(_maxLength.HasValue)
                {
                    if(messageResult.Result.Content.Length > _maxLength.Value)
                    {
                        await TryAgain(channel, $"Your input is {messageResult.Result.Content.Length - _maxLength.Value} characters too long").ConfigureAwait(false);
                        continue;
                    }
                }

                OnValidResult(messageResult.Result.Content);

                return false;
            }
        }
    }
}