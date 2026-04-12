using System.Collections.Generic;
using MKU.Scripts.Enums;
using MKU.Scripts.ItemSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MKU.Scripts.Tasks.UI
{
    public class ItemReward : MonoBehaviour
    {
        public _Reward _reward;
        public List<Sprite> _sprites = new ();
        public Image _image;
        public TextMeshProUGUI _number;

        public void DisplayReward(_Reward reward)
        {
            _reward = reward;
            if (_reward._rewardType == RewardType.xp)
            {
                _image.sprite = _sprites[0];
                _number.text = $"{_reward.number}";
            }
            if (_reward._rewardType == RewardType.coin)
            {
                _image.sprite = _sprites[1];
                _number.text = $"{_reward.number}";
            }
            if (_reward._rewardType == RewardType.item)
            {
                var items = Resources.Load("ItemContainer") as ItemContainer;
                _image.sprite = items.items.Find(x => x.itemID == _reward.itemId).icon;
                _number.text = $"{_reward.number}";
            }
        }
    }
}