using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMSClient.Models
{
    public class ResponseModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public string ResponseText { get; set; }
    }
}