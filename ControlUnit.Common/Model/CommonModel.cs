using System;
using System.Collections.Generic;

namespace ControlUnit.Common.Model
{
    public class CommonModel
    {
        public string Id { get; set; }
        public string Session { get;set;}
        public string Source { get; set; }
        public string Version { get; set; }
        public string Payload { get; set; }
        public string Query { get;set;}
        public string Intent { get;set;}
        public IDictionary<string, string> Tokens { get; set; }
        public double Confidence { get;set;}
        public string ResponseString { get; set; }        
    }

    
}
