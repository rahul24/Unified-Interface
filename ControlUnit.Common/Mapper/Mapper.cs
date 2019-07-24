using ControlUnit.Common.Model;
using Google.Cloud.Dialogflow.V2;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using ControlUnit.Common.Extension;

namespace ControlUnit.Common.Mapper
{
    public class Mapper
    {

        public static CommonModel Convert(string requestBody)
        {
            CommonModel model = null;
            if (requestBody.Contains("google"))
            {
                var webhookRequest = requestBody.ConvertToDialogflow();
                model = new CommonModel()
                {
                    Id = webhookRequest.ResponseId,
                    Source = webhookRequest.OriginalDetectIntentRequest.Source,
                    Version = webhookRequest.OriginalDetectIntentRequest.Version,
                    Query = webhookRequest.QueryResult.QueryText,
                    Session = webhookRequest.Session,
                    Confidence = webhookRequest.QueryResult.IntentDetectionConfidence,
                    Intent = GetIntent(webhookRequest.QueryResult.Intent.DisplayName),
                    Tokens = GetTokens(webhookRequest.QueryResult.Parameters.Fields),
                    Payload = webhookRequest?.OriginalDetectIntentRequest?.Payload?.ToString()
                }; 
            }

            return model;
        }

        private static string GetIntent(string displayName)
        {
            string intent = string.Empty;
            if(displayName.ToLower().Contains("welcome"))
            {
                intent = "ControlUnit.Intent.WelcomeIntent";
            }
            else if (displayName.ToLower().Contains("buycloth"))
            {
                intent = "ControlUnit.Intent.BuyCloth";
            }

            return intent;
        }

        private static IDictionary<string, string> GetTokens(MapField<string, Value> fields)
        {
            IDictionary<string, string> collection = new Dictionary<string, string>();
            foreach (var item in fields)
            {
                if (!string.IsNullOrEmpty(item.Value.StringValue))
                {
                    if (!collection.ContainsKey(item.Key))
                        collection.Add(item.Key, item.Value.StringValue);
                    else
                        collection[item.Key] += "," + item.Value.StringValue;
                }
            }

            return collection;
        }


        public static T Convert<T>(CommonModel model)
        {
            object result = null;

            if (model.Source.Equals("google"))
            {
                result = new WebhookResponse()
                {
                    FulfillmentText = model.ResponseString
                };
            }

            return (T)result;
        }

    }
}
