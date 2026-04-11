using MKU.Scripts.Singletons;

namespace MKU.Scripts.Character
{
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class CharacterController : MonoBehaviour
    {
        public Animator animator;
        [Header("Movement & Curve Settings")] public float speed = 5f;

        [Tooltip(
            "How fast the character turns while walking. Lower values create a wider parabolic curve. Higher values create a tighter curve.")]
        public float turnCurveSpeed = 120f;

        public LayerMask groundLayer;

        [Header("Realism (Sine Bobbing)")] public float bobbingFrequency = 15f;
        public float bobbingAmplitude = 0.1f;

        private Vector3 targetPosition;
        private Camera mainCamera;
        public bool isMoving = false;
        public bool isRunning = false;
        private float stepTimer = 0f;
        private float initialY;

        // Tracks the current mathematical angle for the curve
        private float currentMovementAngleRad;

        void Start()
        {
            Singleton.Instance.controller = this;
            mainCamera = Camera.main;
            targetPosition = transform.position;
            initialY = transform.position.y;

            // Initialize the starting angle based on where the character is currently facing
            currentMovementAngleRad = Mathf.Atan2(transform.forward.z, transform.forward.x);
        }

        void Update()
        {
            HandleInput();
            MoveCharacterWithCurve();
        }

        private void HandleInput()
        {
            if (Mouse.current == null) return;

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
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

        private void MoveCharacterWithCurve()
        {
            if (!isMoving) return;

            // Calculate distance ignoring the Y axis
            float dx = targetPosition.x - transform.position.x;
            float dz = targetPosition.z - transform.position.z;
            float distanceToTarget = Mathf.Sqrt(dx * dx + dz * dz);

            // Stop moving if we are close enough to the destination
            if (distanceToTarget > 0.1f)
            {
                // 1. Calculate the absolute angle to the destination
                float targetAngleRad = Mathf.Atan2(dz, dx);

                // 2. Convert radians to degrees to use Unity's smooth angle transition
                float currentDeg = currentMovementAngleRad * Mathf.Rad2Deg;
                float targetDeg = targetAngleRad * Mathf.Rad2Deg;

                // 3. Smoothly transition the CURRENT angle towards the TARGET angle (This creates the Parabola/Curve)
                currentDeg = Mathf.MoveTowardsAngle(currentDeg, targetDeg, turnCurveSpeed * Time.deltaTime);

                // Convert back to radians for the math functions
                currentMovementAngleRad = currentDeg * Mathf.Deg2Rad;

                // 4. --- SINE AND COSINE MOVEMENT ---
                // Because currentMovementAngleRad is constantly shifting while the character moves, 
                // Cos and Sin will naturally draw a curved trajectory.
                float moveX = Mathf.Cos(currentMovementAngleRad) * speed * Time.deltaTime;
                float moveZ = Mathf.Sin(currentMovementAngleRad) * speed * Time.deltaTime;

                // Anti-overshoot: If the movement step is larger than the distance, just snap to the end to prevent orbiting
                if (new Vector2(moveX, moveZ).magnitude > distanceToTarget)
                {
                    transform.position = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
                    isMoving = false;
                    return;
                }

                // Realism bobbing (Sine)
                stepTimer += Time.deltaTime * bobbingFrequency;
                float bobbingOffset = Mathf.Sin(stepTimer) * bobbingAmplitude;

                // Apply curved movement
                transform.position += new Vector3(moveX, 0, moveZ);
                transform.position = new Vector3(transform.position.x, initialY + Mathf.Abs(bobbingOffset),
                    transform.position.z);

                // 5. Instantly align the character's visual rotation to match the mathematical curve
                Vector3 moveDirection = new Vector3(moveX, 0, moveZ).normalized;
                if (moveDirection != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(moveDirection);
                }
            }
            else
            {
                // Reached destination
                isMoving = false;
                stepTimer = 0f;
                transform.position = new Vector3(transform.position.x, initialY, transform.position.z);
            }
        }
    }
}