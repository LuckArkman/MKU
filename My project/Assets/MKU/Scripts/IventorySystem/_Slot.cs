using System;
using MKU.Scripts.ItemSystem;

namespace MKU.Scripts.IventorySystem
{
    [Serializable]
    public class _Slot
    {
        public _Slot(){}
        public _Item item;
        public int number;

        public _Slot(_Item item, int number)
        {
            this.item = item;
            this.number = number;
        }
    }
}