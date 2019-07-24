using System;
using System.Collections.Generic;
using System.Text;
using ControlUnit.Common.Model;

namespace ControlUnit.Intent
{
    public class BuyCloth : IIntent
    {
        private static readonly IDictionary<string, string> prices = CreatePrices();

        public CommonModel Process(CommonModel commonModel)
        {            
            string fallbackString = "Please tell us the {0}. For which you want the price.";
            string responseString = "The price of {0} {1} is Rs. {2}.";


            //validatons
            if (!commonModel.Tokens.ContainsKey("clothtype"))
            {
                commonModel.ResponseString = string.Format(fallbackString, "cloth type");
            }
            else if (!commonModel.Tokens.ContainsKey("size"))
            {
                commonModel.ResponseString = string.Format(fallbackString, "size");
            }
            else
            {
                string type = commonModel.Tokens["clothtype"];
                string size = commonModel.Tokens["size"];
                if (prices.ContainsKey(type + "_" + size))
                {                    
                    string price = prices[type + "_" + size];
                    commonModel.ResponseString = string.Format(responseString, size, type, price);
                }
                else
                {
                    commonModel.ResponseString = "Prices of specified item is not available. Please try someother time.";
                }
            }

            //commonModel.ResponseString = "reached";

            return commonModel;
        }

        private static IDictionary<string, string> CreatePrices()
        {
            IDictionary<string, string> p = new Dictionary<string, string>();
            p.Add("trouser_large", "3000");
            p.Add("trouser_small", "3500");
            p.Add("trouser_medium", "2000");

            p.Add("shirt_large", "1000");
            p.Add("shirt_small", "1500");
            p.Add("shirt_medium", "2000");

            p.Add("lower_large", "3100");
            p.Add("lower_small", "3200");
            p.Add("lower_medium", "2300");

            p.Add("capri_large", "1000");
            p.Add("capri_small", "3500");
            p.Add("capri_medium", "2400");

            return p;
        }


    }
}
