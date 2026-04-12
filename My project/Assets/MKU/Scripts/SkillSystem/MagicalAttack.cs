using System;
using MKU.Scripts.Enums;
using UnityEngine;

namespace MKU.Scripts.SkillSystem
{
    [CreateAssetMenu(fileName = "MagicalAttack", menuName = "CursedStone/SkillSystem/MagicalAttack/New MagicalAttack", order = 0)]
    public class MagicalAttack: Skills, ISerializationCallbackReceiver
    {
        public void OnBeforeSerialize()
        {
            if (string.IsNullOrEmpty(skillID))
            {
                skillID = Guid.NewGuid().ToString();
                _attackType = AttackType.Magical;
            }
        }

        public void OnAfterDeserialize(){}
    }
}