using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CoreBot.Dto;
using Microsoft.Bot.Schema;
using AdaptiveCards;

namespace CoreBot.Cards;

public class CatalogueCard
{
    public static Attachment CreateCardAttachment(
        List<ProductResponse> products)
    {
        var productNameColumn = new List<AdaptiveElement>
        {
            new AdaptiveTextBlock
            {
                Text = "Name",
                Weight = AdaptiveTextWeight.Bolder
            },
        };
        productNameColumn.AddRange(products.Select(p =>
        {
            return new AdaptiveTextBlock { Text = p.Name };
        }).ToList());
        
        var productPriceColumn = new List<AdaptiveElement>
        {
            new AdaptiveTextBlock
            {
                Text = "Price",
                Weight = AdaptiveTextWeight.Bolder
            },
        };
        productPriceColumn.AddRange(products.Select(p =>
        {
            return new AdaptiveTextBlock
            {
                Text = p.Price.ToString("#.##")
            };
        }).ToList());
       
        var card = new AdaptiveCard(new AdaptiveSchemaVersion(1,3))
        {
            Body = new List<AdaptiveElement>
            {
                new AdaptiveTextBlock
                {
                    Text = "Catalogue",
                    Weight = AdaptiveTextWeight.Bolder,
                    Size = AdaptiveTextSize.Medium
                },
                new AdaptiveColumnSet
                {
                    Columns = new List<AdaptiveColumn>
                    {
                        new()
                        {
                            Items = productNameColumn
                        },
                        new()
                        {
                            Items = productPriceColumn
                        }
                    }
                }
            }
        };

        var attachment = new Attachment
        {
            ContentType = "application/vnd.microsoft.card.adaptive",
            Content = card
        };

        return attachment;
    }
}