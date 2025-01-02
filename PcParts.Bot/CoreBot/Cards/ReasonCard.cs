using System.Collections.Generic;
using System.Linq;
using AdaptiveCards;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using PcParts.API.Dto;

namespace CoreBot.Cards
{
    public class ReasonCard
    {
        public static Attachment CreateCardAttachment(List<ReasonResponse> reasons)
        {
            var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0))
            {
                Body = new List<AdaptiveElement>
                {
                    // Title TextBlock
                    new AdaptiveTextBlock
                    {
                        Text = "Reasons",
                        Weight = AdaptiveTextWeight.Bolder,
                        Size = AdaptiveTextSize.Large
                    },
                    // Subtitle TextBlock
                    new AdaptiveTextBlock
                    {
                        Text = "Choose a reason:",
                        Wrap = true
                    },
                    // ChoiceSet for reasons
                    new AdaptiveChoiceSetInput
                    {
                        Id = "reasonChoice",
                        Value = reasons[0].Code,
                        Style = AdaptiveChoiceInputStyle.Compact,
                        Choices = reasons.Select(reason => new AdaptiveChoice
                        {
                            Title = reason.Name,
                            Value = reason.Code,
                        }).ToList()
                    }
                },
                Actions = new List<AdaptiveAction>
                {
                    // Submit Action
                    new AdaptiveSubmitAction
                    {
                        Title = "Submit choice",
                        Data = new { action = "submitChoice" }
                    }
                }
            };

            // Create an attachment
            var adaptiveCardAttachment = new Attachment
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JObject.FromObject(card)
            };

            return adaptiveCardAttachment;
        }
    }
}
