using System;
using MKU.Scripts.Enums;
using UnityEditor;
using UnityEngine;

namespace MKU.Scripts.Tasks
{
    [Serializable]
    public class _Reward
    {
        public RewardType _rewardType = RewardType.None;
            [Range(0,1000)]
            public int number;
            
            public string itemId;

            public _Reward(RewardType rewardType, int number, string itemId)
            {
                _rewardType = rewardType;
                this.number = number;
                this.itemId = itemId;
            }
    }
}