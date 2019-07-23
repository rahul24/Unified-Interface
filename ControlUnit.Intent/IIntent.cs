using ControlUnit.Common.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ControlUnit.Intent
{
    public interface IIntent
    {
        CommonModel Process(CommonModel commonModel);
    }
}
