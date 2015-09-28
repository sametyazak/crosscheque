using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedListLoop.entities
{
    public class AjaxResult
    {
        public bool IsError { get; set; }

        public string ErrorMessage { get; set; }

        public object ResultObject { get; set; }
    }
}