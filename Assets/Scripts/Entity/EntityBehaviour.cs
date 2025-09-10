using System.Collections;
using UnityEngine;

public class EntityBehaviour : BTAgent
{
    public GameObject[] patrolPoints;
    public GameObject player;
    public Transform eyeTransform;
    public float attackRange = 3f;

    public float maxAngle = 80f;
    public float distance = 20f;
    public float memoryDuration = 5f;
    float nextActionTime;

    new void Start()
    {
        base.Start();
        player = GameObject.FindWithTag("Player");

        if(player == null)
        {
            Debug.LogError("Player Missing / Not Found!");
            return;
        }

        /*
        Entity Root (Selector)
        -Engage If Player Is Visible (Sequence)
        --Can See Player? (Leaf)
        --Engage Player (Selector)
        ---Attack (Sequence)
        ----Player In Attack Range? (Leaf)
        ----Attack the player (Leaf)
        ---Chase Player (Leaf)
        -Patrol until, (Dependance Sequence)
        --Select patrol point (Sequence)
        ---Go To Sphere (Leaf)
        ---Go To Sphere (1) (Leaf)
        ---Go To Sphere (2) (Leaf)
        ---Go To Sphere (3) (Leaf)
        */

        // General
        Leaf canSeePlayer = new Leaf("Can See Player? (Leaf)", CanSeePlayer);

        // Attack
        Leaf playerInAttackRange = new Leaf("Player In Attack Range? (Leaf)", PlayerInAttackRange);
        Leaf attackPlayer = new Leaf("Attack the player (Leaf)", AttackPlayer);
        Sequence attack = new Sequence("Attack (Sequence)");
        attack.AddChild(playerInAttackRange);
        attack.AddChild(attackPlayer);

        // Chase
        Leaf chasePlayer = new Leaf("Chase Player (Leaf)", ChasePlayer);

        // Search
        Leaf recentlySawPlayer = new Leaf("Recently saw player?", RecentlySawPlayer);
        Leaf goToLastLocation = new Leaf("Go to last location", GoToLastLocation);
        Leaf lookAround = new Leaf("Look Around", LookAround);

        Sequence goLookAround = new Sequence("Go look Around for player (Sequence)");
        goLookAround.AddChild(goToLastLocation);
        goLookAround.AddChild(lookAround);

        Sequence searchForPlayer = new Sequence("Search for player (Sequence)");
        searchForPlayer.AddChild(recentlySawPlayer);
        searchForPlayer.AddChild(goLookAround);

        // Patrol
        BehaviourTree patrolConditionsTree = new BehaviourTree();
        Sequence patrolConditions = new Sequence("Patrol conditions (Sequence)");
        Inverter cantSeePlayer = new Inverter("Can't See Player (Inverter)");
        cantSeePlayer.AddChild(canSeePlayer);
        patrolConditions.AddChild(cantSeePlayer);
        patrolConditionsTree.AddChild(patrolConditions);
        DependencySequence patrol = new DependencySequence("Patrol until, (Dependance Sequence)", patrolConditionsTree, agent);

        Sequence selectPatrolPoint = new Sequence("Select patrol point (Sequence)");

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            Leaf goToPatrolPoint = new Leaf("Go To " + patrolPoints[i].name + " (Leaf)", i, GoToPoint);

            selectPatrolPoint.AddChild(goToPatrolPoint);
        }
        patrol.AddChild(selectPatrolPoint);

        // Player engagement
        Selector engagePlayer = new Selector("Engage Player (Selector)");
        engagePlayer.AddChild(attack);
        engagePlayer.AddChild(chasePlayer);

        // Only engage with player if the player is visible
        Sequence engageIfPlayerIsVisible = new Sequence("Engage If Player Is Visible (Sequence)");
        engageIfPlayerIsVisible.AddChild(canSeePlayer);
        engageIfPlayerIsVisible.AddChild(engagePlayer);

        // Entity Root
        Selector entityRoot = new Selector("Entity Root (Selector)");
        entityRoot.AddChild(engageIfPlayerIsVisible);
        entityRoot.AddChild(searchForPlayer);
        entityRoot.AddChild(patrol);

        // Final tree
        tree.AddChild(entityRoot);
        tree.PrintTree();
    }

    // Needed Functions here...
    public Node.Status GoToPoint(int i)
    {
        Node.Status s = GoToLocation(patrolPoints[i].transform.position);

        return s;
    }

    public Node.Status CanSeePlayer()
    {
        Vector3 directionToTarget = (player.transform.position - this.transform.position).normalized;


        float angle = Vector3.Angle(directionToTarget, this.transform.forward);

        if (angle <= maxAngle && directionToTarget.magnitude <= distance)
        {
            RaycastHit hitInfo;

            if (Physics.Raycast(eyeTransform.position, directionToTarget, out hitInfo, distance))
            {
                if (hitInfo.collider.gameObject.CompareTag("Player"))
                {
                    Blackboard.Instance.SetLastSeenPlayerPosition(player.transform.position);
                    Blackboard.Instance.SetlastSeenPlayerTime(Time.time);

                    return Node.Status.SUCCESS;
                }
            }
        }
        return Node.Status.FAILURE;
    }

    public Node.Status ChasePlayer()
    {
        Node.Status s = GoToLocation(player.transform.position);

        return s;
    }

    public Node.Status PlayerInAttackRange()
    {
        float distanceToPlayer = Vector3.Distance(this.transform.position, player.transform.position);
        
        if (distanceToPlayer <= attackRange)
        {
            return Node.Status.SUCCESS;
        }

        return Node.Status.FAILURE;
    }

    public Node.Status AttackPlayer()
    {
        Debug.Log("Attacking Player!!!");
        return Node.Status.SUCCESS;
    }

    public Node.Status RecentlySawPlayer()
    {
        if (Time.time - Blackboard.Instance.lastSeenPlayerTime <= memoryDuration)
        {
            return Node.Status.SUCCESS;
        }
        return Node.Status.FAILURE;
    }


    public Node.Status GoToLastLocation()
    {
        Node.Status s = GoToLocation(Blackboard.Instance.lastSeenPlayerPosition);

        return s;
    }

    private bool isLookingAround = false;
    private float lookAroundEndTime;


    public Node.Status LookAround()
    {
        // Initialize on first tick
        if (!isLookingAround)
        {
            isLookingAround = true;
            lookAroundEndTime = Time.time + 3f; // look around for 3 seconds
            Debug.Log("Started looking around...");
        }

        // Still looking?
        if (Time.time < lookAroundEndTime)
        {
            Debug.Log("Looking around for player...");
            return Node.Status.RUNNING;
        }

        // Done
        isLookingAround = false; // reset for next time
        Debug.Log("Finished looking around.");
        return Node.Status.SUCCESS;
    }
}
