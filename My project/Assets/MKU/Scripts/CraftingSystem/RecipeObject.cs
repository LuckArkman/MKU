using UnityEngine;
using UnityEngine.UI;

namespace MKU.Scripts.CraftingSystem
{
    public class RecipeObject : MonoBehaviour
    {
        public Button _button;
        public CraftingUI _craftingUI;
        public Recipe _recipes;

        public void OnStart()
        {
            _button.onClick.AddListener(() => SelectRecipe());
        }

        private void SelectRecipe()
        {
            _craftingUI.SelectRecipe(_recipes);
        }
    }
}