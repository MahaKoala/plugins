using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SpotifySampler.Models;
using SpotifySampler.Network;

namespace SpotifySampler.Logic
{
    public sealed class SpotifyLogic
    {
        public List<TrackModel> Data;

        public async Task<List<TrackModel>> Search(string term)
        {
            var client = new SpotifyRequestHandler();
            client.DataReceivedHandler += client_DataReceivedHandler;
            await client.Search(term);
            return Data;
        }

        private void client_DataReceivedHandler(object sender, EventArgs e)
        {
            var response = (ResponseModel) e;
            dynamic responseJson = JsonConvert.DeserializeObject(response.Data);
            var jsonTracks = responseJson.tracks.items;
            var playlist = new List<TrackModel>();
            foreach (var track in jsonTracks)
            {
                var song = new TrackModel
                {
                    Name = track.name,
                    Artists = track.artists[0].name,
                    Url = track.preview_url,
                    CoverArt = track.album.images[0].url
                };
                playlist.Add(song);
            }
            Data = playlist;
        }
    }
}