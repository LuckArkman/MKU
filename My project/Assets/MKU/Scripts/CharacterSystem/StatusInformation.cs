using MKU.Scripts.HelthSystem;
using TMPro;
using UnityEngine;

namespace MKU.Scripts.CharacterSystem
{
    public class StatusInformation : MonoBehaviour
    {

        public TextMeshProUGUI generalXP;
        public TextMeshProUGUI strength;
        public TextMeshProUGUI agility;
        public TextMeshProUGUI dexterity;
        public TextMeshProUGUI vitality;
        public TextMeshProUGUI luck;
        public TextMeshProUGUI intelligence;
        
        public async void SetPlayerStatusUI(_Stats stats)
        {/*
            Debug.Log($"{nameof(SetPlayerStatusUI)} >> {stats == null}");
            if (stats != null)
            {
                strength.text = stats._attributes.Strength.ToString();
                intelligence.text = stats._attributes.Intelligence.ToString();
                agility.text = stats._attributes.Agility.ToString();
                dexterity.text = stats._attributes.Dexterity.ToString();
                luck.text = stats._attributes.Luck.ToString();
                vitality.text = stats._attributes.Vitality.ToString();
            }
            */
        }
    }
}
