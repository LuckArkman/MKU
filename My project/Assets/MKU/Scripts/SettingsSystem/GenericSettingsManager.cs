using System.Collections.Generic;
using MKU.Scripts.InputSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MKU.Scripts.SettingsSystem
{
    public class GenericSettingsManager : MonoBehaviour
    {
        [Header("Quality")] public TMP_Dropdown qualityDropDown;
        private int m_QualityIndex = 2;
        public Transform camera;
        public bool open;
        public InputManager inputManager;

        [Header("Resolution")] public TMP_Dropdown resolutionDropDown;
        private List<string> resolutions = new List<string>();
        private string m_ResolutionIndex = "";

        [Header("FullScreen")] public Toggle fullScreenToggle;
        private int m_FullScreenIndex = 1;

        [Header("RenderdDstance")] public Slider renderdDstanceSlider;
        public TMP_Text renderdDstanceText;
        private float renderdDstanceIndex = 1;

        [Header("Shadow Distance")] public Slider shadowDistanceSlider;
        public TMP_Text shadowDistanceText;
        private float shadowDistanceIndex;

        [Header("Grass Distance")] public Slider grassDistanceSlider;
        public TMP_Text grassDistanceText;
        private float grassDistanceIndex;
        public Button _apply, _cancel;

        public void ApplySettings()
        {
            int selectedQualityIndex = qualityDropDown.value;
            QualitySettings.SetQualityLevel(selectedQualityIndex);

            int selectedResolutionIndex = resolutionDropDown.value;
            string selectedResolution = resolutions[selectedResolutionIndex];
            string[] resolutionParts = selectedResolution.Split('x');
            int width = int.Parse(resolutionParts[0]);
            int height = int.Parse(resolutionParts[1]);
            bool isFullScreen = fullScreenToggle.isOn;
            Screen.SetResolution(width, height, isFullScreen);

            float renderDistance = renderdDstanceSlider.value;
            camera.GetComponent<Camera>().farClipPlane = renderDistance;

            float shadowDistance = shadowDistanceSlider.value;
            QualitySettings.shadowDistance = shadowDistance;

            float grassDistance = grassDistanceSlider.value;
            Terrain[] terrains = FindObjectsOfType<Terrain>();
            foreach (Terrain terrain in terrains)
            {
                terrain.detailObjectDistance = grassDistance;
            }
            Destroy(this.gameObject);
        }
    }
}