using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedListLoop.src.server.entities
{
    public class CustomeDbAttribute : Attribute
    {
        public string Name { get; private set; }

        public CustomeDbAttribute(string name)
        {
            this.Name = name;
        }
    }
}