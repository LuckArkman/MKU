namespace MKU.Scripts.IventorySystem
{
    public class InventoryItem
    {
        public InventoryItem(){}
        public int level { get; set; }
        public int position { get; set; }
        public int quantity { get; set; }

        public InventoryItem(int level, int position, int quantity)
        {
            this.level = level;
            this.position = position;
            this.quantity = quantity;
        }
    }
}