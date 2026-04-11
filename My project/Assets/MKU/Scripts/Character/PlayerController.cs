namespace MKU.Scripts.Character
{
    using UnityEngine;
    using UnityEngine.InputSystem; // <-- Necessário para o Novo Input System

    public class PlayerController : MonoBehaviour
    {
        public Animator animator;
        [Header("Configurações de Movimento")] public float speed = 5f;
        public float rotationSpeed = 10f;
        public LayerMask groundLayer;

        [Header("Realismo (Senoide de Passos)")]
        public float bobbingFrequency = 15f;

        public float bobbingAmplitude = 0.1f;

        private Vector3 targetPosition;
        private Camera mainCamera;
        private bool isMoving = false;
        private float stepTimer = 0f;
        private float initialY;

        void Start()
        {
            mainCamera = Camera.main;
            targetPosition = transform.position;
            initialY = transform.position.y;
        }

        void Update()
        {
            HandleInput();
            animator.SetBool("walk", isMoving);
            MoveCharacter();
        }

        private void HandleInput()
        {
            // Prevenção de erro caso o mouse não seja detectado
            if (Mouse.current == null) return;

            // Verifica se o botão ESQUERDO do mouse foi clicado neste frame
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                // Lê a posição do cursor na tela no Novo Input System
                Vector2 mousePosition = Mouse.current.position.ReadValue();
                Ray ray = mainCamera.ScreenPointToRay(mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
                {
                    targetPosition = hit.point;
                    targetPosition.y = initialY;
                    isMoving = true;
                }
            }
        }

        private void MoveCharacter()
        {
            if (!isMoving) return;

            float distanceToTarget = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
                new Vector3(targetPosition.x, 0, targetPosition.z));

            if (distanceToTarget > 0.1f)
            {
                // Lógica Baseada em Seno e Cosseno
                float dx = targetPosition.x - transform.position.x;
                float dz = targetPosition.z - transform.position.z;
                float angle = Mathf.Atan2(dz, dx);

                float moveX = Mathf.Cos(angle) * speed * Time.deltaTime;
                float moveZ = Mathf.Sin(angle) * speed * Time.deltaTime;

                stepTimer += Time.deltaTime * bobbingFrequency;
                float bobbingOffset = Mathf.Sin(stepTimer) * bobbingAmplitude;

                transform.position += new Vector3(moveX, 0, moveZ);
                transform.position = new Vector3(transform.position.x, initialY + (Mathf.Abs(bobbingOffset)),
                    transform.position.z);

                Vector3 direction = new Vector3(moveX, 0, moveZ).normalized;
                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation =
                        Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
            }
            else
            {
                isMoving = false;
                stepTimer = 0f;
                transform.position = new Vector3(transform.position.x, initialY, transform.position.z);
            }
        }
    }
}