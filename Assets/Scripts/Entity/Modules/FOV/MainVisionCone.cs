using UnityEngine;
using UnityEngine.AI;

namespace Entity_Script
{
    public class MainVisionCone : MonoBehaviour
    {
        public EnemyPreset preset;

        [Header("Debugging")]
        public GameObject sawSomethignEyes;
        public GameObject cantSeePlayer;

        [Header("Components")]
        public GameObject player;
        public Transform eyeOrigin;

        // Testing
        public float visionTime { get; private set; } = 0f;
        public float timeSinceSawSomething => Time.time - visionTime;


        private float timer = 0;

        private void Start()
        {
            player = GameObject.FindWithTag("Player");

            if (player == null)
            {
                Debug.LogError("Player Missing!");
                return;
            }

            timer = preset.delay;
        }

        private void Update()
        {
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                FOVCheck();
                timer = preset.delay;
            }

            if (timeSinceSawSomething >= preset.forgetClimpseTime)
            {
                ForgetClimpse();
            }
        }

        private void FOVCheck()
        {
            Collider[] rangeChecks = Physics.OverlapSphere(eyeOrigin.position, preset.mainConeFovRadius, preset.targetMask);

            if (rangeChecks.Length != 0)
            {
                Transform target = rangeChecks[0].transform;

                Vector3 directionToTarget = (target.position - eyeOrigin.position).normalized;

                if (Vector3.Angle(eyeOrigin.forward, directionToTarget) < preset.mainConeAngle / 2)
                {
                    float distanceToTarget = Vector3.Distance(eyeOrigin.position, target.position);

                    if (!Physics.Raycast(eyeOrigin.position, directionToTarget, distanceToTarget, preset.obstructionMask))
                    {
                        UpdateInterestPoint();
                        sawSomethignEyes.SetActive(true);
                        cantSeePlayer.SetActive(false);

                        // WIP
                        Blackboard.Instance.sawSomething = true;
                        visionTime = Time.time;
                    }
                }
            }
        }

        // In this i could add logic that draws a cirsle around the seen target and randomly choosing a point on the navmesh on it
        private void UpdateInterestPoint()
        {
            if (NavMesh.SamplePosition(player.transform.position, out NavMeshHit hit, 4f, NavMesh.AllAreas))
            {
                Blackboard.Instance.UpdateInterestPoint(hit.position);
                //Blackboard.Instance.lastSeenPosition = hit.position;
                //Blackboard.Instance.UpdateHotspotOrigin(hit.position);
            }
            else
            {
                Blackboard.Instance.UpdateInterestPoint(player.transform.position);
                //Blackboard.Instance.lastHeardPosition = player.transform.position;
                //Blackboard.Instance.UpdateHotspotOrigin(player.transform.position);
            }
        }

        public void ForgetClimpse()
        {
            Blackboard.Instance.sawSomething = false;
            sawSomethignEyes.SetActive(false);
            cantSeePlayer.SetActive(true);
        }
    }
}
