using MKU.Scripts.ItemSystem;
using MKU.Scripts.Menus;
using UnityEngine;

namespace MKU.Scripts.DropSystem
{
    public abstract class DropItem : MonoBehaviour
    {
        [SerializeField] protected GameObject itemPrefab;
        protected readonly float powerImpulse = 1f;

        private Vector3 GetDirection()
        {
            return new(Random.Range(-1f, 1f) * 1.5f * powerImpulse, powerImpulse, Random.Range(-1f, 1f) * 1.5f * powerImpulse);
        }

        public GameObject CallDropItem(ItemData itemData)
        {
            var index = itemData.idobject;
            var go = Instantiate(itemPrefab, transform.position + Vector3.up, Quaternion.identity);

            go.GetComponent<Rigidbody>().AddForce(GetDirection(), ForceMode.Impulse);

            var itemObj = go.GetComponent<Item_Obj>();
            itemObj.itemData = itemData;

            var item = Item_Data.Instance.Get_Item(index);
            //itemObj.item_id = item.itemID;
            itemObj.SetItem();

            return go;
        }

        public Item_Obj CallDropItem(int itemID)
        {
            var go = Instantiate(itemPrefab, transform.position + Vector3.up, Quaternion.identity);
            go.GetComponent<Rigidbody>().AddForce(GetDirection(), ForceMode.Impulse);

            var itemObj = go.GetComponent<Item_Obj>();
            itemObj.item_id = itemID;
            itemObj.SetItem();

            return itemObj;
        }
    }
}