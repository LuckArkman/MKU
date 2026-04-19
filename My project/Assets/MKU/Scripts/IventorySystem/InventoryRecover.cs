using System;
using System.Net.Http;
using System.Threading.Tasks;
using MKU.Scripts.CharacterSystem;
using MKU.Scripts.ConsumablesSystem;
using MKU.Scripts.Enums;
using MKU.Scripts.EquipamentsSystem;
using MKU.Scripts.ItemSystem;
using MKU.Scripts.Singletons;
using MKU.Scripts.UsablesSystem;
using Newtonsoft.Json;
using UnityEngine;

namespace MKU.Scripts.IventorySystem
{
    public class InventoryRecover
    {
        private CharController _charController;
        private Inventory _inventory;

        public InventoryRecover()
        {
        }

        public async Task OnInvetoryRegen(CharController charController)
        {
            Debug.Log($"{nameof(OnInvetoryRegen)} >> {DateTime.Now}");
            string url = $"http://cursed.agencia4red.com:6050/Inventory/{charController.Id}";
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
                        Debug.Log($"{nameof(OnInvetoryRegen)} >> {responseBody}");
                        CharSettings._Instance.inventory = JsonConvert.DeserializeObject<CharInventory>(responseBody);
                        if (CharSettings._Instance.inventory != null)
                        {
                            var Inventory = Singleton.Instance._inventory;
                            _inventory = Inventory;
                            foreach (var key in CharSettings._Instance.inventory._bag)
                            {
                                key.Value.items.ForEach(i =>
                                {
                                    if (i.position <= 24)
                                    {
                                        var container = Resources.Load("ItemContainer") as ItemContainer;
                                        var item = container.items.Find(x => x.itemID == key.Key);
                                        if (item.itemCategory == ItemCategory.None)
                                        {
                                            if (item.stackable)
                                            {
                                                while (i.quantity > 99)
                                                {
                                                    i.quantity -= 99;
                                                    var obj = ScriptableObject.CreateInstance<_Item>();
                                                    obj.SetItem(item.itemID, item.itemToken, item.displayName,
                                                        item.itemCategory, i.level, item.description,
                                                        item.icon, item.pickup, item.upgradable,
                                                        item.allowedEquipLocation, item.dualhand, item.lefthand,
                                                        item.righthand, item.stackable,
                                                        item.price, item.Upgradable, item._parcentes);
                                                    Inventory.AddItemToSlot(i.position, obj, i.quantity);
                                                }

                                                var obj2 = ScriptableObject.CreateInstance<_Item>();
                                                obj2.SetItem(item.itemID, item.itemToken, item.displayName,
                                                    item.itemCategory, i.level, item.description,
                                                    item.icon, item.pickup, item.upgradable, item.allowedEquipLocation,
                                                    item.dualhand, item.lefthand, item.righthand, item.stackable,
                                                    item.price, item.Upgradable, item._parcentes);
                                                Inventory.AddItemToSlot(i.position, obj2, i.quantity);
                                            }
                                        }

                                        if (item.itemCategory == ItemCategory.Consumable)
                                        {
                                            if (item.stackable)
                                            {
                                                while (i.quantity > 99)
                                                {
                                                    i.quantity -= 99;
                                                    var obj = ScriptableObject.CreateInstance<Consumable>();
                                                    obj.SetItem(item.itemID, item.itemToken, item.displayName,
                                                        item.itemCategory, i.level, item.description,
                                                        item.icon, item.pickup, item.upgradable,
                                                        item.allowedEquipLocation, item.dualhand, item.lefthand,
                                                        item.righthand, item.stackable,
                                                        item.price, item.Upgradable, item._parcentes);
                                                    Inventory.AddItemToSlot(i.position, obj, i.quantity);
                                                }

                                                var obj2 = ScriptableObject.CreateInstance<Consumable>();
                                                obj2.SetItem(item.itemID, item.itemToken, item.displayName,
                                                    item.itemCategory, i.level, item.description,
                                                    item.icon, item.pickup, item.upgradable, item.allowedEquipLocation,
                                                    item.dualhand, item.lefthand, item.righthand, item.stackable,
                                                    item.price, item.Upgradable, item._parcentes);
                                                Inventory.AddItemToSlot(i.position, obj2, i.quantity);
                                            }
                                        }

                                        if (item.itemCategory == ItemCategory.Equipable)
                                        {
                                            var obj = ScriptableObject.CreateInstance<EquipableItem>();
                                            obj.SetEquipableItem(item.itemID, item.itemToken, item.displayName,
                                                item.itemCategory, i.level, item.description,
                                                item.icon, item.pickup, item.upgradable, item.allowedEquipLocation,
                                                item.dualhand, item.lefthand, item.righthand, item.stackable,
                                                item.price, item.Upgradable, item._parcentes);
                                            Inventory.AddItemToSlot(i.position, obj, i.quantity);
                                        }

                                        if (item.itemCategory == ItemCategory.Usable)
                                        {
                                            if (item.stackable)
                                            {
                                                while (i.quantity > 99)
                                                {
                                                    i.quantity -= 99;
                                                    var obj = ScriptableObject.CreateInstance<Usable>();
                                                    obj.SetItem(item.itemID, item.itemToken, item.displayName,
                                                        item.itemCategory, i.level, item.description,
                                                        item.icon, item.pickup, item.upgradable,
                                                        item.allowedEquipLocation, item.dualhand, item.lefthand,
                                                        item.righthand, item.stackable,
                                                        item.price, item.Upgradable, item._parcentes);
                                                    Inventory.AddItemToSlot(i.position, obj, i.quantity);
                                                }

                                                var obj2 = ScriptableObject.CreateInstance<Usable>();
                                                obj2.SetItem(item.itemID, item.itemToken, item.displayName,
                                                    item.itemCategory, i.level, item.description,
                                                    item.icon, item.pickup, item.upgradable, item.allowedEquipLocation,
                                                    item.dualhand, item.lefthand, item.righthand, item.stackable,
                                                    item.price, item.Upgradable, item._parcentes);
                                                Inventory.AddItemToSlot(i.position, obj2, i.quantity);
                                            }
                                        }
                                    }

                                    if (i.position > 24)
                                    {
                                        EquipamentUI _equipment = Singleton.Instance._equipment;
                                        Debug.Log($"{nameof(OnInvetoryRegen)} >> {i.position}");
                                        _equipment._equipmentSlots.ForEach(e =>
                                        {
                                            if (e.Index == i.position)
                                            {
                                                var container = Resources.Load("ItemContainer") as ItemContainer;
                                                var item = container.items.Find(x => x.itemID == key.Key);
                                                var obj = ScriptableObject.CreateInstance<EquipableItem>();
                                                obj.SetEquipableItem(item.itemID, item.itemToken, item.displayName,
                                                    item.itemCategory, i.level, item.description,
                                                    item.icon, item.pickup, item.upgradable, item.allowedEquipLocation,
                                                    item.dualhand, item.lefthand, item.righthand, item.stackable,
                                                    item.price, item.Upgradable, item._parcentes);
                                                Debug.Log($"{nameof(OnInvetoryRegen)} >> equipament >> {obj == null}");
                                                e.AddItems(obj, i.quantity);
                                            }
                                        });
                                        _charController.OnUpdateAtributtes();
                                    }
                                });
                            }
                        }
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