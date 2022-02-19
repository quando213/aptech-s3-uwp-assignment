using Assignment.Entities;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Assignment.Services
{
    class UserService
    {
        private string apiBaseUrl = "https://music-i-like.herokuapp.com/api";
        private PasswordVault store = new PasswordVault();

        public async Task<bool> Register(User user)
        {
            var userJson = JsonConvert.SerializeObject(user);
            var http = new HttpClient();
            var httpContent = new StringContent(userJson, Encoding.UTF8, "application/json");
            var request = await http.PostAsync($"{apiBaseUrl}/v1/accounts", httpContent);
            if (request.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return true;
            } else
            {
                return false;
            }
        }

        public async Task<Credential> Login(LoginInformation loginInformation)
        {
            try
            {
                var accountJson = JsonConvert.SerializeObject(loginInformation);
                HttpClient httpClient = new HttpClient();
                var httpContent = new StringContent(accountJson, Encoding.UTF8, "application/json");
                var requestConnection = await httpClient.PostAsync($"{apiBaseUrl}/v1/accounts/authentication", httpContent);
                if (requestConnection.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var content = await requestConnection.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<Credential>(content);
                    store.Add(new PasswordCredential("token", "tongthimeomi", result.access_token));
                    return result;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return null;
        }

        public void Logout()
        {
            try
            {
                PasswordCredential credential = store.Retrieve("token", "tongthimeomi");
                store.Remove(credential);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(Pages.Login));
        }


        public async Task<User> GetProfile()
        {
            string token = GetToken();
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var requestConnection =
                    await httpClient.GetAsync($"{apiBaseUrl}/v1/accounts");
                if (requestConnection.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var content = await requestConnection.Content.ReadAsStringAsync();
                    var account = JsonConvert.DeserializeObject<User>(content);
                    return account;
                }
            }
            catch (Exception e)
            {
                Logout();
                Console.WriteLine(e.Message);
            }
            return null;
        }

        public string GetToken()
        {
            try
            {
                PasswordCredential credential = store.Retrieve("token", "tongthimeomi");
                var token = credential.Password;
                return token;
            }
            catch (Exception e)
            {
                Logout();
                Console.WriteLine(e.Message);
            }
            return null;
        }
    }
}
