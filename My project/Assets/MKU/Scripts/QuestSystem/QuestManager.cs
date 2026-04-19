using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MKU.Scripts.CharacterSystem;
using MKU.Scripts.Singletons;
using MKU.Scripts.Tasks.UI;
using UnityEngine;

namespace MKU.Scripts.Tasks
{
    public class QuestManager : MonoBehaviour
    {
        public _Quest quest;
        public List<_Quest> _questLast = new();
        public QuestBox _questBox;
        public Transform position;
        public _Quest _quest;

        private void Start()
        {
            Singleton.Instance.questManager = this;
        }

        private void Update()
        {
            if (quest == null)
            {
                _questBox.gameObject.SetActive(quest != null);
            }
        }

        public async Task OnReturnQuests()
        {
            CharController charController = Singleton.Instance._charController;
            var _Quests = await new RecoverQuests().RecoverQuest(charController.Id);
            foreach (var key in _Quests._quests)
            {
                Quest task = key.Value;
                if (task.isComplete)
                {
                    var questContainer = Resources.Load("QuestPanel") as QuestPanel;
                    var q = questContainer.items.Find(q => q.Name == task.name);
                    if (q != null)
                    {
                        var obj = ScriptableObject.CreateInstance<_Quest>();
                        Debug.Log($"{nameof(OnReturnQuests)} >> {obj == null}");
                        
                        List<Objective> _objectives = new();
                        List<_Reward> _rewards = new ();
                        task._objectives.ForEach(o =>
                        {
                            _objectives.Add(new Objective(o.taskCondition, o.description,o.items, o.id, o.isComplete,
                                o.number));
                        });
                        
                        Debug.Log($"Objetives >> {_objectives.Count}");
                        task._rewards.ForEach(o =>
                        {
                            _rewards.Add(new _Reward(o._rewardType, o.number, o.itemId));
                        });
                        Debug.Log($"Rewards >> {_objectives.Count}");
                        obj.OnQuest(null, task.title, task.task_type, task.name, task.task_description, _objectives, _rewards,task.isComplete);
                        _questLast.Add(obj);
                    }
                }

                if (!task.isComplete)
                {
                    var questContainer = Resources.Load("QuestPanel") as QuestPanel;
                    var q = questContainer.items.Find(q => q.Name == task.name);
                    if (q != null)
                    {
                        var obj = ScriptableObject.CreateInstance<_Quest>();
                        Debug.Log($"{nameof(OnReturnQuests)} >> {obj == null}");
                        
                        List<Objective> _objectives = new();
                        List<_Reward> _rewards = new ();
                        task._objectives.ForEach(o =>
                        {
                            _objectives.Add(new Objective(o.taskCondition, o.description,o.items, o.id, o.isComplete,
                                o.number));
                        });
                        
                        Debug.Log($"Objetives >> {_objectives.Count}");
                        task._rewards.ForEach(o =>
                        {
                            _rewards.Add(new _Reward(o._rewardType, o.number, o.itemId));
                        });
                        Debug.Log($"Rewards >> {_objectives.Count}");
                        obj.OnQuest(null, task.title, task.task_type, task.name, task.task_description, _objectives, _rewards,task.isComplete);
                        quest = obj;
                        _questBox.gameObject.SetActive(true);
                        _questBox.OnStart(obj);
                    }
                }
            }
        }

        public async void OnQuestReceive(string Id)
        {
            var questContainer = Resources.Load("QuestPanel") as QuestPanel;
            var q = questContainer.items.Find(q => q.Name == Id);
            List<Objectives> _objectives = new();
            List<Rewards> _rewards = new();
            Dictionary<string, Quest> _quests = new();
            q._objectives.ForEach(o =>
            {
                _objectives.Add(new Objectives(o.taskCondition, o.description, o.Items, o.id, o.IsComplete,
                    o.number));
            });
            q._rewards.ForEach(r => { _rewards.Add(new Rewards(r._rewardType, r.number, r.itemId)); });
            Quest task = new Quest(Id, q.Title, q.task_type, q.Name, q.task_description, q.IsComplete, _objectives,
                _rewards);
            _quests.TryAdd(task.id, task);
            CharQuest charQuest =
                new CharQuest(
                    string.IsNullOrWhiteSpace(Singleton.Instance.Id)
                        ? Singleton.Instance._charController.Id
                        : Singleton.Instance.Id, _quests);
            var response = await new InsertQuestInCharacter().InsertQuest(charQuest);
            Debug.Log($"{nameof(OnQuestReceive)} >> {response}");
            if (response == "200")
            {
                quest = q;
                _questBox.gameObject.SetActive(true);
                _questBox.OnStart(q);
            }
        }
    }
}