using System;
using MKU.Scripts.Enums;
using UnityEngine;

namespace MKU.Scripts.FightSystem
{
[CreateAssetMenu(fileName = "Fighter", menuName = "CursedStone/FightSystem/Attack/New Fighter", order = 0)]
    public class Fighter : _Attack, ISerializationCallbackReceiver
    {
        public void OnBeforeSerialize()
        {
            if (string.IsNullOrEmpty(attackID))attackID = Guid.NewGuid().ToString();
        }

        public void OnAfterDeserialize(){}
    }
}