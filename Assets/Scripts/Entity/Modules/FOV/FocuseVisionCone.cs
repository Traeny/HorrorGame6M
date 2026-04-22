using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

namespace Entity_Script
{
    public class FocuseVisionCone : MonoBehaviour
    {
        public EnemyPreset preset;

        [Header("Debugging")]
        public GameObject canSeePlayer;
        public GameObject cantSeePlayer;

        [Header("Components")]
        public GameObject player;
        public Transform eyeOrigin;

        private float delayTimer = 0;
        public float sightTimer = 0;

        

        private void Start()
        {
            player = GameObject.FindWithTag("Player");

            if (player == null)
            {
                Debug.LogError("Player Missing!");
                return;
            }

            delayTimer = preset.delay;
            sightTimer = preset.focuseConeGraceDelay;
        }

        private void Update()
        {
            delayTimer -= Time.deltaTime;

            if (delayTimer < 0)
            {
                FOVCheck();
                delayTimer = preset.delay;
            }
        }

        private void FOVCheck()
        {
            Collider[] rangeChecks = Physics.OverlapSphere(eyeOrigin.position, preset.focuseConeFovRadius, preset.targetMask);

            if(rangeChecks.Length != 0)
            {
                Transform target = rangeChecks[0].transform;

                Vector3 directionToTarget = (target.position - eyeOrigin.position).normalized;

                if(Vector3.Angle(eyeOrigin.forward, directionToTarget) < preset.focuseConeAngle / 2)
                {
                    float distanceToTarget = Vector3.Distance(eyeOrigin.position, target.position);

                    if(!Physics.Raycast(eyeOrigin.position, directionToTarget, distanceToTarget, preset.obstructionMask))
                    {
                        sightTimer -= Time.deltaTime;

                        if(sightTimer <= 0)
                        {
                            UpdateInterestPoint();
                            canSeePlayer.SetActive(true);
                            cantSeePlayer.SetActive(false);
                            Blackboard.Instance.isPlayerVisible = true;
                        }
                    }
                    else
                    {
                        canSeePlayer.SetActive(false);
                        cantSeePlayer.SetActive(true);
                        Blackboard.Instance.isPlayerVisible = false;
                    }
                }
                else
                {
                    sightTimer = preset.focuseConeGraceDelay;

                    canSeePlayer.SetActive(false);
                    cantSeePlayer.SetActive(true);
                    Blackboard.Instance.isPlayerVisible = false;
                }
            }
            else if (Blackboard.Instance.isPlayerVisible)
            {
                canSeePlayer.SetActive(false);
                cantSeePlayer.SetActive(true);
                Blackboard.Instance.isPlayerVisible = false;
            }
        }

        private void UpdateInterestPoint()
        {
            if (NavMesh.SamplePosition(player.transform.position, out NavMeshHit hit, 4f, NavMesh.AllAreas))
            {
                Blackboard.Instance.UpdateInterestPoint(hit.position);
                Blackboard.Instance.lastSeenPosition = hit.position;
                Blackboard.Instance.UpdateHotspotOrigin(hit.position);
            }
            else
            {
                Blackboard.Instance.UpdateInterestPoint(player.transform.position);
                Blackboard.Instance.lastHeardPosition = player.transform.position;
                Blackboard.Instance.UpdateHotspotOrigin(player.transform.position);
            }
        }
    }
}
