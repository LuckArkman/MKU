using MKU.Scripts.Enums;
using MKU.Scripts.Interfaces;
using MKU.Scripts.ItemSystem;
using UnityEngine;

namespace MKU.Scripts.IventorySystem
{
    public class ItemTooltipSpawner : TooltipSpawner
    {
        public override bool CanCreateTooltip()
        {
            _Item item = GetComponent<IItemHolder>().GetItem();
            if (!item) return false;
            return true;
        }

        public override void UpdateTooltip(GameObject tooltip)
        {
            var itemToolTip = tooltip.GetComponent<ItemTooltip>();
            if (!itemToolTip) return;

            var item = GetComponent<IItemHolder>().GetItem();
            if (item != null) itemToolTip.Setup(item);
        }
    }
}