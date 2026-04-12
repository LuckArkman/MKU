using System.Threading.Tasks;
using MKU.Scripts.Enums;
using MKU.Scripts.HelthSystem;
using UnityEngine;

namespace MKU.Scripts.SkillSystem
{
    public class Skills : ScriptableObject
    {
        public KeyCode toggleKey = KeyCode.Escape;
        public AnimationType animationType = AnimationType.None;
        [Range(0.0f,10.0f)]
        public float range;
        public float time;
        public string skillID;
        public string skillToken;
        public string Name;
        public Sprite Icon;
        public int Level = 1;
        public int MaxLevel;
        public int SPCost;
        public AttackType _attackType = AttackType.None;
        public Element element = Element.Neutro;
        public Skills skill;
        public float Power;
        [TextArea(0,10)]
        public string Description;
        [Range(0.0f,180.0f)]
        public float timeRefresh;
        public GameObject _skillPrefab;
        public float parcent;

        public bool OnUse()
        {
            return time == 0;
        }

        public async void OnRefresh()
        {
            time = timeRefresh;
            while (time > 0)
            {
                parcent = time / timeRefresh;
                await Task.Delay(1000);
                time--;
            }

            await Task.CompletedTask;
        }
    }
}