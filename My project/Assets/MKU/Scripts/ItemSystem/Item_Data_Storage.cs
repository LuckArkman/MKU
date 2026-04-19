using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MKU.Scripts.ItemSystem
{
    [CreateAssetMenu(fileName = "New ItemDataStorage", menuName = "StorageData/DataStorage")]
public class Item_Data_Storage : ScriptableObject {

    public List<Item> item_equipment_set = new List<Item>();
    public List<Item> item_usable_set = new List<Item>();
    public List<Item> item_etc_set = new List<Item>();
    public List<Item> item_gold = new List<Item>(1);
    public List<Item> trophies = new List<Item>();
    public List<Item> item_gather = new List<Item>();

    [ContextMenu("Refresh")]
    private void Refresh() {

    }

    private int IDComparation(Item A, Item B)
    {
        return 0;
    }

    public Item Get_Item(int item_id) {

        Item item = null;

        item ??= FindID(item_id, item_equipment_set.ToArray());
        item ??= FindID(item_id, item_usable_set.ToArray());
        item ??= FindID(item_id, item_etc_set.ToArray());
        item ??= FindID(item_id, item_gold.ToArray());
        item ??= FindID(item_id, trophies.ToArray());
        item ??= FindID(item_id, item_gather.ToArray());

        return item;

    }

    private Item FindID(int item_id, Item[] ItemList) {

        return null;

    }

#if UNITY_EDITOR
    #region EDITOR
    [Space(20)] public List<Item> filteredItemList = new();
    [HideInInspector] public string filterInput = "";

    public void ApplyFilter()
    {
        UnityEditor.EditorUtility.SetDirty(this);
    }
    #endregion
#endif

}

#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(Item_Data_Storage))]
public class Item_Data_StorageEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        Item_Data_Storage script = (Item_Data_Storage)target;
        
        DrawDefaultInspector();
        UnityEditor.EditorGUILayout.LabelField("Filtro:");
        script.filterInput = UnityEditor.EditorGUILayout.TextField(script.filterInput);

        if (GUILayout.Button("Aplicar Filtro", GUILayout.Height(30)))
        {
            script.ApplyFilter();
        }

        if (GUILayout.Button("Limpar Filtro", GUILayout.Height(30)))
        {
            script.filterInput = null;
            script.ApplyFilter();
        }
    }
}
#endif
}