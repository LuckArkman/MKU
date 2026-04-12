using System;
using UnityEngine;

namespace MKU.Scripts.CharacterSystem
{
    [Serializable]
    public class CharactersSelect
    {
        public string characterName;
        public CharacterPatterns _character;
        public SelectCharacter _SelectCharacter;
        public GameObject characterModel;
    }
}