using System;
using System.Diagnostics;
using MKU.Scripts.Enums;
using MKU.Scripts.Interfaces;
using MKU.Scripts.Tasks;
using UnityEngine;

namespace MKU.Scripts.Dialogue
{
    [Serializable]
    public class AIConversant
    {
        public AIConversant(){}
        public Dialogue currentDialogue;
        public IPlayerBase _playerBase;
        public QuestManager _QuestManager;
        public EventsType _Events;
        public Transform position;
        public int quantity;
        public GameObject _GameObject;

        public void OnEnventsAction(EventsSystem eventsSystem, GameObject playerBase)
        {
            UnityEngine.Debug.Log($"{nameof(OnEnventsAction)} >> {eventsSystem is null}");
            _Events = eventsSystem._EventsType;
            quantity = eventsSystem.quantity;
            _GameObject = eventsSystem._GameObject;
            if(eventsSystem._Actions == ActionsType.None){}
            if(eventsSystem._Actions == ActionsType.SendQuest) playerBase.GetComponent<IPlayerBase>().OnQuestReceive(eventsSystem.Id);
            if (eventsSystem._Actions == ActionsType.CompleteQuest) playerBase.GetComponent<IPlayerBase>().OnCompleteQuest(_QuestManager._quest);
            if (eventsSystem._Actions == ActionsType.LookCam) OnLookCam(eventsSystem.Id);
            if (eventsSystem._EventsType == EventsType.SpawnMonsters) OnSpawnMonsters(eventsSystem);
            if(eventsSystem._Actions == ActionsType.Movie){}
        }

        private void OnSpawnMonsters(EventsSystem eventsSystem)
        {
            for (int i = 0; i < eventsSystem.quantity; i++)
            {
                GameObject obj = UnityEngine.Object.Instantiate(eventsSystem._GameObject, _GameObject.transform);
                obj.transform.position += new Vector3(UnityEngine.Random.Range(0.0f, 50.0f), 0.5f, UnityEngine.Random.Range(0.0f, 50.0f));

            }
        }

        private void OnLookCam(string eventsSystemId)
        {
            
        }
    }
}