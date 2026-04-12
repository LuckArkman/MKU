using System;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using MKU.Scripts.Models;
using UnityEngine;
using UnityEngine.Networking;

namespace MKU.Scripts.Strucs
{
    public static class UtilityLinks {

        public static readonly string NFTBOX = "https://marketplace.cursedstone.com/";

    }

    public static class Endpoints {

        public static readonly string TimeDateString = "yyyy-MM-ddTHH:mm:ss.fffZ";

        //Account
        public static readonly string DeleteAccount = "/account/delete_account.php";
        //public static readonly string SelectAccount = "https://cursed-stone-api.vercel.app/api/login";

        public static readonly string SelectAccount = "https://backend-cst.herokuapp.com/user/login";

        //NFT
        public static readonly string SelectAllNFTsDev = "https://cursed-stone-nft-box-inv-dev.vercel.app/api/moralis/getByAddress?address=";
        public static readonly string SelectAllNFTsProd = "https://cursed-stone-nft-box-inv.vercel.app/api/moralis/getByAddress?address=";
        //public static readonly string SelectAllNFTs = "http://backend-cst-dev.herokuapp.com/character/selectAllNft";

        //Character
#if UNITY_EDITOR

        public static readonly string InsertCharacter = "&/character/insert";

#endif

        public static readonly string SelectAllCharacters = "&/character/selectAllCharacters";

        public static readonly string SelectCharacterStatus = "&/character/selectCharacterStatus";
        public static readonly string UpdateCharacter = "&/character/updateCharacter";

        //Character Rent
        public static readonly string SelectCharacterRent = "&/character/selectCharacterRent";
        public static readonly string UpdateCharacterRent = "&/character/updateCharacterRent";

        //Inventory
        public static readonly string DeleteItem = "&/inventory/delete";
        public static readonly string InsertItem = "&/inventory/insert";
        public static readonly string SelectAllItems = "&/inventory/selectAll";
        public static readonly string UpdateItem = "&/inventory/update";

        //Quest
        public static readonly string SelectQuest = "&/quest/selectQuest";
        public static readonly string SelectDailyQuest = "&/quest/selectDailyQuest";
        public static readonly string SelectAllQuestLog = "&/quest/selectAllQuestLogs";
        public static readonly string SelectTasksLog = "&/task/selectTaskLog";
        public static readonly string InsertQuestLog = "&/quest/insertQuestLog";
        public static readonly string InsertTaskLog = "&/task/insertTaskLog";

        //Gather
        public static readonly string SelectAllGatherLog = "&/collectible/selectAllCollectibles";
        public static readonly string InsertGatherLog = "&/collectible/inserCollectibles";

        //Daily Reward
        public static readonly string InsertDailyRewardCollected = "&/dailyRewardStreak/currentUserStreak";
        public static readonly string CurrentUserDailyStreak = "&/dailyRewardStreak/currentUserStreak";
        public static readonly string SelectDailyReward = "&/dailyRewardStreak/currentUserStreak";

        //Timestamp
        public static readonly string GetTimestamp = "&/character/getTimestamp";

        //Events
        public static readonly string InsertEvent01 = "&/interactions/insert_interaction";
        public static readonly string SelectEvent01 = "&/interactions/select_interactions_owner";

        //Coin History
        public static readonly string InsertCoinHistoryLog = "&/transactionHistory/insertTransaction";

        public static readonly string SelectCoinHistoryLogById = "&/transactionHistory/selectTransactionById";
        public static readonly string SelectCoinHistoryLogByType = "&/transactionHistory/selectTransactionByType";

        public static readonly string SelectCoinSpentHistoryLog = "&/transactionHistory/selectSpentTransaction";
        public static readonly string SelectCoinEarnHistoryLog = "&/transactionHistory/selectEarnTransaction";

        //New Daily Reward
        public static readonly string SelectDailyStreak = "&/dailyStreak/selectDailyStreak";
        public static readonly string RegisterUserStreak = "&/collectedStreak/registerStreak";
        public static readonly string GetLastUserStreak = "&/collectedStreak/getLastStreak";

        //Immutable
        public static readonly string InsertLead = "&/leads/insertData";

        //User
        public static readonly string GetUser = "&/user/";
        public static readonly string Register = "&/user/register";

        //Access Key
        public static readonly string CheckCode = "&/accessKey/checkCode";
        public static readonly string CheckUser = "&/accessKey/checkUser";
        public static readonly string Bound = "&/accessKey/bound";

        public static string GetAPIPath() {

#if UNITY_EDITOR

#elif DEBUG
            return "https://cursed.agencia4red.com/cursedstone/dev";
#endif

            return "https://cursed.agencia4red.com/cursedstone/beta";
        }

        public static string GetAPIPaath() 
        {
#if DEBUG
            #if UNITY_EDITOR
            #endif

            return "https://backend-cst-dev.herokuapp.com";
#endif
            return "http://backend-cst.herokuapp.com";
        }

        private static string GetURL(string urlData) {
            if (urlData[0] == '/') {
                urlData = GetAPIPath() + urlData;
            }
            if (urlData[0] == '&') {
                urlData = GetAPIPaath() + urlData.TrimStart('&');
            }

            return urlData;
        }

        public static bool CheckValidData(string data, long code = 0) {
            return CheckValidData(data, null, code);
        }

