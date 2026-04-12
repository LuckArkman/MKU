using System.Collections.Generic;
using System.Threading.Tasks;
using MKU.Scripts.Enums;
using MKU.Scripts.Singletons;
using UnityEngine;
using UnityEngine.UI;

namespace MKU.Scripts.Tasks.UI
{
    public class QuestPanel : MonoBehaviour
    {
        public _Quest quest;
        public Transform panel;
        public QuestInformation _questInformation;
        public List<_Quest> _Quests = new List<_Quest>();
        public Button close;
        public QuestObject _questObject;
        public Transform _questList;
        public QuestInfoUI _QuestInfoUI;
        public bool show;
        public Transform player;

        public void Start()
        {
            show = true;
            close.onClick.AddListener(() => Onclose());
            for (int i = 0; i < _questList.childCount; i++) Destroy(_questList.GetChild(i).gameObject);
            var component = Singleton.Instance.questManager;
            RestoreQuest(component);

        }

        private async Task RestoreQuest(QuestManager component)
        {
            if (component != null)
            {
                if (component._questLast.Count > 0)
                {
                    for (int i = 0; i < _questList.childCount; i++)
                    {
                        Destroy(_questList.GetChild(i).gameObject);
                    }
                    component._questLast.ForEach(q =>
                    {
                        var obj = Instantiate<QuestObject>(_questObject, _questList);
                        obj.background.sprite = obj._questIcons[1];
                        _questInformation.OnStart(q, obj);
                        obj.QuestStatus(q, _questInformation);
                    });
                }
                if (component.quest != null)
                {
                    _questInformation.gameObject.SetActive(true);

                    var obj = Instantiate<QuestObject>(_questObject, _questList);
                    _questInformation.OnStart(component.quest, obj);
                    obj.QuestStatus(component.quest, _questInformation);
                }
            }await Task.CompletedTask;
        }

        void Onclose()
        {
            var qp = Singleton.Instance._inputManager._inputSettings.Find(q => q.ui == UIType.questUI);
            qp.show = false;
            Destroy(this.gameObject);
        }
    }
}