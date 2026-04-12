using MKU.Scripts.Interfaces;
using MKU.Scripts.ItemSystem;
using MKU.Scripts.IventorySystem;
using UnityEngine;

namespace MKU.Scripts.CraftingSystem
{
    [RequireComponent(typeof(ItemTooltipSpawner))]
    public class CraftSlotUI : MonoBehaviour, IItemHolder, IDragContainer<_Item>
    {// CONFIG DATA
        public ItemIcon icon = null;

        // STATE
        public int index;
        public _Item item;
        Crafting _crafting;

        // PUBLIC

        public void Setup(Crafting _crafting, int index)
        {
            this._crafting = _crafting;
            this.index = index;
            icon.SetItem(_crafting.GetItemInSlot(index), _crafting.GetNumberInSlot(index));
        }

        public void AddItems(_Item item, int number)
        {
            this.item = item;
            _crafting.AddItemToSlot(index, item, number);
        }

        public ItemIcon GetIcon() => icon;

        public _Item GetItem()
        {
            return _crafting.GetItemInSlot(index);
        }

        public int GetNumber()
        {
            return _crafting.GetNumberInSlot(index);
        }

        public void RemoveItems(int number)
        {
            _crafting.RemoveFromSlot(index, number);
        }

        public int MaxAccetable(_Item item)
        {
            if (_crafting.HasSpaceFor(item))
            {
                return int.MaxValue;
            }
            return 0;
        }
    }
}