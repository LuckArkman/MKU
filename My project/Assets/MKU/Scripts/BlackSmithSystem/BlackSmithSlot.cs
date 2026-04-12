using MKU.Scripts.CharacterSystem;
using MKU.Scripts.Enums;
using MKU.Scripts.EquipamentsSystem;
using MKU.Scripts.Interfaces;
using MKU.Scripts.ItemSystem;
using MKU.Scripts.IventorySystem;
using UnityEngine;

namespace MKU.Scripts.BlackSmithSystem
{
    public class BlackSmithSlot : MonoBehaviour, IItemHolder, IDragContainer<_Item>
    {
        public ItemIcon icon = null;

        public UpgradableItems items = UpgradableItems.None;
        public BlackSmithUI _blackSmithUI;

        public BlackSmith blackSmith;

        public void Start()
        {
            blackSmith =  CharSettings._Instance._blackSmith;
            blackSmith.blacksmithUpdated += RedrawUI;
            RedrawUI();
        }


        public void AddItems(_Item item, int number)
        {
            blackSmith.AddItem(items, (BlackSmithItem)item);
        }

        public _Item GetItem()
        {
            return blackSmith.GetItemInSlot(items);
        }

        public int GetNumber()
        {
            if (GetItem() != null) return 1;
            return 0;
        }

        public int MaxAccetable(_Item item)
        {
            EquipableItem equipableItem = item as EquipableItem;
            if (equipableItem == null) return 0;
            if (equipableItem.GetAllowedUpgradableItems() != items) return 0;
            if (GetItem() != null) return 0;

            return 1;
        }

        public void RemoveItems(int number)
        {
            blackSmith.RemoveItems(items);
        }

        void RedrawUI()
        {
            icon.SetItem(blackSmith.GetItemInSlot(items));
        }
    }
}