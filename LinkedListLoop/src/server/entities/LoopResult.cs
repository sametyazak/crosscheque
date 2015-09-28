using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedListLoop.entities
{
    public class LoopResult
    {
        public LoopResult()
        {
            this.Items = new List<string>();
            this.Loop = new List<string>();
        }

        public List<string> Items { get; set; }

        public List<string> Loop { get; set; }

        public bool HasLoop { get; set; }

        public string LoopHtmlText { get; set; }
    }

    public class UpdatedSteps
    {
        public string Receiver { get; set; }

        public string Sender { get; set; }

        public decimal Amount { get; set; }

        public string Date { get; set; }

        public string id { get; set; }
    }
}