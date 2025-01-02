// Generated with CoreBot .NET Template version v4.22.0

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoreBot.CognitiveModels;
using CoreBot.DialogDetails;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;

namespace CoreBot.Dialogs
{
    public class MainDialog : ComponentDialog
    {
        private readonly PcPartsCLURecognizer _recognizer;
        private readonly ILogger _logger;

        // Dependency injection uses this constructor to instantiate MainDialog
        public MainDialog(PcPartsCLURecognizer pcPartsCluRecognizer,
            OrderDialog orderDialog,
            OpeningHoursDialog openingHoursDialog,
            ReturnOrderDialog returnOrderDialog,
            CatalogueDialog catalogueDialog,
            ILogger<MainDialog> logger)
            : base(nameof(MainDialog))
        {
            _recognizer = pcPartsCluRecognizer;
            _logger = logger;

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(orderDialog);
            AddDialog(returnOrderDialog);
            AddDialog(openingHoursDialog);
            AddDialog(catalogueDialog);

            var waterfallSteps = new WaterfallStep[]
            {
                FirstActionStepAsync,
                ActionActStepAsync,
                FinalStepAsync,
            };

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> FirstActionStepAsync(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            if (!_recognizer.IsConfigured)
            {
                throw new InvalidOperationException("ERROR: CLU not configured");
            }

            // Use the text provided in FinalStepAsync or the default if it is the first time.
            var messageText = stepContext.Options?.ToString() ?? "What would you like to do?";
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput) }, cancellationToken);
        }

        private async Task<DialogTurnResult> ActionActStepAsync(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            var result = await _recognizer.RecognizeAsync<PcPartsBotModel>(stepContext.Context, cancellationToken);
            
            switch(result.GetTopIntent().intent)
            {
                case PcPartsBotModel.Intent.OrderProduct:
                    var orderDetails = new OrderDetails();
                    orderDetails.ProductName = result.Entities.GetComponent();
                    orderDetails.Name = result.Entities.GetName();
                    orderDetails.EmailAddress = result.Entities.GetEmail();
                    orderDetails.PhoneNumber = result.Entities.GetPhone();
                    orderDetails.StreetAddress = result.Entities.GetStreet();
                    orderDetails.City = result.Entities.GetCity();
                    orderDetails.ZipCode = result.Entities.GetZipcode();
                    orderDetails.DeliveryOption = result.Entities.GetDeliveryOption();
                    return await stepContext.BeginDialogAsync(nameof(OrderDialog), orderDetails, cancellationToken: cancellationToken);
                case PcPartsBotModel.Intent.ReturnOrder:
                    var returnOrderDetails = new ReturnOrderDetails();
                    returnOrderDetails.OrderNumber = result.Entities.GetOrderNumber();
                    return await stepContext.BeginDialogAsync(nameof(ReturnOrderDialog), returnOrderDetails, cancellationToken: cancellationToken);
                case PcPartsBotModel.Intent.ShowCatalogue:
                    var catalogueDetails = new CatalogueDetails();
                    catalogueDetails.Category = result.Entities.GetComponent();
                    return await stepContext.BeginDialogAsync(nameof(CatalogueDialog), catalogueDetails, cancellationToken: cancellationToken);
                case PcPartsBotModel.Intent.ShowOpeningHours:
                    return await stepContext.BeginDialogAsync(nameof(OpeningHoursDialog), cancellationToken: cancellationToken);
                default:
                    return await stepContext.NextAsync("None", cancellationToken);
            }
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            // Restart the main dialog with a different message the second time around
            var promptMessage = "What else can I do for you?";
            if (stepContext.Result is string result && result == "None")
            {
                promptMessage = "Sorry, I didn't understand. Can you say it differently?";
            }
            return await stepContext.ReplaceDialogAsync(InitialDialogId, promptMessage, cancellationToken);
        }
    }
}