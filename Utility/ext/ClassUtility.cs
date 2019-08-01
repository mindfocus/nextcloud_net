using System;
using System.Collections.Generic;
using System.Text;

namespace ext
{
    public static class ClassUtility
    {
        public static bool HasMethod(object objectToCheck, string methodName)
        {
            var type = objectToCheck.GetType();
            return type.GetMethod(methodName) != null;
        }
    }
}
