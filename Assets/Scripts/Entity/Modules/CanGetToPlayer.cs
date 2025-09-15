using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CanGetToPlayer : MonoBehaviour
{
    private NavMeshPath path;

    public GameObject player;
    public NavMeshAgent agent;
    private float delay = 0.1f;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player Missing / Not Found!");
            return;
        }

        agent = GetComponentInParent<NavMeshAgent>();

        if(agent == null)
        {
            Debug.LogError("No NavmeshAgent found! " + gameObject.name);
            return;
        }

        path = new NavMeshPath();

        StartCoroutine(RouteCheckRoutine());
    }

    private IEnumerator RouteCheckRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;
            CanReachTarget();
        }
    }

    public void CanReachTarget()
    {
        if (NavMesh.CalculatePath(agent.transform.position, player.transform.position, NavMesh.AllAreas, path))
        {
            Blackboard.Instance.canReachPlayer = true;
        }
        else
        {
            Blackboard.Instance.canReachPlayer = false;
        }   
    }
}
