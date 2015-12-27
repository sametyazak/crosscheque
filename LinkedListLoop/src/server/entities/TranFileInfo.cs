using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using LinkedListLoop.src.server.transaction_types;

namespace LinkedListLoop.src.server.entities
{
    [Table("Files")]
    public class TranFileInfo
    {
        public string Path { get; set; }

        public string Id { get; set; }

        public string TransactionType { get; set; }

        public Nullable<TimeSpan> UploadTime { get; set; }

        public Nullable<DateTime> UploadStartDate { get; set; }

        public Nullable<DateTime> UploadEndDate { get; set; }

        public Nullable<TimeSpan> InsertTime { get; set; }

        public Nullable<DateTime> InsertStartDate { get; set; }

        public Nullable<DateTime> InsertEndDate { get; set; }

        public Nullable<TimeSpan> ReadTime { get; set; }

        public Nullable<DateTime> ReadStartDate { get; set; }

        public Nullable<DateTime> ReadEndDate { get; set; }

        public string UserInfo { get; set; }

        public string CompanyInfo { get; set; }

        public string FileName { get; set; }

        public int RecordCount { get; set; }

        public string TempTableName { get; set; }

        public FileContentLocation ContentLocation { get; set; } 
    }
}