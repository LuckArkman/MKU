using MKU.Scripts.ItemSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MKU.Scripts.MarketSystem
{
    public class RowUI : MonoBehaviour
    {
        public Image IconField;
        public int transaction;
        public TextMeshProUGUI nameField, availabilityField, price, quantity;

        public Market currentMarket = null;
        public _Item item = null;
        
        public void Setup(Market currentMarket, MarketItem _item, bool isBuyingmode)
        {
            if (isBuyingmode)
            {
                this.currentMarket = currentMarket;
                this.item = _item.item;
                IconField.sprite = item.icon;
                nameField.text = item.displayName;
                availabilityField.text = $"{_item.availability}";
                price.text = $"{_item.buyPrice}";
                quantity.text = $"{_item.quantityInTransaction}";
            }
            if (!isBuyingmode)
            {
                this.currentMarket = currentMarket;
                item = _item.item;
                IconField.sprite = item.GetIcon();
                nameField.text = item.displayName;
                availabilityField.text = $"{_item.availability}";
                price.text = $"{_item.sellPrice}";
                quantity.text = $"{_item.quantityInTransaction}";
            }
        }

        public void Add()
        {
            currentMarket.AddTransaction(item, transaction);
        }

        public void Remove()
        {
            currentMarket.AddTransaction(item, -transaction);
        }
    }
}