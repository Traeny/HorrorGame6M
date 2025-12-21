using Player_Script;
using UnityEngine;

public class ProxyimityDetector : MonoBehaviour
{
    public GameObject player;
    public GameObject entity;
    public float targetTime = 0;

    private void Update()
    {
        float maxRange = 20;

        if (Vector3.Distance(entity.transform.position, player.transform.position) < maxRange)
        {
            targetTime += Time.deltaTime;
        }
        else
        {
            targetTime -= Time.deltaTime;
        }
    }
}