using System.Collections.Generic;
using MKU.Scripts.InputSystem;
using MKU.Scripts.BlackSmithSystem;
using MKU.Scripts.CookingSystem;
using MKU.Scripts.CraftingSystem;
using MKU.Scripts.Dialogue;
using MKU.Scripts.Enums;
using MKU.Scripts.EquipamentsSystem;
using MKU.Scripts.FightSystem;
using MKU.Scripts.HelthSystem;
using MKU.Scripts.Interfaces;
using MKU.Scripts.IventorySystem;
using MKU.Scripts.MarketSystem;
using MKU.Scripts.SettingsSystem;
using MKU.Scripts.Singletons;
using MKU.Scripts.SkillSystem;
using MKU.Scripts.FisicsSystem;
using MKU.Scripts.Strucs;
using TMPro;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.Serialization;
using Gravity = MKU.Scripts.FisicsSystem.Gravity;

namespace MKU.Scripts.CharacterSystem
{
    public class Controller : MonoBehaviour
    {
        public string Id;
        public bool jump;
        public TextMeshProUGUI _charName;
        public bool LevelUp;
        public bool _IsDeah;
        public Transform head;
        public BlackSmithUI _blackSmithUI;
        public CookingUI _cookingUI;
        public CraftingUI _craftingUI;
        public float smooth;
        public bool _IsScape;
        public bool StartMove;
        public Transform canva;
        public Transform camera;
        public GameObject _knight;
        public EquipamentUI _equipamentUI;
        public MarketUI _marketUI;
        public CharSettings _Settings;
        public Inventory _inventory;
        public GameObject spawnUI;
        public EscapeManager prefab;
        public Canvas canvas;
        public bool weapon;
        public float angle = 45f;
        public int rayCount = 10;
        public bool combo;
        public PickupSpawner _pickupSpawner;
        public Gravity _gravity = new Gravity();
        [FormerlySerializedAs("characterController")]
        public CharacterController charController;
        public InputManager inputManager;
        public bool IsWalk, Attack;
        public AIConversant _aiconversant;
        public bool interect;
        public InteractManager _interact;
        public Base targetBase = null;
        public bool notcanvas, regen;
        public string charId;
        public CharacterProgression progression = new CharacterProgression();
        public GameObject target;
        public Animator animator;
        public GameObject _hitPoint;
        public Camera _camera;
        public Transform look, sensor;
        public Vector3 position;
        public Vector3 rot = Vector3.zero;
        public float turn_Smooth = 0.015f;
        public float turn_Smooth_Time = 0.025f;
        public float turn_Smooth_angle = 0.025f;
        public _Attributs _attributes = new _Attributs();
        public _Attributs _originalAttributes;
        public _Stats _status;
        public _Stats status;
        public Base _base = new Base();
        public SkillTree _skillTree = new SkillTree();
        public List<WeaponCollection> _Collections = new();
        public List<CombateCollection> _combates = new();
        public TextMeshProUGUI charNameText;
        public TextMeshProUGUI _damage;

        public void GetHitPosition()
        {
            bool hitSomething = false;
            RaycastHit hit;
            if (Physics.Raycast(sensor.position, sensor.forward * 3.5f, out hit))
            {
                var _IpickupSpawner = hit.transform.GetComponent<IPickupSpawner>();
                if (_IpickupSpawner != null)
                {
                    _pickupSpawner = _IpickupSpawner.getPickupSpawner() as PickupSpawner;
                    interect = Vector3.Distance(this.transform.position, _pickupSpawner.transform.position) <= 2.5f;
                    if (interect)
                    {
                        _interact.gameObject.SetActive(_pickupSpawner.spawnedPickup != null);
                        _interact._text.text = $"to collect !";
                    }
                    else
                    {
                        _interact.gameObject.SetActive(false);
                        interect = false;
                    }
                }

                if (hit.transform == null)
                {
                    _aiconversant = null;
                    _pickupSpawner = null;
                }
            }
        }

        public async void OnRegenHP()
        {
            if (_status.CurrentHP < _status.MaxHP && _base.IsAlive()) _status.CurrentHP = _status.CurrentHP + 1;
            if (_status.CurrentSP < _status.MaxSP && _base.IsAlive()) _status.CurrentSP = _status.CurrentSP + 1;
        }

        public void OnWeapon()
        {
            _equipamentUI._equipmentSlots.ForEach(e =>
            {
                if (e.equipLocation == EquipLocation.Weapon)
                {
                    if (e.icon.item != null)
                    {
                        if (e.icon.item.dualhand)
                        {
                            _Collections.ForEach(w =>
                            {
                                w.left.left.enabled = w.left.Id == e.icon.item.itemID;
                                w.Right.right.enabled = w.Right.Id == e.icon.item.itemID;
                            });
                        }
                    }
                }
            });
        }

        public void OffWeapon()
        {
            _Collections.ForEach(w =>
            {
                w.left.left.enabled = false;
                w.Right.right.enabled = false;
            });
        }

        public void OnUpdateAtributtes()
        {
            List<_Attributs> _attribut = new();
            _equipamentUI._equipmentSlots.ForEach(e =>
            {
                if (e.icon.item != null)
                {
                    weapon = e.equipLocation == EquipLocation.Weapon;
                    EquipableItem equipableItem = e.icon.item as EquipableItem;
                    _attribut.Add(equipableItem._parcentes[equipableItem.level].attributes);
                }
            });
            weapon = _attribut.Count > 0;
            if (_attribut.Count > 0)
            {
                _originalAttributes = Singleton.Instance._originalAttributes;
                _attributes.RestoreOriginalValues(_originalAttributes);
                _attribut.ForEach(_ =>
                {
                    _attributes._addAttributs(_.Strength, _.Agility, _.Vitality, _.Intelligence, _.Dexterity,
                        _.Luck);
                });
                _base.StartCalc(_attributes, _status, progression.level);
            }

            if (_attribut.Count <= 0)
            {
                _attributes.RestoreOriginalValues(_originalAttributes);
            }

            _base.StartCalc(_attributes, _status, progression.level);
            status = _status.Clone();
        }
    }
}