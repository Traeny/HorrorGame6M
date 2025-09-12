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

        // Nodes

        // Selector Nodes
        Selector entityRoot = new Selector("Entity Root (Selector)");
        Selector nextAttackAction = new Selector("Next attack action (Selector)");
        Selector becomeSuspicious = new Selector("Become Suspicious? (Selector)");
        Selector quitInvestigate = new Selector("Quit Investigation? (Selector)");

        // Sequence Nodes
        Sequence attackBehaviour = new Sequence("Attack Behaviour (Sequence)");
        Sequence canAttack = new Sequence("Can Attack Check? (Sequence)");
        Sequence chaseBehaviour = new Sequence("Chase Behaviour (Sequence)");
        Sequence investigateBehaviour = new Sequence("Investigate Behaviour (Sequence)");
        Sequence patrolBehaviour = new Sequence("Patrol Behaviour (Sequence)");

        // Condition Leaf Nodes
        Leaf isPlayerVisible = new Leaf("Is Player Visible? (Condition Leaf)", IsPlayerVisible); // Done
        Leaf playerInAttackRange = new Leaf("Is Player In Attack Range? (Condition Leaf)", IsPlayerInAttackRange);
        Leaf canGetToPlayer = new Leaf("Can Get To Player? (Condition Leaf)", CanGetToPlayer);
        Leaf heardSomething = new Leaf("Heard Something? (Condition Leaf)", HeardSomething);
        Leaf sawSomething = new Leaf("Saw Something? (Condition Leaf)", SawSomething);
        Leaf searchTimeExpired = new Leaf("Search Time Expired? (Condition Leaf)", SearchTimeExpired);

        // Action Leaf Nodes
        Leaf moveTowardsPlayer = new Leaf("Move Towards Player! (Action Leaf)", MoveTowardsPlayer);
        Leaf killPlayer = new Leaf("Kill Player! (Action Leaf)", KillPlayer);
        Leaf moveToLastKnownLocation = new Leaf("Move To Last Known Location! (Action Leaf)", MoveToLastKnownLocation);
        Leaf lookAround = new Leaf("Look Around! (Action Leaf)", LookAround);
        Leaf wanderRandomly = new Leaf("Wander Randomly! (Action Leaf)", WanderRandomly);


        // Building the behavioral tree

        // Attack branch
        chaseBehaviour.AddChild(canGetToPlayer);
        chaseBehaviour.AddChild(moveTowardsPlayer);

        canAttack.AddChild(playerInAttackRange);
        canAttack.AddChild(killPlayer);

        nextAttackAction.AddChild(canAttack);
        nextAttackAction.AddChild(chaseBehaviour);

        attackBehaviour.AddChild(isPlayerVisible);
        attackBehaviour.AddChild(nextAttackAction);

        // Investigate branch
        quitInvestigate.AddChild(isPlayerVisible);
        quitInvestigate.AddChild(searchTimeExpired);

        becomeSuspicious.AddChild(heardSomething);
        becomeSuspicious.AddChild(sawSomething);

        investigateBehaviour.AddChild(becomeSuspicious);
        investigateBehaviour.AddChild(moveToLastKnownLocation);
        investigateBehaviour.AddChild(lookAround);
        investigateBehaviour.AddChild(quitInvestigate);

        // Patrol Branch
        patrolBehaviour.AddChild(wanderRandomly);
        patrolBehaviour.AddChild(heardSomething);

        // Finalising the tree 

        entityRoot.AddChild(attackBehaviour);
        entityRoot.AddChild(investigateBehaviour);
        entityRoot.AddChild(patrolBehaviour);

        tree.AddChild(entityRoot);
        tree.PrintTree();
    }

    // Placeholder Functions
    public Node.Status IsPlayerVisible()
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
                    Blackboard.Instance.isPlayerVisible = true;
                    Blackboard.Instance.SetLastSeenPlayerPosition(player.transform.position);
                    Blackboard.Instance.SetlastSeenPlayerTime(Time.time);

                    return Node.Status.SUCCESS;
                }
            }
        }
        Blackboard.Instance.isPlayerVisible = false;
        return Node.Status.FAILURE;
    }
    public Node.Status IsPlayerInAttackRange() 
    {
        return Node.Status.SUCCESS; 
    }
    public Node.Status CanGetToPlayer()
    {
        return Node.Status.SUCCESS;
    }
    public Node.Status HeardSomething()
    {
        return Node.Status.SUCCESS;
    }
    public Node.Status SawSomething()
    {
        return Node.Status.SUCCESS;
    }
    public Node.Status SearchTimeExpired()
    {
        return Node.Status.SUCCESS;
    }
    public Node.Status MoveTowardsPlayer()
    {
        return Node.Status.SUCCESS;
    }
    public Node.Status KillPlayer()
    {
        return Node.Status.SUCCESS;
    }
    public Node.Status MoveToLastKnownLocation()
    {
        return Node.Status.SUCCESS;
    }
    public Node.Status LookAround()
    {
        return Node.Status.SUCCESS;
    }
    public Node.Status WanderRandomly()
    {
        return Node.Status.SUCCESS;
    }

    // Needed Functions here...
    /*

    public Node.Status GoToPoint(int i)
    {
        Node.Status s = GoToLocation(patrolPoints[i].transform.position);

        return s;
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


    /*

    // General
    Leaf canSeePlayer = new Leaf("Can See Player? (Leaf)", CanSeePlayer);

    // Attack
    //Leaf playerInAttackRange = new Leaf("Player In Attack Range? (Leaf)", PlayerInAttackRange);
    Leaf attackPlayer = new Leaf("Attack the player (Leaf)", AttackPlayer);
    Sequence attack = new Sequence("Attack (Sequence)");
    attack.AddChild(playerInAttackRange);
    attack.AddChild(attackPlayer);

    // Chase
    Leaf chasePlayer = new Leaf("Chase Player (Leaf)", ChasePlayer);

    // Search
    Leaf recentlySawPlayer = new Leaf("Recently saw player?", RecentlySawPlayer);
    Leaf goToLastLocation = new Leaf("Go to last location", GoToLastLocation);
    //Leaf lookAround = new Leaf("Look Around", LookAround);

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

    entityRoot.AddChild(engageIfPlayerIsVisible);
    entityRoot.AddChild(searchForPlayer);
    entityRoot.AddChild(patrol);

    // Final tree
    tree.AddChild(entityRoot);
    tree.PrintTree();
    */

    /*
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
}*/

    /*
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
    }*/
}