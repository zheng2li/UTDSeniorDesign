using System;
using System.Collections.Generic;

namespace SMSClient.Models
{
    public class PatientResponseApiPostModel
    {
        public int PatientId { get; set; }
        public int PatientSurveyQuestionId { get; set; }
        public int SurveyQuestionTypeId { get; set; }
        public int PatientResponseInputMethodId { get; set; }
        public bool IsPatientSurveyCompleted { get; set; }
        public System.DateTime ObservationDateTime_UTC { get; set; }

        public List<PatientResponseValueApiPostModel> PatientResponseValues { get; set; }
    }

    public class PatientResponseValueApiPostModel
    {
        public int SurveyParameterTypeId { get; set; }
        public string Value { get; set; }
        public int? PatientSurveyOptionId { get; set; }
    }

    public class PatientResponseModel
    {
        public int PatientResponseId { get; set; }
        public int PatientId { get; set; }
        public int PatientSurveyQuestionId { get; set; }
        public int SurveyQuestionTypeId { get; set; }
        public int PatientResponseInputMethodId { get; set; }
        public System.DateTime ObservationDateTime_UTC { get; set; }
        public System.DateTime ObservationDate { get; set; }
        public System.DateTime CreatedDateTime_UTC { get; set; }
        public int CreatedBy_Id { get; set; }
        public Nullable<System.DateTime> DeletedDateTime_UTC { get; set; }
        public Nullable<int> DeletedBy_Id { get; set; }
        public AlertSeverityLevel HighestAlertSeverityLevel
        {
            get
            {
                AlertSeverityLevel retval = AlertSeverityLevel.None;

                foreach (PatientResponseValueModel PatientResponseValue in PatientResponseValues)
                {
                    if (PatientResponseValue.AlertSeverityLevelId < (int)retval)
                    {
                        retval = (AlertSeverityLevel)PatientResponseValue.AlertSeverityLevelId;
                    }
                }

                return retval;
            }
        }
        public bool IsPatientSurveyCompleted { get; set; }

        public ICollection<PatientResponseValueModel> PatientResponseValues { get; set; }
        public PatientSurveyQuestionModel PatientSurveyQuestion { get; set; }
        public ICollection<PatientAlertModel> PatientAlerts { get; set; }

        public bool IsBiometricsType()
        {
            if (SurveyQuestionTypeId == (int)SurveyQuestionType.BloodPressure || SurveyQuestionTypeId == (int)SurveyQuestionType.BloodSugar
                || SurveyQuestionTypeId == (int)SurveyQuestionType.PulseOx || SurveyQuestionTypeId == (int)SurveyQuestionType.Weight)
            {
                return true;
            }
            return false;
        }
    }

    public class PatientResponseValueModel
    {
        public PatientResponseValueModel()
        {
            this.AlertSeverityLevelId = (int)AlertSeverityLevel.None;
        }
        public int PatientResponseValueId { get; set; }
        public int PatientId { get; set; }
        public int PatientResponseId { get; set; }
        public int SurveyParameterTypeId { get; set; }
        public int PatientSurveyOptionId { get; set; }
        public string Value { get; set; }
        public int AlertSeverityLevelId { get; set; }
        public System.DateTime ObservationDate { get; set; }
        public System.DateTime ObservationDateTime_UTC { get; set; }
        public System.DateTime CreatedDateTime_UTC { get; set; }
        public int CreatedBy_Id { get; set; }
        public Nullable<System.DateTime> DeletedDateTime_UTC { get; set; }
        public Nullable<int> DeletedBy_Id { get; set; }

        public PatientSurveyOptionModel PatientSurveyOption { get; set; }
    }

    public class PatientAlertModel
    {
        public int PatientAlertId { get; set; }
        public int AlertTypeId { get; set; }
        public int AlertSeverityLevelId { get; set; }
        public string AlertSeverityName { get; set; }
        public int AlertStatusId { get; set; }
        public Nullable<int> PatientResponseId { get; set; }
        public int PatientId { get; set; }
        public string Message { get; set; }
        public int CreatedBy_Id { get; set; }
        public System.DateTime CreatedDateTime_UTC { get; set; }
        public Nullable<int> AlertStatusSetBy_Id { get; set; }
        public Nullable<System.DateTime> AlertStatusSetDateTime_UTC { get; set; }

        //public static bool IsBiometricAlertType(AlertType AlertType)
        //{
        //    switch (AlertType)
        //    {
        //        case AlertType.BloodPressureBiometric:
        //        case AlertType.BloodSugarBiometric:
        //        case AlertType.PulseBiometric:
        //        case AlertType.OxygenBiometric:
        //        case AlertType.WeightScaleBiometric:
        //            {
        //                return true;
        //            }
        //    }

        //    return false;
        //}
    }

    public enum SurveyParameterTypeEnum
    {
        Systolic = 1,
        Diastolic = 2,
        BloodSugar = 3,
        Weight = 4,
        Pulse = 5,
        Oxygen = 6,
        ReadingType = 7,
        Survey = 8
    }
}