#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MKU.Scripts.Dialogue.Editor
{
    public class EditorDao : EditorWindow
    {
        public Dialogue selectedDialogue = null;
        public GUIStyle nodeStyle;
        public GUIStyle playerNodeStyle, playerDialogue;
        public DialogueNode draggingNode = null;
        public Vector2 draggingOffset;
        public DialogueNode creatingNode = null;
        public DialogueNode deletingNode = null;
        public DialogueNode linkingParentNode = null;
        public Vector2 scrollPosition;
        public bool draggingCanvas = false;
        public Vector2 draggingCanvasOffset;

        public float canvasSize = 4000;
        public float backgroundSize = 50;
        
        public void ProcessEvents()
        {
            if (Event.current.type == EventType.MouseDown && draggingNode == null)
            {
                draggingNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition);
                if (draggingNode != null)
                {
                    draggingOffset = draggingNode.GetRect().position - Event.current.mousePosition;
                    Selection.activeObject = draggingNode;
                }
                else
                {
                    draggingCanvas = true;
                    draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
                    Selection.activeObject = selectedDialogue;
                }
            }
            else if (Event.current.type == EventType.MouseDrag && draggingNode != null)
            {
                draggingNode.SetPosition(Event.current.mousePosition + draggingOffset);

                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseDrag && draggingCanvas)
            {
                scrollPosition = draggingCanvasOffset - Event.current.mousePosition;

                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseUp && draggingNode != null)
            {
                draggingNode = null;
            }
            else if (Event.current.type == EventType.MouseUp && draggingCanvas)
            {
                draggingCanvas = false;
            }

        }
        
        public DialogueNode GetNodeAtPoint(Vector2 point)
        {
            DialogueNode foundNode = null;
            foreach (DialogueNode node in selectedDialogue.GetAllNodes())
            {
                if (node.GetRect().Contains(point))
                {
                    foundNode = node;
                }
            }
            return foundNode;
        }
    }
}
#endif