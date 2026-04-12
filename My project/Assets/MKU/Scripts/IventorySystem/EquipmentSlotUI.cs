using MKU.Scripts.CharacterSystem;
using MKU.Scripts.Enums;
using MKU.Scripts.EquipamentsSystem;
using MKU.Scripts.Interfaces;
using MKU.Scripts.ItemSystem;
using UnityEngine;

namespace MKU.Scripts.IventorySystem
{
    [RequireComponent(typeof(ItemTooltipSpawner))]
    public class EquipmentSlotUI : MonoBehaviour, IItemHolder, IDragContainer<_Item>
    {
        public ItemIcon icon = null;
        public int Index;
        public EquipLocation equipLocation = EquipLocation.Weapon;
        public Equipment playerEquipment;

        private void Start()
        {
            if (playerEquipment == null)
            {
                Debug.LogError("PlayerEquipment não foi atribuído!");
                return;
            }

            playerEquipment.equipmentUpdated += RedrawUI;
            RedrawUI();
        }

        public void AddItems(_Item item, int number)
        {
            var equipableItem = item;
            if (equipableItem == null)
            {
                Debug.LogError("Tipo de item inválido!");
                return;
            }

            playerEquipment.AddItem(equipLocation, item as EquipableItem);
            RedrawUI();
        }

        public _Item GetItem()
        {
            return playerEquipment.GetItemInSlot(equipLocation);
        }

        public int GetNumber()
        {
            return GetItem() == null ? 0 : 1;
        }

        public void RemoveItems(int number)
        {
            playerEquipment.RemoveItem(equipLocation);
            RedrawUI();
        }

        void RedrawUI()
        {
            
            _Item currentItem = playerEquipment.GetItemInSlot(equipLocation);
            if (equipLocation == EquipLocation.Weapon)
                CharSettings._Instance._charController.weapon = currentItem !=  null;
            icon.SetItem(currentItem);
            

        }

        public int MaxAccetable(_Item item)
        {
            EquipableItem equipableItem = item as EquipableItem;
            if (equipableItem == null) return 0;
            if (equipableItem.GetAllowedEquipLocation() != equipLocation) return 0;
            if (GetItem() != null) return 0;

            return 1;
        }
    }
}