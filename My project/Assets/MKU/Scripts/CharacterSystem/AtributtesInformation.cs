using System;
using MKU.Scripts.Models;
using TMPro;

namespace MKU.Scripts.CharacterSystem
{
    [Serializable]
    public class AtributtesInformation
    {
        public AtributtesInformation(){}
        public TextMeshProUGUI Inteligence;
        public TextMeshProUGUI Vit;
        public TextMeshProUGUI Def;
        public TextMeshProUGUI Luk;
        public TextMeshProUGUI Agi;
        public TextMeshProUGUI Str;

        public void SetInformation(Character character)
        {
            Inteligence.text = $"{character.inteligence}";
            Vit.text = $"{character.vit}";
            Def.text = $"{character.def}";
            Luk.text = $"{character.luk}";
            Agi.text = $"{character.agi}";
            Str.text = $"{character.str}";
        }
    }
}