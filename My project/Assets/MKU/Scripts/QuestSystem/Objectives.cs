using System;
using System.Collections.Generic;
using MKU.Scripts.Enums;
using UnityEngine;

namespace MKU.Scripts.Tasks
{
    [Serializable]
    public class Objectives
    {
        public Objectives(){}
        public TaskCondition taskCondition { get; set; }
        public string description { get; set; }
        public List<string> items { get; set; }
        public string id { get; set; }
        public bool isComplete { get; set; }
        public int number { get; set; }

        public Objectives(TaskCondition taskCondition, string description, List<string> items, string id, bool isComplete, int number)
        {
            this.taskCondition = taskCondition;
            this.description = description;
            this.items = items;
            this.id = id;
            this.isComplete = isComplete;
            this.number = number;
        }
    }
}