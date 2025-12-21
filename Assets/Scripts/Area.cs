using UnityEngine;
using UnityEngine.AI;

public class Area : MonoBehaviour
{
    public static Area Instance { get; private set; }


    public float outerRadius = 20f;
    public float innerRadius = 5f;

    public GameObject player;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        gameObject.transform.position = player.transform.position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, outerRadius);
        Gizmos.color = Color.orange;
        Gizmos.DrawWireSphere(transform.position, innerRadius);
    }
    
    public void UpdatePatrolRadius(float r)
    {
        outerRadius = r;
    }

    public Vector3 GetRandomPoint()
    {
        for (int i = 0; i < 10; i++)
        {
            float r = Mathf.Sqrt(
                Random.Range(innerRadius * innerRadius, outerRadius * outerRadius)
            );

            float angle = Random.Range(0f, Mathf.PI * 2f);

            Vector3 offset = new Vector3(
                Mathf.Cos(angle) * r,
                0f,
                Mathf.Sin(angle) * r
            );

            Vector3 candidate = transform.position + offset;

            if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        // No valid point found after multiple attempts
        return Vector3.positiveInfinity;
    }

}
