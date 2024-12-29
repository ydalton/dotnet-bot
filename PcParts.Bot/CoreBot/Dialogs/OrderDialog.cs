// Generated with CoreBot .NET Template version v4.22.0

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoreBot.DialogDetails;
using CoreBot.Dto;
using CoreBot.Helpers;
using CoreBot.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;

namespace CoreBot.Dialogs
{
    public class OrderDialog : CancelAndHelpDialog
    {
        private const string ProductStepMsgText =
            "What would you like to order?";
        private const string NameStepMsgText = "What is your name?";
        private const string EmailStepMsgText = "What is your email address?";
        private const string AddressStepMsgText = "What is your street address?";
        private const string CityStepMsgText = "What is your city?";
        private const string PostCodeStepMsgText = "What is your zipcode?";
        private const string DeliveryStepMsgText = "Would you like a delivery or do you want it to pick it up yourself?";
        private const string ConfirmStepMsgText = "Is this correct?";

        private List<ProductResponse> products;

        public OrderDialog()
            : base(nameof(OrderDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));

            var waterfallSteps = new WaterfallStep[]
            {
                ProductStepAsync,
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

        private async Task<DialogTurnResult> ProductStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            OrderDetails orderDetails = (OrderDetails)stepContext.Options;
            //products = await ProductService.GetProducts();

            if (orderDetails.ProductName == null)
            {
                var promptMessage = MessageFactory.Text(ProductStepMsgText,
                    ProductStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt),
                    new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }
            
            return await stepContext.NextAsync(orderDetails.Name, cancellationToken);
        }

        private async Task<DialogTurnResult> FirstNameStepAsync(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            OrderDetails orderDetails = (OrderDetails)stepContext.Options;
            
            orderDetails.ProductName = stepContext.Result.ToString();

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

            if (orderDetails.ZipCode == null)
            {
                var promptMessage = MessageFactory.Text(PostCodeStepMsgText,
                    PostCodeStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt),
                    new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.NextAsync(orderDetails.ZipCode, cancellationToken);
        }

        private async Task<DialogTurnResult> ZipCodeDeliveryStepAsync(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            OrderDetails orderDetails = (OrderDetails)stepContext.Options;
            orderDetails.ZipCode = (string)stepContext.Result;

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
                + $"Post code: {orderDetails.ZipCode}\n"
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
            // TODO: Write info to database
            if (((FoundChoice)stepContext.Result).Value == "Yes")
            {
                var orderDetails = (OrderDetails)stepContext.Options;
                OrderResponse orderResponse = await OrderService.PostOrder(orderDetails);

                ReceiptCard receiptCard = new()
                {
                    Title = "Your Order Summary",
                    Facts = new List<Fact>
                    {
                        new Fact("Order number", orderResponse.Id.ToString()),
                        new Fact("Name", orderResponse.Name),
                        new Fact("Email Address", orderResponse.Email),
                        new Fact("Street address", orderResponse.Street),
                        new Fact("City", orderResponse.City),
                        new Fact("Zip code", orderDetails.ZipCode)
                    },
                    Items = orderResponse.Products.Select(p =>
                    {
                        return new ReceiptItem
                        {
                            Title = p.Name,
                            Price = p.Price.ToString(CultureInfo.InvariantCulture),
                            Quantity = "1",
                        };
                    }).ToList(),
                    Total = orderResponse.Products.Select(p => p.Price)
                                                  .Sum()
                                                  .ToString(CultureInfo.InvariantCulture),
                };
                
                await stepContext.Context.SendActivityAsync(MessageFactory.Attachment(receiptCard.ToAttachment()), cancellationToken);
                return await stepContext.EndDialogAsync(orderDetails, cancellationToken);
            }

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
    }
}