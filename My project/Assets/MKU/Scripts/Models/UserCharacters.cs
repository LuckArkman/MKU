using System.Collections.Generic;
using MKU.Scripts.CharacterSystem;

namespace MKU.Scripts.Models
{
    public class UserCharacters
    {
        public UserCharacters()
        {
        }

        public string Id { get; set; }

        public List<Character> _characters { get; set; }

        public UserCharacters(string Id, List<Character> characters)
        {
            this.Id = Id;
            _characters = characters;
        }
    }
}