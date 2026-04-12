using System;
using MKU.Scripts.Enums;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace MKU.Scripts.Tasks.UI
{
    public class QuestInfoUI : MonoBehaviour
    {
        public _Quest _quest;
        public Button _Buttonfinalize;
        public TextMeshProUGUI _questName, _questDescription;
        public GameObject _questObjetives, _questReward;
        public RewardUI _RewardUI;

        public ObjectiveUI _ObjectiveUI;

        public void OnStart(_Quest _q)
        {
            _quest = _q;
            _questName.text = _quest.Title;
            _questDescription.text = _questDescription.text;
            ShowObjetives(_quest);
            ShowRewards(_quest);
        }

        private void ShowRewards(_Quest quest)
        {
            foreach (var child in _questReward.gameObject.GetComponentsInChildren<RewardUI>())
            {
                Destroy(child.gameObject);
            }
            quest._rewards.ForEach(o =>
            {
                RewardUI ui = Instantiate<RewardUI>(_RewardUI, _questReward.transform);
                ui._quantity.text = o.number.ToString();
                if(o._rewardType == RewardType.xp) ui._Name.text = "Experience Points";
                if(o._rewardType == RewardType.coin) ui._Name.text = "Coins";
                if(o._rewardType == RewardType.item) ui._Name.text = o.itemId.ToString();
            });
        }

        private void ShowObjetives(_Quest quest)
        {
            foreach (var child in _questObjetives.gameObject.GetComponentsInChildren<ObjectiveUI>())
            {
                Destroy(child.gameObject);
            }
            quest._objectives.ForEach(o =>
            {
                ObjectiveUI ui = Instantiate<ObjectiveUI>(_ObjectiveUI, _questObjetives.transform);
                ui._descricao.text = o.description;
                //ui._Objective = o;
                if (o.taskCondition == TaskCondition.Collect) ui._quantia.text = string.Format(" ({0} / {1}) {2} - ({3}) - {4}", o.GetItems().Count,o.number,"Colete ",o.number, o.id.ToString());
                if (o.taskCondition == TaskCondition.NPC) ui._quantia.text = $"Fale com o Npc {o.id.ToString()}";
                if (o.taskCondition == TaskCondition.Hunting)ui._quantia.text = string.Format(" ({0} / {1}) {2} - ({3}) - {4}", o.GetItems().Count, o.number, "Elimine ", o.number,o.id.ToString());
            });
        }
    }
}