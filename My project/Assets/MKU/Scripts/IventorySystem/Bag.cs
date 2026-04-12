using System.Collections.Generic;

namespace MKU.Scripts.IventorySystem
{
    public class Bag
    {
        public Bag() {}
        public Bag(List<InventoryItem> items)
        {
            this.items = items;
        }
        public List<InventoryItem> items { get; set; }
    }
}