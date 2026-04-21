using System.Collections;
using MKU.Scripts.CombateSystem;
using MKU.Scripts.HelthSystem;
using MKU.Scripts.Interfaces;
using MKU.Scripts.Singletons;
using UnityEngine;

namespace MKU.Scripts.AISystem
{
    /// <summary>
    /// Gerencia o comportamento de IA fazendo uso de CharacterController 
    /// focado em baixo uso computacional, eliminando o NavMesh.
    /// Requer modelo físico e detecção própria (Raycast/Whiskers).
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class AIController : MonoBehaviour, IDamageable
    {
        public enum AIState { Patrol, Chase, Return }
        
        public Animator animator;

        [Header("Estado Atual (somente leitura)")]
        [SerializeField] private AIState currentState = AIState.Patrol;
        public bool isMovement = false;

        [HideInInspector] public SpawnPointController homeSpawn;

        [Header("Quest Identidade")]
        [Tooltip("ID usado pelo QuestSystem para registrar abate de monstros (ex: 'Slime', 'Goblin').")]
        public string enemyIdForQuest = "Slime";

        [Header("Navegação (CharacterController)")]
        public float moveSpeed = 3.5f;
        public float rotationSpeed = 8f;
        public float gravity = 15f;
        
        [Tooltip("Camada de física que os ignorará, deixe apenas paredes árvores rochas, etc.")]
        public LayerMask obstacleLayer = ~0;
        public float avoidanceDistance = 2f;

        [Header("Detecção")]
        public float aggroRange = 5f;
        public float aggroResetTime = 10f;

        [Header("Patrulha")]
        public float patrolRadius = 5f;
        public float waypointReachedDistance = 1f;
        public float patrolWaitTime = 1.5f;

        [Header("Combate")]
        public int xpReward = 50;
        public float attackRange = 1.5f;
        private CombatProcessor _combatProcessor;

        [Header("Atributos RPG")]
        public _Attributs attributes = new _Attributs(10, 8, 10, 5, 6, 3);
        public _Stats status;
        public int level = 1;

        [HideInInspector] public Base baseStats = new Base();

        private CharacterController _controller;
        private Transform _player;
        private float _outOfAggroTimer;
        private Vector3 _homePosition;
        private bool _isWaitingAtWaypoint;
        private Coroutine _waypointWaitCoroutine;
        
        // Navigation targets
        private Vector3 _currentTarget;
        private float _verticalVelocity = 0f;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            if (animator == null) animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            _homePosition = transform.position;
            status = new _Stats();
            baseStats.StartCalc(attributes, status, level);
            _combatProcessor = new CombatProcessor(baseStats, transform, animator);

            if (Singleton.Instance != null && Singleton.Instance._charController != null)
                _player = Singleton.Instance._charController.transform;

            GoToNextPatrolPoint();
        }

        private void Update()
        {
            if (_player == null && Singleton.Instance != null && Singleton.Instance._charController != null)
                _player = Singleton.Instance._charController.transform;

            switch (currentState)
            {
                case AIState.Patrol:  UpdatePatrol();  break;
                case AIState.Chase:   UpdateChase();   break;
                case AIState.Return:  UpdateReturn();  break;
            }
            animator.SetBool("walk", isMovement);
        }

