using MKU.Scripts.HelthSystem;
using UnityEngine;

namespace MKU.Scripts.Interfaces
{
    public interface IPlayer
    {
        GameObject GetPlayer();
        void HitAttack(Base _baseEnemy);
        Base GetBase();
        CharacterProgression GetProgression();
        Transform GetspawnUI();
        Transform GetCanvas();
        void SetTaskCollection(object taskCollections);
    }
}