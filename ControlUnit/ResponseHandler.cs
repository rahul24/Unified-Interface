using ControlUnit.Common.Model;
using Google.Cloud.Dialogflow.V2;
using System;
using System.Collections.Generic;
using System.Text;
using ControlUnit.Common.Mapper;
using ControlUnit.Common.Extension;
using Alexa.NET.Response;

namespace ControlUnit
{
    public static class ResponseHandler
    {

        public static string Handle(CommonModel model)
        {
            string result = string.Empty;
            

            if (model.Source.ToLower().Equals("google"))
            {
                WebhookResponse response = Mapper.Convert<WebhookResponse>(model);
                result = response.ConvertDialogflow();
            }
            else if (model.Source.ToLower().Equals("amazon"))
            {
                SkillResponse response = Mapper.Convert<SkillResponse>(model);
                result = response.ConvertAlexaDialog();
            }

            return result;
        }

    }
}
