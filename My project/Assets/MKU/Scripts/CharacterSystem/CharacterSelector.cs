using MKU.Scripts.Models;
using UnityEngine;

namespace MKU.Scripts.CharacterSystem
{
    public class CharacterSelector : MonoBehaviour
    {
        public AtributtesInformation _information = new AtributtesInformation();
        
        public CharacterInformation _CharacterInformation = new CharacterInformation();

        public CharacterSkillsInformation _skillsInformation = new CharacterSkillsInformation();

        public void UIUpdate(Character _character, CharacterPatterns _patterns)
        {
            _information.SetInformation(_character);
            _CharacterInformation.SetInformation(_character);
            _skillsInformation.UpdateSkillInformation(_patterns._Skills);
        }
    }
}