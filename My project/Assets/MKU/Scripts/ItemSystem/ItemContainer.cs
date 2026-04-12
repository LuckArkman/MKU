using System;
using System.Collections.Generic;
using UnityEngine;

namespace MKU.Scripts.ItemSystem
{
    [CreateAssetMenu(fileName = "ItemContainer", menuName = "CursedStone/Administration/Containers/ItemContainer", order = 0)]

    public class ItemContainer : ScriptableObject
    {
        public List<_Item> items = new ();
        public Dictionary<string, _Item> itemsRegister = new ();
        
        
        public void MakeItem<T>(T item) where T : _Item
        {
            T newNode = CreateInstance<T>();
            if (name == "") newNode.name = item.name;
            RegisterItem(item);
        }

        private void RegisterItem(_Item item)
            => itemsRegister.TryAdd(item.name, item);
    }
}