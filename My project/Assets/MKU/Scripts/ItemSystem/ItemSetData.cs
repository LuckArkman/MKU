using UnityEngine;
using System.Collections.Generic;
using MKU.Scripts.HelthSystem;

namespace MKU.Scripts.ItemSystem
{
    [System.Serializable]
    public class SetBonus
    {
        [Tooltip("Quantidade de itens do conjunto necessários para ativar este bônus")]
        public int requiredPieces;
        public _Attributs bonusAttributes;
    }

    [CreateAssetMenu(fileName = "New Item Set", menuName = "CursedStone/ItemSystem/Item Set", order = 1)]
    public class ItemSetData : ScriptableObject
    {
        public string setName;
        public List<SetBonus> bonuses = new List<SetBonus>();
    }
}
