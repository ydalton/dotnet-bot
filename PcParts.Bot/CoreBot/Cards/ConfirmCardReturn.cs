using AdaptiveCards;
using System.Collections.Generic;
using System;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;

namespace CoreBot.Cards
{
    public class ConfirmCardReturn
    {
        public static Attachment CreateCardAttachment(string orderNumber, string reason, string refund)
        {
            var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0))
            {
                Body = new List<AdaptiveElement>
                {
                    new AdaptiveColumnSet
                    {
                        Columns = new List<AdaptiveColumn>
                        {
                            new AdaptiveColumn
                            {
                                Width = "stretch",
                                Items = new List<AdaptiveElement>
                                {
                                    new AdaptiveTextBlock
                                    {
                                        Text = "Pc Parts",
                                        Weight = AdaptiveTextWeight.Bolder,
                                        Size = AdaptiveTextSize.Large
                                    },
                                    new AdaptiveTextBlock
                                    {
                                        Text = "Return",
                                        Weight = AdaptiveTextWeight.Bolder,
                                        Size = AdaptiveTextSize.Medium,
                                        Spacing = AdaptiveSpacing.None
                                    }
                                }
                            },
                            new AdaptiveColumn
                            {
                                Width = "auto",
                                Items = new List<AdaptiveElement>
                                {
                                    new AdaptiveImage
                                    {
                                        Url = new Uri("https://nesimd.pythonanywhere.com/static/logo-pc-part.png"),
                                        Size = AdaptiveImageSize.Medium,
                                        Style = AdaptiveImageStyle.Default
                                    }
                                }
                            }
                        }
                    },
                    new AdaptiveTextBlock
                    {
                        Text = $"• {orderNumber}",
                        Weight = AdaptiveTextWeight.Default,
                        Size = AdaptiveTextSize.Medium,
                        Spacing = AdaptiveSpacing.Large
                    },
                    new AdaptiveTextBlock
                    {
                        Text = $"• {reason}",
                        Weight = AdaptiveTextWeight.Default,
                        Size = AdaptiveTextSize.Medium,
                        Spacing = AdaptiveSpacing.None
                    },
                    new AdaptiveTextBlock
                    {
                        Text = $"• {refund}",
                        Weight = AdaptiveTextWeight.Default,
                        Size = AdaptiveTextSize.Medium,
                        Spacing = AdaptiveSpacing.None
                    }
                }
            };

            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JObject.FromObject(card)
            };

            return adaptiveCardAttachment;
        }
    }
}
