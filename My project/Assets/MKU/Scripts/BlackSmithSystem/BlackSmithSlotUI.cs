using MKU.Scripts.CharacterSystem;
using MKU.Scripts.Enums;
using MKU.Scripts.Interfaces;
using MKU.Scripts.ItemSystem;
using MKU.Scripts.IventorySystem;
using MKU.Scripts.Singletons;
using UnityEngine;
using Random = System.Random;

namespace MKU.Scripts.BlackSmithSystem
{
    [RequireComponent(typeof(ItemTooltipSpawner))]
    public class BlackSmithSlotUI : MonoBehaviour, IItemHolder, IDragContainer<_Item>
    {
        public ItemIcon icon = null;

        public UpgradableItems items = UpgradableItems.None;

        public BlackSmith blackSmith;
        public BlackSmithUI _blackSmithUI;

        public void Start()
        {
            blackSmith =  CharSettings._Instance._blackSmith;
            blackSmith.blacksmithUpdated += RedrawUI;
            RedrawUI();
        }


        public void AddItems(_Item item, int number)
        {
            blackSmith.AddItem(items, item);
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
            _Item equipableItem = item;
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

        private void Update()
        {
            
            ShowParcet(icon.item);
            
            if (CharSettings._Instance.upgrade)
            {
                CharSettings._Instance.upgrade = false;
                UpgradeEquipament();
            }
        }

        private void UpgradeEquipament()
        {
            _Item item = icon.item;
            if (items == UpgradableItems.Upgradable)
            {
                if (item != null)
                {
                    float parcent = item._parcentes[item.level].parcent * 100f;
                    bool value = new Random().NextDouble() <= (parcent / 100);
                    if (value)
                    {
                        _blackSmithUI.OnNotification("Sucess !");
                        item.level++;
                    }

                    if (!value && item.level >= 5)
                    {
                        _blackSmithUI.OnNotification("Fail !");
                        item.level--;
                    }
                    Debug.Log($"{nameof(UpgradeEquipament)} >> {value} >> {item.level}");
                    if (!value) return;
                }
            }

            if (items == UpgradableItems.Crystal)
            {
                Debug.Log($"{nameof(UpgradeEquipament)} >> {items}");
                icon.SetItem(null);
            }

        }

        private void ShowParcet(_Item? item)
        {
            if (item != null && items == UpgradableItems.Upgradable && item.level < 10)
            {
                Singleton.Instance.item = item;
                _blackSmithUI._parcent.text = $"{item._parcentes[item.level].parcent * 100.0f}%";
                _blackSmithUI._price.text = $"{item._parcentes[item.level].price}";
            }
            if (item == null && items == UpgradableItems.Upgradable)_blackSmithUI._parcent.text = $"00%";
            CharSettings._Instance.interactable = (item != null) && (items == UpgradableItems.Crystal) && (item.level < 10);
        }
    }
}