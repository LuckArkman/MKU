using System;
using MKU.Scripts.CharacterSystem;
using UnityEngine;

namespace MKU.Scripts.HelthSystem
{
    [Serializable]
    public class CharacterProgression
    {
        public int level = 1; 
        public int currentExperience = 0;
        public int experienceToNextLevel = 100; 
        public float experienceMultiplier = 1.2f;
        public CharController _CharController;
        public GameObject _levelUpVfxPrefab;
        public Transform position;
        public CharacterProgression(){}


        public void LevelUp()
        {
            var characterProgression = new CharacterProgressionManager(this);
            characterProgression.AddExperience(currentExperience);
        }
        
        public void CalcExperience()
        {
            int experienceNextLevel = 100;
            int i = 1;
            while (i <= level)
            {
                experienceNextLevel = Mathf.RoundToInt(experienceNextLevel * experienceMultiplier);
                experienceToNextLevel = experienceNextLevel;
                i++;
            }
        }
    }
}