using System;
using System.Threading.Tasks;
using MKU.Scripts.CharacterSystem;
using MKU.Scripts.Enums;
using MKU.Scripts.FinanceSystem;
using MKU.Scripts.HelthSystem;
using MKU.Scripts.Models;
using MKU.Scripts.Singletons;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace MKU.Scripts.SettingsSystem
{
    public class MessageModal : MonoBehaviour
    {
        public int price;
        public Text text;

        public void OnStart(ActionCode actionCode)
        {
            text.text = $"Cast {price} Cst to return";
        }

        public void InitReturn()
        {
            PayReturn();
        }

        public async void PayReturn()
        {
          string response = "";
            if (Singleton.Instance._charController == null) response = await new FinanceManager().PostCsts(new Message(Singleton.Instance.Id, ActionCode.Transference, price, "00000000-0000-0000-0000-000000000000"));
            if (Singleton.Instance._charController != null) response = await new FinanceManager().PostCsts(new Message(Singleton.Instance._character.id, ActionCode.Transference, price, "00000000-0000-0000-0000-000000000000"));
            Singleton.Instance._financeController.OnStart();
            if (response == "200")
            {
                _Attributs attributs = CharSettings._Instance._charController._base.Attributes;
                Vector3 position = CharSettings._Instance._charController.transform.position;
                Destroy(CharSettings._Instance._player);
                if(Singleton.Instance._character == null)Instantiate(CharSettings._Instance._reSpawn._gameObject, position, Quaternion.identity);
                if (Singleton.Instance._character != null)
                {
                    Character _character = Singleton.Instance._character;
                    CharSettings._Instance._reSpawn._characters.ForEach(x =>
                    {
                        if (x._character.name == _character.classCharacter)
                        {
                            var character = Instantiate(x.characterModel, position, Quaternion.identity);
                            CharController controller = character.GetComponentInChildren<CharController>();
                            controller.Id = _character.id;
                            controller._base.Attributes = attributs;
                            CharSettings._Instance._charController = controller;
                            CharSettings._Instance._player = character;
                            CharSettings._Instance._inputManager = controller.inputManager;
                            controller.OnStart();
                            controller.charNameText.text = character.name;
                        }
                    });
                }
            }  
        }

        public void InitCancel()
        {
            this.gameObject.SetActive(false);
        }
    }
}