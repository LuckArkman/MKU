using System;
using MKU.Scripts.ItemSystem;
using MKU.Scripts.Interfaces;
using MKU.Scripts.Singletons;
using UnityEngine;

namespace MKU.Scripts.IventorySystem
{
    [RequireComponent(typeof(ItemTooltipSpawner))]
    public class InventorySlotUI : MonoBehaviour, IItemHolder, IDragContainer<_Item>
    {// CONFIG DATA
        public ItemIcon icon = null;

        // STATE
        public int index;
        public _Item item;
        public Inventory inventory;

        // PUBLIC

        public void Setup(Inventory inventory, int index)
        {
            this.inventory = inventory;
            this.index = index;
            icon.SetItem(inventory.GetItemInSlot(index), inventory.GetNumberInSlot(index));
        }

        public void AddItems(_Item item, int number)
        {
            this.item = item;
            inventory.AddItemToSlot(index, item, number);
        }

        public ItemIcon GetIcon() => icon;

        public _Item GetItem()
        {
            return inventory.GetItemInSlot(index);
        }

        public int GetNumber()
        {
            return inventory.GetNumberInSlot(index);
        }

        public void RemoveItems(int number)
        {
            inventory.RemoveFromSlot(index, number);
        }

        public int MaxAccetable(_Item item)
        {
            if (inventory.HasSpaceFor(item))
            {
                return int.MaxValue;
            }
            return 0;
        }
    }
}