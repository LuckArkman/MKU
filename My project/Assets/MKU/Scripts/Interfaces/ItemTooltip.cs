using MKU.Scripts.Enums;
using MKU.Scripts.ItemSystem;
using TMPro;
using UnityEngine;

namespace MKU.Scripts.Interfaces
{
public class ItemTooltip : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI titleText, LevelDisplay = null;
        [SerializeField] TextMeshProUGUI budyText = null;
        [SerializeField] GameObject containerDescription;
        int OnLevel;

        public void Setup(_Item item)
        {
            switch(item.GetCategory())
            {
                case ItemCategory.Equipable:
                    {
                        containerDescription.SetActive(false);
                        LevelDisplay.text = "";
                        titleText.text = item.GetDisplayName();
                        budyText.text = item.description;
                        break;
                    }
                case ItemCategory.Consumable:
                    {
                        containerDescription.SetActive(false);
                        LevelDisplay.text = "";
                        titleText.text = item.GetDisplayName();
                        budyText.text = item.description;
                        break;
                    }
                case ItemCategory.Usable:
                    {
                        containerDescription.SetActive(false);
                        LevelDisplay.text = "";
                        titleText.text = item.GetDisplayName();
                        budyText.text = item.description;
                        break;
                    }
            }
        }
    }
}
