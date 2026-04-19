using MKU.Scripts.BlackSmithSystem;
using MKU.Scripts.CharacterSystem;
using MKU.Scripts.CookingSystem;
using MKU.Scripts.CraftingSystem;
using MKU.Scripts.Enums;
using MKU.Scripts.EquipamentsSystem;
using MKU.Scripts.FinanceSystem;
using MKU.Scripts.HelthSystem;
using MKU.Scripts.InputSystem;
using MKU.Scripts.ItemSystem;
using MKU.Scripts.IventorySystem;
using MKU.Scripts.MarketSystem;
using MKU.Scripts.Strucs;
using MKU.Scripts.Tasks;
using UnityEngine;

namespace MKU.Scripts.Singletons
{
    public class Singleton : MonoBehaviour
    {
        public CharController _charController = null;
        public FinanceController _financeController;
        public InputManager _inputManager = null;
        public Inventory _inventory = null;
        public Character _character = null;
        public Market _market = null;
        public BlackSmith _blackSmith = null;
        public Cooking _cooking = null;
        public _Item item;
        public CraftingManager _craftingManager;
        public InventoryUI inventoryUI = null;
        public EquipamentUI _equipment = null;
        public QuestManager questManager = null;
        public ItemFeedbackController _itemFeedbackController = null;
        public Crafting _crafting = null;
        public CookingManager _cookingManager = null;
        public object portalManager;
        public Portal portal = Portal.None;
        public _Attributs _originalAttributes;
        public string _financeCsts = "";
        public string _financeWallet = "";


        private static Singleton instance;

        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    // Tenta encontrar na cena
                    instance = FindObjectOfType<Singleton>();

                    // Se não existir, cria automaticamente
                    if (instance == null)
                    {
                        GameObject obj = new GameObject("Singleton");
                        instance = obj.AddComponent<Singleton>();

                        Debug.Log("[Singleton] Instância criada automaticamente.");
                    }
                }

                return instance;
            }
        }

        public string Id { get; set; }

        protected virtual void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;

            DontDestroyOnLoad(gameObject);
        }

        public void OnLoadScene(string level)
        {
            throw new System.NotImplementedException();
        }
    }
}