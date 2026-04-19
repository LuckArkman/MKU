using MKU.Scripts.Enums;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace MKU.Scripts.Tasks.UI
{
    public class ObjectiveUI : MonoBehaviour
    {
        public TextMeshProUGUI _descricao, _quantia;
        public Objective _Objective;
        public Image image;
        
        private void LateUpdate()
        {
            if (_Objective != null)
            {
                if (_Objective.GetItems().Count <= _Objective.number)
                {
                    if (_Objective.taskCondition == TaskCondition.Collect){
                        _quantia.text = string.Format(" ({0} / {1}) {2} - ({3}) - {4}",
                        _Objective.GetItems().Count,_Objective.number,"Colete ",_Objective.number, _Objective.id.ToString());
                    }
                    if (_Objective.taskCondition == TaskCondition.Hunting){
                        _quantia.text = string.Format(" ({0} / {1}) {2} - ({3}) - {4}",
                        _Objective.GetItems().Count, _Objective.number, "Elimine ", _Objective.number,_Objective.id.ToString());
                    }
                }
                image.gameObject.SetActive(_Objective.GetItems().Count >= _Objective.number);
                if(_Objective.taskCondition == TaskCondition.NPC) image.gameObject.SetActive(_Objective.IsComplete);
            }
        }
    }
}