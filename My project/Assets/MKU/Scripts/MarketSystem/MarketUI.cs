using MKU.Scripts.CharacterSystem;
using MKU.Scripts.Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MKU.Scripts.MarketSystem
{
    public class MarketUI : MonoBehaviour
    {
        public TextMeshProUGUI shopName;
        public Transform listRoot;
        public RowUI rowPrefab;
        public TextMeshProUGUI totalUI;
        public Button confirmButton;
        public Button SwitchButton;
        public MarketPlace marketPlace = null;
        public Market currentMarket = null;
        public Transform marketParent;
        Color originalTotalTextColor;

        public void OnStart(CharController player)
        {
            currentMarket = Singleton.Instance._market;
            currentMarket = marketParent.GetComponent<Market>();
            originalTotalTextColor = totalUI.color;
            marketPlace = player.GetComponent<MarketPlace>();
            if (marketPlace == null) return;
            confirmButton.onClick.AddListener(ConfirmTransaction);
            SwitchButton.onClick.AddListener(SwitchMode);
            if(currentMarket != null)refreshUI();
        }
        public void OnQuit()=> this.gameObject.SetActive(false);

        private void refreshUI()
        {
            shopName.text = currentMarket.GetMarketName();
            for(int i = 0; i < listRoot.transform.childCount; i++)
            {
                Destroy(listRoot.transform.GetChild(i).gameObject);
            }
            foreach(var item in currentMarket.stockConfig)
            {
                 RowUI row = Instantiate<RowUI>(rowPrefab, listRoot);
                row.Setup(currentMarket,item.Item, currentMarket.isBuyingmode);
            }
            totalUI.text = $"Total: {currentMarket.TransactionTotal()}";
            totalUI.color = currentMarket.HasSuficientFounds() ? originalTotalTextColor : Color.red;
            confirmButton.interactable = currentMarket.CanTransaction();
            TextMeshProUGUI switchText = SwitchButton.GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI sellingText = confirmButton.GetComponentInChildren<TextMeshProUGUI>();

            if (currentMarket.IsBuyingmode())
            {
                switchText.text = "Switch to Selling";
                sellingText.text = "Buy";
            }
            else
            {
                switchText.text = "Switch to Buying";
                sellingText.text = "Sell";
            }
            foreach (FilterButtonUI button in GetComponentsInChildren<FilterButtonUI>())
            {
                button.RefreshUI();
            }
        }
        public void ConfirmTransaction()
        => currentMarket.ConfirmTransaction();
        public void SwitchMode()
        => currentMarket.SelectMode(!currentMarket.IsBuyingmode());
    }
}