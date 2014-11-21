using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMSClient.Integration;
using SMSClient.Models;

namespace SMSClient.Component
{
    public class HandleResponse // if the return is null, then input invalid
    {
        public static PatientResponseApiPostModel HandleMultiSelectionResponse(SurveyInstance patientSurvey, ResponseModel response)
        {
            char[] delimitors = { ' ', ',' };
            string[] parts = response.ResponseText.Split(delimitors);

            bool isValid = SmsValidation.validMultipleSelection(patientSurvey, patientSurvey.CurrentQuestion, response.ResponseText);
            if (isValid)
            {
                PatientResponseApiPostModel patientResponse = patientSurvey.MakePostModelForResponseToCurrentQuestion();
                foreach(string item in parts)
                {
                    PatientResponseValueApiPostModel msResponse = new PatientResponseValueApiPostModel();
                    msResponse.SurveyParameterTypeId = (int)SurveyParameterTypeEnum.Survey;
                    int value = patientSurvey.FetchOptionCodeForResponseValue(item);
                    if (value != -1)
                    {
                        msResponse.PatientSurveyOptionId = value;
                        patientResponse.PatientResponseValues.Add(msResponse);
                    }
                }

                IvrService vivifyService = new IvrService();
                return vivifyService.PostPatientResponse(patientResponse);
            }
            return null;
        }

        public static PatientResponseApiPostModel HandleSingleSelectionResponse(SurveyInstance patientSurvey, ResponseModel response)
        {
            string singleResponse = response.ResponseText;

            bool isValid = SmsValidation.validSingleSelection(patientSurvey, patientSurvey.CurrentQuestion, singleResponse);
            if (isValid)
            {
                PatientResponseApiPostModel patientResponse = patientSurvey.MakePostModelForResponseToCurrentQuestion();
                PatientResponseValueApiPostModel singleResponseModel = new PatientResponseValueApiPostModel();
                singleResponseModel.SurveyParameterTypeId = (int)SurveyParameterTypeEnum.Survey;
                singleResponseModel.PatientSurveyOptionId =patientSurvey.FetchOptionCodeForResponseValue(singleResponse);
                patientResponse.PatientResponseValues.Add(singleResponseModel);
                IvrService vivifyService = new IvrService();
                return vivifyService.PostPatientResponse(patientResponse);
            }

            return null;
        }

        #region questionable handling function
        public static PatientResponseApiPostModel HandleNumberResponse(SurveyInstance patientSurvey, ResponseModel response)
        {
            string number = response.ResponseText;

            bool isValid = number!=null;

            if (isValid)
            {
                PatientResponseApiPostModel patientResponse = patientSurvey.MakePostModelForResponseToCurrentQuestion();

                PatientResponseValueApiPostModel NumberResponse = new PatientResponseValueApiPostModel();
                NumberResponse.SurveyParameterTypeId = (int)SurveyParameterTypeEnum.ReadingType;//not sure about this
                NumberResponse.Value = number;
                patientResponse.PatientResponseValues.Add(NumberResponse);

                IvrService vivifyService = new IvrService();
                return vivifyService.PostPatientResponse(patientResponse);
            }

            return null;
        }
        #endregion

        public static PatientResponseApiPostModel HandlePulseOxResponse(SurveyInstance patientSurvey, ResponseModel response)
        {
            char[] delimitors = { ' ', ',' };
            string[] parts = response.ResponseText.Split(delimitors);

            string oxygen = null;
            string heartRate = null; 

            if (parts.Length > 1)
            {
                oxygen = parts[0];
                heartRate = parts[1];
            }
            bool isValid = SmsValidation.validPulseOxHeartRate(oxygen, heartRate);

            if (isValid)
            {
                PatientResponseApiPostModel patientResponse = patientSurvey.MakePostModelForResponseToCurrentQuestion();

                // Add Heart Rate Value
                PatientResponseValueApiPostModel hearRateResonse = new PatientResponseValueApiPostModel();
                hearRateResonse.SurveyParameterTypeId = (int)SurveyParameterTypeEnum.Pulse; // heart rate
                hearRateResonse.Value = heartRate;
                patientResponse.PatientResponseValues.Add(hearRateResonse);

                // Add Oxygen Value
                PatientResponseValueApiPostModel OxygenResonse = new PatientResponseValueApiPostModel();
                OxygenResonse.SurveyParameterTypeId = (int)SurveyParameterTypeEnum.Oxygen; // Oxygen
                OxygenResonse.Value = oxygen;
                patientResponse.PatientResponseValues.Add(OxygenResonse);

                IvrService vivifyService = new IvrService();
                return vivifyService.PostPatientResponse(patientResponse);
            }

            return null;
        }

