using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace MKU.Scripts.Tasks
{
    public class InsertQuestInCharacter
    {
        public InsertQuestInCharacter(){}
        
        public async Task<string> InsertQuest(CharQuest _charQuest)
        {
            string url = $"http://MKU.Scripts.ddns.net:5100/InsertQuests";
            //string url = $"http://localhost:5100/InsertQuests";
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        string json = JsonConvert.SerializeObject(_charQuest);
                        Debug.Log(json);
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