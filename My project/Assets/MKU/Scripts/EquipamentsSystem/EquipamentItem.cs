namespace MKU.Scripts.EquipamentsSystem
{
    public class EquipamentItem
    {
        public EquipamentItem(){}
        public int quantity { get; set; }
        public int position { get; set; }

        public EquipamentItem(int quantity, int position)
        {
            this.quantity = quantity;
            this.position = position;
        }
    }
}