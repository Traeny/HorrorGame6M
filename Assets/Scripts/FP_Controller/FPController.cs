using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace Player_Script
{
    [RequireComponent(typeof(CharacterController))]
    public class FPController : MonoBehaviour
    {
        public FPControllerPreset preset;

        float maxSpeed
        {
            get
            {
                if (Activity.IsActive(Sprint))
                {
                    return preset.sprintSpeed;
                }
                return preset.walkSpeed;
            }
        }

        public bool sprinting
        {
            get
            {
                return Activity.IsActive(Sprint);
            }
        }

        [SerializeField] float currentPitch = 0f;

        public float CurrentPitch
        {
            get => currentPitch;

            set
            {
                currentPitch = Mathf.Clamp(value, -preset.pitchLimit, preset.pitchLimit);
            }
        }

        [Header("Physics Parameters")]
        public float verticalVelocity = 0f;
        public Vector3 currentVelocity { get; private set; }
        public float currentSpeed { get; private set; }
        private bool wasGrounded = false;
        public bool Grounded => characterController.isGrounded;


        [Header("Inputs")]
        public Vector2 moveInput;
        public Vector2 lookInput;
        public bool sprintInput;

        [Header("Components")]
        [SerializeField] CharacterController characterController;
        [SerializeField] CinemachineCamera fpCamera;

        public CharacterController CharacterController => characterController;
        public CinemachineCamera Camera => fpCamera;
        public Transform CameraTransform => fpCamera.transform;

        [Header("Activities")]
        public FPSprint Sprint;

        [Header("Events")]
        public UnityEvent Landed;
        public UnityEvent Jumped;

        public UnityAction TryJump;

        #region Unity Methods

        private void OnValidate()
        {
            if (characterController == null)
            {
                characterController = GetComponent<CharacterController>();
            }
        }

        private void Update()
        {
            MoveUpdate();
            LookUpdate();
            CameraUpdate();

            if(!wasGrounded && Grounded)
            {
                Landed?.Invoke();
            }

            wasGrounded = Grounded;
        }

        #endregion

        #region Controller Methods



        void MoveUpdate()
        {
            Vector3 motion = transform.forward * moveInput.y + transform.right * moveInput.x;
            motion.y = 0f;
            motion.Normalize();

            if (motion.sqrMagnitude >= 0.01f)
            {
                currentVelocity = Vector3.MoveTowards(currentVelocity, motion * maxSpeed, preset.acceleration * Time.deltaTime);
            }
            else
            {
                currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, preset.acceleration * Time.deltaTime);
            }

            // Jumping logic
            if (Grounded && verticalVelocity <= 0.01f)
            {
                verticalVelocity = -3f;
            }
            else
            {
                verticalVelocity += Physics.gravity.y * preset.gravityScale * Time.deltaTime;
            }

            Vector3 fullVelocity = new Vector3(currentVelocity.x, verticalVelocity, currentVelocity.z);



            CollisionFlags flags = characterController.Move(fullVelocity * Time.deltaTime);

            if((flags & CollisionFlags.Above) != 0 && verticalVelocity > 0.01f)
            {
                verticalVelocity = 0f;
            }


            // Updating speed
            currentSpeed = currentVelocity.magnitude;
        }

        void LookUpdate()
        {
            Vector2 input = new Vector2(lookInput.x * preset.lookSensitivity.x, lookInput.y * preset.lookSensitivity.y);

            // looking up and down
            CurrentPitch -= input.y;
            fpCamera.transform.localRotation = Quaternion.Euler(CurrentPitch, 0f, 0f);

            // looking left and right
            transform.Rotate(Vector3.up * input.x);
        }

        void CameraUpdate()
        {
            float targetFOV = preset.cameraNormalFOV;

            if (sprinting)
            {
                float speedRatio = currentSpeed / preset.sprintSpeed;
                targetFOV = Mathf.Lerp(preset.cameraNormalFOV, preset.cameraSprintFOV, speedRatio);
            }

            fpCamera.Lens.FieldOfView = Mathf.Lerp(fpCamera.Lens.FieldOfView, targetFOV, preset.cameraFOVSmoothing * Time.deltaTime);
        }

        #endregion
    }
}

