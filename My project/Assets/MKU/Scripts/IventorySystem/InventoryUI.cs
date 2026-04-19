using MKU.Scripts.CharacterSystem;
using MKU.Scripts.Singletons;
using UnityEngine;

namespace MKU.Scripts.IventorySystem
{
    public class InventoryUI : MonoBehaviour
    {
        // CONFIG DATA
        [SerializeField] InventorySlotUI InventoryItemPrefab = null;

        // CACHE
        Inventory playerInventory;

        // LIFECYCLE METHODS
        
        public void Start()
        {
            Singleton.Instance.inventoryUI = this;
            if(playerInventory == null)playerInventory = CharSettings._Instance._charController.GetComponent<Inventory>();
            playerInventory.inventoryUpdated += Redraw;
            Redraw();
        }
        
        public void OnStart()
        {
            
            if(playerInventory == null)playerInventory = CharSettings._Instance._charController.GetComponent<Inventory>();
            playerInventory.inventoryUpdated += Redraw;
            Redraw();
        }

        // PRIVATE

        private void Redraw()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            for (int i = 0; i < playerInventory.GetSize(); i++)
            {
                var itemUI = Instantiate(InventoryItemPrefab, transform);
                CharSettings._Instance._inventorySlots.Add(itemUI);
                itemUI.Setup(playerInventory, i);
            }
        }
    }
}