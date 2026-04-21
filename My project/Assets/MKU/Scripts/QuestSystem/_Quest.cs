using System;
using System.Collections.Generic;
using MKU.Scripts.Enums;
using MKU.Scripts.Singletons;
using MKU.Scripts.Tasks.Logics;
using UnityEngine;

namespace MKU.Scripts.Tasks
{
    public class _Quest : ScriptableObject
    {
        public QuestChain questChain;
        String UserId;
        public string Title;
        public int id;
        public int quest_id;
        public TaskType task_type;
        public string Name;
        [TextArea(5, 20)] public string task_description;
        [Range(0, 1000)] public int seconds_to_expire;
        public bool IsComplete = false;
        public QuestController _LogicController;
        public List<Objective> _objectives = new();
        public List<_Reward> _rewards = new ();

        public void OnSendLogic(TaskCondition condition, string _object, string Id)
        {
            UserId = Id;
            Objective obj = _objectives.Find(x => x.taskCondition == condition);
            if (obj != null && !obj.IsComplete)
            {
                StartLogic(obj, condition, _object);
            }
        }

        public async void StartLogic(Objective objective, TaskCondition actionCode, string _object)
        {
            new QuestController().HandleRequest<Objective>(objective.taskCondition, actionCode, objective, _object);

            Quest _quest = new Quest(this.name,
                Title,
                task_type,
                Name,
                task_description,
                IsComplete,
                getObjectives(this._objectives),
                getRewards(this._rewards));
            Dictionary<string, Quest> _quests = new();
            _quests.Add(_quest.name, _quest);
            var update = new CharQuest();
            update.id = UserId;
            update._quests.TryAdd(_quest.id, _quest);
            var response = await new UpdateQuestInCharacter().UpdateQuest(
                new CharQuest(Singleton.Instance._charController.Id, _quests));
            Debug.Log($"{nameof(StartLogic)} >> {response}");
        }

        public Quest GetQuest(_Quest quest)
            => new Quest(quest.Name,
                quest.Title,
                quest.task_type,
                quest.Name,
                quest.task_description,
                quest.IsComplete,
                quest.getObjectives(quest._objectives),
                quest.getRewards(quest._rewards));

        private List<Rewards> getRewards(List<_Reward> questRewards)
        {
            List<Rewards> objectives = new();
            questRewards.ForEach(o => { objectives.Add(new Rewards(o._rewardType, o.number, o.itemId)); });
            return objectives;
        }

        private List<Objectives> getObjectives(List<Objective> questObjectives)
        {
            List<Objectives> objectives = new();
            questObjectives.ForEach(o =>
            {
                objectives.Add(
                    new Objectives(o.taskCondition, o.description, o.Items, o.id, o.IsComplete, o.number));
            });
            return objectives;
        }

        public _Quest(int id, string name, string task_description, int quest_id, int task_type, int seconds_to_expire)
        {
            this.id = id;
            this.Name = name;
            this.task_description = task_description;
            this.quest_id = quest_id;
            this.task_type = (TaskType)task_type;
            this.seconds_to_expire = seconds_to_expire;
        }

        public bool OnComplete(_Quest quest)
        => quest._objectives.Find(x => !x.IsComplete) == null;

        public void OnQuest(QuestChain questChain, string title, TaskType taskType, string name, string taskDescription, List<Objective> _objectives, List<_Reward> _rewards, bool isComplete)
        {
            this.questChain = questChain;
            Title = title;
            task_type = taskType;
            Name = name;
            task_description = taskDescription;
            this._objectives = _objectives;
            this._rewards = _rewards;
            IsComplete = isComplete;
        }
    }
}