using System;

namespace MKU.Scripts.Models
{
    [Serializable]
    public class GameSettings
    {
        public string key;
        public float valueA;
        public float valueB;

        public GameSettings(string key, float valueA, float valueB)
        {
            this.key = key;
            this.valueA = valueA;
            this.valueB = valueB;
        }
    }
}