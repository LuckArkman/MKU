using System;
using System.Collections.Generic;
using MKU.Scripts.CharacterSystem;
using MKU.Scripts.Enums;
using MKU.Scripts.FinanceSystem;
using MKU.Scripts.IventorySystem;
using MKU.Scripts.Models;
using MKU.Scripts.Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MKU.Scripts.BlackSmithSystem
{
    public class BlackSmithUI : MonoBehaviour
    {
        public CharController player;
        public BlackSmith blackSmith;
        public TextMeshProUGUI _parcent, _price;
        public Button _refinar;
        public BlackSmithNotification _blackSmithNotification;
        public InventoryUI _inventoryUI;
        public ItemIcon icon = null;
        public BlackSmithSlotUI _blackSmithSlot;

        private void Start()
        {
            _refinar.onClick.AddListener(()=> OnUgrade());
        }

        private async void OnUgrade()
        {
            string response = "";
            if (Singleton.Instance.item != null)
            {
                int price = Singleton.Instance.item._parcentes[Singleton.Instance.item.level].price;
                if (Singleton.Instance._character == null) response = await new FinanceManager().PostCsts(new Message(Singleton.Instance.Id, ActionCode.Transference, price, Singleton.Instance._blackSmith.Id));
                if (Singleton.Instance._character != null) response = await new FinanceManager().PostCsts(new Message(Singleton.Instance._character.id, ActionCode.Transference, price, Singleton.Instance._blackSmith.Id));
                Singleton.Instance._financeController.OnStart();
                CharSettings._Instance.upgrade = response == "200";
            }
        }

        public void OnClose()
        {
            this.gameObject.SetActive(false);
            
        }

        public void OnStart(CharController playerTransform)
        {
            blackSmith =  CharSettings._Instance._blackSmith;
            player = playerTransform;
        }

        private void Update()
        {
            _refinar.interactable = CharSettings._Instance.interactable && icon.item != null;
        }

        public void OnNotification(string text)
        {

            BlackSmithNotification not = Instantiate<BlackSmithNotification>(_blackSmithNotification, CharSettings._Instance._charController.canva);
            not.OnStart(text);
            _blackSmithSlot.RemoveItems(1);

        }
    }
}