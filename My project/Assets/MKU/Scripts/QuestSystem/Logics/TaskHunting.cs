using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MKU.Scripts.Enums;
using MKU.Scripts.Tasks.Interface;
using UnityEngine;

namespace MKU.Scripts.Tasks.Logics
{
    public class TaskHunting : BaseController, ITaskController
    {
        public TaskCondition requestCode = TaskCondition.Hunting;
        Objective _quest;
        public TaskHunting(Objective quest) => _quest = quest;

        public void VerifyTask<T>(Objective _quest, string data)
        {
            if(_quest.GetItems().Count < _quest.number) _quest.SetCountNumber(data);
            if (_quest.GetItems().Count >= _quest.number) CompleteTask<Objective>(this._quest);
            Debug.Log($"{nameof(VerifyTask)} >> {_quest.GetItems().Count}");
        }

        public async Task CompleteTask<T>(T _quest)
        {
            var task = _quest as Objective;
            task.IsComplete = true;
        }

        public void SetAction<T>(Objective quest, TaskCondition actionCode, string value)
        {
            if (actionCode is TaskCondition.Hunting) VerifyTask<Objective>(quest, value);
        }
    }
}