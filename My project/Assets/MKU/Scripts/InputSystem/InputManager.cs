using System.Collections.Generic;
using MKU.Scripts.Enums;
using MKU.Scripts.SettingsSystem;
using MKU.Scripts.Singletons;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace MKU.Scripts.InputSystem
{
    public class InputManager : MonoBehaviour
    {
        public InputController inputController = new();
        public List<InputSettings> _inputSettings = new();
        public bool isSkill;
        public bool isAttack;
        public Button _settings;
        public float holdTime = 0f;

        public SettingsManager settingsManager;

        private void Start()
        {
            Singleton.Instance._inputManager = this;
            _settings.onClick.AddListener(() => OnUISettings());
        }

        private void OnUISettings()
        {
            _inputSettings.ForEach(i =>
            {
                if (Input.GetKeyDown(i.toggleKey))
                {
                    if (i.ui == UIType.Settings)
                    {                    
                        if (!i.show)
                        {
                            var obj = Instantiate<GameObject>(i._object, i.canvas);
                            i.show = true;
                        }
                    }
                }
            });
        }

        private void OnSettings()
        {
            _inputSettings.ForEach(i =>
            {
                if (Input.GetKeyDown(i.toggleKey))
                {
                    Debug.Log($"{nameof(OnUISettings)} >> {i.toggleKey} >>{i.ui}");
                    if (i.ui == UIType.inventory)
                    {
                        if (!i.show)
                        {
                            Debug.Log($"{nameof(OnUISettings)} >> {i.toggleKey} >> {UIType.inventory}");
                            i._object.SetActive(true);
                        }
                    }
                    else
                    {
                        if (!i.show)
                        {
                            var obj = Instantiate<GameObject>(i._object, i.canvas);
                            obj.SetActive(true);
                            i.show = true;
                        }
                    }
                }
            });
        }

        private void LateUpdate()
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) inputController.ActionB = 0;
            if (Input.GetMouseButton(0))
            {
                holdTime += Time.deltaTime;
                if (holdTime >= 0.1f) inputController.ActionB = 0;
            }
            else
            {
                holdTime = 0f;
                inputController.ActionB = 0;
            }
            OnSettings();
            MouseValues();
        }

        private void MouseValues()
        {
            inputController.mouseX = Mouse.current.delta.ReadValue().x;
            inputController.mouseY = Mouse.current.delta.ReadValue().y;
        }

        public void GetHorizontal(InputAction.CallbackContext callback) =>
            inputController.horizontal = callback.ReadValue<float>();

        public void GetVertical(InputAction.CallbackContext callback) =>
            inputController.vertical = callback.ReadValue<float>();

        public void GetRunning(InputAction.CallbackContext callback) =>
            inputController.run = callback.ReadValue<float>();

        public void Scroll(InputAction.CallbackContext callback) =>
            inputController.scrollValue = callback.ReadValue<float>();

        public void ActionA(InputAction.CallbackContext callback) =>
            inputController.ActionA = callback.ReadValue<float>();

        public void ActionJump(InputAction.CallbackContext callback) =>
            inputController.ActionE = callback.ReadValue<float>();


        public void ActionB(InputAction.CallbackContext callback)
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
            if (holdTime < 0.1f) inputController.ActionB = callback.ReadValue<float>();
        }

        public void Interact(InputAction.CallbackContext callback) =>
            inputController.ActionC = callback.ReadValue<float>();

        public void Settings(InputAction.CallbackContext callback) =>
            inputController.ActionD = callback.ReadValue<float>();
    }
}