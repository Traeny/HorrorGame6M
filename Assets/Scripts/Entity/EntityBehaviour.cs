using System.Collections.Generic;
using UnityEngine;

public class EntityBehaviour : BTAgent
{
    [Header("Modules")]
    public KillPlayer killPlayerModule;
    public CanGetToLocation canGetToLocationModule;
    public Turn turningModule;
    public HotspotArea hotspotModule;

    [Header("Debug")]
    public Renderer rend;

    // Testing
    private Vector3 currentTarget;
    private bool hasTarget = false;


    new void Start()
    {
        base.Start();

        killPlayerModule = GetComponentInChildren<KillPlayer>();

        if(killPlayerModule == null)
        {
            Debug.LogError($"Kill Player module Missing from Entity Behaviour Script");
        }

        canGetToLocationModule = GetComponentInChildren<CanGetToLocation>();

        if (canGetToLocationModule == null)
        {
            Debug.LogError($"can get to module Missing from Entity Behaviour Script");
        }

        turningModule = GetComponentInChildren<Turn>();

        if (turningModule == null)
        {
            Debug.LogError($"Turn module Missing from Entity Behaviour Script");
        }

        hotspotModule = GetComponentInChildren<HotspotArea>();

        if (hotspotModule == null)
        {
            Debug.LogError($"Hotspot module Missing from Entity Behaviour Script");
        }

        // ----------- ( ROOT ) -----------
        Selector entityRoot = new Selector("Entity Root (Selector)");

        // ----------- ( PATROL BRANCH ) -----------
        BehaviourTree patrolConditionTree = new BehaviourTree(); // Patrol
        Sequence patrolConditions = new Sequence("Patrol Conditions (Sequence)"); // Patrol
        Leaf isPlayerVisible = new Leaf("Is Player Visible? (Condition Leaf)", IsPlayerVisible); // Hunt Patrol
        Inverter playerNotVisible = new Inverter("Player Not Visible? (Inverter)"); // Hunt Patrol
        Inverter notSuspicious = new Inverter("Entity Not Suspicious? (Inverter)"); // Patrol
        Leaf isSuspicious = new Leaf("Entity is Suspicious? (Condition Leaf)", IsSuspicious); // Hunt Patrol
        DependencySequence patrolBehaviour = new DependencySequence("Patrol Behaviour (Dependancy Sequence)", patrolConditionTree, agent); // Patrol
        Leaf wanderRandomly = new Leaf("Wander Randomly! (Action Leaf)", WanderRandomly); // Patrol

        // ----------- ( KILL BRANCH ) -----------
        Sequence killBehaviour = new Sequence("Kill Behaviour (Sequence)"); // Kill Branch
        Leaf playerInAttackRange = new Leaf("Is Player In Attack Range? (Condition Leaf)", IsPlayerInAttackRange); // Kill Branch
        Leaf killPlayer = new Leaf("Kill Player! (Action Leaf)", KillPlayer); // Kill Branch

        // ----------- ( HUNT BRANCH ) -----------
        BehaviourTree huntConditionTree = new BehaviourTree(); // Hunt Branc
        Selector huntBehaviour = new Selector("Hunt Behaviour (Selector)"); // Hunt Branch
        Sequence directPursuit = new Sequence("Direct Pursuit (Sequence)"); // Hunt Branch
        Leaf goToPlayerPosition = new Leaf("Go To Player Position (Action Leaf)", GoToPlayerPosition); // Hunt Branch
        Sequence stalkPursuit = new Sequence("Stalk Pursuit (Sequence)"); // Hunt Branch   
        Sequence stalkConditions = new Sequence("Hunt Conditions (Sequence)"); // Hunt Branch
        Leaf heardSomething = new Leaf("Heard Something? (Condition Leaf)", HeardSomething); // Hunt Branch
        Inverter haventHeardAnything = new Inverter("Haven't heard Anything (Inverter)"); // Hunt Branch
        DependencySequence stalkBehaviour = new DependencySequence("Stalk Behaviour (Dependancy Sequence)", huntConditionTree, agent); // Hunt Branch
        Leaf moveToHotspotPoint = new Leaf("Move to Hotspot Point (Action Leaf)", MoveToHotspotPoint); // Hunt Branch
        Leaf generateSearchPoints = new Leaf("Generate Search Points (Action Leaf)", GenerateSearchPoints);
        BehaviourTree searchLoopConditionTree = new BehaviourTree(); // Hunt Branch
        Sequence searchLoopConditions = new Sequence("Search Loop Conditions (Sequence)"); // Hunt Branch
        Leaf searchPointsLeft = new Leaf("Search Points Left (Condition Leaf)", SearchPointsLeft); // Hunt Branch
        Loop searchArea = new Loop("Search Area (Loop)", searchLoopConditionTree); // Hunt Branch
        Leaf lookAround = new Leaf("Look Around! (Action Leaf)", LookAround); // Hunt Branch
        Leaf goToPoint = new Leaf("Go To Point (Action Leaf)", GoToPoint); // Hunt Branch
        Leaf newHotspot = new Leaf("New Hotspot?", IsNewHotspot);
        Inverter noNewHotspot = new Inverter("No new Hotspot (Inverter)");

        // Patrol Conditions Tree
        playerNotVisible.AddChild(isPlayerVisible);
        notSuspicious.AddChild(isSuspicious);

        patrolConditions.AddChild(playerNotVisible);
        patrolConditions.AddChild(notSuspicious);

        patrolConditionTree.AddChild(patrolConditions);

        // Patrol behaviour Branch
        patrolBehaviour.AddChild(wanderRandomly);

        // ---------- ( Hunt Branch ) ------------------

        // Hunt Conditions Tree
        stalkConditions.AddChild(playerNotVisible);
        huntConditionTree.AddChild(stalkConditions);

        // Search Loop Condition Tree
        noNewHotspot.AddChild(newHotspot);
        searchLoopConditions.AddChild(searchPointsLeft);
        searchLoopConditions.AddChild(noNewHotspot);
        searchLoopConditionTree.AddChild(searchLoopConditions);

        searchArea.AddChild(goToPoint);
        searchArea.AddChild(lookAround);

        stalkBehaviour.AddChild(moveToHotspotPoint);
        stalkBehaviour.AddChild(generateSearchPoints);
        stalkBehaviour.AddChild(searchArea);

        stalkPursuit.AddChild(isSuspicious);
        stalkPursuit.AddChild(stalkBehaviour);

        directPursuit.AddChild(isPlayerVisible);
        directPursuit.AddChild(goToPlayerPosition);

        huntBehaviour.AddChild(directPursuit);
        huntBehaviour.AddChild(stalkPursuit);

        // Kill 
        killBehaviour.AddChild(playerInAttackRange);
        killBehaviour.AddChild(killPlayer);

        // ----------- ( Final Tree ) -----------
        entityRoot.AddChild(killBehaviour);
        entityRoot.AddChild(huntBehaviour);
        entityRoot.AddChild(patrolBehaviour);

        tree.AddChild(entityRoot);
        tree.PrintTree();
    }

