using System;
using System.Collections.Generic;
using MKU.Scripts.ItemSystem;

namespace MKU.Scripts.IventorySystem
{
    [Serializable]
    public class InventoryResponseData
    {

        public List<ItemData> invetoryItems;

        public ItemData GetItemDatabyId(int _Id) {

            foreach (var dat in invetoryItems) {

                if (dat.id == _Id) {

                    return new ItemData(dat);

                }

            }

            return null;

        }

        public List<ItemData> GetItemDatabyObjectId(int _Id) {

            List<ItemData> itensbyObjectId = new List<ItemData>();

            foreach (var dat in invetoryItems) {

                if (dat.idobject == _Id) {

                    itensbyObjectId.Add(dat);

                }

            }

            return itensbyObjectId;

        }
        
        public ItemData GetItemDatabySlot(int slot) {

            foreach (var dat in invetoryItems) {

                if (dat.slot == slot) {

                    if (Item_Data.Instance.Get_Item(dat.idobject) != null) {

                        return new ItemData(dat);

                    }

                }

            }

            return null;

        }

        public Item GetItembySlot(string slot) {

            return Item_Data.Instance.Get_Item(GetItemDatabySlot(int.Parse(slot)).idobject);

        }

        public List<ItemData> GetItemBetweenSlots(int _MinSlot, int _MaxSlot) {

            return invetoryItems.FindAll((x) => {
                int itemSlot = x.slot;
                return (_MinSlot <= itemSlot && itemSlot <= _MaxSlot);
            });

        }
        
    }
}