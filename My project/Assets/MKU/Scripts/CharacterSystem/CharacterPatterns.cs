using System.Collections.Generic;
using MKU.Scripts.SkillSystem;
using UnityEngine;

namespace MKU.Scripts.CharacterSystem
{
    [CreateAssetMenu(fileName = "CharacterPatterns", menuName = "CursedStone/Character/CharacterPatterns", order = 0)]
    public class CharacterPatterns : ScriptableObject
    {
        public string nickName;
        public int TokenId;
        public string Clan;
        public string Rarity;
        [Range(0.0f,1000.0f)]
        public long Stone;
        public bool Staked;
        public bool IsRented;
        [Range(0,100f)]
        public int Inteligence;
        [Range(0,1000)]
        public int Vit;
        [Range(0,1000)]
        public int Def;
        [Range(0,1000)]
        public int Luk;
        [Range(0,1000)]
        public int Agi;
        [Range(0,1000)]
        public int Str;
        [Range(0.0f,1000.0f)]
        public long estimatedRewards;
        public bool onMarketplace;
        [Range(0.0f,100.0f)]
        public long ownerPercentage;
        [Range(0.0f,1000.0f)]
        public long rewarded;
        [Range(0.0f,100.0f)]
        public float playerPercentage;
        [Range(0,1000)]
        public int Level;
        [Range(0,1000)]
        public int Xp;
        [Range(0,1000)]
        public int AccXp;
        public string Life;
        public string Stars;
        public double StoneLimit;
        public double StoneDay;
        
        public List<SkillCollection> _Skills = new ();
    }
}