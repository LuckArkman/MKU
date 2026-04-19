using System;
using UnityEngine;

namespace MKU.Scripts.CharacterSystem
{
    [Serializable]
    public class WeaponCollection
    {
        public WeaponCollection(){}
        public Transform _vfx;
        public WeaponLeft left = new WeaponLeft();
        public WeaponRight Right = new WeaponRight();
    }
}