using MKU.Scripts.ItemSystem;
using UnityEngine;

namespace MKU.Scripts.CraftingSystem
{
    [CreateAssetMenu(fileName = "Recipe", menuName = "CursedStone/CraftingSystem/ new Recipe", order = 0)]
    public class Recipe : ScriptableObject
    {
        public string result;
        public Ingredients[] ingredients;
        [Range(0,1000)]
        public int price;
        [Range(0, 100)]
        public int parcent;
    }
}