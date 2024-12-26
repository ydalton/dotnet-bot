// Generated with CoreBot .NET Template version v4.22.0

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
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

        public OrderDialog()
            : base(nameof(OrderDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            AddDialog(new DateResolverDialog());

            var waterfallSteps = new WaterfallStep[]
            {
                NameStepAsync,
                EmailStepAsync,
                AddressStepAsync,
                CityStepAsync,
                PostcodeStepAsync,
                DeliveryStepAsync,
                ConfirmStepAsync,
                FinalStepAsync,
            };

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> NameStepAsync(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            OrderDetails orderDetails = (OrderDetails) stepContext.Options;

            if (orderDetails.Name == null)
            {
                var promptMessage = MessageFactory.Text(NameStepMsgText, 
                    NameStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), 
                    new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }
            
            return await stepContext.NextAsync(orderDetails.Name, cancellationToken);
        }
        
        private async Task<DialogTurnResult> EmailStepAsync(WaterfallStepContext stepContext, 
            CancellationToken cancellationToken)
        {
            OrderDetails orderDetails = (OrderDetails) stepContext.Options;
            
            orderDetails.Name = stepContext.Result.ToString();

            if (orderDetails.EmailAddress == null)
            {
                var promptMessage = MessageFactory.Text(EmailStepMsgText, 
                    EmailStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), 
                    new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }
            
            return await stepContext.NextAsync(orderDetails.EmailAddress, cancellationToken);
        }

        private async Task<DialogTurnResult> AddressStepAsync(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            OrderDetails orderDetails = (OrderDetails) stepContext.Options;
            
            orderDetails.EmailAddress = stepContext.Result.ToString();

            if (orderDetails.StreetAddress == null)
            {
                var promptMessage = MessageFactory.Text(AddressStepMsgText, 
                    AddressStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), 
                    new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }
            
            return await stepContext.NextAsync(orderDetails.StreetAddress, cancellationToken);
        }

        private async Task<DialogTurnResult> CityStepAsync(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            OrderDetails orderDetails = (OrderDetails) stepContext.Options;
            
            orderDetails.StreetAddress = stepContext.Result.ToString();
            
            if (orderDetails.City == null)
            {
                var promptMessage = MessageFactory.Text(CityStepMsgText, 
                    CityStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), 
                    new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }
            
            return await stepContext.NextAsync(orderDetails.City, cancellationToken);
        }

        private async Task<DialogTurnResult> PostcodeStepAsync(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            OrderDetails orderDetails = (OrderDetails) stepContext.Options;
            
            orderDetails.City = stepContext.Result.ToString();

            if (orderDetails.PostCode == null)
            {
                var promptMessage = MessageFactory.Text(PostCodeStepMsgText, 
                    PostCodeStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), 
                    new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }
            
            return await stepContext.NextAsync(orderDetails.PostCode, cancellationToken);
        }
        
        private async Task<DialogTurnResult> DeliveryStepAsync(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            OrderDetails orderDetails = (OrderDetails) stepContext.Options;
            
            orderDetails.City = stepContext.Result.ToString();
            
            if (orderDetails.IsDelivery == null)
            {
                var promptMessage = MessageFactory.Text(DeliveryStepMsgText, 
                    DeliveryStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), 
                    new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }
            
            return await stepContext.NextAsync(orderDetails.IsDelivery, cancellationToken);
        }

        private async Task<DialogTurnResult> ConfirmStepAsync(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            OrderDetails orderDetails = (OrderDetails) stepContext.Options;
            
            string response = stepContext.Result.ToString().ToLower();
            bool isDelivery = response.Contains("deliver");

            /* either one or the other, not both and not neither */
            if (isDelivery ^ response.Contains("pick"))
            {
                stepContext.Values["IsDelivery"] = isDelivery;
                orderDetails.IsDelivery = isDelivery;
            }
            else
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text(
                    "Please answer with 'delivery' or 'pick-up'."), cancellationToken);
                return await stepContext.ReplaceDialogAsync(nameof(OrderDialog), stepContext.Options, cancellationToken);
            }

            string deliveryMode =
                (bool) orderDetails.IsDelivery ? "Delivery" : "Pick up";
            
            var messageText =
                $"Okay, I have written down the details of the order:\n\n"
                + $"Name: {orderDetails.Name}\n"
                + $"Email: {orderDetails.EmailAddress}\n"
                + $"Address: {orderDetails.StreetAddress}\n"
                + $"City: {orderDetails.City}\n"
                + $"Post code: {orderDetails.PostCode}\n"
                + $"Delivery method: {deliveryMode}\n\n"
                + "Is this correct?";
            
            var promptMessage = MessageFactory.Text(messageText, 
                messageText, InputHints.ExpectingInput);

            return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions { Prompt = promptMessage },
                cancellationToken);
        }


        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            if ((bool)stepContext.Result)
            {
                var orderDetails = (OrderDetails)stepContext.Options;

                return await stepContext.EndDialogAsync(orderDetails, cancellationToken);
            }

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
    }
}