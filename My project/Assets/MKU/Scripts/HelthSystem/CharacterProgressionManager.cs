using System;
using MKU.Scripts.Singletons;
using UnityEngine;

namespace MKU.Scripts.HelthSystem
{
    [Serializable]
    public class CharacterProgressionManager
    {
        public CharacterProgressionManager(){}
        public CharacterProgression characterProgression;
        public CharacterProgressionManager(CharacterProgression progression)
            => characterProgression = progression;

        public bool OnLevel;
        
        public void AddExperience(int amount)
        {
            if (amount >= characterProgression.experienceToNextLevel) LevelUp();
        }

        // Subir de nível
        private void LevelUp()
        {
            
            Debug.Log("LevelUP");
            characterProgression.currentExperience -= characterProgression.experienceToNextLevel;
            characterProgression.level++;
            Singleton.Instance._charController.LevelUp = true;
            characterProgression.CalcExperience();
        }
    }
}