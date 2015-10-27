using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedListLoop.src.client
{
    [Serializable()]
    public enum LogDirection
    {
        ServerRequest,
        ServerResponse,
        UIRequest,
        None
    }
}