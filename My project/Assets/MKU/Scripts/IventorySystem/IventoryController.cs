using MKU.Scripts.CharacterSystem;
using MKU.Scripts.HelthSystem;
using MKU.Scripts.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MKU.Scripts.IventorySystem
{
    public class IventoryController : MonoBehaviour
    {
        public Button inventoryButton, equipamentButton, colletButton;
        public _Stats _base;
        public Image _lifeBar, _spBar;
        public TextMeshProUGUI Atk, AtkSpd, Accuracy, Critical, Defense,Hp, currentXP;
        public TextMeshProUGUI Strength, Agility, Vitality, Intelligence, Dexterity, Luck;
        public CharacterProgression _progression;
        public CharController _charController;
        public _Attributs _attributes;
        public Transform player;

        private void Start()
        {
            player = CharSettings._Instance._charController.transform;
            InvokeRepeating("OnUpdate", 1.0f,1.0f);
        }

        private void OnUpdate()
        {
            if(player == null) player = CharSettings._Instance._charController.transform;
            if(_progression == null) _progression = player.GetComponent<CharController>().GetProgression();
            if(_charController == null)_charController = player.GetComponent<CharController>();
            SetStatus(_charController._base.Status);
            _attributes = player.GetComponent<CharController>()._base.Attributes;
            SetAtributtes(_attributes);
        }
        
        private void SetAtributtes(_Attributs attributes)
        {
                Strength.text = $"{attributes.Strength}";
                Agility.text  = $"{attributes.Agility}";
                Vitality.text  = $"{attributes.Vitality}";
                Intelligence.text  = $"{attributes.Intelligence}";
                Dexterity.text  = $"{attributes.Dexterity}";
                Luck.text  = $"{attributes.Luck}";
        }
        
        private void SetStatus(_Stats _status)
        {
            if(player == null) player = CharSettings._Instance._charController.transform;
            if(_progression == null) _progression = player.GetComponent<IPlayer>().GetProgression();
            currentXP.text = _progression.currentExperience.ToString();
            Atk.text = _status.PhysicalAttack.ToString();
            AtkSpd.text = _status.AttackSpeed.ToString();
            Accuracy.text = _status.Flee.ToString();
            Critical.text = _status.CriticalChance.ToString();
            Defense.text = _status.PhysicalDefense.ToString();
            Hp.text = _status.MaxHP.ToString();
        }

        public void OnCloseUI()
        => this.gameObject.SetActive(false);
    }
}