using System.Collections.Generic;
using UnityEngine;

namespace MKU.Scripts.Dialogue
{
    public class GenericDialogue : ScriptableObject
    {
        public bool isOption = false;
        public bool playerDialogue = false;
        [TextArea(5,20)]
        public string text;
        public List<DialogueNode> children = new List<DialogueNode>();
        public Rect rect = new Rect(0, 0, 190, 100);
        public bool onEnterAction;
        public bool onExitAction;
        public EventsSystem[] condition;
    }
}