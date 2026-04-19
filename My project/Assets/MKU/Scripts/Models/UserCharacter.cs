using System;
using MKU.Scripts.CharacterSystem;

namespace MKU.Scripts.Models
{
    [Serializable]
    public class UserCharacter
    {
        public UserCharacter()
        {
        }

        public string Id { get; set; }

        public Character _character { get; set; }

        public UserCharacter(string id, Character character)
        {
            Id = id;
            _character = character;
        }
    }
}