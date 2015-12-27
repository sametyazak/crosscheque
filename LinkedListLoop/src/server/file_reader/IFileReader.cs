using LinkedListLoop.src.server.entities.transaction_types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedListLoop.src.server
{
    public interface IFileReader
    {
        List<T> ReadFileAsList<T>(string filePath, List<UiGridColumn> columnDefinitionList);
    }
}