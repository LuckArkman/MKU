namespace MKU.Scripts.Cam
{
    using UnityEngine;
    using UnityEngine.InputSystem; // <-- Necessário para o Novo Input System

    public class CameraController : MonoBehaviour
    {
        [Header("Alvos e Posição")] public Transform target;
        
        [Header("Zoom (Scroll do Mouse)")]
        public float distance = 2.5f;
        public float minDistance = 2f;
        public float maxDistance = 15f;
        public float zoomSpeed = 0.005f;

        [Tooltip("Deslocamento do ombro. X = Diagonais(direita), Y = Altura da Cabeça/Ombro, Z = Frente")]
        public Vector3 shoulderOffset = new Vector3(0.6f, 1.6f, 0f);

        [Header("Restrições de Rotação (Pitch)")]
        public float minPitch = -70f;
        public float maxPitch = 80f;

        [Header("Movimentação e Suavidade")] public float smoothFollowSpeed = 10f;

        [Tooltip("Velocidade do giro da câmera. Valores menores são recomendados no Novo Input System.")]
        public float rotationSpeed = 0.2f;

        private float currentYaw = 0f;
        private float currentPitch = 20f;
        private Vector3 currentTargetPosition;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            if (target != null)
            {
                currentYaw = target.eulerAngles.y;
                currentPitch = transform.eulerAngles.x;
                
                Quaternion yawRotation = Quaternion.Euler(0, currentYaw, 0);
                currentTargetPosition = target.position + yawRotation * shoulderOffset;
            }
        }

        void LateUpdate()
        {
            if (target == null) return;

            // Se houver um mouse conectado
            if (Mouse.current != null)
            {
                // 1. Gira a câmera se o botão DIREITO do mouse estiver sendo segurado
                // Lê o movimento do mouse nos eixos X e Y
                float mouseX = Mouse.current.delta.x.ReadValue();
                float mouseY = Mouse.current.delta.y.ReadValue();
                    
                currentYaw += mouseX * rotationSpeed;
                currentPitch -= mouseY * rotationSpeed; // invertido para sensação suave de olhar pra cima/baixo

                // Limita o movimento no eixo X (Pitch) para não virar de cabeça para baixo
                currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);

                // Ajusta o Zoom da Câmera (Distância) com a Rolagem do Scroll
                float scrollY = Mouse.current.scroll.y.ReadValue();
                if (scrollY != 0f)
                {
                    distance -= scrollY * zoomSpeed;
                    distance = Mathf.Clamp(distance, minDistance, maxDistance);
                }
            }

            // O offset rotaciona junto com o Yaw da câmera, para que seja sempre o ombro na tela visual
            Quaternion yawRotation = Quaternion.Euler(0, currentYaw, 0);
            Vector3 worldOffset = yawRotation * shoulderOffset;

            // Ponto alvo real onde a câmera foca (agora não no pé, mas no ombro)
            Vector3 targetFocusPoint = target.position + worldOffset;

            // 2. Segue a posição do ombro do jogador suavemente
            currentTargetPosition =
                Vector3.Lerp(currentTargetPosition, targetFocusPoint, smoothFollowSpeed * Time.deltaTime);

            // 3. Aplica o Pitch Dinâmico e a rotação Yaw
            Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);

            // 4. Calcula a posição final afastando da posição focal
            Vector3 desiredPosition = currentTargetPosition + rotation * new Vector3(0, 0, -distance);

            // 5. Posiciona e olha para a altura o ombro
            transform.position = desiredPosition;
            transform.LookAt(currentTargetPosition);
        }
    }
}