using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MKU.Scripts.Dialogue
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "MPLopes/Dialogue/New-Dialogue", order = 0)]
    public class Dialogue : DialogueDao, ISerializationCallbackReceiver
    {
        private void OnValidate()
        {
            nodeLookup.Clear();
            foreach (DialogueNode node in GetAllNodes())
            {
                nodeLookup[node.name] = node;
            }
        }
        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }
        public DialogueNode GetRootNode()
        {
            return nodes[0];
        }
        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
        {
            foreach (var childID in parentNode.GetChildren())
            {
                if (nodeLookup.ContainsKey(childID.name))
                {
                    yield return nodeLookup[childID.name];
                }
            }
        }

        public IEnumerable<DialogueNode> GetPlayerChildren(DialogueNode currentNode)
        {
            foreach (DialogueNode node in GetAllChildren(currentNode))
            {
                if (node.isOption)
                {
                    yield return node;
                }
            }
        }


        public IEnumerable<DialogueNode> GetAIChildren(DialogueNode currentNode)
        {
            foreach (DialogueNode node in GetAllChildren(currentNode))
            {
                if (!node.isOption)
                {
                    yield return node;
                }
            }
        }
        
#if UNITY_EDITOR
        public void CreateNode(DialogueNode parent)
        {
            DialogueNode newNode = MakeNode(parent);
            Undo.RegisterCreatedObjectUndo(newNode, "Created Dialogue Node");
            Undo.RecordObject(this, "Added Dialogue Node");
            AddNode(newNode);
        }

        public void DeleteNode(DialogueNode nodeToDelete){
            Undo.RecordObject(this, "Deleted Dialogue Node");
            nodes.Remove(nodeToDelete);
            OnValidate();
            CleanDanglingChildren(nodeToDelete);
            Undo.DestroyObjectImmediate(nodeToDelete);
        }
        private void AddNode(DialogueNode newNode){
            nodes.Add(newNode);
            OnValidate();
        }

        private void CleanDanglingChildren(DialogueNode nodeToDelete)
        {
            foreach (DialogueNode node in GetAllNodes())
            {
                node.RemoveChild(nodeToDelete);
            }
        }
#endif
        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (nodes.Count <= 0){
                DialogueNode newNode = MakeNode(null);
                AddNode(newNode);
            }
            if (AssetDatabase.GetAssetPath(this) != ""){
                foreach (DialogueNode node in GetAllNodes())
                {
                    if (AssetDatabase.GetAssetPath(node) == "")
                    {
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
#endif
        }
        public void OnAfterDeserialize(){}
    }
}