using System;
using TMPro;
using UnityEngine;

namespace MKU.Scripts.Tasks.UI
{
    public class QuestBox : MonoBehaviour
    {
        public _Quest _quest;
        public TextMeshProUGUI _title;
        public Transform _objectives;
        public ObjectivesQuest _objectivesQuest;
        public void OnStart(_Quest quest)
        {
            if (_quest == null || _quest.quest_id != quest.quest_id)
            {
                _quest = quest;
                _title.text = _quest.Title;
                foreach (Transform child in _objectives)
                {
                    Destroy(child.gameObject);
                }
                _quest._objectives.ForEach(o =>
                {
                    var ob = Instantiate<ObjectivesQuest>(_objectivesQuest, _objectives);
                    ob.OnStart(o);
                });
            }
        }

        private void Update()
        {
            
        }
    }
}