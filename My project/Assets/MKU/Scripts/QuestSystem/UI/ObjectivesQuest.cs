using MKU.Scripts.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MKU.Scripts.Tasks.UI
{
    public class ObjectivesQuest : MonoBehaviour
    {
        public Objective _objective;
        public TextMeshProUGUI objectiveText;
        public Image completedObjective;


        public void OnStart(Objective objective)
        {
            _objective = objective;
            if (_objective != null && _objective.taskCondition == TaskCondition.Collect)objectiveText.text = $"({_objective.Items.Count / _objective.number}){_objective.description}";
            if (_objective != null && _objective.taskCondition == TaskCondition.Hunting)objectiveText.text = $"({_objective.Items.Count} / {_objective.number}){_objective.description}";
            if (_objective != null && _objective.taskCondition == TaskCondition.Hunter)objectiveText.text = $"({_objective.Items.Count / _objective.number}){_objective.description}";
            InvokeRepeating("OnUpdate", 0.1f,1.0f);
        }

        void OnUpdate()
        {
            completedObjective.enabled = _objective.Items.Count >= _objective.number;
            if (_objective.Items.Count >= _objective.number)
            {
                CancelInvoke("OnUpdate");
            }
            if (_objective.taskCondition == TaskCondition.Collect)
            {
                completedObjective.enabled = _objective.Items.Count >= _objective.number;
                objectiveText.text = $"({_objective.Items.Count} / {_objective.number}) {_objective.description}";
            }
            if (_objective.taskCondition == TaskCondition.Hunting)
            {
                completedObjective.enabled = _objective.Items.Count >= _objective.number;
                objectiveText.text = $"({_objective.Items.Count} / {_objective.number}) {_objective.description}";
            }
            if (_objective.taskCondition == TaskCondition.Hunter)
            {
                completedObjective.enabled = _objective.Items.Count >= _objective.number;
                objectiveText.text = $"({_objective.Items.Count} / {_objective.number}) {_objective.description}";
            }
        }
    }
}