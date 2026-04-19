using MKU.Scripts.Interfaces;
using MKU.Scripts.ItemSystem;
using MKU.Scripts.IventorySystem;
using UnityEngine;

namespace MKU.Scripts.CookingSystem
{
    [RequireComponent(typeof(ItemTooltipSpawner))]
    public class CookingSlotUI : MonoBehaviour, IItemHolder, IDragContainer<_Item>
    {// CONFIG DATA
        public ItemIcon icon = null;

        // STATE
        public int index;
        public _Item item;
        Cooking _cooking;

        // PUBLIC

        public void Setup(Cooking _cooking, int index)
        {
            this._cooking = _cooking;
            this.index = index;
            icon.SetItem(this._cooking.GetItemInSlot(index), _cooking.GetNumberInSlot(index));
        }

        public void AddItems(_Item item, int number)
        {
            this.item = item;
            _cooking.AddItemToSlot(index, item, number);
        }

        public ItemIcon GetIcon() => icon;

        public _Item GetItem()
        {
            return _cooking.GetItemInSlot(index);
        }

        public int GetNumber()
        {
            return _cooking.GetNumberInSlot(index);
        }

        public void RemoveItems(int number)
        {
            _cooking.RemoveFromSlot(index, number);
        }

        public int MaxAccetable(_Item item)
        {
            if (_cooking.HasSpaceFor(item))
            {
                return int.MaxValue;
            }
            return 0;
        }
    }
}