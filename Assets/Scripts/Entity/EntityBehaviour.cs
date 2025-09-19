using UnityEngine;

public class EntityBehaviour : BTAgent
{
    public KillPlayer killPlayerModule;
    public CanGetToLocation canGetToLocationModule;
    public Turn turningModule;

    public Renderer rend;


    new void Start()
    {
        base.Start();

        //rend = GetComponentInChildren<Renderer>();

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

        // Nodes

        // Selector Nodes
        //Selector quitInvestigate = new Selector("Quit Investigation? (Selector)");
        Selector interruptInvestigate = new Selector("Interrupt Investigate (Selector)");
        Selector proceedInvestigation = new Selector("Can proceed Investigation (Selector)");
        Selector entityRoot = new Selector("Entity Root (Selector)");
        Selector nextAttackAction = new Selector("Next attack action (Selector)");

        // Sequence Nodes
        Sequence investigateBehaviour = new Sequence("Investigate Behaviour (Sequence)");
        Sequence attackBehaviour = new Sequence("Attack Behaviour (Sequence)");
        Sequence canAttack = new Sequence("Can Attack Check? (Sequence)");
        Sequence chaseBehaviour = new Sequence("Chase Behaviour (Sequence)");
        Sequence investigateRoutine = new Sequence("Investigate Routine (Sequence)");
        Sequence patrolBehaviour = new Sequence("Patrol Behaviour (Sequence)");

        // Condition Leaf Nodes
        Leaf isPlayerVisible = new Leaf("Is Player Visible? (Condition Leaf)", IsPlayerVisible); // Debugged 
        Leaf isSuspicious = new Leaf("Entity is Suspicious (Condition Leaf)", IsSuspicious); // Debugged
        Leaf playerInAttackRange = new Leaf("Is Player In Attack Range? (Condition Leaf)", IsPlayerInAttackRange);
        Leaf canGetToInterestPoint = new Leaf("Can Get To Interest Point? (Condition Leaf)", CanGetToInterestPoint);
        Leaf heardSomething = new Leaf("Heard Something? (Condition Leaf)", HeardSomething);
        
        // Action Leaf Nodes
        Leaf killPlayer = new Leaf("Kill Player! (Action Leaf)", KillPlayer);
        Leaf moveTowardInterestPoint = new Leaf("Move To Last Known Location! (Action Leaf)", MoveTowardInterestPoint);

        // Need to implement
        
        //Leaf suspicionTimeExpired = new Leaf("Suspicion time expired? (Condition Leaf)", CheckSuspicionTime);
        //Leaf searchTimeExpired = new Leaf("Search Time Expired? (Condition Leaf)", SearchTimeExpired);
        //Leaf sawSomething = new Leaf("Saw Something? (Condition Leaf)", SawSomething);
        Leaf lookAround = new Leaf("Look Around! (Action Leaf)", LookAround);
        
        Leaf wanderRandomly = new Leaf("Wander Randomly! (Action Leaf)", WanderRandomly);
        

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
        //quitInvestigate.AddChild(suspicionTimeExpired);
        //quitInvestigate.AddChild(searchTimeExpired);

        investigateRoutine.AddChild(canGetToInterestPoint);
        investigateRoutine.AddChild(moveTowardInterestPoint);
        investigateRoutine.AddChild(lookAround);
        //investigateRoutine.AddChild(quitInvestigate);

        interruptInvestigate.AddChild(heardSomething);
        interruptInvestigate.AddChild(isPlayerVisible);

        proceedInvestigation.AddChild(interruptInvestigate);
        proceedInvestigation.AddChild(investigateRoutine);

        investigateBehaviour.AddChild(isSuspicious);
        investigateBehaviour.AddChild(proceedInvestigation);

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

    // Condition leaf Functions
    public Node.Status IsPlayerVisible() // ?
    {
        if(Blackboard.Instance.isPlayerVisible)
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

    public Node.Status CanGetToInterestPoint() // ?
    {
        canGetToLocationModule.UpdateMoveToTarget(Blackboard.Instance.interestPoint);

        if (Blackboard.Instance.canReachLocation)
        {
            return Node.Status.SUCCESS;
        }

        return Node.Status.FAILURE;
    }

    public Node.Status IsSuspicious() // ?
    {
        //rend.material.color = Color.gray;

        if (Blackboard.Instance.isSuspicious)
        {
            return Node.Status.SUCCESS;
        }

        return Node.Status.FAILURE;
    }

    public Node.Status HeardSomething() // ?
    {
        // rend.material.color = Color.blue;

        if (Blackboard.Instance.heardNoise)
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

    Vector3 currentTarget;

    public Node.Status WanderRandomly() // !
    {
        rend.material.color = Color.green;

        if (state == ActionState.IDLE)
        {
             currentTarget = Area.Instance.GetRandompoint();
        }

        return GoToLocation(currentTarget);
    }

    public Node.Status LookAround() // !
    {
        rend.material.color = Color.yellow;

        Node.Status s = turningModule.LookAround();

        return s;
    }

    public Node.Status MoveTowardInterestPoint() // !
    {
        rend.material.color = Color.gray;

        Node.Status s = GoToLocation(Blackboard.Instance.interestPoint);

        return s;
    }

    // Placeholder Functions

    public Node.Status SawSomething()
    {
        return Node.Status.SUCCESS;
    }
    public Node.Status SearchTimeExpired()
    {
        return Node.Status.SUCCESS;
    }

    public Node.Status CheckSuspicionTime()
    {
        return Node.Status.SUCCESS;
    }
}