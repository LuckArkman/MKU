using System;
using System.Collections.Generic;
using MKU.Scripts.CharacterSystem;
using MKU.Scripts.CraftingSystem;
using MKU.Scripts.ItemSystem;
using MKU.Scripts.Singletons;
using UnityEngine;

namespace MKU.Scripts.CookingSystem
{
    public class Cooking : MonoBehaviour, ISerializationCallbackReceiver
    {
        [TextArea]
        public string Id;
        [Tooltip("Allowed size")]
        public int inventorySize;

        // STATE
        public InventorySlot[] slots;

        public struct InventorySlot
        {
            public _Item item;
            public int number;
        }

        public event Action craftingUpdated;

        private void Start()
        {
            Singleton.Instance._cooking = this;
        }

        public static Cooking GetPlayerInventory(Transform player)
        => CharSettings._Instance._charController.GetComponent<Cooking>();


        public bool HasSpaceFor(_Item item)
        {
            return FindSlot(item) >= 0;
        }


        public bool HasSpaceFrom(IEnumerable<_Item> items)
        {
            int freeSlots = FreeSlots();
            List<_Item> stackedItems = new List<_Item>();
            foreach(var item in items)
            {
                if(item.IsStackable())
                {
                    if(HasItem(item)) continue;
                    if (stackedItems.Contains(item)) continue;
                    stackedItems.Add(item);
                }

                if (freeSlots <= 0) return false;
                freeSlots--;
            }
            return freeSlots <= FreeSlots();
        }

        public int FreeSlots()
        {
            int count = 0;
            foreach(InventorySlot item in slots)
            {
                if(item.number == 0)
                {
                    count++;
                }
            }
            return count;
        }

        public int GetSize()
        {
            return slots.Length;
        }

        public bool AddToFirstEmptySlot(_Item item, int number)
        {
            int i = FindSlot(item);

            if (i < 0)
            {
                return false;
            }

            slots[i].item = item;
            slots[i].number += number;
            if (craftingUpdated != null)
            {
                craftingUpdated();
            }
            return true;
        }

        public bool HasItem(_Item item)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (object.ReferenceEquals(slots[i].item, item))
                {
                    return true;
                }
            }
            return false;
        }

        public _Item GetItemInSlot(int slot)
        {
            return slots[slot].item;
        }

        public int GetNumberInSlot(int slot)
        {
            return slots[slot].number;
        }

        public void RemoveFromSlot(int slot, int number)
        {
            slots[slot].number -= number;
            if (slots[slot].number <= 0)
            {
                slots[slot].number = 0;
                slots[slot].item = null;
            }
            if (craftingUpdated != null)
            {
                craftingUpdated();
            }
        }

        public bool AddItemToSlot(int slot, _Item item, int number)
        {
            if (slots[slot].item != null)
            {
                return AddToFirstEmptySlot(item, number); ;
            }

            var i = FindStack(item);
            if (i >= 0)
            {
                slot = i;
            }

            slots[slot].item = item;
            slots[slot].number += number;
            if (craftingUpdated != null)
            {
                craftingUpdated();
            }
            return true;
        }

        // PRIVATE

        private void Awake()
        {
            slots = new InventorySlot[inventorySize];
        }

        /// <summary>
        /// Find a slot that can accomodate the given item.
        /// </summary>
        /// <returns>-1 if no slot is found.</returns>
        private int FindSlot(_Item item)
        {
            int i = FindStack(item);
            if (i < 0)
            {
                i = FindEmptySlot();
            }
            return i;
        }

        /// <summary>
        /// Find an empty slot.
        /// </summary>
        /// <returns>-1 if all slots are full.</returns>
        private int FindEmptySlot()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item == null)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Find an existing stack of this item type.
        /// </summary>
        /// <returns>-1 if no stack exists or if the item is not stackable.</returns>
        private int FindStack(_Item item)
        {
            if (!item.IsStackable())
            {
                return -1;
            }

            for (int i = 0; i < slots.Length; i++)
            {
                if (object.ReferenceEquals(slots[i].item, item))
                {
                    return i;
                }
            }
            return -1;
        }

        public bool? Evaluate(string predicate, string[] parameters)
        {
            switch (predicate)
            {
                case "HasInventoryItem":
                    return HasItem(_Item.GetFromID(parameters[0]));
            }

            return null;
        }

        [System.Serializable]
        private struct InventorySlotRecord
        {
            public string itemID;
            public int number;
        }

        public int GetSlotWithItem(_Item item, int requiredQuantity)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                // Verifica se o slot contém o item e se tem quantidade suficiente.
                if (slots[i].item == item && slots[i].number >= requiredQuantity)
                {
                    return i; // Retorna o índice do slot.
                }
            }

            return -1; // Retorna -1 se o item não for encontrado.
        }
        public void OnBeforeSerialize()
            => Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;

        public void OnAfterDeserialize(){}
    }
}