using System;
using System.Collections.Generic;
using MKU.Scripts.SkillSystem;
using UnityEngine.UI;

namespace MKU.Scripts.CharacterSystem
{
    [Serializable]
    public class CharacterSkillsInformation
    {
        public CharacterSkillsInformation(){}
        public List<SkillsUIInformation> skillIcon = new();

        public void UpdateSkillInformation(List<SkillCollection> _Skills)
        {
            for (int i = 0; i < _Skills.Count; i++)
            {
                skillIcon[i].SetSkillInfo(_Skills[i]._skill);
            }
        }
    }
}