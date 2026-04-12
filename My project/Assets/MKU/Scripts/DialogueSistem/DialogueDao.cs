using System;
using System.Collections.Generic;
using UnityEngine;

namespace MKU.Scripts.Dialogue
{
    public class DialogueDao : ScriptableObject
    {
        public List<DialogueNode> nodes = new List<DialogueNode>();
        public Vector2 newNodeOffset = new Vector2(250, 0);

        public Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();
        
        public DialogueNode MakeNode(DialogueNode parent)
        {
            DialogueNode newNode = CreateInstance<DialogueNode>();
            newNode.name = Guid.NewGuid().ToString();
            if (parent is null) {
#if UNITY_EDITOR
                parent.AddChild(newNode);
                newNode.SetPosition(parent.GetRect().position + newNodeOffset);
#endif
            }
            return newNode;
        }
    }
}