using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedListLoop.src.server.entities
{
    public enum AccessLevel
    {
        Guest = 0,
        Demo = 1,
        Regular = 5,
        Developer = 10,
        Administrator = 100
    }
}