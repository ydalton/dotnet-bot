using System.Threading;
using System.Threading.Tasks;
using CoreBot.Helpers;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace CoreBot.Dialogs;

public class OpeningHoursDialog : CancelAndHelpDialog
{
    public OpeningHoursDialog()
        : base(nameof(OpeningHoursDialog))
    {
        var waterfallSteps = new WaterfallStep[]
        {
            FirstActStepAsync
        };

        AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
        InitialDialogId = nameof(WaterfallDialog);
    }

    private async Task<DialogTurnResult> FirstActStepAsync(WaterfallStepContext stepContext,
        CancellationToken cancellationToken)
    {
        // Show card with opening hours
        var response = MessageFactory.Attachment(CardHelper.CreateCardAttachment("openinghoursCard"));
        await stepContext.Context.SendActivityAsync(response, cancellationToken);
        // End the dialog
        return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
    }
}