﻿using System;
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
using SMSClient.Components;

namespace SMSClient.Controllers
{

    public class HomeController : Controller
    {
        public static Dictionary<string, SurveyInstance> reg = new Dictionary<string, SurveyInstance>();

        public ActionResult Index()
        {
            ViewBag.reg = reg;
            ViewBag.current = reg.Keys;
            return View();
        }

        public ActionResult TwilioResponse()//twilio's response hits this.
        {
            ResponseModel patientResponse = new ResponseModel();
            patientResponse.From = Request["From"];
            patientResponse.To = Request["To"];
            patientResponse.ResponseText = Request["Body"];

            //string responseText = reg.response(patientResponse.From, patientResponse.To, patientResponse.ResponseText);

            string responseText = SmsResponse.HandleSmsResponse(patientResponse, reg);
            if (responseText != null)
            {
                ViewBag.responseText = responseText;
            }
            else
            {
                ViewBag.responseText = "Something has gone wrong. ";
            }

            return View();
        }
    }
}
