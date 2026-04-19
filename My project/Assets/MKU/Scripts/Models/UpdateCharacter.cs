using System;

namespace MKU.Scripts.Models
{
    public class UpdateCharacter
    {
        public UpdateCharacter(){}
        public string id { get; set; }
        public int level { get; set; }
        public int xp { get; set; }
        public float positionX { get; set; }
        public float positionY { get; set; }
        public float positionZ { get; set; }
        public DateTime lastUpdate { get; set; }

        public UpdateCharacter(string id, int level, int xp, float positionX, float positionY, float positionZ, DateTime lastUpdate)
        {
            this.id = id;
            this.level = level;
            this.xp = xp;
            this.positionX = positionX;
            this.positionY = positionY;
            this.positionZ = positionZ;
            this.lastUpdate = lastUpdate;
        }
    }
}