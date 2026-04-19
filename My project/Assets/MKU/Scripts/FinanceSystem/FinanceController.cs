using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MKU.Scripts.CharacterSystem;
using MKU.Scripts.ItemSystem;
using MKU.Scripts.Models;
using MKU.Scripts.Singletons;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace MKU.Scripts.FinanceSystem
{
    public class FinanceController : MonoBehaviour
    {
        public _Item item;
        public TextMeshProUGUI _coins;
        public int total;
        public int _Csts;
        public Dictionary<string, Cst> balance = new();

        private async Task<int> GetCsts()
        {
            CharWallet charWallet = null;
            if(Singleton.Instance._charController == null) charWallet = await new FinanceManager().GetWallet(Singleton.Instance.Id);
            if(Singleton.Instance._charController != null) charWallet = await new FinanceManager().GetWallet(Singleton.Instance._character.id);
            if (charWallet != null)
            {
                balance = charWallet.csts;
                return charWallet.csts.Count;
            }

            return 0;
        }

        private void Start()
        {
            CharSettings._Instance._financeController = this;
            Singleton.Instance._financeController = this;
            GetBalance();
        }
        
        public void OnStart()
        {
            GetBalance();
        }


        public async void GetBalance()
        {
            _Csts = await GetCsts();
            _coins.text = $"{_Csts}";
            total = _Csts;
        }

        public void UpdateBalance(int price)
        {
            
        }
    }
}