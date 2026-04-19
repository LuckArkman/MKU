using System;
using MKU.Scripts.Enums;
using MKU.Scripts.HelthSystem;
using UnityEngine;

namespace MKU.Scripts.SkillSystem
{
    [CreateAssetMenu(fileName = "BufferSkill", menuName = "CursedStone/SkillSystem/BufferSkill/New BufferSkill", order = 0)]
    public class BufferSkill: Skills, ISerializationCallbackReceiver
    {
        public _Attributs _attributes = new _Attributs();
        [Range(0.0f,180.0f)]
        public float timeEffect;
        public void OnBeforeSerialize()
        {
            if (string.IsNullOrEmpty(skillID))
            {
                skillID = Guid.NewGuid().ToString();
                _attackType = AttackType.Buffer;
            }
        }

        public void OnAfterDeserialize(){}
    }
}