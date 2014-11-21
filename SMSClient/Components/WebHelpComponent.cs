using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace SMSClient.Components
{
    public class WebHelpComponent
    {
        public static string BuildUrl(string pageName, Dictionary<string, string> pageParameters)
        {
            var url = new StringBuilder();

            url.Append(ConfigurationManager.AppSettings["BASE_URL"]);
            url.Append(pageName + ".aspx");

            if (pageParameters != null && pageParameters.Count > 0)
            {
                bool firstRun = true;

                foreach (var kvp in pageParameters)
                {
                    if (firstRun)
                    {
                        url.Append("?");
                        firstRun = false;
                    }
                    else
                    {
                        url.Append("&");
                    }
                    url.Append(kvp.Key + "=" + kvp.Value);
                }
            }

            return url.ToString();
        }

        public static string BuildUrl(string pageName)
        {
            var url = new StringBuilder();

            url.Append(ConfigurationManager.AppSettings["BASE_URL"]);
            url.Append(pageName + ".aspx");

            return url.ToString();
        }
    }
}