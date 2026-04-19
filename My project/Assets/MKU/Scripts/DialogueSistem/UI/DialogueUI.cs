using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using MKU.Scripts.Interfaces;

namespace MKU.Scripts.Dialogue.UI
{
    public class DialogueUI : MonoBehaviour
    {
        public ClientAgent clientAgent = new ClientAgent();
        public Button next, quit;
        public TextMeshProUGUI UIText, conversantName;
        public Dialogue currentDialogue;
        public GameObject OptionElement;
        public GameObject playerBase;
        public DialogueNode _dialogueNode;
        public AIConversant _aiConversant;
        public OptionDialogue _option;

        public List<OptionDialogue> _OptionDialogues = new List<OptionDialogue>();

        public List<GameObject> UIelements = new List<GameObject>();

        public void OnStart(AIConversant aiConversant, GameObject _playerBase)
        {
            _aiConversant = aiConversant;
            playerBase = _playerBase;
            if (_aiConversant != null) currentDialogue = _aiConversant.currentDialogue;
        }

        private void Start()
        {
            if (currentDialogue.nodes.Count > 0) _dialogueNode = currentDialogue.nodes[0];
            if (currentDialogue.nodes.Count <= 0) Debug.Log($"dialogueNode Is Null{_dialogueNode == null}");
            StartTask(_dialogueNode);
            HideElements(false);
        }

        private void HideElements(bool option)
        {
            if (option)
            {
                UIelements[0].SetActive(true);
                UIelements[1].SetActive(false);
            }
            if (!option)
            {
                UIelements[0].SetActive(false);
                UIelements[1].SetActive(true);
            }
        }

        public void QuitTalk()
        {
            playerBase.GetComponent<IPlayerBase>().SetClick(true);

            Destroy(gameObject);
        }

        public void NextTalk()
        {
            if (_dialogueNode.children.Count > 1)
            {
                if (_dialogueNode.children.Find(x => x.isOption))
                    ShowOptions(_dialogueNode.children.FindAll(x => x.isOption));
                if (!_dialogueNode.children.Find(x => x.isOption))
                {
                    _dialogueNode = _dialogueNode.children[0];
                    StartTask(_dialogueNode);
                }
            }
            else if (_dialogueNode.children.Count == 1) StartTask(_dialogueNode.children[0]);
            else if (_dialogueNode.children.Count == 0) QuitTalk();
        }

        private void OnSendQuest(EventsSystem[] condition)
        {
            Debug.Log($"{nameof(OnSendQuest)} >>{condition.Length}");
            for (int i = 0; i < condition.Length; i++)
            {
                if (condition[i]._Actions == Enums.ActionsType.SendQuest)
                {
                    playerBase.GetComponent<IPlayerBase>().OnQuestReceive(condition[i].Id);
                }
            }
            if (_dialogueNode.children.Count <= 0) Destroy(gameObject);
        }

        private void ShowOptions(List<DialogueNode> findAll)
        {
            HideElements(true);
            foreach (Transform child in OptionElement.transform)
            {
                Destroy(child.gameObject);
            }
            findAll.ForEach(o =>
            {
                OptionDialogue option = Instantiate(_option, OptionElement.transform);
                option._DialogueUI = this;
                option._dialogueNode = o;
                option.UIText.text = o.text;
                option.OnStart();
            });
        }

        private void StartTask(DialogueNode dialogueNode)
        {
            if (dialogueNode.playerDialogue) ShowDialogue(dialogueNode, "Player");
            if (!dialogueNode.playerDialogue) ShowDialogue(dialogueNode, "NPCName");
        }

        private void ShowDialogue(DialogueNode dialogueNode, string npcname)
        {
            Debug.Log($"{nameof(ShowDialogue)} >> {_aiConversant is null}");
            HideElements(false);
            UIText.text = dialogueNode.text;
            conversantName.text = npcname;

            if (dialogueNode.onEnterAction)
            {
                for (int i = 0; i < dialogueNode.condition.Length; i++)
                {
                    new ClientAgent().OnEnventsAction(dialogueNode.condition[i], playerBase);
                }
            }
        }

        internal void OnNextTalk(DialogueNode dialogueNode)
        {
            StartTask(dialogueNode);
        }
    }
}