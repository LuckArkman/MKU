using MKU.Scripts.ItemSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MKU.Scripts.IventorySystem
{
    public class ItemTooltip : MonoBehaviour
    {
        public TextMeshProUGUI titleText = null;
        public TextMeshProUGUI budyText = null;
        public GameObject containerAtributs, titleStatus;
        public Image icon;
        public TextMeshProUGUI atributs;
        [SerializeField] GameObject containerStatus;

        public void Setup(_Item item)
        {
            if (item.shoulModifiers)
            {
                titleStatus.SetActive(true);
                titleText.text = item.displayName;
                Debug.Log($"{nameof(Setup)} >> {item.displayName}");
                budyText.text = item.description;
                icon.sprite = item.icon;
                containerAtributs.SetActive(true);
                containerStatus.SetActive(true);
                var text1 = Instantiate<TextMeshProUGUI>(atributs, containerAtributs.transform);
                text1.text = $"str: {item._parcentes[item.level].attributes.Strength}";
                var text2 = Instantiate<TextMeshProUGUI>(atributs, containerAtributs.transform);
                text2.text = $"vit: {item._parcentes[item.level].attributes.Vitality}";
                var text3 = Instantiate<TextMeshProUGUI>(atributs, containerAtributs.transform);
                text3.text = $"agi: {item._parcentes[item.level].attributes.Agility}";
                var text5 = Instantiate<TextMeshProUGUI>(atributs, containerAtributs.transform);
                text5.text = $"int: {item._parcentes[item.level].attributes.Intelligence}";
                var text6 = Instantiate<TextMeshProUGUI>(atributs, containerAtributs.transform);
                text6.text = $"dex: {item._parcentes[item.level].attributes.Dexterity}";
                var text7 = Instantiate<TextMeshProUGUI>(atributs, containerAtributs.transform);
                text7.text = $"luc: {item._parcentes[item.level].attributes.Luck}";
            }
            if (!item.shoulModifiers)
            {
                Debug.Log($"{nameof(Setup)} >> {item.displayName}");
                titleText.text = item.displayName;
                budyText.text = item.description;
                icon.sprite = item.icon;
                titleStatus.SetActive(false);
                containerAtributs.SetActive(false);
                containerStatus.SetActive(false);
            }
        }
    }
}