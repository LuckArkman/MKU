using System;
using UnityEngine;

namespace MKU.Scripts.MarketSystem
{
    public class MarketPlace : MonoBehaviour
    {
        Market Activemarket = null;

        public event Action ActiveMarketChange;
        
        public void SetActiveMarket(Market market)
        {
            if(Activemarket != null)
            {
                Activemarket.SetMarket(null);
            }

            Activemarket = market;
            if(Activemarket != null)
            {
                Activemarket.SetMarket(this);
            }
            if(ActiveMarketChange != null)
            {
                ActiveMarketChange();
            }


        }

        public Market GetActiveMarket()
        {
            return Activemarket;
        }
    }
}