using MKU.Scripts.CharacterSystem;
using MKU.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace MKU.Scripts.IventorySystem
{
    public class InventoryShowUI : MonoBehaviour
    {
        public Button button;

        private void Start()
        {
            button.onClick.AddListener(() => ShowInventoryUI());
        }

        private void ShowInventoryUI()
        {
            CharSettings._Instance.OnShowInventory(UIType.inventory);
        }
    }
}