using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedListLoop.entities
{
    [DelimitedRecord(";")]
    public class ChequeInfoFile
    {
        [FieldOrder(1)]
        public string Sender { get; set; }

        [FieldOrder(2)]
        public string Receiver { get; set; }

        [FieldOrder(3)]
        [FieldConverter(ConverterKind.Decimal, ".")]
        public decimal Amount;

        [FieldOrder(4)]
        public string Date { get; set; }

    }
}