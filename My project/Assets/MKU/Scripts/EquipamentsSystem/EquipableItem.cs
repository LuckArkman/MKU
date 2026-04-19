using System.Collections.Generic;
using MKU.Scripts.Enums;
using MKU.Scripts.HelthSystem;
using MKU.Scripts.ItemSystem;
using UnityEngine;

namespace MKU.Scripts.EquipamentsSystem
{
    public class EquipableItem: _Item
    {
        public GameObject model;
        public Transform position = null;
        [Range(1, 1000)]
        public int min = 1;
        [Range(1, 1000)]
        public int max = 1;
        public bool shoulModifiers;
        public _Attributs attributes = new _Attributs();
        
        [Header("Set System")]
        public ItemSetData itemSet;
        // PUBLIC

        public EquipLocation GetAllowedEquipLocation()
        => allowedEquipLocation;

        public void SetEquipableItem(string itemID, string itemToken, string displayName, ItemCategory itemCategory, int level, string description, Sprite icon, GameObject pickup, UpgradableItems upgradable, EquipLocation allowedEquipLocation, bool dualhand, bool lefthand, bool righthand, bool stackable, int price, bool Upgradable, List<Parcente> _parcentes)
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
    }
}