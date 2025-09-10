using System.Collections;
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


    Vector3 rememberedLocation; 

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

    // This could prob be moved to a module later
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
                    return Node.Status.SUCCESS;
                }
            }
        }
        return Node.Status.FAILURE;        
    }

    private IEnumerator Behave()
    {
        while (true)
        {
            treeStatus = tree.Process();
            yield return waitForSeconds;
        }
    }

    /*
    public Node.Status IsOpen()
    {
        if (Blackboard.Instance.timeOfDay < Blackboard.Instance.openTime || Blackboard.Instance.timeOfDay > Blackboard.Instance.closeTime)
        {
            return Node.Status.FAILURE;
        }
        return Node.Status.SUCCESS;
    }

    public Node.Status Flee(Vector3 location, float distance)
    {
        if(state == ActionState.IDLE)
        {
            rememberedLocation = this.transform.position + (transform.position - location).normalized * distance;
        }
        return GoToLocation(rememberedLocation);
    }



    public Node.Status GoToDoor(GameObject door)
    {
        Node.Status s = GoToLocation(door.transform.position);

        if (s == Node.Status.SUCCESS)
        {
            if (!door.GetComponent<Lock>().isLocked)
            {
                door.GetComponent<NavMeshObstacle>().enabled = false;
                return Node.Status.SUCCESS;
            }
            return Node.Status.FAILURE;
        }
        else
            return s;
    }
    */
}
