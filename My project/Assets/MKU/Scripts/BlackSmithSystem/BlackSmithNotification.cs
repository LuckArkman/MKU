using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace MKU.Scripts.BlackSmithSystem
{
    public class BlackSmithNotification : MonoBehaviour
    {
        public TextMeshProUGUI _text;
        public void OnStart(string text)
        {
            _text.text = text;
            OnTimeDestroy();
        }

        private async void OnTimeDestroy()
        {
            await Task.Delay(2000);
            Destroy(this.gameObject);
        }
    }
}