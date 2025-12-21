using System.Collections;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    private ProxyimityDetector proximityDetector;

    private void Start()
    {
        proximityDetector = GetComponentInChildren<ProxyimityDetector>();

        StartCoroutine(CheckProximity());
    }

    private IEnumerator CheckProximity()
    {
        while (true)
        {
            if(proximityDetector.targetTime >= 20f)
            {
                proximityDetector.targetTime = 0;
                Area.Instance.outerRadius += 10f;
            }

            if (proximityDetector.targetTime <= -20f)
            {
                proximityDetector.targetTime = 0;
                Area.Instance.outerRadius -= 10f;
            }

            yield return 0.1f;
        }
    }
}
