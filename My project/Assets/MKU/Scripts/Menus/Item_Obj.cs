using MKU.Scripts.ItemSystem;
using MKU.Scripts.Strucs;
using TMPro;
using UnityEngine;

namespace MKU.Scripts.Menus
{
    public class Item_Obj : InteractionBase {

        public int item_id;
        public int item_amount;
        public string item_name;
        public GameObject item_img_obj;

        [SerializeField] private GameObject rewardLabelPrefab;
        private TextMeshProUGUI rewardLabelText;
        public ItemData itemData;

        void OnCollisionEnter(Collision c) {

            if (c.collider.CompareTag("Ground")) {

                this.GetComponent<Rigidbody>().isKinematic = true;

            }

        }

        private void Update() {
            if (rewardLabelText == null) { return; }
            rewardLabelText.transform.parent.LookAt(Camera.main.transform);
        }

        public void SetItem()
        {
            throw new System.NotImplementedException();
        }
    }
}