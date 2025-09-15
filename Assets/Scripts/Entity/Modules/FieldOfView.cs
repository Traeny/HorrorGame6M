using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

namespace Entity_Script
{
    public class FieldOfView : MonoBehaviour
    {
        public float radius;

        [Range(0, 360)]
        public float angle;

        public GameObject player;

        public LayerMask targetMask;
        public LayerMask obstructionMask;

        public Transform eyeOrigin;

        [SerializeField]
        float delay = 0.1f;

        public bool isPlayerVisible = false;

        private void Start()
        {
            player = GameObject.FindWithTag("Player");

            if (player == null)
            {
                Debug.LogError("Player Missing / Not Found!");
                return;
            }

            StartCoroutine(FOVRoutine());
        }

        private IEnumerator FOVRoutine()
        {
            WaitForSeconds wait = new WaitForSeconds(delay);

            while (true)
            {
                yield return wait;
                FOVCheck();
            }
        }

        private void FOVCheck()
        {
            Collider[] rangeChecks = Physics.OverlapSphere(eyeOrigin.position, radius, targetMask);

            if(rangeChecks.Length != 0)
            {
                Transform target = rangeChecks[0].transform;

                Vector3 directionToTarget = (target.position - eyeOrigin.position).normalized;

                if(Vector3.Angle(eyeOrigin.forward, directionToTarget) < angle / 2)
                {
                    float distanceToTarget = Vector3.Distance(eyeOrigin.position, target.position);

                    if(!Physics.Raycast(eyeOrigin.position, directionToTarget, distanceToTarget, obstructionMask))
                    {
                        UpdateLastSeenPlayerPosition();
                        Blackboard.Instance.isPlayerVisible = true;
                    }
                    else
                    {
                        Blackboard.Instance.isPlayerVisible = false;
                    }
                }
                else
                {
                    Blackboard.Instance.isPlayerVisible = false;
                }
            }
            else if (Blackboard.Instance.isPlayerVisible)
            {
                Blackboard.Instance.isPlayerVisible = false;
            }
        }

        private void UpdateLastSeenPlayerPosition()
        {
            if (NavMesh.SamplePosition(player.transform.position, out NavMeshHit hit, 4f, NavMesh.AllAreas))
            {
                Blackboard.Instance.lastSeenPosition= hit.position;
            }
            else
            {
                Blackboard.Instance.lastHeardPosition = player.transform.position;
            }
        }
    }


}
