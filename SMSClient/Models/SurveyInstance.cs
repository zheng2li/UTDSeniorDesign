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
    public class SurveyInstance
    {

        public int CurrentQuestion { get; set; }

        private static string AccountSid = "AC7a6db27538ba8ed863c14e825beb35f4";
        private static string AuthToken = "56cb022777274d5e98fdeed9523987a7";
        private static TwilioRestClient twilio = new TwilioRestClient(AccountSid, AuthToken);
        private static string server = "+17743077070";
        public int userID;
        public string phone;
        //string emergencyContact;
        
        //public DateTime previousContact;
        List<PatientSurveyModel> surveyData;

        public SurveyInstance(int userID, string phone)
        {
            this.userID = userID;
            this.phone = phone;
            CurrentQuestion = -1;//-1 means prep stage.
            surveyData = null;
            //previousContact = DateTime.Now;
            var send = twilio.SendMessage(server, phone, "Are you ready to take your survey?");
        }

        public void fetchSurvey()//gets survey AND sets first currentQuestion value
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            var client = new RestClient("https://demoutdesign.dev.vivifyhealth.com/api/PatientSurvey");
            var request = new RestRequest(Method.GET);
            request.AddParameter("authtoken", "26881576-3F9B-4F97-B7F7-91532DE1586A", ParameterType.QueryString);
            request.AddParameter("PatientId", userID.ToString(), ParameterType.QueryString);
            surveyData = client.Execute<List<PatientSurveyModel>>(request).Data;
            CurrentQuestion = 0;
        }
        public bool NextQuestion()
        {
            CurrentQuestion++;
            return CurrentQuestion < surveyData.Count;
        }
        public PatientSurveyQuestionModel GetCurrentQuestion()
        {
            return GetQuestion(CurrentQuestion);
        }
        private PatientSurveyQuestionModel GetQuestion(int questionNumber)
        {
            PatientSurveyQuestionModel retVal = null;
            List<PatientSurveyQuestionModel> results = new List<PatientSurveyQuestionModel>();
            foreach (PatientSurveyModel properties in surveyData)
            {
                foreach (PatientSurveyQuestionModel question in properties.PatientSurveyQuestions)
                {
                    results.Add(question);
                }
            }
            if (questionNumber < results.Count)
            {
                retVal = results[questionNumber];
            }
            return retVal;
        }

        internal int FetchOptionCodeForResponseValue(string value)//returns option code for a response string.
        {
            int retval = -1;
            value = value.Trim();
            List<PatientSurveyOptionModel> currentq = GetCurrentQuestion().PatientSurveyOptions;
            int opcount = 1;
            foreach(PatientSurveyOptionModel item in currentq)
            {
                bool matchesthisoption = false;
                foreach(PatientSurveyOptionTextModel text in item.PatientSurveyOptionTexts)
                {
                    if(string.Compare(value, text.Text, true)==0)
                    {
                        matchesthisoption = true;
                        break;
                    }  
                }
                if(opcount.ToString().Equals(value))
                {
                    matchesthisoption = true;
                }
                if(matchesthisoption)
                {
                    retval = item.PatientSurveyOptionId;
                    break;
                }
                opcount++;
            }
            return retval;
        }

        internal static string getFormattedQuestionText(PatientSurveyQuestionModel patientQuestion)
        {
            string retVal = "";
            if ( patientQuestion != null )
            {
                switch (  patientQuestion.SurveyQuestionTypeId )
                {
                    case (int)SurveyQuestionType.PulseOx: // SurveyQuestionTypeEnum.PulseOx
                        {
                            retVal = "Please enter your oxygen level and heart rate.";
                            break;
                        }
                    case (int)SurveyQuestionType.BloodPressure: // SurveyQuestionTypeEnum.BloodPressure
                        {
                            retVal = "Please enter your blood pressure. Systolic,Diastolic.";
                            break;
                        }
                    case (int)SurveyQuestionType.BloodSugar:
                        {
                            retVal = "Please enter your Blood Sugar Level.";
                            break;
                        }
                    case (int)SurveyQuestionType.Weight:
                        {
                            retVal = "Please enter your weight.";
                            break;
                        }
                    /*case (int)SurveyQuestionType.SingleSelection:
                        {

                        }
                    case (int)SurveyQuestionType.MultiSelection:
                        {

                        }*/
                    default:
                        {
                            retVal = patientQuestion.PatientSurveyQuestionTexts.First<PatientSurveyQuestionTextModel>().Text + "\n";
                            int i = 1;
                            if (patientQuestion.PatientSurveyOptions != null)
                            {
                                foreach (PatientSurveyOptionModel option in patientQuestion.PatientSurveyOptions)
                                {
                                    retVal += i + ": ";
                                    foreach (PatientSurveyOptionTextModel temp in option.PatientSurveyOptionTexts)
                                    {
                                        retVal += temp.Text + " ";
                                    }
                                    retVal += "\n";
                                    i++;
                                }
                            }
                            break;
                        }
                }
            }
            return retVal;
        }

        internal PatientResponseApiPostModel MakePostModelForResponseToCurrentQuestion()//will populate with appropriate PatientResponseValueApiPostModel with empty PatientResponseValues
        {
            PatientResponseApiPostModel output = new PatientResponseApiPostModel();
            output.PatientId = userID;
            PatientSurveyQuestionModel currentQuestion = GetCurrentQuestion();
            output.PatientSurveyQuestionId = currentQuestion.PatientSurveyQuestionId;
            output.SurveyQuestionTypeId = currentQuestion.SurveyQuestionTypeId;
            output.PatientResponseInputMethodId = 1;
            output.ObservationDateTime_UTC = System.DateTime.Now;
            output.PatientResponseValues = new List<PatientResponseValueApiPostModel>();

            return output;
        }

        internal bool CurrentlyOnOptionlessResponse()
        {
            return GetCurrentQuestion().PatientSurveyOptions.Count == 0;
        }
    }
}