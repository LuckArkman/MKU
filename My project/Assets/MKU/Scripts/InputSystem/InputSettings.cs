using System;
using MKU.Scripts.Enums;
using UnityEngine;

namespace MKU.Scripts.InputSystem
{
    [Serializable]
    public class InputSettings
    {
        public KeyCode toggleKey = KeyCode.Escape;
        public UIType ui = UIType.None;
        public Transform canvas;
        public GameObject _object;
        public bool show;
    }
}