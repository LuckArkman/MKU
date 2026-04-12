using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MKU.Scripts.Enums;
using MKU.Scripts.ItemSystem;
using MKU.Scripts.SceneSystem;
using MKU.Scripts.Strucs;
using Mono.Cecil.Cil;
using UnityEditor;
using UnityEngine;

namespace MKU.Scripts.IventorySystem
{
    public class InventoryData : MonoBehaviour {

        public Guid guid { get; set; }

        public static InventoryData Instance { get; private set; }

        [SerializeField]
        private InventoryResponseData InventoryResponseData;
        public InventoryResponseData m_InventoryResponseData { get { return InventoryResponseData; } private set { InventoryResponseData = value; } }

        public static event Action OnLoadInventory;

        public string LastInventoryUpdateRequest { get; private set; }

        public ExecutionQueue _queue = new ExecutionQueue();
        private Task<bool> CallInventoryTask;

        public int TaskOnQueue;

        private void Awake() {

            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            } else {
                Instance = this;
            }

        }

        #region GetFuncitions

        public ItemData GetItemDataById(int id) => m_InventoryResponseData.GetItemDatabyId(id);
        public List<ItemData> GetItemDataByObjectId(int id) => m_InventoryResponseData.GetItemDatabyObjectId(id);

        public ItemData GetItemDataBySlot(int slot) => m_InventoryResponseData.GetItemDatabySlot(slot);
        public ItemData GetNotNullItemDataBySlot(int slot) => m_InventoryResponseData.GetItemDatabySlot(slot);

        public List<ItemData> GetItemBetweenSlots(int _MinSlot, int _MaxSlot) => m_InventoryResponseData.GetItemBetweenSlots(_MinSlot, _MaxSlot);

        public InventoryResponseData GetNFTInventaryData() {
            return m_InventoryResponseData;
        }

        #endregion

        //Set não está sendo utilizado ainda
        #region SetFuncitions

        public async Task SetPosition(int _ItemID, int _SlotIndex) {

            var itemData = GetItemDataById(_ItemID);
            itemData.slot = _SlotIndex;
            await UpdateNFTInventaryItemData(itemData);

        }

        public async Task SetLife(int _ItemID, int _LifeAmount) {

            var itemData = GetItemDataById(_ItemID);
            itemData.life = _LifeAmount;
            await UpdateNFTInventaryItemData(itemData);

        }

        public async Task SetAmount(int _ItemID, int _ItemAmount) {

            var itemData = GetItemDataById(_ItemID);
            itemData.amount = _ItemAmount;
            await UpdateNFTInventaryItemData(itemData);

        }

        public async Task<int> RemoveAsyncAmount(int _ItemID, int _ItemAmount) {

            var itemData = GetItemDataById(_ItemID);

            if (_ItemAmount >= itemData.amount) {

                await DeleteNFTInventaryItemData(itemData);

                return _ItemAmount - itemData.amount;

            }

            await SetAmount(_ItemID, itemData.amount - _ItemAmount);
            return 0;

        }

        #endregion

        #region CallInventory

        [ContextMenu("CallInventory")]
        public void CallInventory() {

            _ = CallInventoryAwaiter();

        }

        public async Task<bool> CallInventoryAwaiter() {

            UpdatedTime();

            if (CallInventoryTask == null || CallInventoryTask.IsCompleted) {

                CallInventoryTask = CallAsyncInventory();

            }

            return await CallInventoryTask;

        }

        public async Task<bool> WaitInventoryCalls() {

            return await CallInventoryTask;

        }

        private async Task<bool> CallAsyncInventory(int _TryCycle = 0)
            =>true;

        #endregion

        #region Insert

        public async Task<ItemData> InsertItemAsync(ItemData itemData)
            => null;

        #endregion

        #region Update

        [ContextMenu("Update")]
        public async Task UpdateNFTInventaryData() {

            string data = JsonUtility.ToJson(m_InventoryResponseData);
            Task<(string, long)> queueItem = _queue.Run(() => Endpoints.SendDataNewAPICode(data, Endpoints.UpdateItem));

            await CallInventoryAwaiter();

            (string result, long code) = await queueItem;
            
        }

