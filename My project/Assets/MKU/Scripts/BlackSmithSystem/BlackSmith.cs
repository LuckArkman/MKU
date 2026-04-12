using System;
using System.Collections.Generic;
using MKU.Scripts.CharacterSystem;
using MKU.Scripts.Enums;
using MKU.Scripts.EquipamentsSystem;
using MKU.Scripts.ItemSystem;
using MKU.Scripts.Singletons;
using UnityEngine;

namespace MKU.Scripts.BlackSmithSystem
{
    public class BlackSmith : MonoBehaviour, ISerializationCallbackReceiver
    {
        [TextArea]
        public string Id;
        
        Dictionary<UpgradableItems, _Item> updatable = new Dictionary<UpgradableItems, _Item>();

        public event Action blacksmithUpdated;

        private void Start()
        {
            CharSettings._Instance._blackSmith = this;
            Singleton.Instance._blackSmith = this;
        }

        public _Item GetItemInSlot(UpgradableItems upgradable)
        {
            if (updatable.ContainsKey(upgradable))return updatable[upgradable];
            return null;
        }
        public void AddItem(UpgradableItems slot, _Item item)
        {
            Debug.Assert(item.GetAllowedUpgradableItems() == slot);

            updatable[slot] = item;

            if(blacksmithUpdated != null)
            {
                blacksmithUpdated();
            }
        }

        public void RemoveItems(UpgradableItems slot)
        {
            updatable.Remove(slot);
            if (blacksmithUpdated != null)
            {
                blacksmithUpdated();
            }
        }
        public void OnBeforeSerialize()
            => Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;

        public void OnAfterDeserialize(){}
    }
}