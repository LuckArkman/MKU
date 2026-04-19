using System.Collections.Generic;
using System.Threading.Tasks;
using MKU.Scripts.Enums;
using MKU.Scripts.Tasks.Interface;

namespace MKU.Scripts.Tasks.Logics
{
    public class TaskNPC : BaseController, ITaskController
    {
        public TaskCondition requestCode = TaskCondition.NPC;
        Objective _quest;
        public TaskNPC(Objective quest) => _quest = quest;
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
            if (actionCode is TaskCondition.talk) VerifyTask<Objective>(quest, value);
        }
    }
}