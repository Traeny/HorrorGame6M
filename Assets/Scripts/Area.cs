using UnityEngine;
using UnityEngine.AI;

public class Area : MonoBehaviour
{
    public static Area Instance { get; private set; }

    public EnemyPreset preset;
    public GameObject player;

    [Header("Unlocked Areas")]
    public bool bWingUnlocked = false;
    public bool middleAreaUnlocked = false;

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
        Gizmos.DrawWireSphere(transform.position, preset.patrolArea);
        Gizmos.color = Color.orange;
        Gizmos.DrawWireSphere(transform.position, preset.innerRadius);
    }
    
    public void UpdatePatrolRadius(float r)
    {
        preset.patrolArea = r;
    }

    public Vector3 GetRandomPoint()
    {
        // 10 is so we dont search infinitively i think
        for (int i = 0; i < 20; i++)
        {
            // Getting a random radius inside the allowed area (between inner and outer radius)
            float r = Mathf.Sqrt(
                Random.Range(
                    preset.innerRadius * preset.innerRadius, 
                    preset.patrolArea * preset.patrolArea
                )
            );

            // Gettig a random angle vertically 
            float angle = Random.Range(0f, Mathf.PI * 2f);

            // offset that gets added to the position (middle of player)
            Vector3 offset = new Vector3(
                Mathf.Cos(angle) * r,
                0f,
                Mathf.Sin(angle) * r
            );

            // random point around the player
            Vector3 candidate = transform.position + offset;

            // Checking if the point is valid
            if (CheckIfAreaUnlocked(candidate))
            {
                // Lets see if this can be all areas 
                if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }
        }

        // No valid point found after multiple attempts
        // The random error that pops up sometimes might happen after this
        return Vector3.positiveInfinity;
    }

    public bool CheckIfAreaUnlocked(Vector3 point)
    {
        int walkableMask = 1 << NavMesh.GetAreaFromName("Walkable");
        int bMask = 1 << NavMesh.GetAreaFromName("B_Wing");
        int middleMask = 1 << NavMesh.GetAreaFromName("Middle_Area");

        // Cheking the default mask
        if (NavMesh.SamplePosition(point, out NavMeshHit hitDefault, 2f, walkableMask))
        {
            Debug.Log("Point is in walkable mask");
            return true;
        }
        // Cheking the b wing mask
        else if (NavMesh.SamplePosition(point, out NavMeshHit hitB, 2f, bMask) && bWingUnlocked)
        {
            Debug.Log("Point is in b wing mask");
            return true;
        }
        // Cheking the Middle area mask
        else if (NavMesh.SamplePosition(point, out NavMeshHit hitMiddle, 2f, middleMask) && middleAreaUnlocked)
        {
            Debug.Log("Point is in middle area mask");
            return true;
        }

        // If all fail we return false -> failed point (out of bounds)
        Debug.LogError(point + " Point is not in any mask!");
        return false;
    }
}
