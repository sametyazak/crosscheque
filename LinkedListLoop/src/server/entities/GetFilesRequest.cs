﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedListLoop.src.server.entities
{
    public class GetFilesRequest
    {
        public string FileId { get; set; }

        public string TranTypeName { get; set; }

        public bool DeleteFile { get; set; }
    }
}