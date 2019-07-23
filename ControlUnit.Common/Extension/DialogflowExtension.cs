using Google.Cloud.Dialogflow.V2;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ControlUnit.Common.Extension
{
    public static class DialogflowExtension
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

    }

}

