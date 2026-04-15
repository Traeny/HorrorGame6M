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

                        Blackboard.Instance.sawSomething = true;
                        visionTime = Time.time;
                    }
                }
            }
        }

        private void UpdateInterestPoint()
        {
            if (NavMesh.SamplePosition(player.transform.position, out NavMeshHit hit, 4f, NavMesh.AllAreas))
            {
                Blackboard.Instance.UpdateInterestPoint(hit.position);
            }
            else
            {
                Blackboard.Instance.UpdateInterestPoint(player.transform.position);
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
