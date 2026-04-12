using MKU.Scripts.IventorySystem;
using MKU.Scripts.SceneSystem;
using MKU.Scripts.Strucs;

namespace MKU.Scripts.DropSystem
{
    sealed class PlayerItemDrop : DropItem
    {
        public void CheckForDeathItemDrop()
        {
            if (SceneDataManager.Instance.GetCurrentScene().sceneType != MapData.SceneType.Tower)
            {
                PerformAllItemsDrop(InventoryData.Instance.GetNFTInventaryData());
            }
        }

        private async void PerformAllItemsDrop(InventoryResponseData inventory)
        {
            return; //Foi desativado por causa do modo single player que não usaria mais o drop de todos os itens, porem estava rolando um problema de crash em alguns momentos quando tinha muito item ou coisa parecida

            var data = inventory.invetoryItems;

            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].locked == 0 && data[i].slot < 1000)
                {
                    if (await InventoryData.Instance.DeleteNFTInventaryItemData(data[i]))
                    {
                        if (CallDropItem(data[i]).TryGetComponent(out DestroyForTime destroyForTime))
                        {
                            destroyForTime.time = 3600f;

                        }
                    }
                }
            }
        }
    }
}