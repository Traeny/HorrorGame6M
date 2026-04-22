using System.Collections;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    private ProxyimityDetector proximityDetector;
    public EnemyPreset preset;

    private void Start()
    {
        proximityDetector = GetComponentInChildren<ProxyimityDetector>();

        StartCoroutine(CheckProximity());
    }

    private IEnumerator CheckProximity()
    {
        while (true)
        {
            if(proximityDetector.targetTime >= preset.updatePatrolAreaTreshold)
            {
                proximityDetector.targetTime = 0;
                Area.Instance.preset.patrolArea += preset.increasePatrolAreaAmount;
            }

            if (proximityDetector.targetTime <= -preset.updatePatrolAreaTreshold)
            {
                proximityDetector.targetTime = 0;
                Area.Instance.preset.patrolArea -= preset.decreasePatrolAreaAmount;
            }

            yield return preset.delay;
        }
    }
}
