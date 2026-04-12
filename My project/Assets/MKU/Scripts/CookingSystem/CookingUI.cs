using MKU.Scripts.ItemSystem;
using MKU.Scripts.IventorySystem;
using MKU.Scripts.Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MKU.Scripts.CookingSystem
{
    public class CookingUI : MonoBehaviour
    {
        public CookingManager _cookingManager;
        public Transform recipeListParent;
        public RecipeObject recipeButtonPrefab;
        public TextMeshProUGUI selectedRecipeDetails;
        public Button craftButton;
        public CookingSlotUI CraftItemPrefab = null;
        public Transform slotsParent;
        public Cooking playerCrafting;
        public InventoryUI _inventoryUI;
        
        public Recipe _selectedRecipe;

        public void OnStart()
        {
            _cookingManager = Singleton.Instance._cookingManager;
            playerCrafting = Singleton.Instance._cookingManager._crafting;
            PopulateRecipeList();
            craftButton.onClick.AddListener(CraftSelectedRecipe);
            playerCrafting.craftingUpdated += Redraw;
            Redraw();
        }
        
        private void Redraw()
        {
            foreach (Transform child in slotsParent)
            {
                Destroy(child.gameObject);
            }
            for (int i = 0; i < _cookingManager._crafting.GetSize(); i++)
            {
                var itemUI = Instantiate(CraftItemPrefab, slotsParent);
                itemUI.Setup(playerCrafting, i);
            }
        }

        private void PopulateRecipeList()
        {
            foreach (Transform child in recipeListParent)
            {
                Destroy(child.gameObject);
            }
            foreach (Recipe recipe in _cookingManager.recipes)
            {
                RecipeObject buttonObj = Instantiate<RecipeObject>(recipeButtonPrefab, recipeListParent);
                buttonObj._cookingUI = this;
                buttonObj._recipes = recipe;
                buttonObj.OnStart();
            }
        }
        
        public void OnClose()
        => this.gameObject.SetActive(false);

        public void SelectRecipe(Recipe recipe)
        {
            var container = Resources.Load("ItemContainer") as ItemContainer;
            _selectedRecipe = recipe;
            Debug.Log($"Crafting: {recipe.result}");
            selectedRecipeDetails.text = $"Crafting: {container.items.Find(i => i.itemID == recipe.result).displayName}\n";
            for (int i = 0; i < recipe.ingredients.Length; i++)
            {
                selectedRecipeDetails.text += container.items.Find(x => x.itemID == recipe.ingredients[i].itemId).displayName +
                                              " x" + recipe.ingredients[i].quantity + "\n";
            }
        }

        private void CraftSelectedRecipe()
        {
            if (_selectedRecipe != null)
            {
                _cookingManager.Craft(_selectedRecipe);
            }
        }
    }
}