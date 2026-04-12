using System;
using MKU.Scripts.Models;
using TMPro;

namespace MKU.Scripts.CharacterSystem
{
    [Serializable]
    public class CharacterInformation
    {
        public CharacterInformation(){}
        public TextMeshProUGUI name;
        public TextMeshProUGUI clas;
        public TextMeshProUGUI guild;
        public TextMeshProUGUI currentXp;

        public void SetInformation(Character character)
        {
            name.text = $"{character.nickName}";
            clas.text = $"{character.classCharacter}";
            guild.text = $"{character.clan}";
            currentXp.text = $"{character.xp}";
        }
    }
}