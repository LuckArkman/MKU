using System;
using MKU.Scripts.Enums;
using UnityEngine;

namespace MKU.Scripts.SkillSystem
{
    [CreateAssetMenu(fileName = "PhysicalAttack", menuName = "CursedStone/SkillSystem/PhysicalAttack/New PhysicalAttack", order = 0)]
    public class PhysicalAttack: Skills, ISerializationCallbackReceiver
    {
        public void OnBeforeSerialize()
        {
            if (string.IsNullOrEmpty(skillID))
            {
                skillID = Guid.NewGuid().ToString();
                _attackType = AttackType.Physical;
            }
        }

        public void OnAfterDeserialize(){}
    }
}