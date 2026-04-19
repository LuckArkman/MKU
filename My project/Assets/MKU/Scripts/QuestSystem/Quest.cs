using System;
using System.Collections.Generic;
using MKU.Scripts.Enums;

namespace MKU.Scripts.Tasks
{
    [Serializable]
    public class Quest
    {
        public Quest(){}
        public string id { get; set; }
        public string title { get; set; }
        public TaskType task_type { get; set; }
        public string name { get; set; }
        public string task_description { get; set; }
        public bool isComplete { get; set; }
        public List<Objectives> _objectives { get; set; }
        public List<Rewards> _rewards { get; set; }

        public Quest(string id, string title, TaskType task_type, string name, string task_description, bool isComplete, List<Objectives> _objectives, List<Rewards> _rewards)
        {
            this.id = id;
            this.title = title;
            this.task_type = task_type;
            this.name = name;
            this.task_description = task_description;
            this.isComplete = isComplete;
            this._objectives = _objectives;
            this._rewards = _rewards;
        }
    }
}
