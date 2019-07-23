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
                    Payload = webhookRequest?.QueryResult?.WebhookPayload?.ToString()
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

            return intent;
        }

        private static IDictionary<string, string> GetTokens(MapField<string, Value> fields)
        {
            IDictionary<string, string> collection = new Dictionary<string, string>();
            foreach (var item in fields.Values)
            {
                if (item.ListValue?.Values?.Count > 0)
                {
                    if (!collection.ContainsKey(item.ListValue.Values[0].StringValue))
                        collection.Add(item.ListValue.Values[0].StringValue, item.ListValue.Values[1].StringValue);
                    else
                        collection[item.ListValue.Values[0].StringValue] += "," + item.ListValue.Values[1].StringValue;
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
