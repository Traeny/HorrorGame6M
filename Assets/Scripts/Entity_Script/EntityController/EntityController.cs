using UnityEngine;
using UnityEngine.AI;

namespace Entity_Script
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EntityController : MonoBehaviour
    {
        [SerializeField] Vector3 currentTarget;

        public Transform[] neighbors;


        [Header("Components")]
        [SerializeField] NavMeshAgent agent;

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
            UpdateMovement();

            // when the entity is close to it's current target we command it to find a new one
            if(agent.remainingDistance < 0.5f)
            {
                CalculateRandomNeighbor();
            }
        }
        #endregion

        #region Controller Methods

        private void UpdateMovement()
        {
            // Moving to the current target
            agent.destination = currentTarget;
        }

        public void UpdateTarget(Transform newTarget)
        {
            // Updtating the target we want to move to
            currentTarget = newTarget.transform.position;
        }

        void CalculateRandomNeighbor()
        {
            // select a random target from the list
            Transform next = neighbors[Random.Range(0, neighbors.Length)];
            UpdateTarget(next.transform);
        }
        #endregion

    }
}

