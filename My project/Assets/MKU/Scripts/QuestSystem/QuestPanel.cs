using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using MKU.Scripts.Tasks;

namespace MKU.Scripts.Tasks
{
    //[CreateAssetMenu(fileName = "QuestPanel", menuName = "CursedStone/QuestManager", order = 0)]
    public class QuestPanel : ScriptableObject
    {
        public List<_Quest> items = new List<_Quest>();
        private Dictionary<string, _Quest> _quests = new Dictionary<string, _Quest>();
        public bool show;
        public void OnInfo(string name) => items.ForEach(n =>{ if(n.name == name){} });
#if UNITY_EDITOR
        public void MakeItem(_Quest item)
        {
            _Quest newNode = CreateInstance<_Quest>();
            if (name == "") newNode.name = item.name;
            newNode.Name = item.name;
            RegisterQuest(item);
            AssetDatabase.SaveAssets();
        }

        private void RegisterQuest(_Quest item)
        => _quests.TryAdd(item.name, item);
#endif
    }
}
