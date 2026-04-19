using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MKU.Scripts.Models;
using MKU.Scripts.Singletons;
using Newtonsoft.Json;
using UnityEngine;

namespace MKU.Scripts.FinanceSystem
{
    public class FinanceManager : GenericManager
    {
        public FinanceManager(){}
        
        public async Task<CharWallet?> GetWallet(string Id)
        {
            
            CharWallet _charWallet = null;
            //string url = $"http://MKU.Scripts.ddns.net:5100/Quests/{Id}";
            string url = $"{Singleton.Instance._financeWallet}{Id}";
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
                        Debug.Log($"{nameof(GetWallet)} >> {responseBody}");
                        _charWallet  = JsonConvert.DeserializeObject<CharWallet>(responseBody);
                        return _charWallet;

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error No Data: {ex.Message}");
                    }
                }
            }
            return _charWallet;
        }
        
        public async Task<string?> PostCsts(Message msg)
        {
            //string url = $"http://MKU.Scripts.ddns.net:5100/Quests/{Id}";
            string url = $"{Singleton.Instance._financeCsts}";
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        string json = JsonConvert.SerializeObject(msg);
                        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                        HttpResponseMessage response = await client.PostAsync(url, content);
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Debug.Log($"{nameof(GetWallet)} >> {responseBody}");
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