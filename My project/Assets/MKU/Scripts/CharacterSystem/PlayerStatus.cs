using MKU.Scripts.HelthSystem;
using UnityEngine;

namespace MKU.Scripts.CharacterSystem
{
    public class PlayerStatus : MonoBehaviour
    {
        public string playerName; //Hero name
        public CharacterPatterns _character;
        public _Attributs _attributes;
        public _Stats _status;
        [ContextMenu(nameof(Teste))]
        public void Teste()
        {
            _attributes = new _Attributs(_character.Str,
                _character.Agi, _character.Vit, _character.Inteligence, _character.Def,
                _character.Luk);
            _status = new _Stats();
            OnPlayerStatus();
        }

        public void OnPlayerStatus()
            => new Base().StartCalc(_attributes, _status, _character.Level);
    }
}