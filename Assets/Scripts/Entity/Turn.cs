using UnityEngine;
using UnityEngine.AI;

public class Turn : MonoBehaviour
{
    public NavMeshAgent agent;

    private float lookElapsed = 0f;
    private float lookDuration = 3f;
    private bool isLooking = false;

    private void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();

        if(agent == null)
        {
            Debug.LogError("No NavMeshAgent reference found in Turn script");
        }
    }

    public Node.Status LookAround()
    {
        if (!isLooking)
        {
            isLooking = true;
            lookElapsed = 0f;
        }

        if (lookElapsed < lookDuration)
        {
            lookElapsed += Time.deltaTime;
            return Node.Status.RUNNING;
        }
        else
        {
            isLooking = false;
            return Node.Status.SUCCESS;
        }
    }
}
