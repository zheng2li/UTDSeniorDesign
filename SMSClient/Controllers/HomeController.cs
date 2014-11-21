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

//using Vivify.Platform.Models;

namespace SMSClient.Controllers
{
    public class HomeController : Controller
    {
        public static string AccountSid = "AC7a6db27538ba8ed863c14e825beb35f4";
        public static string AuthToken = "56cb022777274d5e98fdeed9523987a7";
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Received()
        {
            // Find your Account Sid and Auth Token at twilio.com/user/account 
            var twilio = new TwilioRestClient(AccountSid, AuthToken);
            // Build the parameters 
            var options = new MessageListRequest();
            options.DateSent = DateTime.Today;

            var messages = twilio.ListMessages(options);
            int counter = 0;
            ViewBag.messagebuffer = new string[int.Parse(messages.Total.ToString())];
            foreach (var message in messages.Messages)
            {
                ViewBag.messagebuffer[counter] = "Recieved from " + message.From + " at " + message.DateSent + ": " + message.Body;
                counter++;
            } 

            return View();
        }

        public ActionResult Vivify()
        {
            List<string> listOfQuestion = new List<string>();

            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            var client = new RestClient("https://demoutdesign.dev.vivifyhealth.com/api/PatientSurvey");
            var request = new RestRequest(Method.GET);
            request.AddParameter("authtoken", "26881576-3F9B-4F97-B7F7-91532DE1586A", ParameterType.QueryString);
            request.AddParameter("PatientId", "5", ParameterType.QueryString);
            var response = client.Execute<List<PatientSurveyModel>>(request).Data;


            //////
            // Find your Account Sid and Auth Token at twilio.com/user/account 
            var twilio = new TwilioRestClient(AccountSid, AuthToken);
            // Build the parameters 
            var options = new MessageListRequest();
            options.To = getPatientNumber();
            options.DateSent = DateTime.Today;

            var patientRecieved = twilio.ListMessages(options);
            
            options.To = "+17743077070";
            options.From = getPatientNumber();
            var patientSent = twilio.ListMessages(options);


            //This part parse the questions

            foreach (var properties in response)
            {
                foreach (var question in properties.PatientSurveyQuestions)
                {
                    listOfQuestion.Add(question.PatientSurveyQuestionTexts.First().Text);
                }
            }

            return View();
        }

        public ActionResult ResponseCount()
        {
            return View();
        }


        public string SendQuestion(int counter)
        {
            List<string> listOfQuestion = new List<string>();

            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            var client = new RestClient("https://demoutdesign.dev.vivifyhealth.com/api/PatientSurvey");
            var request = new RestRequest(Method.GET);
            request.AddParameter("authtoken", "26881576-3F9B-4F97-B7F7-91532DE1586A", ParameterType.QueryString);
            request.AddParameter("PatientId", "5", ParameterType.QueryString);
            var response = client.Execute<List<PatientSurveyModel>>(request).Data;


            //////
            // Find your Account Sid and Auth Token at twilio.com/user/account 
            var twilio = new TwilioRestClient(AccountSid, AuthToken);
            // Build the parameters 
            var options = new MessageListRequest();
            //options.To = "+17743077070";
            options.To = getPatientNumber();
            options.DateSent = DateTime.Today;

            var patientRecieved = twilio.ListMessages(options);

            options.To = "+17743077070";
            options.From = getPatientNumber();
            var patientSent = twilio.ListMessages(options);
            

            foreach (var properties in response)
            {
                foreach (var question in properties.PatientSurveyQuestions)
                {
                    listOfQuestion.Add(question.PatientSurveyQuestionTexts.First().Text);
                }
            }

            string questionToSend = listOfQuestion[counter];

            return questionToSend;
        }
        private void sendQuestion(string questionToSend, string number)
        {
            var twilio = new TwilioRestClient(AccountSid, AuthToken);

            var send = twilio.SendMessage("+17743077070", number, questionToSend);
        }

        private string getPatientNumber()
        {
            return "+12817813990";
        }

        public ActionResult Success()
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }

        #region team3Model that might not be used in final product

        [HttpPost]
        public ActionResult SendButton(Team3 model)
        {
            var twilio = new TwilioRestClient(AccountSid, AuthToken);

            var number = model.Number;
            var message = model.Message;
            var pass = model.Pass;
            if (pass != "1111")
                return View("Error");

            if (number.Substring(0, 2) != "+1")
            {
                number = "+1" + number;
            }

            var send = twilio.SendMessage("+17743077070", number, message);

            return View("Success");
        }

        #endregion

        #region no need of this

        //public ActionResult SendButton()
        //{
        //    Team3[] team3Members = new Team3[1];
        //    //team3Members[0] = new Team3 { Name = "Zheng Li", Number = "+12817813990" };
        //    //team3Members[1] = new Team3 { Name = "Hansel Sutiono", Number = "+19729630930" };
        //    //team3Members[2] = new Team3 { Name = "Elena Zhang", Number = "+12062889006" };
        //    //team3Members[3] = new Team3 { Name = "Dylan Boor", Number = "+14093546970" };
        //    team3Members[0] = new Team3 { Name = "Gerardus Hardjo", Number = "+12148548186" };



        //    string AccountSid = "AC7a6db27538ba8ed863c14e825beb35f4";
        //    string AuthToken = "56cb022777274d5e98fdeed9523987a7";
        //    var twilio = new TwilioRestClient(AccountSid, AuthToken);

        //    foreach (var member in team3Members)
        //    {
        //        var message = twilio.SendMessage("+17743077070", member.Number, "Hello");
        //    }

        //    return View("Success");
        //}

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
        #endregion
    }
}