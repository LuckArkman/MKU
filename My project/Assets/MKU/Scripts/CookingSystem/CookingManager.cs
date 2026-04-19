using System.Collections.Generic;
using System.Threading.Tasks;
using MKU.Scripts.CharacterSystem;
using MKU.Scripts.Enums;
using MKU.Scripts.FinanceSystem;
using MKU.Scripts.ItemSystem;
using MKU.Scripts.IventorySystem;
using MKU.Scripts.Models;
using MKU.Scripts.Singletons;
using UnityEngine;

namespace MKU.Scripts.CookingSystem
{
    public class CookingManager : MonoBehaviour
    {
        public List<UsedSlots> usedSlots = new ();
        public List<Recipe> recipes = new ();
        public Cooking _crafting;
        private void Start()
        {
            Singleton.Instance._cookingManager = this;
        }

        public async Task<bool> Craft(Recipe recipe)
        {
            string response = "";
            for (int i = 0; i < recipe.ingredients.Length; i++)
            {
                string ingredient = recipe.ingredients[i].itemId;
                int requiredQuantity = recipe.ingredients[i].quantity;
                var container = Resources.Load("ItemContainer") as ItemContainer;
                _Item item = container.items.Find(x => x.itemID == ingredient);
                if (!_crafting.HasItem(item))
                {
                    Debug.Log("Not enough " + ingredient);
                    return false; 
                }
                int slotIndex = _crafting.GetSlotWithItem(item, recipe.ingredients[i].quantity);

                // Se não encontrar o item em quantidade suficiente, falha.
                if (slotIndex == -1)
                {
                    Debug.Log($"Falta o item: {ingredient} para a receita!");
                    return false; // Não é possível realizar o craft.
                }
                usedSlots.Add(new UsedSlots(slotIndex, recipe.ingredients[i]));
            }
            
            for (int i = 0; i < recipe.ingredients.Length; i++)
            {
                var container = Resources.Load("ItemContainer") as ItemContainer;
                _Item ingredient = container.items.Find(x => x.itemID == recipe.ingredients[i].itemId);
                int requiredQuantity = recipe.ingredients[i].quantity;
            }
            foreach (var slotIndex in usedSlots)
            {
                // Itera pelos slots e remove os ingredientes.
                var container = Resources.Load("ItemContainer") as ItemContainer;
                _Item ingredient = container.items.Find(x => x.itemID == slotIndex.item.itemId);
                int requiredQuantity = slotIndex.item.quantity;
                _crafting.RemoveFromSlot(slotIndex.index, requiredQuantity);
            }
            if (Singleton.Instance._character == null) response = await new FinanceManager().PostCsts(new Message(Singleton.Instance.Id, ActionCode.Transference, recipe.price, Singleton.Instance._cooking.Id));
            if (Singleton.Instance._character != null) response = await new FinanceManager().PostCsts(new Message(Singleton.Instance._character.id, ActionCode.Transference, recipe.price, Singleton.Instance._crafting.Id));
            Singleton.Instance._financeController.OnStart();
            Debug.Log($"{nameof(Craft)} >> response >> {response}");
            if (response == "200")
            {
                var _container = Resources.Load("ItemContainer") as ItemContainer;
                var inventory = CharSettings._Instance._charController.GetComponent<Inventory>();
                inventory.AddToFirstEmptySlot(_container.items.Find(x => x.itemID == recipe.result), 1);
                Debug.Log("Crafted: " + recipe.result);
                return true;
            }
            return false;
        }
    }
}