using System;

namespace MKU.Scripts.Strucs
{
    [Serializable]
    public class ItemDropCollection
    {
        public ItemDropCollection(){}
        public string ItemId;
        public int number = 1;

        public ItemDropCollection(string itemId, int number)
        {
            ItemId = itemId;
            this.number = number;
        }
    }
}