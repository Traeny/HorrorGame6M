using UnityEngine;
using UnityEngine.AI;


public class EntityAnimationController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent agent;

    private void OnValidate()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (agent == null)
        {
            agent = GetComponentInParent<NavMeshAgent>();
        }
    }
    private void Update()
    {
        Vector3 velocity = agent.velocity;

        float forwardSpeed = Vector3.Dot(velocity, transform.forward);
        float strafeSpeed = Vector3.Dot(velocity, transform.right);

        Debug.Log("Forward speed: " + forwardSpeed + "| Strafe speed: " + strafeSpeed);

        animator.SetFloat("Speed", forwardSpeed);
        animator.SetFloat("StrafeSpeed", strafeSpeed);
    }

}
