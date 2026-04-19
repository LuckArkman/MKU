using System.Collections.Generic;
using MKU.Scripts.CharacterSystem;
using UnityEngine;

namespace MKU.Scripts.Strucs
{
    public class ReSpawn : MonoBehaviour
    {
        public Character _character;
        public GameObject _isOn;
        public GameObject _gameObject;
        public List<CharactersSelect> _characters = new();
        private void Start()
        {
            CharSettings._Instance._reSpawn = this;
        }
        
        
    }
}