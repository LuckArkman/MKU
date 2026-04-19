using MKU.Scripts.HelthSystem;
using MKU.Scripts.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MKU.Scripts.CharacterSystem
{
    

    public class CharacterUIInfo : MonoBehaviour
    {
        public _Stats _base;
        public Image _lifeBar, _spBar, _nextLevelBar;
        public TextMeshProUGUI life, sp, level, _currentXP, _XPnextLevel;
        public CharacterProgression _progression;
        public CharController player;
        public Button _levelUP;
        
        

        private void Start()
        {
            
            player = CharSettings._Instance._charController;
            InvokeRepeating("OnUpdate", 1.0f,1.0f);
        }

        private void OnUpdate()
        {
            if(player == null) player = CharSettings._Instance._charController;
            if (player != null)
            {
                _base = player.GetComponent<IPlayer>().GetBase().Status;
                _progression = player.GetComponent<IPlayer>().GetProgression();
                if (_base != null)
                {
                    _lifeBar.fillAmount = _base.CurrentHP / _base.MaxHP;
                    _spBar.fillAmount = _base.CurrentSP / _base.MaxSP;
                    life.text = $"{_base.CurrentHP} / {_base.MaxHP}";
                    sp.text = $"{_base.CurrentSP} / {_base.MaxSP}";
                }

                if (_progression != null)
                {
                    level.text = _progression.level.ToString();
                    _currentXP.text = _progression.currentExperience.ToString();
                    _XPnextLevel.text = _progression.experienceToNextLevel.ToString();
                    float current = _progression.currentExperience;
                    float next = _progression.experienceToNextLevel;
                    _nextLevelBar.fillAmount = current / next;
                    _levelUP.interactable = _progression.currentExperience >= _progression.experienceToNextLevel;
                }
            }
        }
        public void OnLevelUP()
        => _progression.LevelUp();
        
    }
}