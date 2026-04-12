using System;
using MKU.Scripts.SkillSystem;
using TMPro;
using UnityEngine.UI;

namespace MKU.Scripts.CharacterSystem
{
    [Serializable]
    public class SkillsUIInformation
    {
        public SkillsUIInformation(){}
        public Skills _skill;
        public Image _image;
        public TextMeshProUGUI name;
        public TextMeshProUGUI description;

        public void SetSkillInfo(Skills _skill)
        {
            this._skill = _skill;
            this._image.sprite = _skill.Icon;
            this.name.text = _skill.Name;
            this.description.text = _skill.Description;
        }
    }
}