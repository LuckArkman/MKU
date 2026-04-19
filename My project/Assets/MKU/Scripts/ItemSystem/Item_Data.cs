using System.Collections.Generic;
using UnityEngine;

namespace MKU.Scripts.ItemSystem
{
    public class Item_Data : MonoBehaviour {

        [SerializeField]
        private List<Item> item_equipment_set = new List<Item>();
        [SerializeField]
        private List<Item> item_usable_set = new List<Item>();
        [SerializeField]
        private List<Item> item_etc_set = new List<Item>();
        [SerializeField]
        private Item[] item_gold = new Item[1];

        public static Item_Data Instance;

        public Item_Data_Storage m_Item_Data_Storage;

        public void Awake() {

            Instance = this;

        }

        public Item Get_Item(int item_id) {

            return m_Item_Data_Storage.Get_Item(item_id);

        }

    }
}