using System;

namespace MKU.Scripts.CookingSystem
{
    [Serializable]
    public class UsedSlots
    {
        public int index;
        public Ingredients item;

        public UsedSlots(int index, Ingredients item)
        {
            this.index = index;
            this.item = item;
        }
    }
}