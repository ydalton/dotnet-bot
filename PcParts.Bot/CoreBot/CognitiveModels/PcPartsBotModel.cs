using Microsoft.Bot.Builder;
using Microsoft.BotBuilderSamples.Clu;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace CoreBot.CognitiveModels
{
    public class PcPartsBotModel : IRecognizerConvert
    {
        public enum Intent
        {
            OrderProduct,
            ReturnOrder,
            ShowCatalogue,
            ShowOpeningHours,
            None
        }

        public string Text { get; set; }

        public string AlteredText { get; set; }

        public Dictionary<Intent, IntentScore> Intents { get; set; }

        public CluEntities Entities { get; set; }

        public IDictionary<string, object> Properties { get; set; }

        public void Convert(dynamic result)
        {
            var jsonResult = JsonConvert.SerializeObject(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var app = JsonConvert.DeserializeObject<PcPartsBotModel>(jsonResult);

            Text = app.Text;
            AlteredText = app.AlteredText;
            Intents = app.Intents;
            Entities = app.Entities;
            Properties = app.Properties;
        }

        public (Intent intent, double score) GetTopIntent()
        {
            var maxIntent = Intent.None;
            var max = 0.0;
            foreach (var entry in Intents)
            {
                if (entry.Value.Score > max)
                {
                    maxIntent = entry.Key;
                    max = entry.Value.Score.Value;
                }
            }

            return (maxIntent, max);
        }

        public class CluEntities
        {
            public CluEntity[] Entities;

            public string GetComponent() => Entities.Where(e => e.Category == "Component").ToArray().FirstOrDefault()?.Text;
            public string GetDeliveryOption() => Entities.Where(e => e.Category == "DeliveryOption").ToArray().FirstOrDefault()?.Text;
            public string GetName() => Entities.Where(e => e.Category == "Name").ToArray().FirstOrDefault()?.Text;
            public string GetEmail() => Entities.Where(e => e.Category == "Email").ToArray().FirstOrDefault()?.Text;
            public string GetStreet() => Entities.Where(e => e.Category == "Street").ToArray().FirstOrDefault()?.Text;
            public string GetZipcode() => Entities.Where(e => e.Category == "Zipcode").ToArray().FirstOrDefault()?.Text;
            public string GetCity() => Entities.Where(e => e.Category == "City").ToArray().FirstOrDefault()?.Text;
            public string GetPhone() => Entities.Where(e => e.Category == "Phone").ToArray().FirstOrDefault()?.Text;

            public string GetOrderNumber() => Entities.Where(e => e.Category == "OrderNumber").ToArray().FirstOrDefault()?.Text;

            public string GetReason()
            {
                CluEntity reason = Entities.Where(e => e.Category == "RefundReason").ToArray().FirstOrDefault();
                if (reason == null)
                {
                    return null;
                }
                /* CLU provides an extra information attribute which gives us the key of the reason */
                if (reason.ExtraInformation != null && reason.ExtraInformation.Length > 0)
                {
                    if(reason.ExtraInformation[0].extraInformationKind == "ListKey")
                        return reason.ExtraInformation[0].key;
                }
                return reason.Text;
            }
            public string GetRefundOption() => Entities.Where(e => e.Category == "RefundOption").ToArray().FirstOrDefault()?.Text;
        }
    }

}
