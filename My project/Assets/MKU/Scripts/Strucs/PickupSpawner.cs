using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MKU.Scripts.CharacterSystem;
using MKU.Scripts.Enums;
using MKU.Scripts.EquipamentsSystem;
using MKU.Scripts.Interfaces;
using MKU.Scripts.ItemSystem;
using MKU.Scripts.IventorySystem;
using MKU.Scripts.Singletons;
using UnityEngine;

namespace MKU.Scripts.Strucs
{
    public class PickupSpawner : MonoBehaviour, IPickupSpawner
    {
        public Pickup _pickup = null;
        public bool collected;
        public Pickup spawnedPickup;
        public AnimationType animationType = AnimationType.None;
        public ItemContainer container;
        public List<_Slot> slots = new ();
        [Range(0.0f, 600.0f)]
        public int seconds;
        
        private void Awake()
        {
            for (int i = 0; i < 25; i++)
            {
                slots.Add(new _Slot());
            }
        }

        private void Start()
        {
            OnSpawn();
        }

        private void OnSpawn()
        {
            Debug.Log($"{nameof(OnSpawn)} >> {DateTime.Now}");
            if(spawnedPickup == null) SpawnPickup();
        }

        void Update()
        {
            
        }

        public async void OnPikup(int time, CharController _charController)
        {
            await Task.Delay(time);
            _pickup._ItemDropCollections.ForEach(i =>
            {
                var inventory = Singleton.Instance._inventory;
                if (container == null) container = Resources.Load("ItemContainer") as ItemContainer;
                var item = container.items.Find(x => x.itemID == i.ItemId);
                if (item.itemCategory == ItemCategory.Equipable)
                {
                    Debug.Log($"{nameof(OnPikup)} >> {item.itemCategory}");
                    var obj = ScriptableObject.CreateInstance<EquipableItem>();
                    obj.SetEquipableItem(
                        item.itemID, item.itemToken, item.displayName, item.itemCategory, item.level,
                        item.description, item.icon, item.pickup, item.upgradable, item.allowedEquipLocation,
                        item.dualhand, item.lefthand, item.righthand, item.stackable,
                        item.price, item.Upgradable, item._parcentes
                    );
                    inventory.AddToFirstEmptySlot(obj, i.number);
                }
                else
                {
                    inventory.AddToFirstEmptySlot(item, i.number);
                }
            });
            await Task.Delay(time);
            DestroyPickup();
            Debug.Log($"{nameof(OnPikup)} >> {DateTime.Now}");
            int milliseconds = seconds  * 1000;
            await Task.Delay(milliseconds);
            OnSpawn();
            
        }

        public Pickup GetPickup()
        {
            return GetComponentInChildren<Pickup>();
        }

        public bool isCollected()
            => collected;

        public object getPickupSpawner()
            => this;

        //PRIVATE

        private void SpawnPickup()
        {
            spawnedPickup = Instantiate<Pickup>(_pickup, this.transform.position, Quaternion.identity); 
            spawnedPickup.transform.SetParent(transform);
            spawnedPickup.isCollected = false;
        }

        private void DestroyPickup()
        {
            if (spawnedPickup != null)Destroy(spawnedPickup.gameObject);
        }
    }
}