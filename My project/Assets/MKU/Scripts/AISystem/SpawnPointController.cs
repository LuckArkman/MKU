using System.Collections;
using System.Collections.Generic;
using MKU.Scripts.HelthSystem;
using UnityEngine;

namespace MKU.Scripts.AISystem
{
    /// <summary>
    /// Gerencia o spawn e o ciclo de vida dos agentes de IA dentro de uma área circular.
    /// Garante que o número de agentes ativos nunca ultrapasse o limite configurado.
    /// Agora desvinculado do NavMesh.
    /// </summary>
    public class SpawnPointController : MonoBehaviour
    {
        // ─── Configurações de Spawn ───────────────────────────────────────────

        [Header("Prefab do Agente")]
        [Tooltip("Prefab do monstro/inimigo. Deve conter um AIController e um CharacterController.")]
        public GameObject monsterPrefab;

        [Header("Limites da Área")]
        [Tooltip("Raio da circunferência de patrulha e spawn dos agentes.")]
        public float patrolRadius = 5f;

        [Header("Controle de Quantidade")]
        [Tooltip("Quantidade máxima de agentes que podem estar ativos simultaneamente nesta área.")]
        [Min(1)] public int maxAgents = 5;

        [Tooltip("Quantidade de agentes a tentar gerar em cada intervalo de spawn.")]
        [Min(1)] public int agentsPerWave = 1;

        [Tooltip("Intervalo de tempo (segundos) entre cada tentativa de spawn.")]
        [Min(0.5f)] public float spawnInterval = 5f;

        // ─── Configurações dos Agentes ────────────────────────────────────────

        [Header("Atributos dos Agentes (Base RPG)")]
        [Tooltip("Nível base dos monstros gerados nesta área.")]
        [Min(1)] public int monsterLevel = 1;

        [Tooltip("Atributos base dos monstros gerados nesta área.")]
        public _Attributs monsterAttributes = new _Attributs(10, 8, 10, 5, 6, 3);

        [Header("Detecção dos Agentes")]
        [Tooltip("Raio de detecção de aggro de cada agente gerado.")]
        public float aggroRange = 5f;

        [Tooltip("Segundos fora do range antes de resetar o aggro do agente.")]
        public float aggroResetTime = 10f;

        // ─── Controle Interno ─────────────────────────────────────────────────

        private readonly List<AIController> _activeAgents = new List<AIController>();
        private Coroutine _spawnCoroutine;

        // ─────────────────────────────────────────────────────────────────────

        private void OnEnable()
        {
            _spawnCoroutine = StartCoroutine(SpawnRoutine());
        }

        private void OnDisable()
        {
            if (_spawnCoroutine != null)
                StopCoroutine(_spawnCoroutine);
        }

        private void Update()
        {
            // Remove referências de agentes destruídos da lista
            _activeAgents.RemoveAll(a => a == null);
        }

        // ─── Rotina de Spawn ──────────────────────────────────────────────────

        private IEnumerator SpawnRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnInterval);

                int toSpawn = Mathf.Min(agentsPerWave, maxAgents - ActiveAgentCount());

                for (int i = 0; i < toSpawn; i++)
                {
                    TrySpawnAgent();
                }
            }
        }

        /// <summary>
        /// Tenta gerar um agente em uma posição aleatória dentro do raio de spawn usando Raycast.
        /// </summary>
        private void TrySpawnAgent()
        {
            if (monsterPrefab == null)
            {
                Debug.LogWarning($"[SpawnPointController] {name}: monsterPrefab não está configurado!", this);
                return;
            }

            if (ActiveAgentCount() >= maxAgents) return;

            Vector3 spawnPos = GetRandomSpawnPoint();
            if (spawnPos == Vector3.zero) return; // Fallback se não detectar chão corretamente

            GameObject agentObject = Instantiate(monsterPrefab, spawnPos, Quaternion.identity);
            agentObject.name = $"{monsterPrefab.name}_Agent_{System.Guid.NewGuid().ToString()[..4]}";

            AIController agent = agentObject.GetComponent<AIController>();
            if (agent == null)
            {
                Debug.LogError($"[SpawnPointController] O prefab '{monsterPrefab.name}' não possui um AIController associado.", this);
                Destroy(agentObject);
                return;
            }

            // Inicializa o agente com os parâmetros desta zona
            agent.Initialize(spawnPos, patrolRadius, monsterLevel, monsterAttributes);
            agent.aggroRange      = aggroRange;
            agent.aggroResetTime  = aggroResetTime;
            agent.homeSpawn       = this;

            _activeAgents.Add(agent);
        }

        // ─── Utilitários ──────────────────────────────────────────────────────

        private int ActiveAgentCount()
        {
            // Garante consistência ao contar (remove nulls inline)
            _activeAgents.RemoveAll(a => a == null);
            return _activeAgents.Count;
        }

        /// <summary>
        /// Retorna um ponto aleatório varrendo o chão via Raycast (elimina NavMesh).
        /// </summary>
        private Vector3 GetRandomSpawnPoint()
        {
            for (int i = 0; i < 15; i++)
            {
                Vector2 circle = Random.insideUnitCircle * patrolRadius;
                // Dispara um raio de 20 metros de altura em direção ao solo
                Vector3 candidate = transform.position + new Vector3(circle.x, 20f, circle.y);

                if (Physics.Raycast(candidate, Vector3.down, out RaycastHit hit, 40f))
                {
                    return hit.point;
                }
            }

            Debug.LogWarning($"[SpawnPointController] {name}: não foi possível localizar o chão abaixo do Spawner.", this);
            return transform.position; 
        }

        // ─── API Pública ──────────────────────────────────────────────────────

        public void ForceSpawn()
        {
            if (ActiveAgentCount() < maxAgents)
                TrySpawnAgent();
        }

        public int GetActiveAgentCount() => ActiveAgentCount();

        public void ClearAllAgents()
        {
            foreach (var agent in _activeAgents)
            {
                if (agent != null) Destroy(agent.gameObject);
            }
            _activeAgents.Clear();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // Círculo preenchido translúcido (área de patrulha/spawn)
            Gizmos.color = new Color(0.1f, 0.8f, 0.3f, 0.12f);
            DrawGizmoCircle(transform.position, patrolRadius, 48);

            // Círculo de borda (área de patrulha/spawn)
            Gizmos.color = new Color(0.1f, 0.9f, 0.3f, 0.9f);
            DrawGizmoWireCircle(transform.position, patrolRadius, 48);

            Gizmos.color = new Color(0.1f, 0.9f, 0.3f, 1f);
            Gizmos.DrawWireSphere(transform.position, 0.25f);

#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                UnityEditor.Handles.color = Color.white;
                UnityEditor.Handles.Label(
                    transform.position + Vector3.up * 1.5f,
                    $"Agentes: {ActiveAgentCount()} / {maxAgents}"
                );
            }
#endif
        }

        private void DrawGizmoWireCircle(Vector3 center, float radius, int segments)
        {
            float angleStep = 360f / segments;
            Vector3 prev = center + new Vector3(radius, 0, 0);
            for (int i = 1; i <= segments; i++)
            {
                float angle = i * angleStep * Mathf.Deg2Rad;
                Vector3 next = center + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
                Gizmos.DrawLine(prev, next);
                prev = next;
            }
        }

        private void DrawGizmoCircle(Vector3 center, float radius, int segments)
        {
            DrawGizmoWireCircle(center, radius * 0.9f, segments);
            DrawGizmoWireCircle(center, radius * 0.7f, segments);
            DrawGizmoWireCircle(center, radius * 0.5f, segments);
        }
#endif
    }
}
