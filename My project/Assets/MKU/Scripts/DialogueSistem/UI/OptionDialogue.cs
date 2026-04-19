using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MKU.Scripts.Dialogue.UI
{
    public class OptionDialogue : MonoBehaviour
    {
        public Button option;
        public DialogueUI _DialogueUI;
        public DialogueNode _dialogueNode;
        public TextMeshProUGUI UIText;
        

        public void OnStart()
        {
            option.onClick.AddListener(() =>
            {
                OnClick();
            });
        }

        private void OnClick()
        {
            _DialogueUI._dialogueNode = _dialogueNode.children[0];
            _DialogueUI.OnNextTalk(_DialogueUI._dialogueNode);
        }
    }
}