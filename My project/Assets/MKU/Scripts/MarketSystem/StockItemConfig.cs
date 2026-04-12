using System;
using MKU.Scripts.ItemSystem;
using UnityEngine;

namespace MKU.Scripts.MarketSystem
{
    [Serializable]
    public class StockItemConfig
    {
        public StockItemConfig(){}
        public MarketItem Item;

        public StockItemConfig(MarketItem item)
        {
            Item = item;
        }
    }
}