        public async Task UpdateNFTInventaryItemData(ItemData itemData) {

            for (int i = 0; i < m_InventoryResponseData.invetoryItems.Count; i++) {

                if (m_InventoryResponseData.invetoryItems[i].id == itemData.id) {

                    m_InventoryResponseData.invetoryItems[i] = itemData;

                    await UpdateNFTInventaryData();

                    return;

                }

            }

            m_InventoryResponseData.invetoryItems.Add(itemData);
            await UpdateNFTInventaryData();

        }

        #endregion

        #region Destroy

        public async Task<bool> DeleteNFTInventaryItemData(ItemData itemData) {

            string data = "{\"idList\":" + "[" + $"{itemData.id}" + "]" + "}";

            Task<(string, long)> queueItem = _queue.Run<(string, long)>(() => Endpoints.SendDataNewAPICode(data, Endpoints.DeleteItem));

            await CallInventoryAwaiter();

            (string result, long code) = await queueItem;

            return Endpoints.CheckValidData(result, Endpoints.DeleteItem, code);

        }

        public async Task<bool> DeleteNFTInventaryItemData(int[] itemIdList) {

            string data = "{\"idList\":" + "[";

            for (int i = 0; i < itemIdList.Length; i++) {
                data += $"{itemIdList[i]},";
            }

            data = data.Remove(data.Length - 1);
            data += "]" + "}";

            Task<(string, long)> queueItem = _queue.Run<(string, long)>(() => Endpoints.SendDataNewAPICode(data, Endpoints.DeleteItem));

            await CallInventoryAwaiter();

            (string result, long code) = await queueItem;

            return Endpoints.CheckValidData(result, Endpoints.DeleteItem, code);

        }

        public async Task<bool> DeleteNFTInventaryItemData(ItemData[] itemIdList) {

            string data = "{\"idList\":" + "[";

            for (int i = 0; i < itemIdList.Length; i++) {
                data += $"{itemIdList[i].id},";
            }

            data = data.Remove(data.Length - 1);
            data += "]" + "}";

            Task<(string, long)> queueItem = _queue.Run<(string, long)>(() => Endpoints.SendDataNewAPICode(data, Endpoints.DeleteItem));

            await CallInventoryAwaiter();

            (string result, long code) = await queueItem;

            return Endpoints.CheckValidData(result, Endpoints.DeleteItem, code);

        }

        #endregion

        #region Utility

        private void UpdatedTime() {

            LastInventoryUpdateRequest = System.DateTime.Now.ToString();

        }

        //check de armas
        public bool CheckPlayerWeapons() {

            InventoryResponseData inventory = InventoryData.Instance.GetNFTInventaryData();
            int activeWeapons = 0;

            for (int i = 0; i < inventory.invetoryItems.Count; i++) {
                float weaponLife = inventory.invetoryItems[i].life;

                if (inventory.invetoryItems[i].tokenid != 0 && weaponLife > 0)
                    activeWeapons++;
            }

            return activeWeapons > 0;

        }

        #endregion

        #region EditorCheats

        private void Update() {

            if (Input.GetKeyDown(KeyCode.Alpha0)) {

                print("Alpha0");
                MarketShop();

            }

        }

        [ContextMenu("Insert Character Itens")]
        public void InsertCharacterItens() {

            RemoveAllItemCheatUse();
            MageStaffKIT();
            SurvivalKitCheat();

        }


        [ContextMenu("Market Shop")]
        public async void MarketShop() {

        }

        public ItemData itemDatass;

        [ContextMenu("ItemCheat")]
        public async void ItemCheat() {

            await InsertItemAsync(itemDatass);

            await CallInventoryAwaiter();

        }

        public async void ItemCheat(ItemData itemDatas) {

            await InsertItemAsync(itemDatas);

            await CallInventoryAwaiter();

        }

        [ContextMenu("ConsItemCheat")]
        public async void ItemCheatUse() {

            ItemData itemCheatInfo = new ItemData(itemDatass);
            itemCheatInfo.idobject = 1004;
            await InsertItemAsync(itemCheatInfo);
            itemCheatInfo.idobject = 1006;
            await InsertItemAsync(itemCheatInfo);

        }

