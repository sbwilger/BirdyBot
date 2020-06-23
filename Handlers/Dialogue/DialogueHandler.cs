using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

/// <summary>
/// Serana Wilger
/// 06/23/2020
/// DialogueHandler.cs
/// 
/// The class for handling dialogue trees
/// </summary>

namespace BirdyBot.Handlers.Dialogue
{
    public class DialogueHandler
    {
        private readonly DiscordClient _client;
        private readonly DiscordChannel _channel;
        private readonly DiscordUser _user;
        private IDialogueStep _currentStep;

        //constructor
        public DialogueHandler(DiscordClient client, DiscordChannel channel, DiscordUser user, IDialogueStep startingStep)
        {
            _client = client;
            _channel = channel;
            _user = user;
            _currentStep = startingStep;
        }

        //list of messages
        private readonly List<DiscordMessage> messages = new List<DiscordMessage>();

        public async Task<bool> ProcessDialogue()
        {
            while(_currentStep != null)
            {
                _currentStep.OnMessageAdded += (message) => messages.Add(message);

                bool cancelled = await _currentStep.ProcessStep(_client, _channel, _user).ConfigureAwait(false);

                //cancels the dialogue
                if(cancelled)
                {
                    await DeleteMessages().ConfigureAwait(false);

                    DiscordEmbedBuilder cancelEmbed = new DiscordEmbedBuilder()
                    {
                        Title = "The Dialogue Has Successfully Been Cancelled",
                        Description = _user.Mention,
                        Color = DiscordColor.Cyan
                    };

                    await _channel.SendMessageAsync(embed: cancelEmbed).ConfigureAwait(false);

                    return false;
                }

                _currentStep = _currentStep.NextStep;
            }

            await DeleteMessages().ConfigureAwait(false);

            return true;
        }

        // deletes all messages in the dialogue
        private async Task DeleteMessages()
        {
            if(_channel.IsPrivate)
            {
                return;
            }

            foreach(DiscordMessage message in messages)
            {
                await message.DeleteAsync().ConfigureAwait(false);
            }
        }
    }
}
