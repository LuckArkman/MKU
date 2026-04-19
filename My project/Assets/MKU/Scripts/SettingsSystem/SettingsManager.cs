using System;
using System.Collections.Generic;
using MKU.Scripts.InputSystem;
using MKU.Scripts.CharacterSystem;
using MKU.Scripts.Enums;
using MKU.Scripts.Models;
using MKU.Scripts.Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MKU.Scripts.SettingsSystem
{
    public class SettingsManager : GenericSettingsManager
    {
        [Header("Quality")]
        public TMP_Dropdown qualityDropDown;
        private int m_QualityIndex = 2;

        [Header("Resolution")]
        public TMP_Dropdown resolutionDropDown;
        private List<string> resolutions = new List<string>();
        private string m_ResolutionIndex = "";

        [Header("FullScreen")]
        public Toggle fullScreenToggle;
        private int m_FullScreenIndex = 1;

        [Header("RenderdDstance")]
        public Slider renderdDstanceSlider;
        public TMP_Text renderdDstanceText;
        private float renderdDstanceIndex = 1;

        [Header("Shadow Distance")]
        public Slider shadowDistanceSlider;
        public TMP_Text shadowDistanceText;
        private float shadowDistanceIndex;

        public InputManager inputManager;

        [Header("Grass Distance")]
        public Slider grassDistanceSlider;
        public TMP_Text grassDistanceText;
        private float grassDistanceIndex;
        public Button _apply, _cancel;


        public void Start()
        {
            open = true;
            ShowAllResolutions();
            _apply.onClick.AddListener(() => OnApply());
            _cancel.onClick.AddListener(() => InCancel());
        }

        private void InCancel()
        {
            open = false;
            var qp = Singleton.Instance._inputManager._inputSettings.Find(q => q.ui == UIType.Settings);
            qp.show = false;
            Destroy(this.gameObject);
        }

        private void OnApply()
        {
            for (int i = 0; i < 6; i++)
            {
                switch (i)
                {
                    case 1:
                    {
                        CharSettings._Instance._gameSettings.Add(new GameSettings("Quality", qualityDropDown.value, 0.0f));
                        break;
                    }
                    case 2:
                    {
                        CharSettings._Instance._gameSettings.Add(new GameSettings("Resolution", resolutionDropDown.value, 0.0f));
                        break;
                    }
                    case 3:
                    {
                        CharSettings._Instance._gameSettings.Add(new GameSettings("FullScreen", fullScreenToggle.isOn?1:0,0));
                        break;
                    }
                    case 4:
                    {
                        CharSettings._Instance._gameSettings.Add(new GameSettings("RenderdDstance", 0,renderdDstanceSlider.value));
                        break;
                    }
                    case 5:
                    {
                        CharSettings._Instance._gameSettings.Add(new GameSettings("Shadow Distance", 0,shadowDistanceSlider.value));
                        break;
                    }
                    default:
                    {
                        CharSettings._Instance._gameSettings.Add(new GameSettings("Grass Distance", 0,grassDistanceSlider.value));
                        break;
                    }
                }
            }
            open = false;
            var qp = Singleton.Instance._inputManager._inputSettings.Find(q => q.ui == UIType.Settings);
            qp.show = false;
            ApplySettings();
        }
        private void ShowAllResolutions()
        {
            resolutions.Clear();
            foreach (var resolution in Screen.resolutions)
            {
                string option = $"{resolution.width}x{resolution.height}";

                if (!resolutions.Contains(option))
                {
                    resolutions.Add(option);
                }
            }

            resolutionDropDown.ClearOptions();
            resolutionDropDown.AddOptions(resolutions);
        }
    }
}