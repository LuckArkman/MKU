using System;
using System.Collections.Generic;
using MKU.Scripts.Enums;
using MKU.Scripts.FinanceSystem;
using MKU.Scripts.ItemSystem;
using MKU.Scripts.IventorySystem;
using MKU.Scripts.Singletons;
using UnityEngine;

namespace MKU.Scripts.MarketSystem
{
    public class Market : MonoBehaviour, ISerializationCallbackReceiver
    {
        [TextArea]
        public string Id;
        
        [SerializeField] string marketName;
        [Range(0.0f, 30.0f)]
        [SerializeField] float SellingDiscountParcentage = 0.15f;
        public List<StockItemConfig> stockConfig = new ();

        public string itemId;
        [ContextMenu(nameof(OnAddItemInMarket))]
        public void OnAddItemInMarket()
        {
            var items = Resources.Load("ItemContainer") as ItemContainer;
            items.items.ForEach(i =>
            {
                if(i.itemID == itemId) stockConfig.Add(new StockItemConfig(new MarketItem(i)));
            });
        }
        Dictionary<_Item, int> transaction = new ();
        
        Dictionary<_Item, int> stock = new ();
        MarketPlace currentMarket = null;
        public bool isBuyingmode = true;
        ItemCategory filter = ItemCategory.None;
        public event Action OnChange;

        private void Awake()
        {
            foreach(StockItemConfig config in stockConfig)
            {
                stock[config.Item.item] = config.Item.availability;
            }
        }

        private void Start()
        {
            Singleton.Instance._market = this;
        }

        public void SetMarket(MarketPlace market)
        {
            this.currentMarket = market;
        }

        public IEnumerable<MarketItem> GetFilteredItems()
        {
            foreach(MarketItem marketItem in GetAllItems())
            {
                _Item item = marketItem.GetInventoryItem();
                if(filter == ItemCategory.None || item.GetCategory() == filter)
                {
                    yield return marketItem;
                }
            }
        }

        public List<MarketItem> GetAllItems()
        {
            List<MarketItem> items =  new List<MarketItem>();
            stockConfig.ForEach(s =>
            {
                items.Add(s.Item);
            });
            return items;
        }

        private int GetAvailability(_Item config)
        {
            if (isBuyingmode)
            {
                return stock[config];
            }
            return countItemsInInventory(config);
        }

        public int countItemsInInventory(_Item item)
        {
            Inventory inventory = currentMarket.GetComponent<Inventory>();
            if (inventory == null) return 0;

            int total = 0;
            for(int i = 0; i < inventory.GetSize(); i++)
            {
                if(inventory.GetItemInSlot(i) == item)
                {
                    total += inventory.GetNumberInSlot(i);
                }
            }
            return total;
        }

        int GetPriceDiscount(StockItemConfig config)
        {
            if (isBuyingmode)
            {
                return (int)(config.Item.buyPrice);
            }
            return (int)(config.Item.sellPrice);
        }

        public void SelectFilter(ItemCategory category)
        {
            filter = category;
            print(category);

            if(OnChange != null)
            {
                OnChange();
            }
        }

        public ItemCategory GetFilter()
        {
            return filter;
        }

        public void ConfirmTransaction()
        {
            Inventory marketInventory = currentMarket.GetComponent<Inventory>();
            FinanceController silver = currentMarket.GetComponent<FinanceController>();
            if (marketInventory == null || silver == null) return;


            foreach(MarketItem marketItem in GetAllItems())
            {
                _Item item = marketItem.GetInventoryItem();
                int quantity = marketItem.GetquatityInTransaction();
                int price = marketItem.buyPrice;
                for (int i = 0; i < quantity; i++)
                {
                    if (isBuyingmode)
                    {
                        BuyItem(marketInventory, silver, item, price);
                    }
                }
            }
        }
        private void SellItem(Inventory marketInventory, GameObject action, _Item item, int price)
        {
            int slot = FindFirstItemSlot(marketInventory, item);
            if (slot == -1) return;
            
            AddTransaction(item, -1);
            marketInventory.RemoveFromSlot(slot, 1);
            stock[item]++;
            //action.GetComponent<ActionHouseSilver>().UpdateBalance(price);          
        }

        private int FindFirstItemSlot(Inventory marketInventory, _Item item)
        {
            for(int i = 0; i < marketInventory.GetSize(); i++)
            {
                if(marketInventory.GetItemInSlot(i) == item)
                {
                    return i;
                }
            }
            return -1;
        }
        private void BuyItem(Inventory marketInventory, FinanceController silver, _Item item, int price)
        {
            silver.GetBalance();
            int balance = silver._Csts;
            if (balance < price) return;

                bool success = marketInventory.AddToFirstEmptySlot(item, 1);
            if (success)
            {

                stock[item]--;
                silver.UpdateBalance(price);
                AddTransaction(item, -1);
            }
        }

        public void SelectMode(bool isBuying)
        {
            isBuyingmode = isBuying;
            if(OnChange != null)
            {
                OnChange();
            }
        }

        public bool IsBuyingmode() 
        {
            return isBuyingmode;
        }

        public bool CanTransaction()
        {
            if (IsTransactionEmpty()) return false;
            if (!HasSuficientFounds()) return false;
            if (!HasSuficientSpace()) return false;
            return true;
        }

        public string GetMarketName()
        {
            return marketName;
        }

        public float TransactionTotal()
        {
            float total = 0;
            foreach(MarketItem item in GetAllItems())
            {
                total += item.buyPrice * item.GetquatityInTransaction();
            }


            return total;
        }

        public void AddTransaction(_Item item, int quatity)
        {
            if (!transaction.ContainsKey(item))
            {
                transaction[item] = 0;
            }

            int availability = GetAvailability(item);
            if (transaction[item] + quatity > availability)
            {
                transaction[item] = availability;
            }
            else
            {
                transaction[item] += quatity;
            }

            if (transaction[item] <= 0)
            {
                transaction.Remove(item);
            }

            if(OnChange != null)
            {
                OnChange();
            }
        }

        public InteractCursor GetCursor()
        {
            return InteractCursor.Interact;
        }

        public bool HandleRaycast(CharacterController control)
        {
            if(Input.GetMouseButtonDown(0))
            {
                control.GetComponent<MarketPlace>().SetActiveMarket(this);
            }
            return true;
        }

        public bool HasSuficientFounds()
        {
            FinanceController silver = currentMarket.GetComponent<FinanceController>();
            if (silver == null) return false;
            silver.OnStart();
            int balance = silver._Csts;
            return balance >= TransactionTotal();
            return false;
        }
        

        public bool IsTransactionEmpty()
        {
            return transaction.Count == 0;
        }

        public bool HasSuficientSpace()
        {
            bool result = false;
            Inventory inventory = currentMarket.GetComponent<Inventory>();
            if (inventory == null) return result;

            List<_Item> flatItems = new List<_Item>();
            foreach(MarketItem marketItem in GetAllItems())
            {               
                _Item item = marketItem.GetInventoryItem();
                int quantity = marketItem.GetquatityInTransaction();
                for(int i = 0; i < quantity; i++)
                {
                    flatItems.Add(item);
                }
            }
            result = inventory.HasSpaceFrom(flatItems);
            return result;
        }
        public void OnBeforeSerialize()
            => Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;

        public void OnAfterDeserialize(){}
    }
}