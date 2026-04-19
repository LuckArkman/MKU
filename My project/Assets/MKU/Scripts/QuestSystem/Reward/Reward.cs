using System;
using MKU.Scripts.Enums;
using UnityEditor;
using UnityEngine;

namespace MKU.Scripts.Tasks.Reward
{
    [Serializable]
    public class Reward
    {
        public RewardType _rewardType = RewardType.None;
            [Range(0,1000)]
            public int number;
            
            public string itemId;

    }
}