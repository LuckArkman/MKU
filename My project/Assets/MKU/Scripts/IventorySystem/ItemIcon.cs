using MKU.Scripts.ItemSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MKU.Scripts.IventorySystem
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(InventoryDragItem))]
    
    public class ItemIcon : MonoBehaviour
    {
        public _Item item = null;
        public int number = 0;
        [SerializeField] GameObject textContainer = null;
        [SerializeField] TextMeshProUGUI itemNumber = null;

        // PUBLIC

        public void SetItem(_Item item)
        {
            SetItem(item, 0);
        }

        public void SetItem(_Item item, int number)
        {
            this.item = item;
            this.number = number;
            var iconImage = GetComponent<Image>();
            if (item == null)
            {
                iconImage.enabled = false;
            }
            else
            {
                iconImage.enabled = true;
                iconImage.sprite = item.GetIcon();
            }

            if (itemNumber)
            {
                if (number < 0)
                {
                    if(textContainer != null)textContainer.SetActive(false);
                }
                if (number > 0)
                {
                    if(textContainer != null)textContainer.SetActive(true);
                    if(itemNumber != null)itemNumber.text = number.ToString();
                }
            }
        }
    }
}
