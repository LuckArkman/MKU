using System.Collections.Generic;
using MKU.Scripts.Enums;
using UnityEngine;

namespace MKU.Scripts.Tasks.UI
{
    public class Questlist : MonoBehaviour
    {
        /*
        public List<_Quest> _Quests = new List<_Quest>();
        public List<string> _questId = new List<string>();
        public GameObject _questList;
        public QuestUI _QuestUI;
        public QuestInfoUI _QuestInfoUI;
        public TaskCondition taskCondition = TaskCondition.None;
        void Start()
        {
            _QuestInfoUI.gameObject.SetActive(false);
            _Quests.Clear();
            GetQuests(_questId);
        }
        
        [ContextMenu(nameof(CompleteObjetive))]
        void CompleteObjetive()
            => _Quests.ForEach(q =>
            {
                var o = q._objectives.Find(o => o.taskCondition == taskCondition);
                o.IsComplete = !o.IsComplete;
            });

        private void GetQuests(List<string> questId)
        {
            var items = Resources.Load("QuestPanel") as QuestPanel;
            if (_questId.Count > 0)
            {
                _questId.ForEach(s =>
                {
                    var q = items.items.Find(x => x.name == s);
                    if (!_Quests.Contains(q)) _Quests.Add(q);
                });
            }
            
            _Quests.ForEach(q =>
            {
                QuestUI ui = Instantiate<QuestUI>(_QuestUI, _questList.transform);
                ui._quest = q;
                ui._questName.text = q.Title;
                ui._questId.text = q.name;
                ui._Questlist = this;
                ui.OnStart();
            });

        }

        public void ShowPanel(_Quest quest)
        {
            Debug.Log(nameof(ShowPanel));
            _QuestInfoUI.gameObject.SetActive(true);
            _QuestInfoUI.OnStart(quest);
        }
        */
    }
}