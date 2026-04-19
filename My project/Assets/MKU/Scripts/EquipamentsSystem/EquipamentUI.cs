using System;
using System.Collections.Generic;
using MKU.Scripts.IventorySystem;
using MKU.Scripts.Singletons;
using UnityEngine;

namespace MKU.Scripts.EquipamentsSystem
{
    public class EquipamentUI : MonoBehaviour
    {
        public List<EquipmentSlotUI> _equipmentSlots = new();

        public Equipment Helmet, Necklace, Body, Trousers, Boots, Weapon, Shield, Gloves, Pickup, Pet, Collect, Wings, Special, Crystal, Ring;


        private void Start()
        {
            Singleton.Instance._equipment = this;
        }
    }
}