using System.Collections.Generic;
using System.Linq;
using MKU.Scripts.Cam;
using MKU.Scripts.FightSystem;
using MKU.Scripts.FinanceSystem;
using MKU.Scripts.IventorySystem;
using MKU.Scripts.AISystem;
using MKU.Scripts.BlackSmithSystem;
using MKU.Scripts.InputSystem;
using MKU.Scripts.Models;
using MKU.Scripts.Strucs;
using UnityEngine;

namespace MKU.Scripts.CharacterSystem
{
    public class GenericSettings : MonoBehaviour
    {
        public bool update;
        public bool upgrade;
        public bool interactable;
        public CameraController camSystem;
        public GameObject _player;
        public FinanceController _financeController;
        public Vector3 _lastPosition;
        public ReSpawn _reSpawn;
        public GameObject _knight;
        public BlackSmith _blackSmith;
        public CharController _charController;
        public Transform _camera;
        public List<InventorySlotUI> _inventorySlots = new();
        public List<EquipmentSlotUI> _equipmentSlots = new();
        public List<Bag> _bags = new ();
        public CharInventory inventory;
        public AIController _aIController;
        public InputManager _inputManager;
        public Inventory _inventory;
        public int hit;
        public List<AIController> _enemys = new ();
        public List<GameSettings> _gameSettings = new ();
        public AIController target;
        public Fighter _Fighter = new Fighter();

        public AIController GetNearestEnemyGameObject()
        {
            if (_enemys.Count > 0 && _player != null && _charController != null)
            {
                var nearestEnemy = _enemys
                    .OrderBy(enemy =>
                    {
                        return Vector3.Distance(_charController.transform.position, enemy.transform.position);
                    })
                    .FirstOrDefault();

                return nearestEnemy != null ? nearestEnemy : null;
            }
            return null;
        }

        public void OnActiveEquipament()
            => _charController.OnUpdateAtributtes();
    }
}