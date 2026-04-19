using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using MKU.Scripts.CharacterSystem;
using MKU.Scripts.EquipamentsSystem;
using MKU.Scripts.ItemSystem;
using MKU.Scripts.Singletons;
using Newtonsoft.Json;
using UnityEngine;

namespace MKU.Scripts.IventorySystem
{
    public class UpdateInventory
    {
        public UpdateInventory(){}
        
        public async void OnUpdateInventory(CharController player)
        {
            EquipamentUI _equipamentUI = player.GetComponent<EquipamentUI>();
            Dictionary<string, Bag> _bag = new();
            Dictionary<string, EquipamentItem> _equipament = new();
            var itemDictionary = CharSettings._Instance._inventorySlots.Where(slot => slot.GetItem() != null);
            var dic = itemDictionary.GroupBy(slot => slot.GetItem().itemID)
                .ToDictionary(
                    group => group.Key, // A chave é o nome do item
                    group => new Bag(
                        group.Select(slot =>
                                new InventoryItem(slot.GetItem().level,slot.index,slot.GetNumber()) // Criando o InventoryItem com número e índice
                        ).ToList()  // Convertendo o resultado para uma lista
                    )
                );
            player.GetComponent<EquipamentUI>()._equipmentSlots.ForEach(e =>
            {
                if (e.icon.item != null)
                {
                    if (_bag.ContainsKey(e.icon.item.name))
                    {
                        _bag[e.icon.item.name].items.Add(new InventoryItem(e.Index,e.GetItem().level, e.GetNumber()));
                    }
                }
            });
            var inventory = new CharInventory(player.Id, dic);
            var eqip = _equipamentUI._equipmentSlots.Where(o => o.GetItem() != null);
            foreach (var item in eqip)
            {
                if (inventory._bag.ContainsKey(item.GetItem().itemID))
                {
                    inventory._bag[item.GetItem().itemID].items.Add(new InventoryItem(item.GetItem().level,item.Index, item.GetNumber()));
                }
                if (!inventory._bag.ContainsKey(item.GetItem().itemID))
                {
                    List<InventoryItem> newItems = new List<InventoryItem>();
                    newItems.Add(new InventoryItem(item.GetItem().level,item.Index, item.GetNumber()));
                    inventory._bag.TryAdd(item.GetItem().itemID, new Bag(newItems));
                }
            }
            string url = $"http://cursed.agencia4red.com:6050/Inventory/";
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (HttpClient client = new HttpClient(handler))
                {
                    try
                    {
                        string json = JsonConvert.SerializeObject(inventory);
                        Debug.Log($"{nameof(OnUpdateInventory)} >> {url}");
                        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                        HttpResponseMessage response = await client.PostAsync(url, content);
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Singleton.Instance._charController.OnUpdateStatus();
                        Debug.Log(responseBody);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error No Data: {ex.Message}");
                    }
                }
            }
        }
    }
}