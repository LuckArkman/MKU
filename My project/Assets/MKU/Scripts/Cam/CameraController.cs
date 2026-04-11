namespace MKU.Scripts.Cam
{
    using UnityEngine;
    using UnityEngine.InputSystem; // <-- Necessário para o Novo Input System

    public class CameraController : MonoBehaviour
    {
        [Header("Alvos e Posição")] public Transform target;
        public float distance = 10f;
        public float pitchAngle = 30f;

        [Header("Movimentação e Suavidade")] public float smoothFollowSpeed = 5f;

        [Tooltip("Velocidade do giro da câmera. Valores menores são recomendados no Novo Input System.")]
        public float rotationSpeed = 0.2f;

        private float currentYaw = 0f;
        private Vector3 currentTargetPosition;

        void Start()
        {
            if (target != null)
            {
                currentYaw = target.eulerAngles.y;
                currentTargetPosition = target.position;
            }
        }

        void LateUpdate()
        {
            if (target == null) return;

            // Se houver um mouse conectado
            if (Mouse.current != null)
            {
                // 1. Gira a câmera se o botão DIREITO do mouse estiver sendo segurado
                if (Mouse.current.rightButton.isPressed)
                {
                    // Lê o movimento do mouse no eixo X (Delta X)
                    float mouseX = Mouse.current.delta.x.ReadValue();
                    currentYaw += mouseX * rotationSpeed;
                }
            }

            // 2. Segue o jogador suavemente
            currentTargetPosition =
                Vector3.Lerp(currentTargetPosition, target.position, smoothFollowSpeed * Time.deltaTime);

            // 3. Aplica os 30 graus (Pitch) e a rotação
            Quaternion rotation = Quaternion.Euler(pitchAngle, currentYaw, 0);

            // 4. Calcula a posição final
            Vector3 desiredPosition = currentTargetPosition + rotation * new Vector3(0, 0, -distance);

            // 5. Posiciona e olha para o jogador
            transform.position = desiredPosition;
            transform.LookAt(currentTargetPosition);
        }
    }
}