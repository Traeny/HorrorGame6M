using UnityEngine;

namespace Entity_Script
{
    [CreateAssetMenu(menuName = "Entity/EntityControllerPreset")]
    public class EntityControllerPreset : MonoBehaviour
    {
        [Header("Movement Parameters")]
        public float acceleration = 10f;

        [Space(10)]
        public float walkSpeed = 3.5f;
        public float sprintSpeed = 10f;
    }
}

