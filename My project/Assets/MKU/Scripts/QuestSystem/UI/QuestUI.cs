#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MKU.Scripts.Tasks.UI
{
    public class QuestUI : MonoBehaviour
    {
        public Button _Button;
        public GameObject _questComplete;
        public _Quest _quest;
        public TextMeshProUGUI _questName, _questId;
        public Questlist _Questlist;

        public void OnStart()
        {
            _Button.onClick.AddListener(() =>
            {
                ShowInfo();
            });
        }

        private void ShowInfo()
        {
            //_Questlist.ShowPanel(_quest);
        }
    }
}
#endif