using System.Collections.Generic;
using UnityEngine;

public class EntityBehaviour : BTAgent
{
    [Header("Modules")]
    public KillPlayer killPlayerModule;
    public CanGetToLocation canGetToLocationModule;
    public Turn turningModule;
    public HotspotArea hotspotModule;
    Vector3 currentTarget;

    [Header("Debug")]
    public Renderer rend;


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
        BehaviourTree huntConditionTree = new BehaviourTree(); // Hunt Branch

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

        // Patrol Conditions Tree
        playerNotVisible.AddChild(isPlayerVisible);
        notSuspicious.AddChild(isSuspicious);

        patrolConditions.AddChild(playerNotVisible);
        patrolConditions.AddChild(notSuspicious);

        patrolConditionTree.AddChild(patrolConditions);

        // Patrol behaviour Branch
        patrolBehaviour.AddChild(wanderRandomly);


        // Hunt Conditions Tree
        haventHeardAnything.AddChild(heardSomething);

        stalkConditions.AddChild(playerNotVisible);
        stalkConditions.AddChild(haventHeardAnything);

        huntConditionTree.AddChild(stalkConditions);

        // Search Loop Condition Tree
        searchLoopConditions.AddChild(searchPointsLeft);
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



        /*
         Leaf getHotspotPoint = new Leaf("Get Hotspot Point (Action Leaf)", GetHotspotPoint); // Hunt Branch
        //Leaf sawSomething = new Leaf("Saw Something? ()Condition Lead)", SawSomething); // Hunt Branch
        //Inverter didntSeeAnything = new Inverter("haventSeenAnything (Inverter)"); // Hunt Branch
        Selector interruptInvestigate = new Selector("Interrupt Investigate (Selector)");
        Selector proceedInvestigation = new Selector("Can proceed Investigation (Selector)");
        Selector nextAttackAction = new Selector("Next attack action (Selector)");
        
        // Sequence Nodes
        Sequence investigateBehaviour = new Sequence("Investigate Behaviour (Sequence)");
        Sequence canAttack = new Sequence("Can Attack Check? (Sequence)");
        Sequence chaseBehaviour = new Sequence("Chase Behaviour (Sequence)");
        Sequence investigateRoutine = new Sequence("Investigate Routine (Sequence)");

        // Condition Leaf Nodes
        Leaf canGetToInterestPoint = new Leaf("Can Get To Interest Point? (Condition Leaf)", CanGetToInterestPoint);


        // Action Leaf Nodes
        Leaf moveTowardInterestPoint = new Leaf("Move To Last Known Location! (Action Leaf)", MoveTowardInterestPoint);

        // Building the behavioral tree
 
        // Attack branch
        chaseBehaviour.AddChild(canGetToInterestPoint);
        chaseBehaviour.AddChild(moveTowardInterestPoint);

        canAttack.AddChild(playerInAttackRange);
        canAttack.AddChild(killPlayer);

        nextAttackAction.AddChild(canAttack);
        nextAttackAction.AddChild(chaseBehaviour);

        attackBehaviour.AddChild(isPlayerVisible);
        attackBehaviour.AddChild(nextAttackAction);

        // Investigate branch
        investigateRoutine.AddChild(canGetToInterestPoint);
        investigateRoutine.AddChild(moveTowardInterestPoint);
        investigateRoutine.AddChild(lookAround);

        interruptInvestigate.AddChild(heardSomething);
        interruptInvestigate.AddChild(isPlayerVisible);

        proceedInvestigation.AddChild(interruptInvestigate);
        proceedInvestigation.AddChild(investigateRoutine);

        investigateBehaviour.AddChild(isSuspicious);
        investigateBehaviour.AddChild(proceedInvestigation);

        // Patrol Branch
        notSuspicious.AddChild(isSuspicious);
        playerNotVisible.AddChild(isPlayerVisible);

        patrolConditions.AddChild(playerNotVisible);
        patrolConditions.AddChild(notSuspicious);

        patrolConditionTree.AddChild(patrolConditions);

        patrolBehaviour.AddChild(wanderRandomly);

        // Finalising the tree 

        entityRoot.AddChild(attackBehaviour);
        entityRoot.AddChild(investigateBehaviour);
        entityRoot.AddChild(patrolBehaviour);

        // Finalizing the tree
        killBehaviour.AddChild(playerInAttackRange);
        killBehaviour.AddChild(killPlayer);

        entityRoot.AddChild(killBehaviour);
            public Node.Status MoveTowardInterestPoint() // !
    {
        rend.material.color = Color.gray;

        Node.Status s = GoToLocation(Blackboard.Instance.interestPoint);

        return s;
    }
        */
    }

    // Tested
    public Node.Status IsPlayerVisible()
    {
        if(Blackboard.Instance.isPlayerVisible)
        {
            return Node.Status.SUCCESS;
        }

        return Node.Status.FAILURE;
    }

    public Node.Status IsSuspicious()
    {
        if (Blackboard.Instance.isSuspicious)
        {
            return Node.Status.SUCCESS;
        }

        return Node.Status.FAILURE;
    }

    public Node.Status WanderRandomly()
    {
        rend.material.color = Color.green;

        if (state == ActionState.IDLE)
        {
            currentTarget = Area.Instance.GetRandompoint();
        }

        return GoToLocation(currentTarget);
    }

    public Node.Status GoToPlayerPosition()
    {
        Node.Status s = GoToLocation(Blackboard.Instance.lastSeenPosition);
        return s;
    }

    public Node.Status MoveToHotspotPoint()
    {
        Node.Status s = GoToLocation(Blackboard.Instance.hotspotOrigin);

        return s;
    }

    public Node.Status IsPlayerInAttackRange() // ?
    {
        if (Blackboard.Instance.playerInAttackRange)
        {
            return Node.Status.SUCCESS;
        }

        return Node.Status.FAILURE;
    }

    public Node.Status HeardSomething()
    {

        if (Blackboard.Instance.heardNoise)
        {
            return Node.Status.SUCCESS;
        }

        return Node.Status.FAILURE;
    }

    public Node.Status GenerateSearchPoints()
    {
        // Placeholfer magic number 3. Will be turned to random based on a range later!
        List<Vector3> points = hotspotModule.GetRandomPoints(3, Blackboard.Instance.hotspotOrigin);

        if (points.Count <= 0 || points == null)
        {
            return Node.Status.FAILURE;
        }

        return Node.Status.SUCCESS;
    }

    public Node.Status GoToPoint()
    {
        // move to the firs point on the list
        Node.Status s = GoToLocation(Blackboard.Instance.searchPoints[0]);

        if (s == Node.Status.SUCCESS)
        {
            Blackboard.Instance.searchPoints.RemoveAt(0);
        }

        // PLLSSS WORK. Im on my knees

        return s;
    }

    // Not Tested?

    public Node.Status SawSomething() // ?
    {
        if (Blackboard.Instance.isPlayerVisible)
        {
            return Node.Status.SUCCESS;
        }

        return Node.Status.FAILURE;
    }



    public Node.Status CanGetToInterestPoint() // ?
    {
        canGetToLocationModule.UpdateMoveToTarget(Blackboard.Instance.interestPoint);

        if (Blackboard.Instance.canReachLocation)
        {
            return Node.Status.SUCCESS;
        }

        return Node.Status.FAILURE;
    }

    public Node.Status KillPlayer() // !
    {
        rend.material.color = Color.red;

        killPlayerModule.KillPlayerAction();
        return Node.Status.SUCCESS;
    }

    



    public Node.Status LookAround() // !
    {
        rend.material.color = Color.yellow;

        Node.Status s = turningModule.LookAround();

        return s;
    }

    public Node.Status GetHotspotPoint() // IDK if needed
    {
        return Node.Status.SUCCESS;
    }


    public Node.Status SearchPointsLeft()
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



    // Go To Positions


    // Needs work 



}