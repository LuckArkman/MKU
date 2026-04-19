using System;

namespace MKU.Scripts.CharacterSystem
{
    [Serializable]
    public class Character
    {
        public Character(){}

        public string id { get; set; }
        public string nickName { get; set; }
        public int tokenId { get; set; }
        public string clan { get; set; }
        public string rarity { get; set; }
        public string classCharacter { get; set; }
        public long stone { get; set; }
        public bool staked { get; set; }
        public bool isRented { get; set; }
        public int inteligence { get; set; }
        public int vit { get; set; }
        public int def { get; set; }
        public int luk { get; set; }
        public int agi { get; set; }
        public int str { get; set; }
        public int level { get; set; }
        public int xp { get; set; }
        public int accXp { get; set; }
        public string life { get; set; }
        public string stars { get; set; }
        public double positionX { get; set; }
        public double positionY { get; set; }
        public double positionZ { get; set; }
        public DateTime lastUpdate { get; set; }

        public Character(string id, string nickName, int tokenId, string clan, string rarity, string classCharacter,
            long stone, bool staked, bool isRented, int inteligence, int vit, int def, int luk, int agi, int str,
            int level, int xp, int accXp, string life, string stars, double positionX, double positionY,
            double positionZ, DateTime lastUpdate)
        {
            this.id = id;
            this.nickName = nickName;
            this.tokenId = tokenId;
            this.clan = clan;
            this.rarity = rarity;
            this.classCharacter = classCharacter;
            this.stone = stone;
            this.staked = staked;
            this.isRented = isRented;
            this.inteligence = inteligence;
            this.vit = vit;
            this.def = def;
            this.luk = luk;
            this.agi = agi;
            this.str = str;
            this.level = level;
            this.xp = xp;
            this.accXp = accXp;
            this.life = life;
            this.stars = stars;
            this.positionX = positionX;
            this.positionY = positionY;
            this.positionZ = positionZ;
            this.lastUpdate = lastUpdate;
        }
    }
}