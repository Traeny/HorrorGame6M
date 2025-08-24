using UnityEngine;

namespace Player_Script
{
    [CreateAssetMenu(menuName = "PlayerFP/FPControllerPreset")]
    public class FPControllerPreset : ScriptableObject
    {
        [Header("Movement Parameters")]
        public float acceleration = 15f;

        public float walkSpeed = 3.5f;
        public float sprintSpeed = 10f;

        [Header("Jump Parameters")]
        [Tooltip("This is how high the character can jump")]
        public float jumpHeight = 2f;
        public float coyoteTime = 0.1f;

        [Header("Looking Parameters")]
        public Vector2 lookSensitivity = new Vector2(0.1f, 0.1f);
        public float pitchLimit = 85f;

        [Header("Camera Parameters")]
        public float cameraNormalFOV = 60f;
        public float cameraSprintFOV = 67.5f;
        public float cameraFOVSmoothing = 5f;

        [Header("Physics Parameters")]
        public float gravityScale = 2f;
        public LayerMask obstacleLayerMask = Physics.DefaultRaycastLayers;

        [Header("Sounds & Footstep Parameters")]
        public float footstepWalkRate = 0.8f;
        public float footstepSprintRate = 0.3f;

        [Space(15)]
        public float footstepWalkVolume = 0.1f;
        public float footstepSprintVolume = 0.5f;

        [Space(15)]
        public float maxLandSoundVolume = 0.7f;
        public float jumpSoundVolume = 0.5f;
    }
}