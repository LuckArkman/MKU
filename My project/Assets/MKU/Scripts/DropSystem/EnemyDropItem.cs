using System.Collections.Generic;
using MKU.Scripts.Menus;
using MKU.Scripts.SceneSystem;
using MKU.Scripts.Strucs;
using UnityEngine;

namespace MKU.Scripts.DropSystem
{
    public class EnemyDropItem : DropItem
    {
        [SerializeField]
        private List<Drop> item_Drop_List = new List<Drop>();
        [SerializeField] private double cst = 0.02;
        public List<Drop> Item_Drop_List
        {
            get { return item_Drop_List; }
            set { item_Drop_List = value; }
        }

        public double CST => cst;

        public void AddDropItemToList(Drop newDrop)
        {
            item_Drop_List.Add(newDrop);
        }

        public void CallPerformEnemyDrop()
        {
            PerformEnemyDrop(100);
        }

        public List<Item_Obj> PerformEnemyDrop(float destroyTime = 0)
        {
            List<Item_Obj> list = new List<Item_Obj>();
            double value = SceneDataManager.Instance.GetCurrentScene().sceneType == MapData.SceneType.Cave ? (cst * 2) : cst;

            for (int i = 0; i < item_Drop_List.Count; i++)
            {
                float percentResult = Random.Range(0, 100);

                if (percentResult < item_Drop_List[i].percentDrop)
                {
                    var item = CallDropItem(item_Drop_List[i].item_id);
                    list.Add(item);
                }
            }

            return list;
        }

        public void SetCST(double amount)
        {
            cst = amount;
        }
    }
}