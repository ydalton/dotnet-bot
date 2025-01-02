using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CoreBot.Cards;
using CoreBot.Dto;
using CoreBot.Helpers;
using CoreBot.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using PcParts.API.Dto;

namespace CoreBot.Dialogs;

public class ReturnOrderDialog : CancelAndHelpDialog
{
    private readonly string ReasonDialogID = "ReasonDialogID";
    private const string OrderNumberStepMsgText = "What is your order number?";
    private const string ReasonStepMsgText =
        "We would like to understand the reason for your return. Could you please provide us with some details about why you are returning the item?";
    private const string RefundOptionStepMsgText = "Would you prefer to receive your refund in cash or as a gift card?";
    private const string ConfirmStepMsgText = "Are you sure you want to return this order?";
   
    private readonly string OrderNumberDialogID = "OrderNumberDialogID";
    
    public ReturnOrderDialog()
        : base(nameof(ReturnOrderDialog))
    {
        AddDialog(new TextPrompt(nameof(TextPrompt)));
        AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
        AddDialog(new SelectPromptHelper(ReasonDialogID));
        AddDialog(new TextPrompt(OrderNumberDialogID, OrderNumberValidation));

        var waterfallSteps = new WaterfallStep[]
        {
            FirstOrderNumberStepAsync,
            OrderNumberReasonStepAsync,
            ReasonRefundOptionStepAsync,
            RefundOptionConfirmStepAsync,
            ConfirmActStepAsync,
        };

        AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
        InitialDialogId = nameof(WaterfallDialog);
    }

    private async Task<DialogTurnResult> FirstOrderNumberStepAsync(WaterfallStepContext stepContext,
        CancellationToken cancellationToken)
    {
        var returnOrderDetails = (ReturnOrderDetails)stepContext.Options;

        if (returnOrderDetails.OrderNumber == null)
        {
            var promptMessage =
                MessageFactory.Text(OrderNumberStepMsgText, OrderNumberStepMsgText, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(OrderNumberDialogID, new PromptOptions { Prompt = promptMessage },
                cancellationToken);
        }

        return await stepContext.NextAsync(returnOrderDetails.OrderNumber, cancellationToken);
    }

    private async Task<DialogTurnResult> OrderNumberReasonStepAsync(WaterfallStepContext stepContext,
        CancellationToken cancellationToken)
    {
        var returnOrderDetails = (ReturnOrderDetails)stepContext.Options;
        returnOrderDetails.OrderNumber = (string)stepContext.Result;

        if (returnOrderDetails.Reason == null)
        {
            var promptMessage = MessageFactory.Text(ReasonStepMsgText,
                ReasonStepMsgText, InputHints.ExpectingInput);
            await stepContext.Context.SendActivityAsync(promptMessage, cancellationToken);

            var reasons = await ReasonService.GetReasonsAsync();
            var attachment = ReasonCard.CreateCardAttachment(reasons);

            var options = new PromptOptions
            {
                Prompt = new Activity
                {
                    Attachments = new List<Attachment>() { attachment },
                    Type = ActivityTypes.Message
                }
            };

            return await stepContext.PromptAsync(ReasonDialogID, options, cancellationToken);
        }

        return await stepContext.NextAsync(returnOrderDetails.Reason, cancellationToken);
    }

    private async Task<DialogTurnResult> ReasonRefundOptionStepAsync(WaterfallStepContext stepContext,
        CancellationToken cancellationToken)
    {
        var returnOrderDetails = (ReturnOrderDetails)stepContext.Options;

        try
        {
            var result = stepContext.Result.ToString();
            dynamic data = JObject.Parse(result);
            returnOrderDetails.Reason = data.reasonChoice.ToString();
        }
        catch (Exception)
        {
            returnOrderDetails.Reason = (string)stepContext.Result;
        }

        if (returnOrderDetails.RefundOption == null)
        {
            await stepContext.Context.SendActivityAsync(MessageFactory.Text(RefundOptionStepMsgText),
                cancellationToken);
            List<string> refundOptionList = new List<string> { "cash", "gift card" };
            return await ChoicePromptHelper.PromptChoiceAsync(refundOptionList, stepContext, cancellationToken);
        }

        return await stepContext.NextAsync(returnOrderDetails.RefundOption, cancellationToken);
    }

    private async Task<DialogTurnResult> RefundOptionConfirmStepAsync(WaterfallStepContext stepContext,
        CancellationToken cancellationToken)
    {
        var returnOrderDetails = (ReturnOrderDetails)stepContext.Options;
        try
        {
            returnOrderDetails.RefundOption = ((FoundChoice)stepContext.Result).Value;
        }
        catch (Exception)
        {
            returnOrderDetails.RefundOption = (string)stepContext.Result;
        }

        ReasonResponse reason;
        try
        {
            reason = await ReasonService.GetReasonByCodeAsync(returnOrderDetails.Reason);
        }
        catch (Exception)
        {
            reason = await ReasonService.GetReasonByCodeAsync("other");
        }

        var attachment = ConfirmCardReturn.CreateCardAttachment(
            $"Order Number: {returnOrderDetails.OrderNumber}",
            $"Reason: {reason.Name}",
            $"Refund Option: {returnOrderDetails.RefundOption}");
        
        var activity = MessageFactory.Attachment(attachment);
        await stepContext.Context.SendActivityAsync(activity, cancellationToken);

        await stepContext.Context.SendActivityAsync(MessageFactory.Text(ConfirmStepMsgText), cancellationToken);
        var yesnoList = new List<string> { "Yes", "No" };
        return await ChoicePromptHelper.PromptChoiceAsync(yesnoList, stepContext, cancellationToken);
    }
    
    private async Task<DialogTurnResult> ConfirmActStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    {
        if (((FoundChoice)stepContext.Result).Value == "Yes")
        {
            var returnOrderDetails = (ReturnOrderDetails)stepContext.Options;
            
            var reason = await ReasonService.GetReasonByCodeAsync(returnOrderDetails.Reason);
            await ReturnOrderService.InsertReturnOrderAsync(new ReturnOrderRequest()
            {
                OrderId = returnOrderDetails.OrderNumber,
                RefundOption = returnOrderDetails.RefundOption,
                Reason = returnOrderDetails.Reason,
            });
            await stepContext.Context.SendActivityAsync(MessageFactory.Text("Your return is created!"), cancellationToken);
            await stepContext.Context.SendActivityAsync(MessageFactory.Text("Place all items back in the original box. Seal the box. Then, drop off the package at a post office, Post point, or Parcel Point."), cancellationToken);

            return await stepContext.EndDialogAsync(returnOrderDetails, cancellationToken);
        }

        return await stepContext.EndDialogAsync(null, cancellationToken);
    }
    
    private async Task<bool> OrderNumberValidation(PromptValidatorContext<string> promptcontext, CancellationToken cancellationtoken)
    {
        const string OrderNumberError = "The order number you entered is not valid or there has been a request for a return already for this order number. Please enter a valid order number.";
        Guid orderNumberGuid;

        string orderNumber = promptcontext.Recognized.Value;
        try
        {
            orderNumberGuid = Guid.Parse(orderNumber);
        }
        catch
        {
            goto fail;
        }
        
        var returnOrder = await ReturnOrderService.CheckOrderNumberExistsInReturnOrder(orderNumberGuid);
        
        if (!returnOrder)
        {
            return true;
        }
        
        fail:
        await promptcontext.Context.SendActivityAsync(OrderNumberError,
            cancellationToken: cancellationtoken);
        return false;
    }
}