        [ContextMenu("SurvivalKitCheat")]
        public async void SurvivalKitCheat() {

            ItemData itemCheatInfo = new ItemData();
            itemCheatInfo.amount = 10;
            itemCheatInfo.idobject = 3003;
            itemCheatInfo.slot = 23;
            InsertItemAsync(itemCheatInfo);
            itemCheatInfo.slot = 24;
            InsertItemAsync(itemCheatInfo);
            itemCheatInfo.slot = 25;
            InsertItemAsync(itemCheatInfo);

        }

        public async void Tools() {

            int Slot = 1;
            int idobject = 2076;

            ItemData itemCheatInfo = new ItemData();
            itemCheatInfo.amount = 1;

            for (idobject = 2076, Slot = 1; idobject < 2080; idobject++, Slot++) {
                itemCheatInfo.idobject = idobject;
                itemCheatInfo.slot = Slot;
                InsertItemAsync(itemCheatInfo);
            }
            
            for (idobject = 2081, Slot = 6; idobject < 2085; idobject++, Slot++) {
                itemCheatInfo.idobject = idobject;
                itemCheatInfo.slot = Slot;
                InsertItemAsync(itemCheatInfo);
            }
            
            for (idobject = 2086, Slot = 11; idobject < 2090; idobject++, Slot++) {
                itemCheatInfo.idobject = idobject;
                itemCheatInfo.slot = Slot;
                InsertItemAsync(itemCheatInfo);
            }
            
            for (idobject = 2091, Slot = 16; idobject < 2095; idobject++, Slot++) {
                itemCheatInfo.idobject = idobject;
                itemCheatInfo.slot = Slot;
                InsertItemAsync(itemCheatInfo);
            }

        }

        public async void MageStaffKIT() {

            int Slot = 1;

            ItemData itemCheatInfo = new ItemData();
            itemCheatInfo.amount = 1;
            itemCheatInfo.idobject = 2065;
            itemCheatInfo.slot = Slot;
            InsertItemAsync(itemCheatInfo);

            int idobject = 2068;
            Slot++;

            for (; idobject < 2076; idobject++, Slot++) {

                itemCheatInfo.idobject = idobject;
                itemCheatInfo.slot = Slot;
                InsertItemAsync(itemCheatInfo);

            }

            itemCheatInfo.idobject = 2076;
            itemCheatInfo.slot = 21;
            InsertItemAsync(itemCheatInfo);
            itemCheatInfo.idobject = 2081;
            itemCheatInfo.slot = 22;
            InsertItemAsync(itemCheatInfo);
            itemCheatInfo.idobject = 2086;
            itemCheatInfo.slot = 23;
            InsertItemAsync(itemCheatInfo);
            itemCheatInfo.idobject = 2091;
            itemCheatInfo.slot = 24;
            InsertItemAsync(itemCheatInfo);
            
            itemCheatInfo.idobject = 2024;
            itemCheatInfo.slot = 16;
            InsertItemAsync(itemCheatInfo);
            itemCheatInfo.idobject = 2034;
            itemCheatInfo.slot = 17;
            InsertItemAsync(itemCheatInfo);
            itemCheatInfo.idobject = 2041;
            itemCheatInfo.slot = 18;
            InsertItemAsync(itemCheatInfo);
            itemCheatInfo.idobject = 2051;
            itemCheatInfo.slot = 19;
            InsertItemAsync(itemCheatInfo);
            itemCheatInfo.idobject = 2055;
            itemCheatInfo.slot = 20;
            InsertItemAsync(itemCheatInfo);

        }

        [ContextMenu("RemoveItemCheat")]
        public async void RemoveItemCheatUse() {

            string data = "{\"idList\":" + "[" + $"{itemDatass.id}" + "]" + "}";
            string result = await Endpoints.SendDataNewAPI(data, Endpoints.DeleteItem);

            await CallInventoryAwaiter();

        }

        [ContextMenu("RemoveAllItemCheat")]
        public async void RemoveAllItemCheatUse() {

            if (InventoryResponseData.invetoryItems.Count > 0) {

                string id = InventoryResponseData.invetoryItems[0].id.ToString();

                for (int i = 1; i < InventoryResponseData.invetoryItems.Count; i++) {

                    id += "," + InventoryResponseData.invetoryItems[i].id;

                }

                string data = "{\"idList\":" + "[" + $"{id}" + "]" + "}";
                string result = await Endpoints.SendDataNewAPI(data, Endpoints.DeleteItem);

                await CallInventoryAwaiter();

            }

        }

        #endregion

    }
}