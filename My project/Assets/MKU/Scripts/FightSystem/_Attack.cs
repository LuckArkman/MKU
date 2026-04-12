using MKU.Scripts.Enums;
using UnityEngine;

namespace MKU.Scripts.FightSystem
{
    public class _Attack : ScriptableObject
    {
        [Range(0.0f,10.0f)]
        public float range;
        public float time;
        public string attackID;
        public string attackToken;
        public AttackType _attackType = AttackType.None;
        public Element element = Element.Neutro;
        public GameObject _skillPrefab;
    }
}