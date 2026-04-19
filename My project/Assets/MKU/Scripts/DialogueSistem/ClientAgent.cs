using MKU.Scripts.Enums;
using MKU.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MKU.Scripts.Tasks;
using UnityEngine;

namespace MKU.Scripts.Dialogue
{
    [Serializable]
    public class ClientAgent
    {
        public ClientAgent()
        {

        }

        public string Id;
        public Dialogue currentDialogue;
        public IPlayerBase _playerBase;
        public QuestManager _QuestManager;
        public EventsType _Events;
        public Transform position;
        public int quantity;
        public GameObject _GameObject;

        public void OnEnventsAction(EventsSystem eventsSystem, GameObject playerBase)
        {
            Debug.Log($"{nameof(OnEnventsAction)} >> {playerBase is null}");
            _Events = eventsSystem._EventsType;
            quantity = eventsSystem.quantity;
            _GameObject = eventsSystem._GameObject;
            if (eventsSystem._Actions == ActionsType.None) { }
            if (eventsSystem._Actions == ActionsType.SendQuest) AdduestInCharacter(playerBase, eventsSystem.Id);
            //if (eventsSystem._Actions == ActionsType.CompleteQuest) playerBase.GetComponent<IPlayerBase>().OnCompleteQuest(_QuestManager._quest);
            if (eventsSystem._Actions == ActionsType.LookCam) OnLookCam(eventsSystem.Id);
            if (eventsSystem._EventsType == EventsType.SpawnMonsters) OnSpawnMonsters(eventsSystem);
            if (eventsSystem._Actions == ActionsType.Movie) { }
        }
        
        async Task AdduestInCharacter(GameObject playerBase, string Id)
        {
            var quest = await playerBase.GetComponent<IPlayerBase>().OnQuestReceive(Id) as _Quest;
            if (quest != null)
            {
                List<Objectives> _objectives = new List<Objectives>();
                List<Rewards> _rewards = new List<Rewards>();
                quest._objectives.ForEach(o =>
                {
                    //_objectives.Add(new Objectives(o.taskCondition, o.description, o.Items, o.Id, o.IsComplete, o.number));
                });
                quest._rewards.ForEach(o =>
                {
                    _rewards.Add(new Rewards(o._rewardType, o.number, o.itemId));
                });
                Quest _quest = new Quest(Guid.NewGuid().ToString(),
                    quest.Title,
                    quest.task_type,
                    quest.Name,
                    quest.task_description,
                    quest.IsComplete,
                    _objectives,
                    _rewards);
                
                //await new InsertQuestInCharacter().InsertQuest(new CharQuest(Guid.NewGuid().ToString(), _quest));
            }
        }

        private void OnSpawnMonsters(EventsSystem eventsSystem)
        {
            for (int i = 0; i < eventsSystem.quantity; i++)
            {
                GameObject obj = UnityEngine.Object.Instantiate<GameObject>(eventsSystem._GameObject, _GameObject.transform);
                obj.transform.position += new Vector3(UnityEngine.Random.Range(0.0f, 50.0f), 0.5f, UnityEngine.Random.Range(0.0f, 50.0f));
            }
        }

        private void OnLookCam(string eventsSystemId)
        {

        }
    }

}
