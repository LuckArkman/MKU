using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MKU.Scripts.CharacterSystem
{
    public class SelectCharacter : MonoBehaviour
    {
        public AccountCharacters accountCharacters;
        public string classCharacter;
        public GameObject rent, staked;
        public TextMeshProUGUI _level;
        public Button button;

        private void Start()
        {
            button.onClick.AddListener(() => AvatarSelected());
        }

        private void AvatarSelected()
        {
            accountCharacters.OnSelectAvatar(classCharacter);
        }
    }
}