    // Condition nodes
    public Node.Status IsPlayerVisible() // ?
    {
        if(Blackboard.Instance.isPlayerVisible)
        {
            return Node.Status.SUCCESS;
        }
        return Node.Status.FAILURE;
    }

    public Node.Status IsNewHotspot() // ?
    {
        if (Blackboard.Instance.CheckIfNewHotspot())
        {
            return Node.Status.SUCCESS;
        }

        return Node.Status.FAILURE;
    }

    public Node.Status IsSuspicious() // ?
    {
        if (Blackboard.Instance.isSuspicious)
        {
            return Node.Status.SUCCESS;
        }

        return Node.Status.FAILURE;
    }

    public Node.Status IsPlayerInAttackRange() // ?
    {
        if (Blackboard.Instance.playerInAttackRange)
        {
            return Node.Status.SUCCESS;
        }

        return Node.Status.FAILURE;
    }

    public Node.Status HeardSomething() // ?
    {
        if (Blackboard.Instance.heardNoise)
        {
            return Node.Status.SUCCESS;
        }

        return Node.Status.FAILURE;
    }

    public Node.Status SearchPointsLeft() // ?
    {
        if(Blackboard.Instance.searchPoints.Count > 0)
        {
            return Node.Status.SUCCESS;
        }
        else
        {
            return Node.Status.FAILURE;
        }   
    }

    // Action nodes

    public Node.Status GoToPoint() // !
    {
        rend.material.color = Color.gray;

        Blackboard.Instance.UpdateMovementSpeed(2.5f); // OK

        Node.Status s = GoToLocation(Blackboard.Instance.searchPoints[0]);

        if (s == Node.Status.SUCCESS)
        {
            Blackboard.Instance.searchPoints.RemoveAt(0);
        }

        return s;
    }

    public Node.Status KillPlayer() // !
    {
        // This function could trigger the different kill animations

        rend.material.color = Color.red;

        killPlayerModule.KillPlayerAction();

        return Node.Status.SUCCESS;
    }

    public Node.Status LookAround() // !
    {
        rend.material.color = Color.yellow;

        // This look around function does not work like it should!
        Node.Status s = turningModule.LookAround();

        return s;
    }

    public Node.Status GenerateSearchPoints() // !
    {
        rend.material.color = Color.white;

        List<Vector3> points = hotspotModule.GetRandomPoints(3, Blackboard.Instance.hotspotOrigin);

        if (points.Count <= 0 || points == null)
        {
            return Node.Status.FAILURE;
        }

        return Node.Status.SUCCESS;
    }

    public Node.Status MoveToHotspotPoint() // !
    {
        rend.material.color = Color.black;

        Blackboard.Instance.SetCurrentHotspot();

        Node.Status s = GoToLocation(Blackboard.Instance.hotspotOrigin);

        return s;
    }

    public Node.Status GoToPlayerPosition() // !
    {
        rend.material.color = Color.cyan;

        Blackboard.Instance.UpdateMovementSpeed(5f); // OK

        Node.Status s = GoToLocation(Blackboard.Instance.lastSeenPosition);

        return s;
    }

    public Node.Status WanderRandomly() // !
    {
        rend.material.color = Color.green;

        Blackboard.Instance.UpdateMovementSpeed(3.5f); // OK

        if (!hasTarget)
        {
            currentTarget = Area.Instance.GetRandomPoint();
            hasTarget = true;
        }

        float distance = Vector3.Distance(transform.position, currentTarget);

        if (distance <= 2f)
        {
            hasTarget = false;
        }

        return GoToLocation(currentTarget);
    }
}