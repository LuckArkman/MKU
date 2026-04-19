using System.Collections;
using System.Collections.Generic;
using MKU.Scripts.CharacterSystem;
using MKU.Scripts.Enums;
using MKU.Scripts.FinanceSystem;
using MKU.Scripts.ItemSystem;
using MKU.Scripts.IventorySystem;
using MKU.Scripts.Models;
using MKU.Scripts.Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MKU.Scripts.Tasks.UI
{
    public class QuestInformation : MonoBehaviour
    {
        public _Quest _quest;
        public TextMeshProUGUI _title;
        public TextMeshProUGUI _questDescription;
        public Transform _objectives;
        public Transform _rewards;
        public Button _request;
        public ObjectivesQuest _objectivesQuest;
        public ItemReward _itemReward;
        public QuestObject _questObject;

        public void OnStart(_Quest quest, QuestObject questObject)
        {
            _quest = quest;
            _title.text = _quest.Title;
            _request.interactable = quest.IsComplete? false: true;
            _questDescription.text = _quest.task_description;
            _questObject = questObject;
            for (int i = 0; i < _objectives.childCount; i++)
            {
                Destroy(_objectives.GetChild(i).gameObject);
            }
            _quest._objectives.ForEach(o =>
            {
                var ob = Instantiate<ObjectivesQuest>(_objectivesQuest, _objectives);
                ob.OnStart(o);
            });
            for (int i = 0; i < _rewards.childCount; i++)
            {
                Destroy(_rewards.GetChild(i).gameObject);
            }
            _quest._rewards.ForEach(r =>
            {
                var ob = Instantiate<ItemReward>(_itemReward, _rewards);
                ob.DisplayReward(r);
            });
        }

        public async void OnRequestReward()
        {
            var _manager = Singleton.Instance.questManager;
            _manager.quest = null;
            _manager._questLast.Add(_quest);
            _manager._questBox.gameObject.SetActive(false);
            _quest.IsComplete = true;
            _questObject._button.interactable = false;
            _request.interactable = false;
            string response = "";
            _quest._rewards.ForEach(async x =>
            {
                if (x._rewardType == RewardType.item)
                {
                    var items = Resources.Load("ItemContainer") as ItemContainer;
                    var item = items.items.Find(i => i.itemID == x.itemId);
                    var obj = ScriptableObject.CreateInstance<_Item>();
                    obj.SetItem(item.itemID, item.itemToken,item.displayName, item.itemCategory, item.level, item.description,
                        item.icon, item.pickup, item.upgradable, item.allowedEquipLocation,item.dualhand, item.lefthand, item.righthand, item.stackable,
                        item.price,item.Upgradable, item._parcentes);
                    Singleton.Instance._inventory.AddToFirstEmptySlot(obj, x.number);
                }
                if (x._rewardType == RewardType.xp)
                {
                    Singleton.Instance._charController.progression.currentExperience+=x.number;
                }
                
                if (x._rewardType == RewardType.coin)
                {
                    if (Singleton.Instance._character == null) response = await new FinanceManager().PostCsts(new Message("", ActionCode.Transference, x.number, Singleton.Instance.Id));
                    if (Singleton.Instance._character != null) response = await new FinanceManager().PostCsts(new Message("", ActionCode.Transference, x.number, Singleton.Instance._character.id));
                    if(string.IsNullOrWhiteSpace(response))Singleton.Instance._financeController.GetBalance();
                    
                }
                CharController charController = CharSettings._Instance._charController.GetComponent<CharController>();
                var quest = _quest.GetQuest(_quest);
                Dictionary<string, Quest> _quests = new();
                _quests.Add(_quest.Name, quest);
                response = await new UpdateQuestInCharacter().UpdateQuest(new CharQuest(charController.Id, _quests));
            });
        }
    }
}