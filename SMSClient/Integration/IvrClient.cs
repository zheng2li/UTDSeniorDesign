using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using NLog;
using RestSharp;
using RestSharp.Deserializers;
using SMSClient.Models;

namespace SMSClient.Integration
{
    public class IvrClient //interface with Vivify API
    {
        private const string BaseUrl = "https://demoutdesign.dev.vivifyhealth.com";

        private const string UrlCreateSession = "/api/session/{id}";
        private const string UrlGetPatient = "/api/Patient/{PatientId}";
        private const string UrlGetAllPatients = "/api/Patient";
        private const string UrlGetPatientSurveys = "/api/PatientSurvey";
        private const string UrlGetPatientSurveyQuestion = "/api/PatientSurveyQuestion";
        private const string UrlPostPatientResponse = "/api/PatientResponse?AuthToken={AUTHTOKEN}";

        private const string ClientId = "UTDSMSClient";
        private const string ApiKey = "c54cccac-0d78-4010-bbb8-f3c19c813b0a";
        private const string ApiSecret = "e4dd052f-fb45-4a95-afc1-183b293a5cf1";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly string _url;

        public IvrClient(string url = BaseUrl)
        {
            if (url.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                _url = url;
            }
            else
            {
                _url = "https://" + url;
            }

            // ignore SSL certificate errors.
            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, certificate, chain, sslPolicyErrors) => true;
        }

        public SessionTokenModel CreateSession()
        {
            var request = new RestRequest(UrlCreateSession, Method.POST);
            request.AddUrlSegment("id", ClientId);
            request.AddParameter("ApplicationKey", ApiKey);
            request.AddParameter("SecretKey", ApiSecret);
            IRestResponse<SessionTokenModel> response = Execute<SessionTokenModel>(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                ErrorResponse error = deserializerError(response);

                if (response.StatusCode != HttpStatusCode.NotFound)
                {
                    Log.Error("Error creating a session. Status Code: {0}; Message: {1} ", response.StatusCode,
                        error.Message);
                }
                else
                {
                    Log.Debug("Error creating a session. Status Code: {0}; Message: {1} ", response.StatusCode,
                        error.Message);
                }

                throw new HttpResponseException {StatusCode = response.StatusCode};
            }
            return response.Data;
        }

        public PatientModel GetPatient(string sessionToken, int patientId)
        {
            var request = new RestRequest(UrlGetPatient, Method.GET);
            request.AddUrlSegment("PatientId", patientId.ToString(CultureInfo.InvariantCulture));
            request.AddParameter("AUTHTOKEN", sessionToken);
            IRestResponse<PatientModel> response = Execute<PatientModel>(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                ErrorResponse error = deserializerError(response);
                Log.Error("Error fetching stats. Status Code: {0}; Message: {1} ", response.StatusCode, error.Message);
                throw new HttpResponseException {StatusCode = response.StatusCode};
            }
            return response.Data;
        }

        public List<PatientModel> GetPatientsByPhoneNumber(string sessionToken, string phoneNumber)
        {
            var request = new RestRequest(UrlGetAllPatients, Method.GET);
            request.AddParameter("Phone", phoneNumber);
            request.AddParameter("AUTHTOKEN", sessionToken);
            IRestResponse<List<PatientModel>> response = Execute<List<PatientModel>>(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                ErrorResponse error = deserializerError(response);
                Log.Error("Error fetching stats. Status Code: {0}; Message: {1} ", response.StatusCode, error.Message);
                throw new HttpResponseException {StatusCode = response.StatusCode};
            }
            return response.Data;
        }

        public List<PatientSurveyModel> GetPatientSurveys(string sessionToken, int patientId)
        {
            var request = new RestRequest(UrlGetPatientSurveys, Method.GET);
            request.AddParameter("PatientId", patientId);
            request.AddParameter("AUTHTOKEN", sessionToken);
            IRestResponse<List<PatientSurveyModel>> response = Execute<List<PatientSurveyModel>>(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                ErrorResponse error = deserializerError(response);
                Log.Error("Error fetching stats. Status Code: {0}; Message: {1} ", response.StatusCode, error.Message);
                throw new HttpResponseException {StatusCode = response.StatusCode};
            }
            return response.Data;
        }

        public PatientSurveyQuestionModel GetPatientSurveyQuestion(string sessionToken, int patientSurveyQuestionId)
        {
            var request = new RestRequest(UrlGetPatientSurveyQuestion, Method.GET);
            request.AddParameter("PatientSurveyQuestionId", patientSurveyQuestionId);
            request.AddParameter("AUTHTOKEN", sessionToken);
            IRestResponse<PatientSurveyQuestionModel> response = Execute<PatientSurveyQuestionModel>(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                ErrorResponse error = deserializerError(response);
                Log.Error("Error fetching stats. Status Code: {0}; Message: {1} ", response.StatusCode, error.Message);
                throw new HttpResponseException {StatusCode = response.StatusCode};
            }
            return response.Data;
        }

        public PatientResponseApiPostModel PostPatientResponse(string sessionToken,
            PatientResponseApiPostModel patientResponse)
        {
            var request = new RestRequest(UrlPostPatientResponse, Method.POST) {RequestFormat = DataFormat.Json};
            request.AddUrlSegment("AUTHTOKEN", sessionToken);
            request.AddBody(new List<PatientResponseApiPostModel> {patientResponse});

            IRestResponse<List<PatientResponseApiPostModel>> response =
                Execute<List<PatientResponseApiPostModel>>(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                ErrorResponse error = deserializerError(response);
                Log.Error("Error fetching stats. Status Code: {0}; Message: {1} ", response.StatusCode, error.Message);
                throw new HttpResponseException {StatusCode = response.StatusCode};
            }
            return response.Data.FirstOrDefault();
        }

        private ErrorResponse deserializerError(IRestResponse response)
        {
            var retVal = new ErrorResponse();
            if (response != null && !String.IsNullOrEmpty(response.Content))
            {
                IDeserializer deserializer = new JsonDeserializer();
                try
                {
                    retVal = deserializer.Deserialize<ErrorResponse>(response);
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                }
            }
            return retVal;
        }

        private IRestResponse<T> Execute<T>(RestRequest request) where T : new()
        {
            return Execute<T>(request, _url);
        }

        private IRestResponse<T> Execute<T>(RestRequest request, string baseUrl) where T : new()
        {
            var client = new RestClient {BaseUrl = baseUrl};
            //This shit may be killing my post request.
            //request.AddParameter("data_type", "JSON"); 

            IRestResponse<T> response = null;
            try
            {
                logRequest(client, request);
                response = client.Execute<T>(request);
                logResponse(response);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return response;
        }

        private void logRequest(RestClient client, IRestRequest request)
        {
            Log.Debug("URI: {0}", client.BuildUri(request).AbsoluteUri);
            if (request.Parameters != null && request.Parameters.Count > 0)
            {
                var sb = new StringBuilder();
                foreach (Parameter param in request.Parameters)
                {
                    sb.Append(string.Format("{0}={1},", param.Name, param.Value));
                }
                Log.Debug(sb.ToString());
            }
        }

        private void logResponse(IRestResponse response)
        {
            Log.Debug("RESPONSE STATUS: [{0}] {1} {2}", response.ResponseStatus, response.StatusCode,
                response.StatusDescription);
            if (!string.IsNullOrEmpty(response.Content))
            {
                Log.Debug("RESPONSE: {0}", response.Content.Replace("\n", "").Replace("\r", ""));
            }
            if (response.ErrorMessage != null)
            {
                Log.Error(response.ErrorMessage);
            }
        }
    }
}