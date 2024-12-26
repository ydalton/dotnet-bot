// Generated with CoreBot .NET Template version v4.22.0

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CoreBot.Helpers;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;

namespace CoreBot.Dialogs
{
    public class OrderDialog : CancelAndHelpDialog
    {
        private const string NameStepMsgText = "What is your name?";
        private const string EmailStepMsgText = "What is your email address?";
        private const string AddressStepMsgText = "What is your street address?";
        private const string CityStepMsgText = "What is your city?";
        private const string PostCodeStepMsgText = "What is your postcode?";
        private const string DeliveryStepMsgText = "Would you like a delivery or do you want it to pick it up yourself?";
        private const string ConfirmStepMsgText = "Is this correct?";

        public OrderDialog()
            : base(nameof(OrderDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));

            var waterfallSteps = new WaterfallStep[]
            {
                FirstNameStepAsync,
                NameEmailStepAsync,
                EmailAddressStepAsync,
                AddressCityStepAsync,
                CityZipCodeStepAsync,
                ZipCodeDeliveryStepAsync,
                DeliveryConfirmOrderStepAsync,
                FinalStepAsync,
            };

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> FirstNameStepAsync(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            OrderDetails orderDetails = (OrderDetails)stepContext.Options;

            if (orderDetails.Name == null)
            {
                var promptMessage = MessageFactory.Text(NameStepMsgText,
                    NameStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt),
                    new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.NextAsync(orderDetails.Name, cancellationToken);
        }

        private async Task<DialogTurnResult> NameEmailStepAsync(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            OrderDetails orderDetails = (OrderDetails)stepContext.Options;
            orderDetails.Name = (string)stepContext.Result;

            if (orderDetails.EmailAddress == null)
            {
                var promptMessage = MessageFactory.Text(EmailStepMsgText,
                    EmailStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt),
                    new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.NextAsync(orderDetails.EmailAddress, cancellationToken);
        }

        private async Task<DialogTurnResult> EmailAddressStepAsync(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            OrderDetails orderDetails = (OrderDetails)stepContext.Options;
            orderDetails.EmailAddress = (string)stepContext.Result;

            if (orderDetails.StreetAddress == null)
            {
                var promptMessage = MessageFactory.Text(AddressStepMsgText,
                    AddressStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt),
                    new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.NextAsync(orderDetails.StreetAddress, cancellationToken);
        }

        private async Task<DialogTurnResult> AddressCityStepAsync(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            OrderDetails orderDetails = (OrderDetails)stepContext.Options;
            orderDetails.StreetAddress = (string)stepContext.Result;

            if (orderDetails.City == null)
            {
                var promptMessage = MessageFactory.Text(CityStepMsgText,
                    CityStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt),
                    new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.NextAsync(orderDetails.City, cancellationToken);
        }

        private async Task<DialogTurnResult> CityZipCodeStepAsync(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            OrderDetails orderDetails = (OrderDetails)stepContext.Options;
            orderDetails.City = (string)stepContext.Result;

            if (orderDetails.PostCode == null)
            {
                var promptMessage = MessageFactory.Text(PostCodeStepMsgText,
                    PostCodeStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt),
                    new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.NextAsync(orderDetails.PostCode, cancellationToken);
        }

        private async Task<DialogTurnResult> ZipCodeDeliveryStepAsync(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            OrderDetails orderDetails = (OrderDetails)stepContext.Options;
            orderDetails.PostCode = (string)stepContext.Result;

            if (orderDetails.IsDelivery == false)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text(DeliveryStepMsgText), cancellationToken);
                List<string> deliveryList = new List<string> { "Delivery", "Pick-up" };
                return await ChoicePromptHelper.PromptChoiceAsync(deliveryList, stepContext, cancellationToken);
            }

            return await stepContext.NextAsync(orderDetails.IsDelivery, cancellationToken);
        }
        
        private async Task<DialogTurnResult> DeliveryConfirmOrderStepAsync(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            OrderDetails orderDetails = (OrderDetails)stepContext.Options;
            
            if (((FoundChoice)stepContext.Result).Value == "Delivery")
            {
                orderDetails.IsDelivery = true;
            }
            else if (((FoundChoice)stepContext.Result).Value == "Pick-up")
            {
                orderDetails.IsDelivery = false;
            }
            
            var deliveryMethod = orderDetails.IsDelivery ? "Delivery" : "Pick-up";
            var messageText =
                $"Okay, I have written down the details of the order:\n\n"
                + $"Name: {orderDetails.Name}\n"
                + $"Email: {orderDetails.EmailAddress}\n"
                + $"Address: {orderDetails.StreetAddress}\n"
                + $"City: {orderDetails.City}\n"
                + $"Post code: {orderDetails.PostCode}\n"
                + $"Delivery method: {deliveryMethod}\n\n";

            var promptMessage = MessageFactory.Text(messageText,
                messageText, InputHints.ExpectingInput);
            await stepContext.Context.SendActivityAsync(promptMessage, cancellationToken);

            await stepContext.Context.SendActivityAsync(MessageFactory.Text(ConfirmStepMsgText), cancellationToken);
            var yesnoList = new List<string> { "Yes", "No" };
            return await ChoicePromptHelper.PromptChoiceAsync(yesnoList, stepContext, cancellationToken);
        }
        
        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            string orderNumber = "02bsl52osnbfk";
            // TODO: Write info to database
            // if (((FoundChoice)stepContext.Result).Value == "Yes")
            // {
                    // var orderDetails = (OrderDetails)stepContext.Options;
                    // await stepContext.Context.SendActivityAsync(MessageFactory.Text($"I have placed the order. Your order number is {orderNumber}. Check your inbox for details.), cancellationToken"));
            //     return await stepContext.EndDialogAsync(orderDetails, cancellationToken);
            // }

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
    }
}