using System.Collections.Generic;
using System.Threading.Tasks;
using MKU.Scripts.Enums;
using UnityEngine;

namespace MKU.Scripts.Tasks
{
    public abstract class QTask : ScriptableObject 
    {
        /*
        public TaskCondition taskType;

        public async virtual Task<bool> VerifyTaskAsync(List<int> Value, QuestTrigger questTrigger) 
        {
            await System.Threading.Tasks.Task.Delay(0);
            return false;
        }

        public virtual bool VerifyTask(List<int> Value, QuestTrigger questTrigger) 
        {
            return false;
        }

        public virtual bool AutoSetTaskIDs(QuestDataLog quest)
        {
            return false;
        }

        public virtual TaskValue ValueChange(TaskValue taskValue, QuestTrigger questTrigger) {

            taskValue.collected_ids += $"{questTrigger.ID},";
            taskValue.achived += questTrigger.quantity;

            return taskValue;

        }
        */
    }
}