using System;
using ControlUnit.Common.Model;

namespace ControlUnit.Intent
{
    public class WelcomeIntent : IIntent
    {
        public CommonModel Process(CommonModel commonModel)
        {
            if(commonModel.Query.Contains("Hi"))
            {
                commonModel.ResponseString = "Whatttttttttt";
            }
            return commonModel;
        }
    }
}
