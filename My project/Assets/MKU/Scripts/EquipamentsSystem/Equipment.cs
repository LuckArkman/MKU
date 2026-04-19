using System;
using System.Collections.Generic;
using MKU.Scripts.CharacterSystem;
using MKU.Scripts.Enums;
using MKU.Scripts.Singletons;
using UnityEngine;

namespace MKU.Scripts.EquipamentsSystem
{
    public class Equipment : MonoBehaviour
    {
        // STATE
        Dictionary<EquipLocation, EquipableItem> equippedItems = new Dictionary<EquipLocation, EquipableItem>();
        

        // PUBLIC

        /// <summary>
        /// Broadcasts when the items in the slots are added/removed.
        /// </summary>
        public event Action equipmentUpdated;

        /// <summary>
        /// Return the item in the given equip location.
        /// </summary>
        public EquipableItem GetItemInSlot(EquipLocation equipLocation)
        {
            if (!equippedItems.ContainsKey(equipLocation))
            {
                return null;
            }

            return equippedItems[equipLocation];
        }

        /// <summary>
        /// Add an item to the given equip location. Do not attempt to equip to
        /// an incompatible slot.
        /// </summary>
        public void AddItem(EquipLocation slot, EquipableItem item)
        {
            Debug.Assert(item.GetAllowedEquipLocation() == slot);

            equippedItems[slot] = item;

            if (equipmentUpdated != null)
            {
                CharSettings._Instance.update = true;
                equipmentUpdated();
            }
        }

        /// <summary>
        /// Remove the item for the given slot.
        /// </summary>
        public void RemoveItem(EquipLocation slot)
        {
            equippedItems.Remove(slot);
            if (equipmentUpdated != null)
            {
                CharSettings._Instance.update = true;
                equipmentUpdated();
            }
        }
    }
}