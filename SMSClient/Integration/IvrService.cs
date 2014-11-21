using System.Collections.Generic;
using SMSClient.Models;

namespace SMSClient.Integration
{
    public class IvrService
    {
        public PatientModel GetPatient(int patientId)
        {
            PatientModel retVal;

            var client = new IvrClient();
            SessionTokenModel sessionToken = client.CreateSession();

            retVal = client.GetPatient(sessionToken.AuthenticationToken, patientId);

            return retVal;
        }

        public List<PatientModel> GetPatientsByPhoneNumber(string phoneNumber)
        {
            List<PatientModel> retVal;

            var client = new IvrClient();
            SessionTokenModel sessionToken = client.CreateSession();

            retVal = client.GetPatientsByPhoneNumber(sessionToken.AuthenticationToken, phoneNumber);

            return retVal;
        }

        public IEnumerable<PatientSurveyModel> GetPatientSurveys(int patientId)
        {
            List<PatientSurveyModel> retVal;

            var client = new IvrClient();
            SessionTokenModel sessionToken = client.CreateSession();

            retVal = client.GetPatientSurveys(sessionToken.AuthenticationToken, patientId);

            return retVal;
        }

        public PatientSurveyQuestionModel GetPatientSurveyQuestion(int patientSurveyQuestionId)
        {
            PatientSurveyQuestionModel retVal;

            var client = new IvrClient();
            SessionTokenModel sessionToken = client.CreateSession();

            retVal = client.GetPatientSurveyQuestion(sessionToken.AuthenticationToken, patientSurveyQuestionId);

            return retVal;
        }

        public PatientResponseApiPostModel PostPatientResponse(PatientResponseApiPostModel patientResponse)
        {
            PatientResponseApiPostModel retVal;
            var client = new IvrClient();
            SessionTokenModel sessionToken = client.CreateSession();

            retVal = client.PostPatientResponse(sessionToken.AuthenticationToken, patientResponse);

            return retVal;
        }
    }
}