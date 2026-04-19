using MKU.Scripts.ItemSystem;
using MKU.Scripts.IventorySystem;
using MKU.Scripts.Singletons;
using UnityEngine;

namespace MKU.Scripts.Strucs
{
    public class ItemFeedbackController : MonoBehaviour
    {
        public ItemFeedback itemFeedback;
        public Transform spawn;

        private void Start()
        {
            Singleton.Instance._itemFeedbackController = this;
        }

        public void OnSpawn(_Item item, int number)
        {
            var feed = Instantiate<ItemFeedback>(itemFeedback, spawn);
            feed.OnStart(number, item.displayName);
        }
    }
}