        public static bool CheckValidData(string data, string urlData, long code = 0) {

            if (FormatCheckValidData(data, urlData, out var responseData)) {

                return CheckJsonValidData(responseData, urlData);

            }

            return CheckValidCode(code, urlData);

        }

        public static bool CheckValidCode(long _Code, string urlData = "") {

            if (_Code > 200 && _Code < 300) {

                return true;

            } else {

                //Debug.LogError($"Code : {_Code} {urlData}");

            }

            return false;

        }

        public static bool CheckJsonValidData(ResponseData _Response, string urlData = "") {

            if (_Response.type == "success" || string.IsNullOrEmpty(_Response.type)) {

                return true;

            } else {

                Debug.LogError($"Erro : {_Response.status} {urlData}");

            }

            return false;

        }

        public static bool FormatCheckValidData(string data, string urlData, out ResponseData _ResponseData) {

            _ResponseData = null;

            if (!string.IsNullOrEmpty(data) && data.TrimStart().StartsWith("{") && data.TrimEnd().EndsWith("}")) {

                try {

                    _ResponseData = JsonUtility.FromJson<ResponseData>(data);

                } catch {

                    //Debug.LogError($"Json Format Error {urlData}");

                }
                return true;

            } else {

                //Debug.LogError($"Erro : {data} {urlData}");

            }

            return false;

        }

        public static IEnumerator SendNFTData(string data, Action<string> callBack) {
#if UNITY_EDITOR
            //Debug.Log($"<color=green>SendNFTData --></color> data: {data}");
#endif
            using UnityWebRequest www = UnityWebRequest.Get(SelectAllNFTsProd + data);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
                callBack(www.downloadHandler.text);
            else
                callBack(www.error);
        }

        public static async Task<string> SendNFTData(string data) {

            string uri = SelectAllNFTsProd;
#if DEBUG
            uri = SelectAllNFTsDev;
            #if UNITY_EDITOR
            #endif
#endif
            using UnityWebRequest www = UnityWebRequest.Get(uri + data);
            www.SendWebRequest();
            await RequestFunc(www);

            if (www.result == UnityWebRequest.Result.Success)
                return www.downloadHandler.text;
            else
                return www.error;
        }

        public static async Task<string> GetDataNewAPI(string data, string urlData) {

            string url = GetURL(urlData);

            using UnityWebRequest www = UnityWebRequest.Get(url + data);
            www.SendWebRequest();
            await RequestFunc(www);

            if (www.result == UnityWebRequest.Result.Success) {

                if (string.IsNullOrEmpty(www.downloadHandler.text)) {

                    return www.responseCode.ToString();

                }

                return www.downloadHandler.text;

            } else
                return www.error;

        }
        
        public static async Task<(string, long)> GetDataNewAPICode(string data, string urlData) {

            string url = GetURL(urlData);

            using UnityWebRequest www = UnityWebRequest.Get(url + data);
            www.SendWebRequest();
            await RequestFunc(www);

            if (www.result == UnityWebRequest.Result.Success)
                return (www.downloadHandler.text, www.responseCode);
            else return (www.error, www.responseCode);

        }

        private static async Task<UnityWebRequest> SendDataCall(string data, string urlData) {

            string url = GetURL(urlData);

            var www = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            www.SendWebRequest();
            return await RequestFunc(www);

        }

        public static async Task<string> SendDataNewAPI(string data, string urlData) {

            var www = await SendDataCall(data, urlData);

            if (www.result == UnityWebRequest.Result.Success) {

                if (string.IsNullOrEmpty(www.downloadHandler.text)) {

                    return www.responseCode.ToString();

                }

                return www.downloadHandler.text;

            } else
                return www.error;
        }

        public static async Task<(string, long)> SendDataNewAPICode(string data, string urlData) {

            var www = await SendDataCall(data, urlData);

            if (www.result == UnityWebRequest.Result.Success)
                return (www.downloadHandler.text, www.responseCode);
            else return (www.error, www.responseCode);
        }

        //Remover futuramente
        public static async Task<string> SendData(string data, string urlData) {

            string url = GetURL(urlData);

            WWWForm form = new WWWForm();
            form.AddField("json", data);

            using UnityWebRequest www = UnityWebRequest.Post(url, form);
            www.SendWebRequest();
            await RequestFunc(www);

            if (www.result == UnityWebRequest.Result.Success)
                return www.downloadHandler.text;
            else
                return www.error;
        }

        static async Task<UnityWebRequest> RequestFunc(UnityWebRequest _Request) {
            float waitTime = 0;

            while (!_Request.isDone) {
                waitTime += 200;
                if (waitTime >= 60*1000) break;

                await Task.Delay(200);
            }

            return _Request;
        }

        //Remover futuramente
        public static async Task<string> SendDataEditor(string data, string urlData) {
#if UNITY_EDITOR
            Debug.Log($"<color=green>SendData --></color> data: {data}, urlData: {urlData}");
#endif

            WWWForm form = new WWWForm();
            form.AddField("json", data);

            using UnityWebRequest www = UnityWebRequest.Post(urlData, form);
            www.SendWebRequest();
            await RequestFunc(www);

            if (www.result == UnityWebRequest.Result.Success)
                return www.downloadHandler.text;
            else
                return www.error;
        }

    }
}