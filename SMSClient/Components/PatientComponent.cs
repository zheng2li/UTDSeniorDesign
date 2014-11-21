using System.Collections.Generic;
using System.Linq;
//using MiscUtil.Reflection;
using SMSClient.Integration;
using SMSClient.Models;

namespace SMSClient.Components
{
    public static class PatientComponent
    {
        public static PatientModel GetPatientByPhoneNumber(string phoneNumber)
        {
            PatientModel retVal = null;
            var service = new IvrService();

            List<PatientModel> patients = service.GetPatientsByPhoneNumber(phoneNumber);

            if (patients != null && patients.Count == 1)
            {
                retVal = patients.First();
            }

            //List<PatientModel> patients = service.GetAllPatients();

            //retVal = patients.Where(o => o.Phone1 == PhoneNumber).FirstOrDefault();

            //if (retVal == null)
            //{
            //    retVal = patients.Where(o => o.Phone2 == PhoneNumber).FirstOrDefault();
            //}

            return retVal;
        }

        public static PatientModel GetPatient(int patientId)
        {
            PatientModel retVal;
            var service = new IvrService();

            retVal = service.GetPatient(patientId);

            return retVal;
        }

        private static IEnumerable<PatientSurveyModel> GetPatientSurveysForToday(int patientId)
        {
            List<PatientSurveyModel> retVal = null;
            var service = new IvrService();

            IEnumerable<PatientSurveyModel> patientSurveys = service.GetPatientSurveys(patientId);

            //Will probably have to do a foreach with a switch case in order to include the proper weekly survey
            if (patientSurveys != null)
            {
                retVal = patientSurveys.Where(o => o.PatientSurveyScheduleModel.IsDaily()).ToList();
            }

            return retVal;
        }

        public static IEnumerable<PatientSurveyQuestionModel> GetPatientSurveyQuestionsForToday(int patientId)
        {
            List<PatientSurveyQuestionModel> retVal = null;

            IEnumerable<PatientSurveyModel> patientSurveys = GetPatientSurveysForToday(patientId);

            if (patientSurveys != null)
            {
                retVal = patientSurveys.SelectMany(patientSurvey => patientSurvey.PatientSurveyQuestions).OrderBy(o => o.PatientSurveyQuestionId).ToList();
                //retVal = SplitBiometricQuestions(retVal);
            }

            return retVal;
        }

        public static PatientSurveyQuestionModel GetPatientSurveyQuestion(int patientSurveyQuestionId)
        {
            PatientSurveyQuestionModel retVal;
            var service = new IvrService();

            retVal = service.GetPatientSurveyQuestion(patientSurveyQuestionId);

            return retVal;
        }

        public static PatientResponseApiPostModel PostPatientResponseApiPostModel(
            PatientResponseApiPostModel patientResponse)
        {
            PatientResponseApiPostModel retVal;
            var service = new IvrService();

            retVal = service.PostPatientResponse(patientResponse);

            return retVal;
        }

        private static List<PatientSurveyQuestionModel> SplitBiometricQuestions(
            IEnumerable<PatientSurveyQuestionModel> patientSurveyQuestions)
        {
            var retVal = new List<PatientSurveyQuestionModel>();

            foreach (PatientSurveyQuestionModel patientSurveyQuestion in patientSurveyQuestions)
            {
                if (patientSurveyQuestion.SurveyQuestionTypeId == (int)SurveyQuestionType.BloodSugar)
                {
                    PatientSurveyQuestionModel bloodSugarQuestion = patientSurveyQuestion;
                    bloodSugarQuestion.PatientSurveyOptions =
                        bloodSugarQuestion.PatientSurveyOptions.Where(
                            o => o.SurveyParameterTypeId == (int)SurveyParameterTypeEnum.BloodSugar).ToList();

                    retVal.Add(bloodSugarQuestion);
                }
                else if (patientSurveyQuestion.SurveyQuestionTypeId == (int)SurveyQuestionType.PulseOx)
                {
                    PatientSurveyQuestionModel pulseQuestion = patientSurveyQuestion;
                    var oxygenQuestion = new PatientSurveyQuestionModel();

                    //PropertyCopy.Copy(patientSurveyQuestion, oxygenQuestion);
                    PatientSurveyQuestionModel.clone(patientSurveyQuestion, oxygenQuestion);

                    //

                    pulseQuestion.PatientSurveyOptions =
                        pulseQuestion.PatientSurveyOptions.Where(
                            o => o.SurveyParameterTypeId == (int)SurveyParameterTypeEnum.Pulse).ToList();

                    oxygenQuestion.PatientSurveyOptions =
                        oxygenQuestion.PatientSurveyOptions.Where(
                            o => o.SurveyParameterTypeId == (int)SurveyParameterTypeEnum.Oxygen).ToList();

                    retVal.Add(pulseQuestion);
                    retVal.Add(oxygenQuestion);
                }
                else
                {
                    retVal.Add(patientSurveyQuestion);
                }
            }

            return retVal;
        }

        public static PatientSurveyQuestionModel GetNextSurveyQuestion(int patientId, int currentPatientSurveyQuestionId, List<int?> currentPatientSurveyOptionIds)
        {
            PatientSurveyQuestionModel retVal = null;

            List<PatientSurveyQuestionModel> patientSurveyQuestions = GetPatientSurveyQuestionsForToday(patientId).ToList();
            List<PatientSurveyQuestionModel> patientSurveyQuestionsOfSelectedOptions =
                patientSurveyQuestions.Where(o => currentPatientSurveyOptionIds != null && currentPatientSurveyOptionIds.Contains(o.ParentPatientSurveyOptionId)).ToList();

            //The selected option leads to a question
            if (patientSurveyQuestionsOfSelectedOptions.Count > 0)
            {
                retVal = patientSurveyQuestionsOfSelectedOptions.First();
            }
            //Otherwise, pick the next question that doesn't have a parent option.
            else
            {
                int index =
                    patientSurveyQuestions.FindIndex(o => o.PatientSurveyQuestionId == currentPatientSurveyQuestionId);
                index++;

                PatientSurveyQuestionModel nextQuestion = null;

                while (nextQuestion == null)
                {
                    if (index >= patientSurveyQuestions.Count)
                    {
                        break;
                    }

                    nextQuestion = patientSurveyQuestions.ElementAt(index);

                    //Is not a child question
                    if (nextQuestion.ParentPatientSurveyOptionId == 0)
                    {
                        retVal = nextQuestion;
                    }
                    else
                    {
                        //Make sure the loop keeps going by rejecting the child question.
                        nextQuestion = null;
                    }

                    index++;
                }
            }

            return retVal;
        }
    }
}