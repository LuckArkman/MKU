using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace MKU.Scripts.IventorySystem
{
    public class ItemFeedback : MonoBehaviour
    {
        public TextMeshProUGUI nunber;
        public TextMeshProUGUI name;

        private void Start()
        {
            StartCoroutine(TimerDestroy());
        }

        public void OnStart(int number, string name)
        {
            this.nunber.text = $"{number}";
            this.name.text = $"{name}";
        }

        private IEnumerator TimerDestroy()
        {
            yield return new WaitForSeconds(2.0f);
            Destroy(this.gameObject);
        }
    }
}