using UnityEngine;
using UnityEngine.AI;

public class Area : MonoBehaviour
{
    public static Area Instance { get; private set; }

    public float radius = 20f;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public Vector3 GetRandompoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection.y = 0f;

        Vector3 randomPoint = transform.position + randomDirection;

        NavMeshHit hit;
        Vector3 finalPosition = transform.position;

        if(NavMesh.SamplePosition(randomPoint, out hit, 2f, 1))
        {
            finalPosition = hit.position;
        }

        return finalPosition;
    }
}
