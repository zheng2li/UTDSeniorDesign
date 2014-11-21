using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Twilio;
using SMSClient.Models;
using System.Web.Mvc.Html;
using RestSharp;
using System.Net;
using Twilio.Mvc;
using Twilio.TwiML;

namespace SMSClient.Models
{
    public class ActiveRegistry
    {
        public Dictionary<string, SurveyInstance> Active;

        public ActiveRegistry()
        {
            Active = new Dictionary<string, SurveyInstance>();
        }

        public void AddInstance(int userID, string phone)
        {
            Active.Add(phone, new SurveyInstance(userID, phone));
        }

        public SurveyInstance GetInstance(string Phone)
        {
            return Active[Phone];
        }


        public string response(string from, string to, string message)
        {
            if (SurveyInstance.server.Equals(to))
            {
                SurveyInstance t = Active[from];
                string temp = t.response(message);
                if (t != null && temp != null) return temp;
                if (temp == null)
                {
                    Active.Remove(from);
                    return "Survey Over.";
                }
            }
            return "";
        }
    }
}
