using System;
using System.Collections.Generic;
using MKU.Scripts.Enums;
using UnityEngine;

namespace MKU.Scripts.Tasks
{
    [Serializable]
    public class Objective
    {
        public TaskCondition taskCondition;
        [TextArea(3,20)]
        public string description;
        public List<string> Items;
        public string id;
        public bool IsComplete = false;
        [Range(0,1000)]
        public int number;

        public Objective(TaskCondition taskCondition, string description, List<string> items, string id, bool isComplete, int number)
        {
            this.taskCondition = taskCondition;
            this.description = description;
            Items = items;
            this.id = id;
            IsComplete = isComplete;
            this.number = number;
        }


        public List<string> GetItems() => Items;

        public void SetCountNumber(string o) => Items.Add(o);
    }
}