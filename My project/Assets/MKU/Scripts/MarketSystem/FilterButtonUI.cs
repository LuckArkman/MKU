using MKU.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace MKU.Scripts.MarketSystem
{
    public class FilterButtonUI  : MonoBehaviour
    {
        [SerializeField]
        public ItemCategory category = ItemCategory.None;

        public Button button;
        public Market currentMarket;

        private void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(selectFilter);
        }

        public void SetMarket(Market currentMarket)
        {
            this.currentMarket = currentMarket;

        }

        public void RefreshUI()
        {
            button.interactable = currentMarket.GetFilter() != category;
        }

        void selectFilter()
        {
            currentMarket.SelectFilter(category);
        }
    }
}