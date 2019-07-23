using ControlUnit.Common.Model;
using Google.Cloud.Dialogflow.V2;
using System;
using System.Collections.Generic;
using System.Text;
using ControlUnit.Common.Mapper;
using ControlUnit.Common.Extension;

namespace ControlUnit
{
    public static class ResponseHandler
    {

        public static string Handle(CommonModel model)
        {
            WebhookResponse response = Mapper.Convert<WebhookResponse>(model);
            return response.ConvertDialogflow();
        }

    }
}
