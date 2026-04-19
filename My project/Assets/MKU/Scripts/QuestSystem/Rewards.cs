using System;
using MKU.Scripts.Enums;

namespace MKU.Scripts.Tasks
{
    [Serializable]
    public class Rewards
    {
        public Rewards(){}
        public RewardType _rewardType { get; set; }
        public int number { get; set; }
        public string itemId { get; set; }

        public Rewards(RewardType _rewardType, int number, string itemId)
        {
            this._rewardType = _rewardType;
            this.number = number;
            this.itemId = itemId;
        }
    }
}