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
        var catalogueDetails = (CatalogueDetails) stepContext.Options ;

        if (catalogueDetails.Category == null)
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
        
        return await stepContext.NextAsync(catalogueDetails.Category, cancellationToken);
    }
    
    private async Task<DialogTurnResult> FinalStepAsync(
        WaterfallStepContext stepContext,
        CancellationToken cancellationToken)
    {
        string message = "Here is our catalogue";
        
        var catalogueDetails = (CatalogueDetails) stepContext.Options;
        
        try
        {
            var result = stepContext.Result.ToString();
            dynamic data = JObject.Parse(result);
            catalogueDetails.Category = data.categoryChoice.ToString();
        }
        catch (Exception)
        {
            catalogueDetails.Category = "None";
        }
        
        List<ProductResponse> products;

        if (catalogueDetails.Category == "None")
        {
            products = await ProductService.GetProducts();
        }
        else
        {
            products = await ProductService.GetProductsByCategory(catalogueDetails.Category);
            message += $" for the {catalogueDetails.Category} category";
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