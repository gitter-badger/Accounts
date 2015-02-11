using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Accounts.Models;
using Newtonsoft.Json;

namespace Accounts.Clients
{
    public interface ICoreClient
    {
        Response<object> UpdateContactDetails(string id, ContactDetails contactDetails);
        Response<ContactDetailsResponse> GetContactDetails(string id);
        Response<object> UpdatePassword(string id, PasswordRequest passwordRequest);
    }

    public class CoreClient : ICoreClient
    {
        private HttpClient _client;

        public CoreClient()
        {
            _client = new HttpClient { BaseAddress = new Uri(EndPoints.IdentityApiAddress) };
        }

        public Response<object> UpdateContactDetails(string id, ContactDetails contactDetails)
        {

            var contactString = JsonConvert.SerializeObject(contactDetails,
                Formatting.None,
                new JsonSerializerSettings()
                {
                    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                });

            var content = new StringContent(contactString, Encoding.UTF8, "application/json");

            // call sync
            var response = _client.PatchAsync("/users/" + id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return new Response<object>{ Errored = false };
            }
            return new Response<object> { Errored = true, ErrorMessage = "Error Occurred"};
        }

        public Response<ContactDetailsResponse> GetContactDetails(string id)
        {
            var response = _client.GetAsync("/users/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                var obj = JsonConvert.DeserializeObject<ContactDetailsResponse>(response.Content.ReadAsStringAsync().Result);
                return new Response<ContactDetailsResponse> { Body = obj };
            }
            return new Response<ContactDetailsResponse> { Errored = true };
        }

        public Response<object> UpdatePassword(string id, PasswordRequest passwordRequest)
        {
            var passwordRequestString = JsonConvert.SerializeObject(passwordRequest,
                Formatting.None,
                new JsonSerializerSettings()
                {
                    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                });

            var content = new StringContent(passwordRequestString, Encoding.UTF8, "application/json");

            // call sync
            var response = _client.PostAsync("/credentials/" + id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return new Response<object>();
            }
            return new Response<object>{ Errored = true };
        }
    }

    public static class HttpClientExtenstions
    {
        public static Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent content)
        {
            var method = new HttpMethod("PATCH");

            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = content
            };

            return client.SendAsync(request);
        }
    }
}