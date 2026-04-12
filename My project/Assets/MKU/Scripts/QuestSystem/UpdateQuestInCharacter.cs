using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace MKU.Scripts.Tasks
{
    public class UpdateQuestInCharacter
    {
        public UpdateQuestInCharacter(){}
        
        public async Task<string> UpdateQuest(CharQuest _charQuest)
        {
            string url = $"http://MKU.Scripts.ddns.net:5100/UpdateQuests";
            //string url = $"http://localhost:5100/UpdateQuests";
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        string json = JsonConvert.SerializeObject(_charQuest);
                        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                        HttpResponseMessage response = await client.PostAsync(url, content);
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        return responseBody;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error No Data: {ex.Message}");
                    }
                }
            }
            return "";
        }
    }
}