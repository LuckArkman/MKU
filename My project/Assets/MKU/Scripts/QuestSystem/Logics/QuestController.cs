using System.Collections.Generic;
using MKU.Scripts.Enums;
using MKU.Scripts.Tasks.Interface;
using UnityEngine;

namespace MKU.Scripts.Tasks.Logics
{
    public class QuestController
    {
        public QuestController(){}
        private Objective _quest;
        private Dictionary<TaskCondition, ITaskController> controllerDict = new Dictionary<TaskCondition, ITaskController>();
        TaskHunting _TaskHunting { get; set; }
        TaskCollect _TaskCollect { get; set; }
        TaskNPC _TaskNpc { get; set; }
        TaskItems _TaskItems { get; set; }
        TaskDungeons _TaskDungeons { get; set; }

        public void HandleRequest<T>(TaskCondition _condition, TaskCondition _actionCode, Objective quest, string value)
        {
            if (_condition is TaskCondition.Hunting) new TaskHunting(quest).SetAction<Objective>(quest, _actionCode, value);
            if (_condition is TaskCondition.Collect) new TaskCollect(quest).SetAction<Objective>(quest, _actionCode, value);
            if (_condition is TaskCondition.talk) new TaskNPC(quest).SetAction<Objective>(quest, _actionCode, value);
            if (_condition is TaskCondition.Item) new TaskItems(quest).SetAction<Objective>(quest, _actionCode, value);
            if (_condition is TaskCondition.Dungeons) new TaskDungeons(quest).SetAction<Objective>(quest, _actionCode, value);
        }
    }
}