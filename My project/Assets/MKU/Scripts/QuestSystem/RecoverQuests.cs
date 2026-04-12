using System;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;

namespace MKU.Scripts.Tasks
{
    public class RecoverQuests
    {
        public RecoverQuests(){}

        [CanBeNull]
        public async Task<CharQuest> RecoverQuest(string id)
        {
            CharQuest _charQuest = null;
            string url = $"http://MKU.Scripts.ddns.net:5100/Quests/{id}";
            //string url = $"http://localhost:5100/Quests/{id}";
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(url);
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        _charQuest  = JsonConvert.DeserializeObject<CharQuest>(responseBody);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error No Data: {ex.Message}");
                    }
                }
            }
            return _charQuest;
        }
    }
}