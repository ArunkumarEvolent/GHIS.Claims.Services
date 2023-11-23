using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
//using System.Text.Json.Serialization;
//using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using GHIS.Claims.Data.NPPES_API.Models;
using System.Net.Http.Headers;

namespace GHIS.Claims.Data.NPPES_API
{
    public class NPPESApiClient
    {
        private const string URL = "https://npiregistry.cms.hhs.gov/api/";
        private string urlParameters = "?enumeration_type=NPI-1&version=2.1";

        public NPPESResponse responseObject = new NPPESResponse();
        public string GetAPIResponse(string NPInumber)
        {
            _ = CallAPIAsync(NPInumber);
            if (responseObject != null)
            {
                if (responseObject.result_count == 1)
                {
                    return "1^" + responseObject.results[0].Basic.first_name +"^"+ responseObject.results[0].Basic.last_name;
                }
                else
                {
                    return "0^Invalid response. Possibility of multiple records in API response.";
                }
            }
            else
            {
                return "0^Call failed. No return object from API.";
            }
        }
        //static void Main(string[] args)
        public async Task CallAPIAsync(string NPInumber)
        {
            NPPESResponse nPPESResponse = new NPPESResponse();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync(urlParameters+"&number="+NPInumber).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var APIinstance = JsonConvert.DeserializeObject<NPPESResponse>(await response.Content.ReadAsStringAsync());
                responseObject = APIinstance;
            }
            else
            {
                string errorStr = "Error Code: " + (int)response.StatusCode + ". Error Reason: " + response.ReasonPhrase;
            }

            // Make any other calls using HttpClient here.

            // Dispose once all HttpClient calls are complete. This is not necessary if the containing object will be disposed of; for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
            client.Dispose();
        }
    }
}
