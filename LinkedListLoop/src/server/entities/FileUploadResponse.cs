using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedListLoop.src.server.entities
{
    public class FileUploadResponse
    {
        public TranFileInfo FileInfo { get; set; }

        public string status { get; set; }
    }
}