        public static PatientResponseApiPostModel HandleWeightResponse(SurveyInstance patientSurvey, ResponseModel response)
        {
            string weight = response.ResponseText;

            bool isValid = SmsValidation.validWeightResponse(weight);

            if (isValid)
            {
                PatientResponseApiPostModel patientResponse = patientSurvey.MakePostModelForResponseToCurrentQuestion();

                PatientResponseValueApiPostModel WeightResponse = new PatientResponseValueApiPostModel();
                WeightResponse.SurveyParameterTypeId = (int)SurveyParameterTypeEnum.Weight;
                WeightResponse.Value = weight;
                patientResponse.PatientResponseValues.Add(WeightResponse);

                IvrService vivifyService = new IvrService();
                return vivifyService.PostPatientResponse(patientResponse);
            }

            return null;
        }

        public static PatientResponseApiPostModel HandleBloodPressureResponse(SurveyInstance patientSurvey, ResponseModel response)
        {
            char[] delimitors = { ' ', ',' };
            string[] parts = response.ResponseText.Split(delimitors);

            string systole = null;
            string diastole = null;

            if (parts.Length > 1)
            {
                systole = parts[0];
                diastole = parts[1];
            }
            bool isValid = SmsValidation.validBloodPressure(systole, diastole);

            if (isValid)
            {
                PatientResponseApiPostModel patientResponse = patientSurvey.MakePostModelForResponseToCurrentQuestion();

                // Add systolic
                PatientResponseValueApiPostModel systolicResponse = new PatientResponseValueApiPostModel();
                systolicResponse.SurveyParameterTypeId = (int)SurveyParameterTypeEnum.Systolic; // 
                systolicResponse.Value = diastole;
                patientResponse.PatientResponseValues.Add(systolicResponse);

                // Add diastolic
                PatientResponseValueApiPostModel diastolicResponse = new PatientResponseValueApiPostModel();
                diastolicResponse.SurveyParameterTypeId = (int)SurveyParameterTypeEnum.Diastolic; // 
                diastolicResponse.Value = systole;
                patientResponse.PatientResponseValues.Add(diastolicResponse);

                IvrService vivifyService = new IvrService();
                return vivifyService.PostPatientResponse(patientResponse);
            }

            return null;
        }

        public static PatientResponseApiPostModel HandleBloodSugarResponse(SurveyInstance patientSurvey, ResponseModel response)
        {
            string bloodsugarResponse = response.ResponseText;

            bool isValid = SmsValidation.validBloodSugarResponse(bloodsugarResponse);

            if (isValid)
            {
                PatientResponseApiPostModel patientResponse = patientSurvey.MakePostModelForResponseToCurrentQuestion();

                PatientResponseValueApiPostModel BloodSugarResponse = new PatientResponseValueApiPostModel();
                BloodSugarResponse.SurveyParameterTypeId = (int)SurveyParameterTypeEnum.BloodSugar;
                BloodSugarResponse.Value = bloodsugarResponse;
                patientResponse.PatientResponseValues.Add(BloodSugarResponse);

                IvrService vivifyService = new IvrService();
                return vivifyService.PostPatientResponse(patientResponse);
            }

            return null;
        }
    }
}