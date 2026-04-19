using System.Collections.Generic;
using MKU.Scripts.Enums;
using UnityEngine;

namespace MKU.Scripts.ItemSystem
{
    public class Item : ScriptableObject
    {
        public string itemID = null;
        [Range(1, 100)]
        public int Level;
        public string itemHash = null;
        public string displayName = null;
        [TextArea]
        public string description = null;
        public Sprite icon = null;
        //public Pickup pickup = null;
        public bool stackable = false;
        public int price;        
        public bool shoulModifiers = false;
        public Dictionary<string, Item> itemLookupCache;
        public string ItemDateTime;
    }
}