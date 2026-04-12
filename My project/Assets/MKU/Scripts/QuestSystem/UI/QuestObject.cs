using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MKU.Scripts.Tasks.UI
{
    public class QuestObject : MonoBehaviour
    {
        public QuestInformation _questInformation;
        public Button _button;
        public _Quest quest;
        public TextMeshProUGUI _title;
        public List<Sprite> _questIcons;
        public Image background;

        public void QuestStatus(_Quest _quest, QuestInformation questInformation)
        {
            _questInformation = questInformation;
            quest = _quest;
            _title.text = _quest.Title;
            if(!quest.OnComplete(quest)) background.sprite = _questIcons[0];
            if(quest.OnComplete(quest)) background.sprite = _questIcons[1];
            _questInformation.OnStart(quest, this);
        }
        
        private void Start()
        {
          _button.onClick.AddListener(() => ShowQuestInfo());   
        }

        private void ShowQuestInfo()
        {
            _questInformation.OnStart(quest, this);
        }

        public void SetComplete()
        {
            background.sprite = _questIcons[1];
        }
    }
}