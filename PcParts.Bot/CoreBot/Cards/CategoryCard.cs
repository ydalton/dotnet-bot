using System.Collections.Generic;
using System.Linq;
using AdaptiveCards;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using PcParts.API.Dto;

namespace CoreBot.Cards;

public class CategoryCard
{
    public static Attachment CreateCardAttachment(List<string> categories)
    {
        var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0))
        {
            Body = new List<AdaptiveElement>
            {
                // Title TextBlock
                new AdaptiveTextBlock
                {
                    Text = "Categories",
                    Weight = AdaptiveTextWeight.Bolder,
                    Size = AdaptiveTextSize.Large
                },
                // Subtitle TextBlock
                new AdaptiveTextBlock
                {
                    Text = "Choose a category:",
                    Wrap = true
                },
                // ChoiceSet for reasons
                new AdaptiveChoiceSetInput
                {
                    Id = "categoryChoice",
                    Value = categories[0],
                    Style = AdaptiveChoiceInputStyle.Compact,
                    Choices = categories.Select(category => new AdaptiveChoice
                    {
                        Title = category,
                        Value = category,
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