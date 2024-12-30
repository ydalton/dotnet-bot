using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CoreBot.Cards;
using CoreBot.DialogDetails;
using CoreBot.Dto;
using CoreBot.Helpers;
using CoreBot.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;

namespace CoreBot.Dialogs;

public class CatalogueDialog : CancelAndHelpDialog
{
    private const string CategoryStepMsgText = "Would you like to view our products of a specific category? Choose \"None\" to show all products.";
    
    private const string CategoryDialogID = "categoryDialog";
    
    public CatalogueDialog()
        : base(nameof(CatalogueDialog))
    {
        AddDialog(new SelectPromptHelper(CategoryDialogID));
        
        var waterfallSteps = new WaterfallStep[]
        {
            CategoryStepAsync,
            FinalStepAsync,
        };
        
        AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
        InitialDialogId = nameof(WaterfallDialog);
    }

    private async Task<DialogTurnResult> CategoryStepAsync(
        WaterfallStepContext stepContext,
        CancellationToken cancellationToken)
    {
        List<string> categories = new List<string>()
        {
            "None",
        };
        categories.AddRange(await ProductService.GetCategories());
        await stepContext.Context.SendActivityAsync(MessageFactory.Text(CategoryStepMsgText), cancellationToken);
        var options = new PromptOptions()
        {
            Prompt = new Activity()
            {
                Type = ActivityTypes.Message,
                Attachments = new List<Attachment>()
                {
                    CategoryCard.CreateCardAttachment(categories)
                }
            }
        };
        
        return await stepContext.PromptAsync(CategoryDialogID, options, cancellationToken);
    }
    
    private async Task<DialogTurnResult> FinalStepAsync(
        WaterfallStepContext stepContext,
        CancellationToken cancellationToken)
    {
        string category;
        string message = "Here is our catalogue";
        
        try
        {
            var result = stepContext.Result.ToString();
            dynamic data = JObject.Parse(result);
            category = data.categoryChoice.ToString();
        }
        catch (Exception)
        {
            category = "None";
        }
        
        List<ProductResponse> products;

        if (category == "None")
        {
            products = await ProductService.GetProducts();
        }
        else
        {
            products = await ProductService.GetProductsByCategory(category);
            message += $" for the {category} category";
        }

        message += ":";
        
        await stepContext.Context.SendActivityAsync(
            MessageFactory.Text(message), cancellationToken);
        
        var catalogueCard = CatalogueCard.CreateCardAttachment(products);
        
        await stepContext.Context.SendActivityAsync(MessageFactory.Attachment(catalogueCard),
                                                    cancellationToken);
        
        return await stepContext.EndDialogAsync(null, cancellationToken);
    }
}