using MKU.Scripts.Enums;
using MKU.Scripts.HelthSystem;
using MKU.Scripts.InputSystem;
using MKU.Scripts.IventorySystem;
using MKU.Scripts.Singletons;
using MKU.Scripts.Tasks;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace MKU.Scripts.CharacterSystem
{
    public class CharSettings : GenericSettings
    {
        private static CharSettings _instance;

        public static CharSettings _Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<CharSettings>();
                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject("CharSettings");
                        _instance = singletonObject.AddComponent<CharSettings>();
                    }
                }

                return _instance;
            }
        }

        public void Start()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }

            if (_inventory == null)
            {
               // Debug.Log($"{nameof(Start)} >> CharSettings");
                if(_charController != null) _charController.GetComponent<Inventory>();
            }

            if(_financeController is not null)_financeController.OnStart();
        }

        public void OnStart()
        {
            Debug.Log($"{nameof(Start)} >> CharSettings");
            if (_inventory == null)
            {
                _inputManager = _charController.GetComponent<InputManager>();
                _charController.GetComponent<Inventory>();
            }
        }


        private void Update()
        {
            OnShowCanvas();
            if (_enemys.Count > 0)
            {
                target = GetNearestEnemyGameObject();
                if (target != null)
                {
                    target.SetTarget(true);
                    _enemys.ForEach(e =>
                    {
                        if (target.GetInstance() != e.GetInstance())
                        {
                            e.SetTarget(false);
                        }
                    });
                }
            }

            if (update)
            {
                update = false;
                _charController.UpdateInventory();
            }
        }

        private void OnShowCanvas()
        {
            if (_inputManager == null && _charController != null) _inputManager = Singleton.Instance._inputManager;
            if (_inputManager != null)
            {
                _inputManager._inputSettings.ForEach(i =>
                {
                    if (Input.GetKeyDown(i.toggleKey) && i.ui == UIType.inventory)
                    {
                        i._object.SetActive(!i._object.activeSelf);
                    }

                    if (Input.GetKeyDown(i.toggleKey) && i.ui == UIType.questUI)
                    {
                        QuestPanel x = i._object.GetComponent<QuestPanel>();
                        if (!x.show)
                        {
                            var questUI = Instantiate<QuestPanel>(x, i.canvas);
                            x.show = true;
                        }
                    }
                });
            }
        }

        public void OnShowInventory(UIType ui)
        {
            if (_inputManager == null) _inputManager = _charController.inputManager;
            if (_inputManager != null)
            {
                _inputManager._inputSettings.ForEach(i => { i._object.SetActive(ui == i.ui); });
            }
        }

        public void OnShowQuest(UIType ui)
        {
            if (_inputManager == null) _inputManager = _charController.inputManager;
            if (_inputManager != null)
            {
                _inputManager._inputSettings.ForEach(i => { i._object.SetActive(ui == i.ui); });
            }
        }

        public void OnReSpawn()
        {
            Destroy(_player);
            if (Singleton.Instance._character == null)Instantiate(_reSpawn._gameObject, _reSpawn.transform.position, quaternion.identity);
            if (Singleton.Instance._character != null)
            {
                Character _character = Singleton.Instance._character;
                _reSpawn._characters.ForEach(x =>
                {
                    if (x._character.name == _character.classCharacter)
                    {
                        var character = Instantiate(x.characterModel, _reSpawn.transform.position, Quaternion.identity);
                        CharController controller = character.GetComponentInChildren<CharController>();
                        controller._base.Attributes = new _Attributs(Singleton.Instance._character.str, _character.agi,
                            _character.vit, _character.inteligence, _character.luk, _character.def);
                        _player = character;
                        controller.Id = _character.id;
                        _inputManager = controller.inputManager;
                        _charController = controller;
                        controller.OnStart();
                        controller.charNameText.text = _character.classCharacter;
                    }
                });
            }
        }
    }
}