using System;
using System.Collections.Generic;
using MKU.Scripts.CharacterSystem;
using MKU.Scripts.Interface;
using MKU.Scripts.ItemSystem;
using MKU.Scripts.Singletons;
using UnityEngine;

namespace MKU.Scripts.IventorySystem
{
    public class Inventory : MonoBehaviour, IPredicateEvaluator
    {
        [Tooltip("Allowed size")] public int inventorySize = 25;

        // STATE
        public List<_Slot> slots = new ();
        
        void Awake()
        {
            for (int i = 0; i < inventorySize; i++)
            {
                slots.Add(new _Slot());
            }
        }

        private void Start()
        {
            Singleton.Instance._inventory = this;
        }

        public event Action inventoryUpdated;

        public static Inventory GetPlayerInventory(Transform player)
            => player.GetComponent<Inventory>();


        public bool HasSpaceFor(_Item item)
        {
            return FindSlot(item) >= 0;
        }


        public bool HasSpaceFrom(IEnumerable<_Item> items)
        {
            int freeSlots = FreeSlots();
            List<_Item> stackedItems = new List<_Item>();
            foreach (var item in items)
            {
                if (item.IsStackable())
                {
                    if (HasItem(item)) continue;
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
            foreach (_Slot item in slots)
            {
                if (item.number == 0)
                {
                    count++;
                }
            }

            return count;
        }

        public int GetSize()
        {
            return slots.Count;
        }

        public bool AddToFirstEmptySlot(_Item item, int number)
        {
            Debug.Log($"{nameof(AddToFirstEmptySlot)} >> {item == null} >> {number}");
            int remaining = number;
            
            if (item.IsStackable())
            {
                for (int i = 0; i < slots.Count; i++)
                {
                    if (slots[i].item != null && slots[i].item.itemID == item.itemID)
                    {
                        int availableSpace = 99 - slots[i].number;
                        int toAdd = Math.Min(remaining, availableSpace);

                        slots[i].number += toAdd;
                        remaining -= toAdd;

                        if (remaining <= 0)
                        {
                            CharSettings._Instance.update = true;
                            inventoryUpdated?.Invoke();
                            return true;
                        }
                    }
                }
            }

            // Se o item não for empilhável ou ainda houver unidades restantes, coloca em slots vazios
            for (int i = 0; i < slots.Count && remaining > 0; i++)
            {
                if (slots[i].item == null)
                {
                    Debug.Log($"{nameof(AddToFirstEmptySlot)} >> {i}");
                    int toAdd = item.IsStackable() ? Math.Min(remaining, 99) : 1;
                    slots[i] = new _Slot(item, toAdd);
                    remaining -= toAdd;
                }
            }
            //CharSettings._Instance.update = true;
            inventoryUpdated?.Invoke();
            return true;
        }

        public _Slot? FindItemByID(string itemID)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].item != null && slots[i].item.itemID == itemID)
                {
                    return slots[i]; // Retorna o slot encontrado
                }
            }
            return null; // Retorna null se não encontrar o item
        }

        public bool HasItem(_Item item)
        {
            for (int i = 0; i < slots.Count; i++)
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

            if (inventoryUpdated != null)
            {
                inventoryUpdated();
            }
            CharSettings._Instance.update = true;
        }

        public bool AddItemToSlot(int slot, _Item item, int number)
        {
            if (slot < 0 || slot >= slots.Count)
                return false;

            // Se o slot já contém um item
            if (slots[slot].item != null)
            {
                // Se for o mesmo item ID, verificar se pode ser empilhado
                if (slots[slot].item.itemID == item.itemID && item.IsStackable())
                {
                    slots[slot].number += number;
                    inventoryUpdated?.Invoke();
                    return true;
                }
                else
                {
                    // O item não é empilhável, então adiciona a um slot vazio
                    return AddToFirstEmptySlot(item, number);
                }
                
            }

            // Se o slot estiver vazio, colocar o item ali
            slots[slot] = new _Slot { item = item, number = number };
            CharSettings._Instance.update = true;
            inventoryUpdated?.Invoke();
            return true;
        }
        private int FindSlot(_Item item)
        {
            int i = FindStack(item);
            if (i < 0)
            {
                i = FindEmptySlot();
            }

            return i;
        }
        private int FindEmptySlot()
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].item == null)
                {
                    return i;
                }
            }

            return -1;
        }
        public int FindStack(_Item item)
        {
            if (!item.IsStackable())
            {
                return -1; // Itens não empilháveis não devem ser colocados no mesmo slot
            }

            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].item != null && slots[i].item.itemID == item.itemID)
                {
                    return i; // Retorna o índice do slot com o mesmo item empilhável
                }
            }

            return -1; // Nenhuma pilha existente encontrada
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
            for (int i = 0; i < slots.Count; i++)
            {
                // Verifica se o slot contém o item e se tem quantidade suficiente.
                if (slots[i].item == item && slots[i].number >= requiredQuantity)
                {
                    return i; // Retorna o índice do slot.
                }
            }

            return -1; // Retorna -1 se o item não for encontrado.
        }
    }
}