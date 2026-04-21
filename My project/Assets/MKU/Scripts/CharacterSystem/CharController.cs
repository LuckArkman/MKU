using MKU.Scripts.CombateSystem;
using MKU.Scripts.HelthSystem;
using MKU.Scripts.Interfaces;
using MKU.Scripts.Singletons;

namespace MKU.Scripts.CharacterSystem
{
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class CharController : Controller, IPlayer, IDamageable
    {
        public Animator animator;
        [Header("Movement & Curve Settings")] 
        public float walkSpeed = 5f;
        public float runSpeed = 8.5f;
        public bool grounded;

        [Tooltip(
            "How fast the character turns while walking. Lower values create a wider parabolic curve. Higher values create a tighter curve.")]
        public float turnCurveSpeed = 120f;

        public LayerMask groundLayer;

        [Header("Realism (Sine Bobbing)")] public float bobbingFrequency = 15f;
        public float bobbingAmplitude = 0.1f;

        private Vector3 inputDirection;
        private Camera mainCamera;
        public bool isMoving = false;
        public bool isRunning = false;
        private float stepTimer = 0f;

        // Tracks the current mathematical angle for the curve
        private float currentMovementAngleRad;
        public bool weapon;
        public CharacterProgression progression = new CharacterProgression();
        
        private CombatProcessor _combatProcessor;

        void Start()
        {
            Singleton.Instance._charController = this;
            mainCamera = Camera.main;

            if (_base.Status == null) _base.Status = new _Stats();
            _base.StartCalc(_base.Attributes, _base.Status, progression.level);

            currentMovementAngleRad = Mathf.Atan2(transform.forward.z, transform.forward.x);
            _combatProcessor = new CombatProcessor(_base, transform, animator);
        }

        void Update()
        {
            grounded = _gravity.IsGrounded(charController);
            HandleInput();
            HandleCombat();
            if(!_gravity.IsGrounded(charController))_gravity.IsGravity(charController);
            MoveCharacterWithCurve();

            if (animator != null)
            {
                animator.SetBool("walk", isMoving && !isRunning);
                animator.SetBool("run", isRunning);
            }
        }

        private void HandleCombat()
        {
            if (_combatProcessor == null) return;
            _combatProcessor.UpdateCooldown(Time.deltaTime);

            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                Vector3 startPos = sensor != null ? sensor.position : transform.position + Vector3.up;
                Vector3 dir = sensor != null ? sensor.forward : transform.forward;

                _combatProcessor.ExecutePhysicsAttack(startPos, dir, 2.5f);
            }
        }

        private void HandleInput()
        {
            if (Keyboard.current == null) return;

            float dirX = 0f;
            float dirZ = 0f;

            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) dirZ += 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) dirZ -= 1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) dirX += 1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) dirX -= 1f;

            inputDirection = new Vector3(dirX, 0, dirZ).normalized;
            isMoving = inputDirection.magnitude > 0.1f;
            isRunning = isMoving && Keyboard.current.shiftKey.isPressed;
        }

        private void MoveCharacterWithCurve()
        {
            if (!isMoving) 
            {
                stepTimer = 0f;
                return;
            }

            // Converter a direção de input (WASD) para ser relativa à câmera (Terceira Pessoa)
            Vector3 camForward = mainCamera.transform.forward;
            Vector3 camRight = mainCamera.transform.right;
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 relativeMoveDirection = camForward * inputDirection.z + camRight * inputDirection.x;

            if (relativeMoveDirection.magnitude > 0.1f)
            {
                // 1. Calcular o ângulo alvo absoluto baseado no movimento relativo à câmera
                float targetAngleRad = Mathf.Atan2(relativeMoveDirection.z, relativeMoveDirection.x);

                // 2. Converter radianos para graus para suavização no Unity
                float currentDeg = currentMovementAngleRad * Mathf.Rad2Deg;
                float targetDeg = targetAngleRad * Mathf.Rad2Deg;

                // 3. Suavizar transição em direção ao alvo (Mantém a Curva Parabólica Seno/Coseno)
                currentDeg = Mathf.MoveTowardsAngle(currentDeg, targetDeg, turnCurveSpeed * Time.deltaTime);

                // Converter de volta para radianos
                currentMovementAngleRad = currentDeg * Mathf.Deg2Rad;

                // 4. --- MOVIMENTO SENO E COSSENO ---
                float currentSpeed = isRunning ? runSpeed : walkSpeed;
                float moveX = Mathf.Cos(currentMovementAngleRad) * currentSpeed * Time.deltaTime;
                float moveZ = Mathf.Sin(currentMovementAngleRad) * currentSpeed * Time.deltaTime;

                // Aplica o movimento curvo usando o CharacterController
                charController.Move(new Vector3(moveX, 0, moveZ));

                // 5. Alinha instantaneamente a rotação visual
                Vector3 curveMoveDirection = new Vector3(moveX, 0, moveZ).normalized;
                if (curveMoveDirection != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(curveMoveDirection);
                }
            }
        }

        public GameObject GetPlayer()
        {
            return gameObject;
        }

        public void HitAttack(Base _baseEnemy)
        {
             // Deprecated legacy interface requirement (maintained to not break IPlayer)
        }

        public void TakeDamage(int damage)
        {
            if (!_base.IsAlive()) return;
            if (animator != null) animator.SetTrigger("Hit"); 
            
            _base.TakeDamage(damage);
            
            if (!_base.IsAlive())
            {
                if (animator != null) animator.SetTrigger("Die");
            }
            else
            {
                OnRegenHP(); // Aciona o gatilho da rotina de cura sobre tempo sempre que ferido
            }
        }

        public Transform GetTransform() => transform;
        public Base GetBaseStats() => _base;

        public Base GetBase()
        {
            return _base;
        }

        public CharacterProgression GetProgression()
        {
            return progression;
        }

        public Transform GetspawnUI()
        {
            throw new System.NotImplementedException();
        }

        public Transform GetCanvas()
        {
            throw new System.NotImplementedException();
        }

        public void SetTaskCollection(object taskCollections)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateInventory()
        {
            throw new System.NotImplementedException();
        }

        public void OnStart()
        {
            throw new System.NotImplementedException();
        }

        public void OnUpdateStatus()
        {
            throw new System.NotImplementedException();
        }

        public void OnAttackHit()
        {
            throw new System.NotImplementedException();
        }

        public bool GetTarget()
        {
            throw new System.NotImplementedException();
        }
    }
}