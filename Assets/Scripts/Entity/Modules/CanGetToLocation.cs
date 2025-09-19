using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CanGetToLocation : MonoBehaviour
{
    public GameObject map;

    private NavMeshPath path;
    
    public Vector3 target;

    public NavMeshAgent agent;
    private float delay = 0.01f;

    private void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();

        if(agent == null)
        {
            Debug.LogError("No NavmeshAgent found! " + gameObject.name);
            return;
        }

        path = new NavMeshPath();

        StartCoroutine(RouteCheckRoutine());
    }

    public void UpdateMoveToTarget(Vector3 newTarget)
    {
        target = newTarget;
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
        if (NavMesh.CalculatePath(agent.transform.position, target, NavMesh.AllAreas, path))
        {
            Blackboard.Instance.canReachLocation = true;
            map.SetActive(false);
        }
        else
        {
            Blackboard.Instance.canReachLocation = false;
            map.SetActive(true);
        }   
    }
}
