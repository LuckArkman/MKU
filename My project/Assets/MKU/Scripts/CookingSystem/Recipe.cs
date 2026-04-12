using UnityEngine;

namespace MKU.Scripts.CookingSystem
{
    [CreateAssetMenu(fileName = "Recipe", menuName = "CursedStone/CookingSystem/ new Recipe", order = 0)]
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