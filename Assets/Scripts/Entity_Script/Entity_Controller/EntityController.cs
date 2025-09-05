using UnityEngine;
using UnityEngine.AI;

namespace Entity_Script
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EntityController : MonoBehaviour
    {
        // Gives access to the preset variavles
        public EntityControllerPreset preset;

        // Current target the entity is moving towards
        [SerializeField] Transform currentTarget;

        // NawmMsh agent needed to move
        [SerializeField] NavMeshAgent agent;

        // Speed which we are moving in?
        [SerializeField] Vector3 currentVelocity;


        // Max speed it ised to determine what speed the entity can move in
        float maxSpeed
        {
            get
            {
                
                if (EntityActivity.IsActive(Sprint))
                {
                    return preset.sprintSpeed;
                }

                return preset.walkSpeed;

            }
        }

        // List of activities the entity can take
        [Header("Activities")]
        public EntitySprint Sprint;
        public EntityPatrol Patrol;

        // pre determined patrol positions around the map
        public Transform[] neighbors;

        #region Unity Methods

        private void OnValidate()
        {
            if (agent == null)
            {
                agent = GetComponent<NavMeshAgent>();
            }
        }

        private void Update()
        {
            // Should stay here
            UpdateMovement();

            // This could be here or moved to patrol
            if(agent.remainingDistance < 0.5f)
            {
                CalculateRandomNeighbor();
            }
        }
        #endregion

        #region Controller Methods

        private void UpdateMovement()
        {
            if (currentTarget == null) return;

            // Desired direction
            Vector3 toTarget = (currentTarget.position - transform.position);
            toTarget.y = 0f;
            Vector3 desiredDirection = toTarget.normalized;

            // Accelerate towards target direction
            if (desiredDirection.sqrMagnitude >= 0.01f)
            {
                currentVelocity = Vector3.MoveTowards(
                    currentVelocity,
                    desiredDirection * maxSpeed,
                    preset.acceleration * Time.deltaTime
                );
            }
            else
            {
                currentVelocity = Vector3.MoveTowards(
                    currentVelocity,
                    Vector3.zero,
                    preset.acceleration * Time.deltaTime
                );
            }

            // Move the agent manually
            agent.Move(currentVelocity * Time.deltaTime);
        }

        public void UpdateTarget(Transform newTarget)
        {
            // Updtating the target we want to move to
            currentTarget = newTarget;
        }

        // Needs to be moved to the module
        void CalculateRandomNeighbor()
        {
            // select a random target from the list
            Transform next = neighbors[Random.Range(0, neighbors.Length)];
            UpdateTarget(next.transform);
        }
        #endregion

    }
}

