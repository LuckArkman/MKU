using System;
using MKU.Scripts.CharacterSystem;
using MKU.Scripts.Enums;
using MKU.Scripts.Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MKU.Scripts.SettingsSystem
{
    public class EscapeManager : MonoBehaviour
    {
        
        public TextMeshProUGUI timerText;
        [Range(0f, 1000f)]
        public MessageModal messageModal;
        public int minutes;
        public int seconds;
        private float currentTime;
        public void OnStart(float totalTime)
        {
            currentTime = totalTime;
            UpdateTimerText();
        }

        public void CharReturInfo()
        {
            messageModal.gameObject.SetActive(true);
            messageModal.OnStart(ActionCode.Return);
        }


        void Update()
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                currentTime = Mathf.Max(currentTime, 0);
                UpdateTimerText();
            }
        }

        void UpdateTimerText()
        {
            minutes = Mathf.FloorToInt(currentTime / 60);
            seconds = Mathf.FloorToInt(currentTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            if (minutes == 00 && seconds == 00)
            {
                CharSettings._Instance.OnReSpawn();
            }
        }
        
        public void ReturnSelectCharacter()
            => Singleton.Instance.OnLoadScene("SelectCharacter");
        
        public void ExitGame()
            => Application.Quit();
    }
}