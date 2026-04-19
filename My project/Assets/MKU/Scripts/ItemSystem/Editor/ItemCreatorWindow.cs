using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using MKU.Scripts.ItemSystem;
using MKU.Scripts.Enums;
using MKU.Scripts.EquipamentsSystem;
using MKU.Scripts.HelthSystem;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MKU.Scripts.ItemSystem.Editor
{
    public class ItemCreatorWindow : EditorWindow
    {
        private Label contentTitle;
        private List<VisualElement> pages = new List<VisualElement>();
        private List<Button> tabs = new List<Button>();
        private ItemCategory currentCategory;

        private ListView itemListView;
        private List<_Item> cachedItems = new List<_Item>();
        private _Item currentlySelectedItem = null;

        [MenuItem("MKU/Item Creator UI")]
        public static void ShowExample()
        {
            ItemCreatorWindow wnd = GetWindow<ItemCreatorWindow>();
            wnd.titleContent = new GUIContent("Item Dashboard");
            wnd.minSize = new Vector2(850, 450);
        }

        public void CreateGUI()
        {
            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/MKU/Scripts/ItemSystem/Editor/ItemCreatorWindow.uxml");
            if (visualTree == null)
            {
                Debug.LogError("Could not load UXML.");
                return;
            }
            visualTree.CloneTree(rootVisualElement);

            // Import USS
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/MKU/Scripts/ItemSystem/Editor/ItemCreatorWindow.uss");
            if (styleSheet != null)
            {
                rootVisualElement.styleSheets.Add(styleSheet);
            }

            SetupDashboard();
            BindLogic();
            SetupItemList();
            LoadItems();
        }

        private void SetupDashboard()
        {
            contentTitle = rootVisualElement.Q<Label>("ContentTitle");
            
            pages.Add(rootVisualElement.Q<VisualElement>("PageBasic"));
            pages.Add(rootVisualElement.Q<VisualElement>("PageAssets"));
            pages.Add(rootVisualElement.Q<VisualElement>("PageAttributes"));
            pages.Add(rootVisualElement.Q<VisualElement>("PageSets"));
            
            var tabBasic = rootVisualElement.Q<Button>("TabBasic");
            var tabAssets = rootVisualElement.Q<Button>("TabAssets");
            var tabAttributes = rootVisualElement.Q<Button>("TabAttributes");
            var tabSets = rootVisualElement.Q<Button>("TabSets");
            
            tabs.Add(tabBasic);
            tabs.Add(tabAssets);
            tabs.Add(tabAttributes);
            tabs.Add(tabSets);
            
            tabBasic.clicked += () => SwitchTab(0, "Basic Info");
            tabAssets.clicked += () => SwitchTab(1, "Assets & Visuals");
            tabAttributes.clicked += () => SwitchTab(2, "Attributes");
            tabSets.clicked += () => SwitchTab(3, "Set System");
        }

        private void SwitchTab(int index, string title)
        {
            contentTitle.text = title;
            for (int i = 0; i < pages.Count; i++)
            {
                pages[i].style.display = (i == index) ? DisplayStyle.Flex : DisplayStyle.None;
                
                if (i == index)
                {
                    tabs[i].AddToClassList("selected-tab");
                }
                else
                {
                    tabs[i].RemoveFromClassList("selected-tab");
                }
            }
        }

        private void BindLogic()
        {
            var btnGenerateGUID = rootVisualElement.Q<Button>("GenerateGUIDBtn");
            var itemIDField = rootVisualElement.Q<TextField>("ItemIDField");
            
            btnGenerateGUID.clicked += () => {
                 itemIDField.value = Guid.NewGuid().ToString();
            };

            var btnCreateItem = rootVisualElement.Q<Button>("SaveItemBtn");
            btnCreateItem.clicked += OnSaveItemClicked;

            var btnNewItem = rootVisualElement.Q<Button>("NewItemBtn");
            btnNewItem.clicked += ClearFields;

            var btnRefresh = rootVisualElement.Q<Button>("RefreshListBtn");
            btnRefresh.clicked += LoadItems;
            
            var categoryField = rootVisualElement.Q<EnumField>("ItemCategoryField");
            categoryField.Init(ItemCategory.None);
            
            var locationField = rootVisualElement.Q<EnumField>("EquipLocationField");
            locationField.Init(EquipLocation.None);

            // Set ObjectField types
            rootVisualElement.Q<ObjectField>("IconField").objectType = typeof(Sprite);
            rootVisualElement.Q<ObjectField>("PickupField").objectType = typeof(GameObject);
            rootVisualElement.Q<ObjectField>("ItemSetField").objectType = typeof(ItemSetData);
            
            // Set initial state
            itemIDField.value = Guid.NewGuid().ToString();
            
            categoryField.RegisterValueChangedCallback(evt => {
                currentCategory = (ItemCategory)evt.newValue;
                UpdateSetSystemVisibility();
            });
            currentCategory = (ItemCategory)categoryField.value;
            UpdateSetSystemVisibility();
        }

        private void SetupItemList()
        {
            itemListView = rootVisualElement.Q<ListView>("ItemListView");
            itemListView.makeItem = () => {
                var label = new Label();
                label.AddToClassList("list-item-label");
                return label;
            };
            itemListView.bindItem = (element, i) => {
                var label = element as Label;
                if(i < cachedItems.Count && cachedItems[i] != null)
                {
                   label.text = string.IsNullOrEmpty(cachedItems[i].displayName) ? cachedItems[i].name : cachedItems[i].displayName;
                }
            };
            itemListView.selectionChanged += (IEnumerable<object> selectedItems) => {
                var selected = selectedItems.FirstOrDefault() as _Item;
                if (selected != null)
                {
                    LoadItemIntoFields(selected);
                }
            };
        }

        private void LoadItems()
        {
            // Load items specifically from Resources folder using Unity APIs
            string folderPath = "Assets/Resources/Items";
            string[] guids = AssetDatabase.FindAssets("t:_Item", new[] { folderPath });
            cachedItems.Clear();
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                _Item item = AssetDatabase.LoadAssetAtPath<_Item>(assetPath);
                if (item != null)
                {
                    cachedItems.Add(item);
                }
            }
            
            itemListView.itemsSource = cachedItems;
            itemListView.Rebuild();
        }

        private void LoadItemIntoFields(_Item item)
        {
            currentlySelectedItem = item;
            
            rootVisualElement.Q<TextField>("ItemIDField").value = item.itemID;
            rootVisualElement.Q<TextField>("ItemTokenField").value = item.itemToken;
            rootVisualElement.Q<TextField>("ItemNameField").value = item.displayName;
            rootVisualElement.Q<EnumField>("ItemCategoryField").value = item.itemCategory;
            currentCategory = item.itemCategory;
            rootVisualElement.Q<EnumField>("EquipLocationField").value = item.allowedEquipLocation;
            rootVisualElement.Q<TextField>("DescriptionField").value = item.description;
            
            rootVisualElement.Q<ObjectField>("IconField").value = item.icon;
            rootVisualElement.Q<ObjectField>("PickupField").value = item.pickup;

            if (item.attributes != null)
            {
                rootVisualElement.Q<IntegerField>("AttrStr").value = item.attributes.Strength;
                rootVisualElement.Q<IntegerField>("AttrAgi").value = item.attributes.Agility;
                rootVisualElement.Q<IntegerField>("AttrVit").value = item.attributes.Vitality;
                rootVisualElement.Q<IntegerField>("AttrInt").value = item.attributes.Intelligence;
                rootVisualElement.Q<IntegerField>("AttrDex").value = item.attributes.Dexterity;
                rootVisualElement.Q<IntegerField>("AttrLuk").value = item.attributes.Luck;
            }

            if (item is EquipableItem equip)
            {
                rootVisualElement.Q<ObjectField>("ItemSetField").value = equip.itemSet;
            }
            else
            {
                rootVisualElement.Q<ObjectField>("ItemSetField").value = null;
            }

            UpdateSetSystemVisibility();
            rootVisualElement.Q<Button>("SaveItemBtn").text = "Update Item";
            
            SwitchTab(0, "Basic Info");
        }

        private void ClearFields()
        {
            currentlySelectedItem = null;
            
            rootVisualElement.Q<TextField>("ItemIDField").value = Guid.NewGuid().ToString();
            rootVisualElement.Q<TextField>("ItemTokenField").value = "";
            rootVisualElement.Q<TextField>("ItemNameField").value = "";
            rootVisualElement.Q<EnumField>("ItemCategoryField").value = ItemCategory.None;
            rootVisualElement.Q<EnumField>("EquipLocationField").value = EquipLocation.None;
            rootVisualElement.Q<TextField>("DescriptionField").value = "";
            
            rootVisualElement.Q<ObjectField>("IconField").value = null;
            rootVisualElement.Q<ObjectField>("PickupField").value = null;
            rootVisualElement.Q<ObjectField>("ItemSetField").value = null;

            rootVisualElement.Q<IntegerField>("AttrStr").value = 0;
            rootVisualElement.Q<IntegerField>("AttrAgi").value = 0;
            rootVisualElement.Q<IntegerField>("AttrVit").value = 0;
            rootVisualElement.Q<IntegerField>("AttrInt").value = 0;
            rootVisualElement.Q<IntegerField>("AttrDex").value = 0;
            rootVisualElement.Q<IntegerField>("AttrLuk").value = 0;

            rootVisualElement.Q<Button>("SaveItemBtn").text = "Create Item";
            itemListView.ClearSelection();
        }

        private void UpdateSetSystemVisibility()
        {
            var warning = rootVisualElement.Q<Label>("SetWarning");
            var setContainer = rootVisualElement.Q<VisualElement>("SetSystemContainer");
            
            bool isEquipable = currentCategory == ItemCategory.Equipable;
            warning.style.display = isEquipable ? DisplayStyle.None : DisplayStyle.Flex;
            setContainer.style.display = isEquipable ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void OnSaveItemClicked()
        {
            string itemName = rootVisualElement.Q<TextField>("ItemNameField").value;
            if (string.IsNullOrEmpty(itemName))
            {
                EditorUtility.DisplayDialog("Error", "O Nome do Item é obrigatório.", "OK");
                SwitchTab(0, "Basic Info");
                return;
            }

            _Item targetItem = currentlySelectedItem;
            bool isNew = targetItem == null;
            
            // Check if we need a new instance, or if category changed and type mismatch happens.
            if (isNew || (currentCategory == ItemCategory.Equipable && !(targetItem is EquipableItem)) || 
                (currentCategory != ItemCategory.Equipable && (targetItem is EquipableItem)))
            {
                if (currentCategory == ItemCategory.Equipable)
                {
                    targetItem = ScriptableObject.CreateInstance<EquipableItem>();
                }
                else
                {
                    targetItem = ScriptableObject.CreateInstance<_Item>();
                }
                isNew = true;
            }

            // Atribuições
            targetItem.itemID = rootVisualElement.Q<TextField>("ItemIDField").value;
            targetItem.itemToken = rootVisualElement.Q<TextField>("ItemTokenField").value;
            targetItem.displayName = itemName;
            targetItem.itemCategory = currentCategory;
            targetItem.allowedEquipLocation = (EquipLocation)rootVisualElement.Q<EnumField>("EquipLocationField").value;
            targetItem.description = rootVisualElement.Q<TextField>("DescriptionField").value;
            targetItem.icon = rootVisualElement.Q<ObjectField>("IconField").value as Sprite;
            targetItem.pickup = rootVisualElement.Q<ObjectField>("PickupField").value as GameObject;

            if (targetItem.attributes == null) targetItem.attributes = new _Attributs();
            targetItem.attributes.Strength = rootVisualElement.Q<IntegerField>("AttrStr").value;
            targetItem.attributes.Agility = rootVisualElement.Q<IntegerField>("AttrAgi").value;
            targetItem.attributes.Vitality = rootVisualElement.Q<IntegerField>("AttrVit").value;
            targetItem.attributes.Intelligence = rootVisualElement.Q<IntegerField>("AttrInt").value;
            targetItem.attributes.Dexterity = rootVisualElement.Q<IntegerField>("AttrDex").value;
            targetItem.attributes.Luck = rootVisualElement.Q<IntegerField>("AttrLuk").value;

            if (targetItem is EquipableItem equip)
            {
                equip.itemSet = rootVisualElement.Q<ObjectField>("ItemSetField").value as ItemSetData;
            }

            // Salvamento
            if (isNew)
            {
                if (!AssetDatabase.IsValidFolder("Assets/Resources")) AssetDatabase.CreateFolder("Assets", "Resources");
                if (!AssetDatabase.IsValidFolder("Assets/Resources/Items")) AssetDatabase.CreateFolder("Assets/Resources", "Items");

                string assetPath = $"Assets/Resources/Items/{itemName.Replace(" ", "_")}.asset";
                assetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);
                
                AssetDatabase.CreateAsset(targetItem, assetPath);
                currentlySelectedItem = targetItem;
                rootVisualElement.Q<Button>("SaveItemBtn").text = "Update Item";
            }
            
            EditorUtility.SetDirty(targetItem);
            AssetDatabase.SaveAssets();
            LoadItems(); // Update the list
            
            string msg = isNew ? "criado" : "atualizado";
            EditorUtility.DisplayDialog("Sucesso", $"Item '{itemName}' {msg} com sucesso!", "Confirmar");
            Selection.activeObject = targetItem;
        }
    }
}
