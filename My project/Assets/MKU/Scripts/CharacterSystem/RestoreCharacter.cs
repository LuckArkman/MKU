using System;
using MKU.Scripts.Models;

namespace MKU.Scripts.CharacterSystem
{
    public class RestoreCharacter
    {
        public RestoreCharacter(){}

        public Character GetCharacter(CharacterPatterns _patt)
            => new Character(Guid.NewGuid().ToString(),
                _patt.nickName,
                _patt.TokenId,
                _patt.Clan,
                _patt.Rarity,
                _patt.nickName,
                _patt.Stone,
                _patt.Staked,
                _patt.IsRented,
                _patt.Inteligence,
                _patt.Vit,
                _patt.Def,
                _patt.Luk,
                _patt.Agi,
                _patt.Str,
                _patt.Level,
                _patt.Xp,
                _patt.AccXp,
                _patt.Life,
                _patt.Stars,
                0.0,
                0.0,
                0.0,
                DateTime.Now);
    }
}