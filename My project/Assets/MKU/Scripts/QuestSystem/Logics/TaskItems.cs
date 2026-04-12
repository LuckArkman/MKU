using System.Collections.Generic;
using System.Threading.Tasks;
using MKU.Scripts.Enums;
using MKU.Scripts.Tasks.Interface;

namespace MKU.Scripts.Tasks.Logics
{
    public class TaskItems : BaseController, ITaskController
    {
        public TaskCondition requestCode = TaskCondition.Item;
        Objective _quest;
        public TaskItems(Objective quest) => _quest = quest;

        public void VerifyTask<T>(Objective _quest, string data)
        {
            var Id = data as string;
            if(_quest.id == Id) CompleteTask<Objective>(this._quest);

        }

        public async Task CompleteTask<T>(T _quest)
        {
            var task = _quest as Objective;
            task.IsComplete = true;
        }

        public void SetAction<T>(Objective quest, TaskCondition actionCode, string value)
        {
            if (actionCode is TaskCondition.Use) VerifyTask<Objective>(quest, value);
            if (actionCode is TaskCondition.Equipe) VerifyTask<Objective>(quest, value);
        }
    }
}