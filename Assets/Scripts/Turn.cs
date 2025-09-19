using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Turn : MonoBehaviour
{
    public NavMeshAgent agent;

    private float lookElapsed = 0f;
    private float lookDuration = 3f;
    private float rotationSpeed = 90f;
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
            agent.updateRotation = false; // stop auto rotation
        }

        // Rotate while duration not finished
        if (lookElapsed < lookDuration)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            lookElapsed += Time.deltaTime;
            return Node.Status.RUNNING;
        }
        else
        {
            // Done looking
            agent.updateRotation = true;
            isLooking = false;
            return Node.Status.SUCCESS;
        }
    }


}
