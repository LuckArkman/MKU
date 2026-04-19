using System;
using MKU.Scripts.Enums;
using UnityEngine;

namespace MKU.Scripts.Dialogue
{
    [Serializable]
    public class EventsSystem
    {
        public ActionsType _Actions = ActionsType.None;
        public string Id;
        public EventsType _EventsType = EventsType.None;
        [Range(0,1000)]
        public int quantity;
        public GameObject _GameObject;

    }
}