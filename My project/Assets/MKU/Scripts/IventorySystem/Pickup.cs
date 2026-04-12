using System.Collections.Generic;
using MKU.Scripts.CharacterSystem;
using MKU.Scripts.Enums;
using MKU.Scripts.EquipamentsSystem;
using MKU.Scripts.ItemSystem;
using MKU.Scripts.Singletons;
using MKU.Scripts.Strucs;
using UnityEngine;
using CharacterController = MKU.Scripts.CharacterSystem.CharacterController;

namespace MKU.Scripts.IventorySystem
{
    public class Pickup : MonoBehaviour
    {
        private _Item item;
        private float m_lifetime;
        [SerializeField] private float m_speed;
        private float m_startTime;
        private Vector3 m_direction;
        private bool m_follow;
        public bool isCollected = false; // Para evitar coleta duplicada
        public PickupType pickupType = PickupType.None;
        [SerializeField] private Vector3 m_offset;
        public CharacterController _charController;
        public List<ItemDropCollection> _ItemDropCollections = new();
        private Inventory inventory;

        public void Setup(_Item item, int number)
        {
            this.item = item;
            if (!item.stackable) number = 1;
        }

        public void AddDropPickup(string id, int quantity)
        {
            _ItemDropCollections.Add(new ItemDropCollection(id, quantity));
        }

        public void PickupItem(CharacterController charController)
        {
            if (isCollected) return; // Evita múltiplas coletas simultâneas

            inventory = Singleton.Instance._inventory;
            var container = Resources.Load("ItemContainer") as ItemContainer;

            foreach (var drop in _ItemDropCollections)
            {
                item = container.items.Find(x => x.itemID == drop.ItemId);
                if (item == null) continue;
                AddItemToInventory(item, drop.number);
            }

            isCollected = true; // Marca como coletado para evitar execução múltipla
            Destroy(gameObject);
        }

        private void AddItemToInventory(_Item item, int quantity)
        {
            if (item.itemCategory == ItemCategory.Equipable)
            {
                var obj = ScriptableObject.CreateInstance<EquipableItem>();
                obj.SetEquipableItem(
                    item.itemID, item.itemToken, item.displayName, item.itemCategory, item.level,
                    item.description, item.icon, item.pickup, item.upgradable, item.allowedEquipLocation,
                    item.dualhand, item.lefthand, item.righthand, item.stackable,
                    item.price, item.Upgradable, item._parcentes
                );
                inventory.AddToFirstEmptySlot(obj, quantity);
            }
            else
            {
                inventory.AddToFirstEmptySlot(item, quantity);
            }

            Singleton.Instance._itemFeedbackController.OnSpawn(item, quantity);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isCollected) return; // Evita múltiplas ativações

            var inventoryComponent = other.GetComponent<Inventory>();
            if (inventoryComponent != null)
            {
                PickupItem(other.GetComponent<CharacterController>());
            }
        }

        void LateUpdate()
        {
            if (_charController == null) _charController = Singleton.Instance._charController;
            if (_charController != null && pickupType == PickupType.Drop)
            {
                float distance = Vector3.Distance(_charController.transform.position, transform.position);

                if (distance <= 1.5f && !isCollected)
                {
                    transform.position = _charController.transform.position + m_offset;
                    m_direction = (_charController.transform.position + m_offset - transform.position).normalized;
                    GetComponent<Rigidbody>().linearVelocity =
                        m_direction * m_speed * (Time.time - m_startTime - 0.3f) / 0.3f;
                }

                if (distance <= 0.5f)
                {
                    PickupItem(_charController.characterController);
                }
            }
        }

        public bool CanBePickedUp() => inventory != null && inventory.HasSpaceFor(item);
    }
}