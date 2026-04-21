using System;
using MKU.Scripts.CharacterSystem;
using UnityEngine;

namespace MKU.Scripts.HelthSystem
{
    [Serializable]
    public class CharacterProgression
    {
        public int level = 1; 
        public long currentExperience = 0;
        public long experienceToNextLevel = 120; 
        public float experienceMultiplier = 1.2f;
        public CharController _CharController;
        public GameObject _levelUpVfxPrefab;
        public Transform position;
        
        public CharacterProgression()
        {
            // O calculo inicial garante que a exp para level up seja baseada no level do objeto logo que é instanciado
            CalcExperience();
        }

        // Removido o LevelUp() antigo circular. Agora a responsabilidade de dar Level UP é puramente do Manager.

        public void CalcExperience()
        {
            // Calcula a exp limite do nível atual dinamicamente com suporte a números Int64 GIGANTES sem crashear
            long experienceNextLevel = 100;
            for (int i = 1; i <= level; i++)
            {
                experienceNextLevel = (long)Mathf.Round(experienceNextLevel * experienceMultiplier);
            }
            experienceToNextLevel = experienceNextLevel;
        }
    }
}