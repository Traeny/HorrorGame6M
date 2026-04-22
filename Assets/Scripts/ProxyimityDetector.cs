using UnityEngine;

public class ProxyimityDetector : MonoBehaviour
{
    public GameObject player;
    public GameObject entity;
    public float targetTime = 0;

    public EnemyPreset preset;

    private void Update()
    {
        if (Vector3.Distance(entity.transform.position, player.transform.position) < preset.proxDetectionRange)
        {
            targetTime += Time.deltaTime;
        }
        else
        {
            targetTime -= Time.deltaTime;
        }
    }
}