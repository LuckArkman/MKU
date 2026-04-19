using System;
using MKU.Scripts.HelthSystem;
using UnityEngine;

namespace MKU.Scripts.ItemSystem
{
    [Serializable]
    public class Parcente
    {
        [Range(0.0f,1.0f)]
        public float parcent;
        [Range(0,1000)]
        public int price;
        public bool _base;
        [Header(nameof(_Attributs))]
        public _Attributs attributes = new _Attributs();
        public _Stats _status = new _Stats();
    }
}