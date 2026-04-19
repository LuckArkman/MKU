using System.Collections.Generic;
using MKU.Scripts.Enums;
using MKU.Scripts.HelthSystem;
using UnityEngine;

namespace MKU.Scripts.ItemSystem
{
    public class _Item : ScriptableObject
    {
        public string itemID;
        public string itemToken;
        public string displayName;
        public ItemCategory itemCategory = ItemCategory.None;
        [Range(1, 100)]
        public int level = 1;
        public int max_level = 15;
        [TextArea]
        public string description;
        public Sprite icon;
        public GameObject pickup;
        public UpgradableItems upgradable = UpgradableItems.None;
        public EquipLocation allowedEquipLocation = EquipLocation.None;
        public bool dualhand, lefthand, righthand;
        public bool stackable;
        [Range(1, 1000)]
        public int price;
        
        
        public bool Upgradable;
        public bool shoulModifiers;
        public _Attributs attributes = new _Attributs();
        public List<Parcente> _parcentes = new ();
        static Dictionary<string, _Item> itemLookupCache;
        
        public void SetItem(string itemID, string itemToken, string displayName, ItemCategory itemCategory, int level, string description, Sprite icon, GameObject pickup, UpgradableItems upgradable, EquipLocation allowedEquipLocation, bool dualhand, bool lefthand, bool righthand, bool stackable, int price, bool Upgradable,List<Parcente> _parcentes)
        {
            this.itemID = itemID;
            this.itemToken = itemToken;
            this.displayName = displayName;
            this.itemCategory = itemCategory;
            this.level = level;
            this.description = description;
            this.icon = icon;
            this.pickup = pickup;
            this.upgradable = upgradable;
            this.allowedEquipLocation = allowedEquipLocation;
            this.dualhand = dualhand;
            this.lefthand = lefthand;
            this.righthand = righthand;
            this.stackable = stackable;
            this.price = price;
            this.Upgradable = Upgradable;
            this._parcentes = _parcentes;
        }

        public bool IsStackable()
            => stackable;

        public static _Item GetFromID(string itemID)
        {
            if (itemLookupCache == null)
            {
                itemLookupCache = new Dictionary<string, _Item>();
                var itemList = Resources.LoadAll<_Item>("");
                foreach (var item in itemList)
                {
                    if (itemLookupCache.ContainsKey(item.itemID))
                    {
                        Debug.LogError(string.Format("Looks like there's a duplicate UI.InventorySystem ID for objects: {0} and {1}", itemLookupCache[item.itemID], item));
                        continue;
                    }

                    itemLookupCache[item.itemID] = item;
                }
            }

            if (itemID == null || !itemLookupCache.ContainsKey(itemID)) return null;
            return itemLookupCache[itemID];
        }

        public Sprite GetIcon()
            => icon;

        public string GetDisplayName()
            => displayName;

        public ItemCategory GetCategory() => itemCategory;

        public int GetPrice()
            => price;
        public UpgradableItems GetAllowedUpgradableItems()
        => upgradable;
    }
}