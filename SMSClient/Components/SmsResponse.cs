using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMSClient.Models;
using SMSClient.Components;

namespace SMSClient.Components
{
    public class SmsResponse
    {
        const string YES = "yes";
        const int NOT_STARTED = -1;
        const string EXIT_MSG = "Thank you for completing your survey.";
        const string REJECT_MSG = "Ok. We will try again later. Send yes to continue";
        const string RETRY_PREFIX = "Retry: ";

        static public string HandleSmsResponse(ResponseModel response, Dictionary<string, SurveyInstance> data)
        {
            string retVal = null;

            if ( response != null && response.From!=null)
            {
                SurveyInstance patientSurvey = data[response.From];
                if (patientSurvey != null)
                {
                    //
                    retVal = ProcessSmsText(patientSurvey, response);
                    if (retVal.Equals(EXIT_MSG))
                    {
                        data.Remove(response.From);
                    }
                }
            }
            return retVal;
        }

        static private string ProcessSmsText(SurveyInstance patientSurvey, ResponseModel response)
        {
            string retVal = null;
            if (patientSurvey != null )
            {
                if (patientSurvey.CurrentQuestion == NOT_STARTED )
                {
                    retVal = PlayFirstQuestion(patientSurvey, response);
                }
                else
                {
                    retVal = HandleQuestionResponse(patientSurvey, response);
                }
            }

            return retVal;
        }

        static private string PlayFirstQuestion(SurveyInstance patientSurvey, ResponseModel response)
        {
            string retVal = null;
            if (patientSurvey != null )
            {
                if ( string.Compare( response.ResponseText, YES, true ) == 0 )
                {
                    patientSurvey.fetchSurvey();
                    PatientSurveyQuestionModel nextQuestion = patientSurvey.GetCurrentQuestion();

                    retVal = SurveyInstance.getFormattedQuestionText(nextQuestion);
                }
                else
                {
                    retVal = REJECT_MSG;
                }
            }
            return retVal;
 
        }

        static private string HandleQuestionResponse(SurveyInstance patientSurvey, ResponseModel response)
        {
            string retVal = "";
            bool success = false;
            PatientResponseApiPostModel serverresponse = null;
            if (patientSurvey != null)
            {
                PatientSurveyQuestionModel nextQuestion = patientSurvey.GetCurrentQuestion();
                if (nextQuestion != null)
                {
                    try
                    {
                        switch ((SurveyQuestionType)nextQuestion.SurveyQuestionTypeId)
                        {
                            case SurveyQuestionType.BloodSugar:
                                {
                                    serverresponse = HandleResponse.HandleBloodSugarResponse(patientSurvey, response);
                                    break;
                                }
                            case SurveyQuestionType.BloodPressure:
                                {
                                    serverresponse = HandleResponse.HandleBloodPressureResponse(patientSurvey, response);
                                    break;
                                }
                            case SurveyQuestionType.PulseOx:
                                {
                                    serverresponse = HandleResponse.HandlePulseOxResponse(patientSurvey, response);
                                    break;
                                    }
                            case SurveyQuestionType.Weight:
                                {
                                    serverresponse = HandleResponse.HandleWeightResponse(patientSurvey, response);
                                    break;
                                }
                            case SurveyQuestionType.Number:
                                {
                                    serverresponse = HandleResponse.HandleNumberResponse(patientSurvey, response);
                                    break;
                                }
                            case SurveyQuestionType.SingleSelection:
                                {
                                    serverresponse = HandleResponse.HandleSingleSelectionResponse(patientSurvey, response);
                                    break;
                                }
                            case SurveyQuestionType.MultiSelection:
                                {
                                    serverresponse = HandleResponse.HandleMultiSelectionResponse(patientSurvey, response);
                                    break;
                                }
                        }
                    }
                    catch (Integration.HttpResponseException e)
                    {
                        Console.WriteLine(e.StackTrace);
                        serverresponse = new PatientResponseApiPostModel();//ignore case
                    };

                    success = serverresponse != null;
                    if(success || patientSurvey.CurrentlyOnOptionlessResponse())
                    {
                        patientSurvey.NextQuestion();
                        retVal += PlayQuestion(patientSurvey, response);
                    }
                    else
                    {
                        retVal += RETRY_PREFIX + PlayQuestion(patientSurvey, response);
                    }
                }
            }
            return retVal;
        }

        static private string PlayQuestion(SurveyInstance patientSurvey, ResponseModel response)
        {
            string retVal = null;
            if (patientSurvey != null)
            {
                PatientSurveyQuestionModel question = patientSurvey.GetCurrentQuestion();
                if (question != null)
                {
                    retVal = SurveyInstance.getFormattedQuestionText(question);
                }
                else
                {
                    retVal = EXIT_MSG;
                }
            }
            return retVal;
        }
    }
}