        private void UpdatePatrol()
        {
            if (_player != null && IsPlayerInAggroRange())
            {
                EnterChase();
                return;
            }

            if (!_isWaitingAtWaypoint)
            {
                isMovement = true;
                MoveTowardsTarget();
                if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(_currentTarget.x, 0, _currentTarget.z)) <= waypointReachedDistance)
                {
                    _waypointWaitCoroutine = StartCoroutine(WaitAtWaypoint());
                }
            }
            else
            {
                isMovement = false;
                ApplyGravityOnly();
            }
        }

        private IEnumerator WaitAtWaypoint()
        {
            _isWaitingAtWaypoint = true;
            yield return new WaitForSeconds(patrolWaitTime);
            _isWaitingAtWaypoint = false;
            GoToNextPatrolPoint();
        }

        private void GoToNextPatrolPoint()
        {
            Vector2 randomCircle = Random.insideUnitCircle * patrolRadius;
            _currentTarget = _homePosition + new Vector3(randomCircle.x, 0, randomCircle.y);
        }

        private void EnterChase()
        {
            if (_waypointWaitCoroutine != null)
            {
                StopCoroutine(_waypointWaitCoroutine);
                _isWaitingAtWaypoint = false;
            }
            _outOfAggroTimer = 0f;
            currentState = AIState.Chase;
        }

        private void UpdateChase()
        {
            if (_player == null)
            {
                EnterReturn();
                return;
            }

            _currentTarget = _player.position;
            float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

            if (_combatProcessor != null) _combatProcessor.UpdateCooldown(Time.deltaTime);

            if (distanceToPlayer <= attackRange)
            {
                isMovement = false;
                ApplyGravityOnly(); 
                
                if (_player != null)
                {
                    Vector3 lookDirection = _player.position - transform.position;
                    lookDirection.y = 0;
                    if (lookDirection.sqrMagnitude > 0.01f)
                    {
                        Quaternion targetRot = Quaternion.LookRotation(lookDirection);
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * 1.5f * Time.deltaTime);
                    }
                }

                if (animator != null) animator.SetBool("Attack", true);
            }
            else
            {
                if (animator != null) animator.SetBool("Attack", false);
                isMovement = true;
                MoveTowardsTarget();
            }

            if (distanceToPlayer > aggroRange)
            {
                _outOfAggroTimer += Time.deltaTime;
                if (_outOfAggroTimer >= aggroResetTime)
                {
                    EnterReturn();
                }
            }
            else
            {
                _outOfAggroTimer = 0f;
            }
        }

        private void EnterReturn()
        {
            _outOfAggroTimer = 0f;
            currentState = AIState.Return;
            _currentTarget = _homePosition;
        }

        private void UpdateReturn()
        {
            if (_player != null && IsPlayerInAggroRange())
            {
                EnterChase();
                return;
            }

            isMovement = true;
            MoveTowardsTarget();

            if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(_currentTarget.x, 0, _currentTarget.z)) <= waypointReachedDistance)
            {
                currentState = AIState.Patrol;
                GoToNextPatrolPoint();
            }
        }

        private void ApplyGravityOnly()
        {
            if (_controller.isGrounded) _verticalVelocity = -gravity * Time.deltaTime;
            else _verticalVelocity -= gravity * Time.deltaTime;
            _controller.Move(new Vector3(0, _verticalVelocity, 0) * Time.deltaTime);
        }

        private void MoveTowardsTarget()
        {
            Vector3 direction = (_currentTarget - transform.position);
            direction.y = 0; // Flat movement
            
            if (direction.sqrMagnitude > 0.01f)
            {
               direction.Normalize();
            }
            else
            {
               ApplyGravityOnly();
               return;
            }

            // Obstacle Avoidance (Raycast Whiskers)
            Vector3 finalDirection = CalculateAvoidance(direction);

            // Rotation
            if (finalDirection != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(finalDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
            }

            // Movement
            Vector3 velocity = finalDirection * moveSpeed;
            
            if (_controller.isGrounded)
            {
                _verticalVelocity = -gravity * Time.deltaTime; // Stick gently to floor
            }
            else
            {
                _verticalVelocity -= gravity * Time.deltaTime; // Accelerate fall
            }

            velocity.y = _verticalVelocity;
            _controller.Move(velocity * Time.deltaTime);
        }

        private Vector3 CalculateAvoidance(Vector3 originalDirection)
        {
            Vector3 origin = transform.position + (Vector3.up * 0.7f); // Raise raycast starting point to chest
            Vector3 forward = transform.forward;
            Vector3 leftAngle = Quaternion.Euler(0, -35, 0) * forward;
            Vector3 rightAngle = Quaternion.Euler(0, 35, 0) * forward;

            bool hitF = Physics.Raycast(origin, forward, out RaycastHit hitFront, avoidanceDistance, obstacleLayer);
            bool hitL = Physics.Raycast(origin, leftAngle, out RaycastHit hitLeft, avoidanceDistance * 0.7f, obstacleLayer);
            bool hitR = Physics.Raycast(origin, rightAngle, out RaycastHit hitRight, avoidanceDistance * 0.7f, obstacleLayer);

            Vector3 finalDirection = originalDirection;

            if (hitF || hitL || hitR)
            {
                // Force desvio (Steering away from normal/hit surface)
                if (hitF)
                {
                    // Obstáculo bem na frente, vira para a direita ou esquerda baseado na normal
                    Vector3 divert = Vector3.Cross(Vector3.up, hitFront.normal); 
                    // Garante que o cross product aponte levemente na direção que o agente já quer ir
                    if (Vector3.Dot(divert, originalDirection) < 0) divert = -divert; 
                    
                    finalDirection = Vector3.Lerp(originalDirection, divert, 0.8f).normalized;
                }
                else if (hitL && !hitR)
                {
                    finalDirection = Vector3.Lerp(originalDirection, transform.right, 0.6f).normalized;
                }
                else if (hitR && !hitL)
                {
                    finalDirection = Vector3.Lerp(originalDirection, -transform.right, 0.6f).normalized;
                }
                
                finalDirection.y = 0;
            }

            return finalDirection.normalized;
        }

        private bool IsPlayerInAggroRange()
            => Vector3.Distance(transform.position, _player.position) <= aggroRange;

        public void SetTarget(bool chase)
        {
            if (chase) EnterChase();
            else EnterReturn();
        }

        public void TakeDamage(int damage)
        {
            if (!baseStats.IsAlive()) return;

            if (animator != null) animator.SetTrigger("Hit");
            baseStats.TakeDamage(damage);

            if (!baseStats.IsAlive())
            {
                Die();
                return;
            }

            if (currentState == AIState.Patrol || currentState == AIState.Return)
            {
                SetTarget(true);
            }
        }

        private void Die()
        {
            if (animator != null) animator.SetTrigger("Die");

            var player = Singleton.Instance._charController;
            if (player != null)
            {
                var manager = new CharacterProgressionManager(player.progression);
                manager.AddExperience((long)(xpReward * level * 1.5f));

                var qManager = Singleton.Instance.questManager;
                if (qManager != null)
                {
                    if (qManager.quest != null)
                        qManager.quest.OnSendLogic(MKU.Scripts.Enums.TaskCondition.Hunting, enemyIdForQuest, player.Id);

                    foreach (var q in qManager._questLast)
                    {
                        if (q != null) q.OnSendLogic(MKU.Scripts.Enums.TaskCondition.Hunting, enemyIdForQuest, player.Id);
                    }
                }
            }
            
            this.enabled = false;
            Destroy(gameObject, 0.4f);
        }

        public Transform GetTransform() => transform;
        public Base GetBaseStats() => baseStats;

        public AIController GetInstance() => this;
        public void Attack()
        {
            Debug.Log("Attack");
            animator.SetBool("Attack", false);
            if (_player != null)
            {
                var damageable = _player.GetComponent<IDamageable>();
                if (damageable != null && Vector3.Distance(transform.position, _player.position) <= attackRange + 0.5f) 
                {
                    if (_combatProcessor != null)
                    {
                        _combatProcessor.ApplyDamageDirectly(damageable);
                    }
                }
            }
        }

        public void Initialize(Vector3 origin, float radius, int level, _Attributs attrs)
        {
            _homePosition = origin;
            patrolRadius  = radius;
            this.level    = level;
            this.attributes = attrs ?? new _Attributs(10, 8, 10, 5, 6, 3);
            status = new _Stats();
            baseStats.StartCalc(this.attributes, status, level);
            _currentTarget = origin;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 0.2f, 0.2f, 0.3f);
            Gizmos.DrawSphere(transform.position, aggroRange);

            if (Application.isPlaying)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(_homePosition, patrolRadius);

                // Draw Whiskers em playmode
                Vector3 origin = transform.position + (Vector3.up * 0.7f);
                Vector3 fwd = transform.forward * avoidanceDistance;
                Vector3 left = Quaternion.Euler(0, -35, 0) * transform.forward * (avoidanceDistance * 0.7f);
                Vector3 right = Quaternion.Euler(0, 35, 0) * transform.forward * (avoidanceDistance * 0.7f);

                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(origin, fwd);
                Gizmos.DrawRay(origin, left);
                Gizmos.DrawRay(origin, right);
            }
        }
#endif
    }
}