using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedListLoop.src
{
    public class Converter
    {
        public static decimal ToDecimal(string value)
        {
            decimal dValue = 0;
            decimal.TryParse(value, out dValue);
            return dValue;
        }
    }
}