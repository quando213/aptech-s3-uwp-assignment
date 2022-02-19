using Assignment.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace Assignment.Services
{
    class SongService
    {
        private string apiBaseUrl = "https://music-i-like.herokuapp.com/api";
        private PasswordVault store = new PasswordVault();
        UserService userService = new UserService();

        public async Task<bool> CreateSong(Song song)
        {
            string token = userService.GetToken();
            var jsonString = JsonConvert.SerializeObject(song);
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            HttpContent contentToSend = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync($"{apiBaseUrl}/v1/songs/mine", contentToSend);
            if (result.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<Song>> GetLatestSongs()
        {
            List<Song> result = new List<Song>();
            string token = userService.GetToken();
            try
            {
                HttpClient httpClient = new HttpClient();
                var requestConnection = await httpClient.GetAsync($"{apiBaseUrl}/v1/songs");
                if (requestConnection.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var content = await requestConnection.Content.ReadAsStringAsync();
                    result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Song>>(content);
                    return result;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return result;
        }


        public async Task<List<Song>> GetMySongs()
        {
            List<Song> result = new List<Song>();
            string token = userService.GetToken();
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var requestConnection = await httpClient.GetAsync($"{apiBaseUrl}/v1/songs/mine");
                if (requestConnection.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var content = await requestConnection.Content.ReadAsStringAsync();
                    result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Song>>(content);
                    return result;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return result;
        }
    }
}
