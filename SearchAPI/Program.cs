using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SAG.Uitpas.Helpers.Implementations;

namespace SearchAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // LINKS
            // https://documentatie.uitdatabank.be/
            // https://documentatie.uitdatabank.be/content/search_api/latest/search_api.html
            // https://documentatie.uitdatabank.be/content/authenticatie-autorisatie/latest/authenticatie-via-consumerrequest.html
            // https://documentatie.uitdatabank.be/content/authenticatie-autorisatie/latest/postman.html

            var key = "cbfa75c0f1426b0f6a5f5efcca6f5e90";
            var secret = "bbe83b15a54b6526767526d02ff207e2";
            
            // Token ophalen werk maar er na geen idee hoe te gebruiken //

            var oAuthService = new OAuthService(key, secret);

            var request = WebRequest.Create("https://test.uitid.be/uitid/rest/requestToken");

            request.Headers.Add("Authorization", oAuthService.GetAuthorizationHeader("https://test.uitid.be/uitid/rest/requestToken", "POST"));
            request.Method = "POST";

            var response = request.GetResponse();
            var responseStream = response.GetResponseStream();

            var oAuthToken = "";
            var oAuthTokenSecret = "";
            var auth = "";

            if (responseStream != null)
            {
                using (var test = new StreamReader(responseStream))
                {
                    var responseText = test.ReadToEnd();
                    var parameters = responseText.Split('&');

                    oAuthToken = parameters[0].Substring(12);
                    oAuthTokenSecret = parameters[1].Substring(19);
                    auth = "Bearer " + oAuthToken;
                }
            }

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", auth);

            var response2 = httpClient.GetAsync("https://test.uitid.be/uitid/rest/searchv2/search?q=city:Gistel");
            var resultString = response2.Result.Content.ReadAsStringAsync(); // => Forbidden

            // Poging om te doen wat in Postman wel werkt //

            var httpClient2 = new HttpClient();
            var authHeader = oAuthService.GetAuthorizationHeader("https://test.uitid.be/uitid/rest/searchv2/search?q=city:Gistel", "GET");
            
            httpClient2.DefaultRequestHeaders.Add("Authorization", authHeader);

            var response3 = httpClient.GetAsync("https://test.uitid.be/uitid/rest/searchv2/search?q=city:Gistel");
            var resultString3 = response2.Result.Content.ReadAsStringAsync(); // => Forbidden
        }
    }
}
