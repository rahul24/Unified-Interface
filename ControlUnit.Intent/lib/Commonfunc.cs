using System;
using System.Collections.Generic;
using System.Text;

namespace ControlUnit.Intent.lib
{
    public static class Commonfunc
    {
        public static T GetInstance<T>(string type)
        {
            object o = null;
            try
            {
                o= (T)Activator.CreateInstance(Type.GetType(type));
            }
            catch(Exception ex) { }

            return (T)o;
        }
    }
}
