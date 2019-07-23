using ControlUnit.Common;
using System;
using System.Collections.Generic;
using System.Text;
using ControlUnit.Common.Model;
using ControlUnit.Common.Extension;
using ControlUnit.Common.Mapper;

namespace ControlUnit
{
    public static class RequestHandler
    {
        public static CommonModel Handle(string body)
        {
            return Mapper.Convert(body); 
        }
    }
}
