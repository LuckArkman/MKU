using MKU.Scripts.Enums;
using UnityEngine;

namespace MKU.Scripts.Tasks
{
    public abstract class QuestLogicController
    {
        protected TaskCondition _taskCondition = TaskCondition.None;

        void HandleRequest<T>(TaskCondition _condition, T _quest){}
    }
}