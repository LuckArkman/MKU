using System;

namespace MKU.Scripts.ItemSystem
{
    [Serializable]
    public class ItemData
    {
        public int id = 0;
        public int idobject = 0;
        public int level = 0;
        public int life = 0;
        public int amount = 1;
        public int slot = 0;
        public int tokenid = 0;
        public string chestid;
        public int locked = 0;

        public bool isStackSeparator;

        public ItemData() {

        }

        public ItemData(ItemData itemData) {

            this.id = itemData.id;
            this.idobject = itemData.idobject;    
            this.level = itemData.level;
            this.life = itemData.life;
            this.amount = itemData.amount;
            this.slot = itemData.slot;
            this.tokenid = itemData.tokenid;
            this.chestid = itemData.chestid;
            this.locked = itemData.locked;

        }

        public ItemData(Item itemData) {

        }

    }
}