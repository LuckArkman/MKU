using System;
using MKU.Scripts.ItemSystem;
using UnityEngine;

namespace MKU.Scripts.MarketSystem
{
    [Serializable]
    public class MarketItem
    {
        public _Item item;
        public int availability;
        public int sellPrice;
        public int buyPrice;
        public int quantityInTransaction;

        public MarketItem(_Item item)
        {
            this.item = item;
        }

        public int GetquatityInTransaction()
        {
            return quantityInTransaction;
        }

        public _Item GetInventoryItem()
        {
            return item;
        }
    }
}