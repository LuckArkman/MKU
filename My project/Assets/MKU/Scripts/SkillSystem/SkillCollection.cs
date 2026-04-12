using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MKU.Scripts.SkillSystem
{
    [Serializable]
    public class SkillCollection
    {
        public Skills _skill;
        public Image _image;
        public Image _imageReset;
        public TextMeshProUGUI timer;
    }
}