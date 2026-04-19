using System;
using MKU.Scripts.Enums;
using MKU.Scripts.HelthSystem;
using UnityEngine;

namespace MKU.Scripts.SkillSystem
{
    [CreateAssetMenu(fileName = "Debuffer", menuName = "CursedStone/SkillSystem/Debuffer/New Debuffer", order = 0)]
    public class Debuffer: Skills, ISerializationCallbackReceiver
    {
        [Range(0.0f, 100.0f)]
        public float parcent;
        public bool damage;
        [Range(0,50)]
        public int damageValue;
        public _Attributs _attributes = new ();
        [Range(0.0f,180.0f)]
        public float timeEffect;
        public void OnBeforeSerialize()
        {
            if (string.IsNullOrEmpty(skillID))
            {
                skillID = Guid.NewGuid().ToString();
                _attackType = AttackType.Debuffer;
            }
        }

        public void OnAfterDeserialize(){}
        
        public bool IsCriticalHit()
        {
            return new System.Random().NextDouble() <= (parcent / 100);
        }
    }
}