using System.Collections;
using Player_Script;
using UnityEngine;
using UnityEngine.AI;

public class BTAgent : MonoBehaviour
{
    public BehaviourTree tree;
    public NavMeshAgent agent;

    public enum ActionState { IDLE, WORKING };
    public ActionState state = ActionState.IDLE;

    public Node.Status treeStatus = Node.Status.RUNNING;

    WaitForSeconds waitForSeconds;

    public void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        tree = new BehaviourTree();
        waitForSeconds = new WaitForSeconds(Random.Range(0.1f, 0.2f));
        StartCoroutine(Behave());
    }

    public Node.Status GoToLocation(Vector3 destination)
    { 
        float distanceToTarget = Vector3.Distance(destination, this.transform.position);
        
        if (state == ActionState.IDLE)
        {
            agent.SetDestination(destination); 
            state = ActionState.WORKING;
        } 
        else if (Vector3.Distance(agent.pathEndPosition, destination) >= 2)
        {
            state = ActionState.IDLE;
            return Node.Status.FAILURE;
        }
        else if (distanceToTarget < 2) 
        {
            state = ActionState.IDLE;
            return Node.Status.SUCCESS;
        }

        return Node.Status.RUNNING;
    }

    /*
    public Node.Status CanSee(Vector3 target, string tag, float distance, float maxAngle, Transform eyeTransform)
    {
        Vector3 directionToTarget = (target - this.transform.position).normalized;


        float angle = Vector3.Angle(directionToTarget, this.transform.forward);

        if(angle <= maxAngle && directionToTarget.magnitude <= distance)
        {
            RaycastHit hitInfo;

            if(Physics.Raycast(eyeTransform.position, directionToTarget, out hitInfo, distance))
            {
                if (hitInfo.collider.gameObject.CompareTag(tag))
                {
                    Blackboard.Instance.SetLastSeenPlayerPosition(player.transform.position);
                    Blackboard.Instance.SetlastSeenPlayerTime(Time.time);

                    return Node.Status.SUCCESS;
                }
            }
        }
        return Node.Status.FAILURE;        
    }
    */

    private IEnumerator Behave()
    {
        while (true)
        {
            treeStatus = tree.Process();
            yield return waitForSeconds;
        }
    }
}
