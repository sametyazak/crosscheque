using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LinkedListLoop.src.server.entities
{
    [Serializable()]
    public class KnownException : Exception
    {
        public KnownException(string message)
            : base(message) {
        }

        public KnownException(Exception inner, string message)
            : base(message, inner) {
        }

        public KnownException(SerializationInfo info, StreamingContext context)
            : base(info, context) {
        }
    }
}