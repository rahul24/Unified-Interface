using Google.Cloud.Dialogflow.V2;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using Alexa.NET.Request;
using Newtonsoft.Json;
using Alexa.NET.Response;

namespace ControlUnit.Common.Extension
{
    public static class DialogExtension
    {
        // A Protobuf JSON parser configured to ignore unknown fields. This make
        // the action robust against new fields being introduced by Dialogflow.

        private static readonly JsonParser jsonParser = new JsonParser(JsonParser.Settings.Default.WithIgnoreUnknownFields(true));
        public static WebhookRequest ConvertToDialogflow(this string requestBody)
        {
            WebhookRequest request = jsonParser.Parse<WebhookRequest>(requestBody);
            return request;
        }

        public static string ConvertDialogflow(this WebhookResponse webhookResponse)
        {
            return webhookResponse.ToString();
        }

        public static SkillRequest ConvertToAlexaDialog(this string requestBody)
        {
            SkillRequest request = JsonConvert.DeserializeObject<SkillRequest>(requestBody);
            return request;
        }

        public static string ConvertAlexaDialog(this SkillResponse skillResponse)
        {
            string response = JsonConvert.SerializeObject(skillResponse, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include
            });
            return response;
        }

    }

}

