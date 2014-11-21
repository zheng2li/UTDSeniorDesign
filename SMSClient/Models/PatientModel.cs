using System;
using System.Collections.Generic;

namespace SMSClient.Models
{
    public class PatientModel
    {
        public int Id { get; set; }
        public string niuPatient_Status { get; set; }
        public string Patient_ThreId { get; set; }
        public string StudyId { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Last_Name { get; set; }

        public string FullName
        {
            get
            {
                return string.Format("{0}{1}{2}",
                    Last_Name,
                    string.IsNullOrEmpty(First_Name) ? "" : ", " + First_Name
                    , string.IsNullOrEmpty(Middle_Name) ? "" : ", " + Middle_Name);
            }
        }

        public string Gender { get; set; }
        public string SSN { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string EmailId1 { get; set; }
        public string PinCode { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Notes { get; set; }
        public string CorepointConfirmationId { get; set; }
        public string GoogleRegistrationId { get; set; }
        public bool? Pin_Resetreq { get; set; }
        public bool IsDeleted { get; set; }
        public int? MonitoringStatusId { get; set; }
        public int? HospitalId { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public string Sip_Uri { get; set; }
        public string Config_XML { get; set; }
        public int? LanguageId { get; set; }
        public string Language { get; set; }
        public string LocaleCode { get; set; }
        public int? PopulationId { get; set; }
        public string PopulationName { get; set; }
        public string AndroidVersionNo { get; set; }
        //public byte[] photo { get; set; }
        public int TimeZoneId { get; set; }
        public DateTime? LastConnectedTimeUTC { get; set; }
        public DateTime? CreatedDtUTC { get; set; }
        public DateTime? ModifiedDtUTC { get; set; }
        public string EmpiIdent { get; set; }
        public string AccountIdent { get; set; }
        public string VisitIdent { get; set; }
        public string HL7DeptIdent { get; set; }
        public string HL7Custom1 { get; set; }
        public string HL7Custom2 { get; set; }
        public string MRNIdent { get; set; }
        public bool IsPasswordInNewAlgo { get; set; }
        public int? PayerId { get; set; }
        public string Payer { get; set; }
        public int? RiskId { get; set; }
        public string RiskLevel { get; set; }
        public string RiskColor { get; set; }
        public int? ReferralId { get; set; }
        public string Referral { get; set; }
        public int? PopulationGroupId { get; set; }
        public string PopulationGroup { get; set; }
        public int? ProviderGroupId { get; set; }
        public string ProviderGroup { get; set; }
        public int? RaceId { get; set; }
        public string Race { get; set; }
        public int? ReligionId { get; set; }
        public string Religion { get; set; }
        public DateTime? DOB_UTC { get; set; }

        public int Age
        {
            get
            {
                int retVal = 0;
                if (DOB_UTC.HasValue)
                {
                    var zero = new DateTime(1, 1, 1);
                    TimeSpan span = DateTime.UtcNow - DOB_UTC.Value;
                    retVal = (zero + span).Year - 1;
                }
                return retVal;
            }
        }

        public int? ProgramDurationId { get; set; }
        public int ProgramDurationTotalDays { get; set; }
        public int ProgramDurationDay { get; set; }
        public string ProgramDurationName { get; set; }
        //public Date ProgramStartDate { get; set; }

        //public ICollection<int> PatientCaregivers { get; set; }
        public string KitNo { get; set; }
        public int ServiceLevelId { get; set; }
        public string ServiceLevel { get; set; }
        public int ServiceLevelTypeId { get; set; }
        public int PersonalCaregiverId { get; set; }
        public string PersonalCaregiverName { get; set; }

        /// <summary>
        ///     Represents Pin Requests and Overdue Careplans
        /// </summary>
        public int CriticalAlertCount { get; set; }

        public int HighCallAlertCount { get; set; }
        public int MedCallAlertCount { get; set; }
        public int HighAlertCount { get; set; }
        public int MedAlertCount { get; set; }
        //public int AlertRank
        //{
        //    get
        //    {
        //        return (CriticalAlertCount > 0 ? CriticalAlertCount * AlertWeight.Critical : 0)
        //            + (HighCallAlertCount > 0 ? HighCallAlertCount * AlertWeight.HighCall : 0)
        //            + (MedCallAlertCount > 0 ? MedCallAlertCount * AlertWeight.MediumCall : 0)
        //            + (HighAlertCount > 0 ? HighAlertCount * AlertWeight.High : 0)
        //            + (MedAlertCount > 0 ? MedAlertCount * AlertWeight.Medium : 0);
        //    }
        //}
        public int TotalAlertCount
        {
            get { return HighAlertCount + MedAlertCount; }
        }
    }

    public class PatientSurveyModel
    {
        public int PatientSurveyId { get; set; }
        public int SurveyTypeId { get; set; }
        public string Name { get; set; }
        public int PatientId { get; set; }
        public DateTime CreatedDateTime_UTC { get; set; }
        public int CreatedBy_Id { get; set; }
        public DateTime? DeletedDateTime_UTC { get; set; }
        public int? DeletedBy_Id { get; set; }

        public List<PatientSurveyQuestionModel> PatientSurveyQuestions { get; set; }
        public PatientSurveyScheduleModel PatientSurveyScheduleModel { get; set; }
    }

    public class PatientSurveyOptionTextModel
    {
        public int PatientSurveyOptionTextId { get; set; }
        public int PatientSurveyOptionId { get; set; }
        public string Text { get; set; }
        public int LanguageId { get; set; }
        public string LanguageName { get; set; }
        public string LocaleCode { get; set; }
        public DateTime CreatedDateTime_UTC { get; set; }
        public int CreatedBy_Id { get; set; }
        public DateTime? DeletedDateTime_UTC { get; set; }
        public int? DeletedBy_Id { get; set; }
    }

    public class PatientSurveyOptionModel
    {
        public PatientSurveyOptionModel()
        {
            AlertSeverityLevelId = (int)AlertSeverityLevel.None;
        }

        public int PatientSurveyOptionId { get; set; }
        public int PatientSurveyQuestionId { get; set; }
        public string OptionName { get; set; }
        public int AlertSeverityLevelId { get; set; }
        public int? SurveyParameterTypeId { get; set; }
        public string SurveyParameterTypeName { get; set; }
        public int SortOrder { get; set; }
        public DateTime CreatedDateTime_UTC { get; set; }
        public int CreatedBy_Id { get; set; }
        public DateTime? DeletedDateTime_UTC { get; set; }
        public int? DeletedBy_Id { get; set; }
        public List<PatientSurveyOptionTextModel> PatientSurveyOptionTexts { get; set; }
        public PatientSurveyQuestionModel PatientSurveyQuestion { get; set; }
        public List<PatientSurveyQuestionModel> PatientSurveyQuestions { get; set; } //child questions
    }

    public class PatientSurveyQuestionTextModel
    {
        public int PatientSurveyQuestionTextId { get; set; }
        public int PatientSurveyQuestionId { get; set; }
        public string Text { get; set; }
        public int LanguageId { get; set; }
        public string LanguageName { get; set; }
        public string LocaleCode { get; set; }
        public DateTime CreatedDateTime_UTC { get; set; }
        public int CreatedBy_Id { get; set; }
        public DateTime? DeletedDateTime_UTC { get; set; }
        public int? DeletedBy_Id { get; set; }
    }

    public class PatientSurveyQuestionModel
    {
        public static void clone(PatientSurveyQuestionModel src, PatientSurveyQuestionModel tgt)
        {
            tgt.PatientSurveyQuestionId     =src.PatientSurveyQuestionId;
            tgt.SurveyQuestionTypeId        =src.SurveyQuestionTypeId;    
            tgt.SurveyQuestionTypeName      =src.SurveyQuestionTypeName;      
            tgt.PatientSurveyId             =src.PatientSurveyId;     
            tgt.SortOrder                   =src.SortOrder;            
            tgt.SurveyQuestionCategoryId    =src.SurveyQuestionCategoryId;   
            tgt.DisplayCondition            =src.DisplayCondition;   
            tgt.ParentPatientSurveyOptionId =src.ParentPatientSurveyOptionId;
            tgt.CreatedDateTime_UTC         =src.CreatedDateTime_UTC;
            tgt.CreatedBy_Id                =src.CreatedBy_Id;        
            //tgt.DateTimeDeletedDateTime_UTC =src.DateTimeDeletedDateTime_UTC;
            tgt.DeletedBy_Id                =src.DeletedBy_Id;         
            tgt.PatientSurvey               =src.PatientSurvey;              
            tgt.PatientSurveyOptions        =src.PatientSurveyOptions;       
            tgt.PatientSurveyQuestionTexts  =src.PatientSurveyQuestionTexts;
            tgt.ParentSurveyOption          =src.ParentSurveyOption;
        }
        public int PatientSurveyQuestionId { get; set; }
        public int SurveyQuestionTypeId { get; set; }
        public string SurveyQuestionTypeName { get; set; }
        public int? PatientSurveyId { get; set; }
        public int SortOrder { get; set; }
        public int SurveyQuestionCategoryId { get; set; }
        public string DisplayCondition { get; set; }
        public int ParentPatientSurveyOptionId { get; set; }
        public DateTime CreatedDateTime_UTC { get; set; }
        public int CreatedBy_Id { get; set; }
        public DateTime? DeletedDateTime_UTC { get; set; }
        public int? DeletedBy_Id { get; set; }

        //public AlertSeverityLevel MaxAlertSeverityLevel
        //{
        //    get
        //    {
        //        var retval = AlertSeverityLevel.None;

        //        foreach (PatientSurveyOptionModel patientSurveyOption in PatientSurveyOptions)
        //        {
        //            if (patientSurveyOption.AlertSeverityLevelId < (int) retval)
        //            {
        //                retval = (AlertSeverityLevel) patientSurveyOption.AlertSeverityLevelId;
        //            }
        //        }

        //        return retval;
        //    }
        //}

        public PatientSurveyModel PatientSurvey { get; set; }
        public List<PatientSurveyOptionModel> PatientSurveyOptions { get; set; }
        public List<PatientSurveyQuestionTextModel> PatientSurveyQuestionTexts { get; set; }
        public PatientSurveyOptionModel ParentSurveyOption { get; set; }

        public bool IsBiometricQuestion()
        {
            switch ((SurveyQuestionType)SurveyQuestionTypeId)
            {
                case SurveyQuestionType.BloodPressure:
                case SurveyQuestionType.BloodSugar:
                case SurveyQuestionType.PulseOx:
                case SurveyQuestionType.Weight:
                    {
                        return true;
                    }
            }
            return false;
        }
    }

    public class PatientSurveyScheduleModel
    {
        public PatientSurveyScheduleModel()
        {
            ActiveDays = new ActiveDaysOfWeek();
        }

        public int PatientSurveyScheduleId { get; set; }
        public int PatientSurveyId { get; set; }
        public bool IsOneTime { get; set; } //or weekly 
        public DateTime StartDate { get; set; } //calculated or absolute, nullable. time is ignored.     

        //calculation props
        public int? DaysAfterProgramStartDate { get; set; }
        public int? DaysBeforeProgramEndDate { get; set; }

        public ActiveDaysOfWeek ActiveDays { get; set; }

        //prompting 
        public TimeSpan SchedulePromptTimeOfDay { get; set; }
        public int StartMinutesBeforePrompt { get; set; }
        public int LateMinutesAfterPrompt { get; set; }
        public int EndMinutesAfterPrompt { get; set; }

        public DateTime CreatedDateTime_UTC { get; set; }
        public int CreatedBy_Id { get; set; }
        public DateTime? DeletedDateTime_UTC { get; set; }
        public int? DeletedBy_Id { get; set; }

        public PatientSurveyModel PatientSurvey { get; set; }

        public bool IsDaily()
        {
            {
                bool retVal = ActiveDays.IsMonday && ActiveDays.IsTuesday && ActiveDays.IsWednesday && ActiveDays.IsThursday &&
                              ActiveDays.IsFriday && ActiveDays.IsSaturday && ActiveDays.IsSunday;

                return retVal;
            }
        }
    }

    public enum AlertSeverityLevel
    {
        High = 1,
        Medium = 2,
        None = 3
    }

    public enum SurveyQuestionType
    {
        BloodSugar = 1,
        BloodPressure = 2,
        PulseOx = 3,
        Weight = 4,
        Number = 5,
        SingleSelection = 6,
        MultiSelection = 7
    }

    public class ActiveDaysOfWeek
    {
        public bool IsSunday { get; set; }
        public bool IsMonday { get; set; }
        public bool IsTuesday { get; set; }
        public bool IsWednesday { get; set; }
        public bool IsThursday { get; set; }
        public bool IsFriday { get; set; }
        public bool IsSaturday { get; set; }
    }
}