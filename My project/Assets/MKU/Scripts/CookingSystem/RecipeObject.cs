using UnityEngine;
using UnityEngine.UI;

namespace MKU.Scripts.CookingSystem
{
    public class RecipeObject : MonoBehaviour
    {
        public Button _button;
        public CookingUI _cookingUI;
        public Recipe _recipes;

        public void OnStart()
        {
            _button.onClick.AddListener(() => SelectRecipe());
        }

        private void SelectRecipe()
        {
            _cookingUI.SelectRecipe(_recipes);
        }
    }
}