using NUnit.Framework.Constraints;
using UnityEngine;

public class EntityBehaviour : BTAgent
{
    public GameObject[] patrolPoints;
    public GameObject player;
    public Transform eyeTransform;
    public float attackRange = 3f;
 
    new void Start()
    {
        base.Start();
        player = GameObject.FindWithTag("Player");

        if(player == null)
        {
            Debug.LogError("Player Missing / Not Found!");
            return;
        }

        /// Selector Root
        /// - Attack Sequence
        /// -- Condition: Can See Player? 
        /// -- Condition: Player In Attack Range?
        /// -- Action: Attack
        /// - Chase Sequence
        /// -- Condition: Can See Player? 
        /// -- Action: Chase
        /// - Action: Patrol

        // General
        Leaf canSeePlayer = new Leaf("Can See Player?", CanSeePlayer);

        // Attack
        Leaf playerInAttackRange = new Leaf("Player In Attack Range?", PlayerInAttackRange);
        Leaf attackPlayer = new Leaf("Attack the player", AttackPlayer);
        Sequence attack = new Sequence("Attack Sequence");
        attack.AddChild(playerInAttackRange);
        attack.AddChild(attackPlayer);


        // Chase
        Leaf chasePlayer = new Leaf("Chase Player", ChasePlayer);

        // Patrol
        BehaviourTree patrolConditionsTree = new BehaviourTree();
        Sequence patrolConditions = new Sequence("Patrol conditions");
        Inverter cantSeePlayer = new Inverter("Can't See Player");
        cantSeePlayer.AddChild(canSeePlayer);
        patrolConditions.AddChild(cantSeePlayer);
        patrolConditionsTree.AddChild(patrolConditions);
        DependencySequence patrol = new DependencySequence("Patrol until,", patrolConditionsTree, agent);

        Sequence selectPatrolPoint = new Sequence("Select patrol point");

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            Leaf goToPatrolPoint = new Leaf("Go To " + patrolPoints[i].name, i, GoToPoint);

            selectPatrolPoint.AddChild(goToPatrolPoint);
        }
        patrol.AddChild(selectPatrolPoint);

        // Player engagement
        Selector engagePlayer = new Selector("Engage Player");
        engagePlayer.AddChild(attack);
        engagePlayer.AddChild(chasePlayer);

        // Only engage with player if the player is visible
        Sequence engageIfPlayerIsVisible = new Sequence("Engage If Player Is Visible");
        engageIfPlayerIsVisible.AddChild(canSeePlayer);
        engageIfPlayerIsVisible.AddChild(engagePlayer);

        // Entity Root
        Selector entityRoot = new Selector("Entity Root");
        entityRoot.AddChild(engageIfPlayerIsVisible);
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
        Debug.Log("Can see player: " + CanSee(player.transform.position, "Player", 5, 60, eyeTransform));
        return CanSee(player.transform.position, "Player", 10, 90, eyeTransform);
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
}

/// - Search Sequence
/// -- Condition: Has last known location 
/// -- Action: Move To Last Known Position