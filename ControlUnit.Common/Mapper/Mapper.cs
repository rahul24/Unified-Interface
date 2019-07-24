using ControlUnit.Common.Model;
using Google.Cloud.Dialogflow.V2;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using ControlUnit.Common.Extension;
using Alexa.NET.Request.Type;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Alexa.NET;

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
            else if (requestBody.Contains("amzn"))
            {
                var skillrequest = requestBody.ConvertToAlexaDialog();
                model = new CommonModel()
                {
                    Id = skillrequest.Request.RequestId,
                    Source = "amazon",
                    Version = skillrequest.Version,
                    Query = string.Empty,
                    Session = skillrequest.Session.SessionId,
                    Confidence = 0,
                    Intent = GetIntent((skillrequest.Request as IntentRequest).Intent.Name),
                    Tokens = GetTokens((skillrequest.Request as IntentRequest).Intent.Slots),
                    Payload = string.Empty
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

        private static IDictionary<string, string> GetTokens(IDictionary<string,Slot> fields)
        {
            IDictionary<string, string> collection = new Dictionary<string, string>();
            foreach (var item in fields)
            {
                if (!string.IsNullOrEmpty(item.Value.Value)) //&& item.Value.ConfirmationStatus.ToLower().Equals("user"))
                {
                    if (!collection.ContainsKey(item.Key))
                        collection.Add(item.Key, item.Value.Value);
                    else
                        collection[item.Key] += "," + item.Value.Value;
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
            else if (model.Source.Equals("amazon"))
            {
                result =  ResponseBuilder.Tell(model.ResponseString);

                //new ResponseBody()
                //{
                //    Reprompt = new Reprompt(model.ResponseString),
                //    ShouldEndSession = false,
                //},
                //Version = model.Version
            
            }

            return (T)result;
        }

    }
}
