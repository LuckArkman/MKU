using System;
using MKU.Scripts.Singletons;
using UnityEngine;

namespace MKU.Scripts.HelthSystem
{
    [Serializable]
    public class CharacterProgressionManager
    {
        public CharacterProgression characterProgression;
        
        public CharacterProgressionManager(){}

        public CharacterProgressionManager(CharacterProgression progression)
        {
            characterProgression = progression;
        }

        public bool OnLevel;
        
        // Agora recebe quantia long (Int64) e aplica no armazenamento global de experiência
        public void AddExperience(long amount)
        {
            if (characterProgression == null) return;

            characterProgression.currentExperience += amount;
            
            // Usamos While em vez de If. Se o jogador ganhar muita exp de uma vez (ex: Matou Boss), 
            // ele pode atravessar múltiplos níveis simultaneamente.
            while (characterProgression.currentExperience >= characterProgression.experienceToNextLevel)
            {
                LevelUp();
            }
        }

        // Subir de nível individual
        private void LevelUp()
        {
            Debug.Log($"LevelUP: Você subiu para o Nível {characterProgression.level + 1}!");
            
            characterProgression.currentExperience -= characterProgression.experienceToNextLevel;
            characterProgression.level++;
            
            if (Singleton.Instance != null && Singleton.Instance._charController != null)
                Singleton.Instance._charController.LevelUp = true;
                
            characterProgression.CalcExperience();
        }
    }
}