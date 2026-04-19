using MKU.Scripts.ItemSystem;
using MKU.Scripts.IventorySystem;
using MKU.Scripts.Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MKU.Scripts.CraftingSystem
{
    public class CraftingUI : MonoBehaviour
    {
        public CraftingManager craftingManager;
        public Transform recipeListParent;
        public RecipeObject recipeButtonPrefab;
        public TextMeshProUGUI selectedRecipeDetails, _price, _parcent, _itemName;
        public Image _craftOnbjectIcon;
        public Button craftButton;
        public CraftSlotUI CraftItemPrefab = null;
        public Transform slotsParent;
        public Crafting playerCrafting;
        public InventoryUI _inventoryUI;
        
        public Recipe _selectedRecipe;

        public void OnStart()
        {
            _craftOnbjectIcon.enabled = false;
            _itemName.text = "";
            craftingManager = Singleton.Instance._craftingManager;
            playerCrafting = Singleton.Instance._crafting;
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
            for (int i = 0; i < playerCrafting.GetSize(); i++)
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
            foreach (Recipe recipe in craftingManager.recipes)
            {
                RecipeObject buttonObj = Instantiate<RecipeObject>(recipeButtonPrefab, recipeListParent);
                buttonObj._craftingUI = this;
                buttonObj._recipes = recipe;
                
                buttonObj.OnStart();
            }
        }
        
        public void OnClose()
        => this.gameObject.SetActive(false);

        public void SelectRecipe(Recipe recipe)
        {
            _price.text = $"{recipe.price}";
            _parcent.text = $"Percentage: {recipe.parcent} %";
            var container = Resources.Load("ItemContainer") as ItemContainer;
            _selectedRecipe = recipe;
            _craftOnbjectIcon.enabled = recipe != null;
            _craftOnbjectIcon.sprite = container.items.Find(x => x.itemID == recipe.result).icon;
            _itemName.text = container.items.Find(x => x.itemID == recipe.result).displayName;
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
                craftingManager.Craft(_selectedRecipe);
            }
        }
    }
}