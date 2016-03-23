using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using SpotifySampler.Models;

namespace SpotifySampler.Network
{
    public sealed class SpotifyRequestHandler
    {
        public string ApiUrl { get; } = "https://api.spotify.com/v1/search/";
        //Need to create event Handler
        public event EventHandler DataReceivedHandler;

        public async Task Search(string term)
        {
            var target = $"{ApiUrl}?q={term}&type=track";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync(target);
                if (response.IsSuccessStatusCode && DataReceivedHandler != null)
                {
                    var responseBodyAsText = response.Content.ReadAsStringAsync().Result;

                    DataReceivedHandler(this, new ResponseModel {Data = responseBodyAsText});
                }
            }
        }
    }
}