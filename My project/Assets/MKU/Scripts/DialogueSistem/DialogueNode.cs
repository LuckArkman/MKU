using System.Collections.Generic;
using MKU.Scripts.Interface;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MKU.Scripts.Dialogue
{
    [CreateAssetMenu(fileName = "FILENAME", menuName = "MENUNAME", order = 0)]
    public class DialogueNode : GenericDialogue
    {
        public Rect GetRect()
        {
            return rect;
        }

        public string GetText()
        {
            return text;
        }

        public List<DialogueNode> GetChildren()
        {
            return children;
        }
        public bool IsPlayerOption()
        {
            return isOption;
        }
        
        public bool IsPlayerDialogue()
        {
            return playerDialogue;
        }

        public bool GetOnEnterAction()
        {
            return onEnterAction;
        }

        public bool GetOnExitAction()
        {
            return onExitAction;
        }
#if UNITY_EDITOR
        public void SetPosition(Vector2 newPosition)
        {
            Undo.RecordObject(this, "Move Dialogue Node");
            rect.position = newPosition;
            EditorUtility.SetDirty(this);
        }

        public void SetText(string newText)
        {
            if (newText != text)
            {
                Undo.RecordObject(this, "Update Dialogue Text");
                text = newText;
                EditorUtility.SetDirty(this);
            }
        }

        public void AddChild(DialogueNode childID)
        {
            Undo.RecordObject(this, "Add Dialogue Link");
            children.Add(childID);
            EditorUtility.SetDirty(this);
        }

        public void RemoveChild(DialogueNode childID)
        {
            Undo.RecordObject(this, "Remove Dialogue Link");
            children.Remove(childID);
            EditorUtility.SetDirty(this);
        }

        public void SetPlayerSpeaking(bool newIsPlayerSpeaking)
        {
            Undo.RecordObject(this, "Change Dialogue Speaker");
            isOption = newIsPlayerSpeaking;
            EditorUtility.SetDirty(this);
        }
#endif

    }
}