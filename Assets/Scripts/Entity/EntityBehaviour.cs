using System.Collections.Generic;
using UnityEngine;

public class EntityBehaviour : BTAgent
{
    [Header("Preset")]
    public EnemyPreset preset;

    [Header("Modules")]
    public KillPlayer killPlayerModule;
    public CanGetToLocation canGetToLocationModule;
    public Turn turningModule;
    public HotspotArea hotspotModule;
    public AnnouncePursuit pursuit;

    [Header("Debug")]
    public Renderer rend;

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

        pursuit = GetComponentInChildren<AnnouncePursuit>();

        if (pursuit == null)
        {
            Debug.LogError($"Pursuit module Missing from Entity Behaviour Script");
        }

        // ----------- ( ROOT ) -----------
        Selector entityRoot = new Selector("Entity Root (Selector)");

        // ----------- ( PATROL BRANCH ) -----------
        BehaviourTree patrolConditionTree = new BehaviourTree(); // Patrol
        Sequence patrolConditions = new Sequence("Patrol Conditions (Sequence)"); // Patrol
        Inverter notSuspicious = new Inverter("Entity Not Suspicious? (Inverter)"); // Patrol
        DependencySequence patrolBehaviour = new DependencySequence("Patrol Behaviour (Dependancy Sequence)", patrolConditionTree, agent); // Patrol
        Leaf wanderRandomly = new Leaf("Wander Randomly! (Action Leaf)", WanderRandomly); // Patrol

        // ----------- ( KILL BRANCH ) -----------
        Sequence killBehaviour = new Sequence("Kill Behaviour (Sequence)"); // Kill Branch
        Leaf playerInAttackRange = new Leaf("Is Player In Attack Range? (Condition Leaf)", IsPlayerInAttackRange); // Kill Branch
        Leaf killPlayer = new Leaf("Kill Player! (Action Leaf)", KillPlayer); // Kill Branch

        // ----------- ( HUNT BRANCH, PATROL BRANCH ) -----------
        Leaf isSuspicious = new Leaf("Entity is Suspicious? (Condition Leaf)", IsSuspicious); // Hunt Branch, Patrol Branch
        Leaf isPlayerVisible = new Leaf("Is Player Visible? (Condition Leaf)", IsPlayerVisible); // Hunt Branch, Patrol Branch
        Inverter playerNotVisible = new Inverter("Player Not Visible? (Inverter)"); // Hunt Branch, Patrol Branch

        // ----------- ( HUNT BRANCH ) -----------
        Selector huntBehaviour = new Selector("Hunt Behaviour (Selector)"); // Hunt Branch
        Sequence pursueSequence = new Sequence("Pursue Sequence (Sequence)"); // Hunt Branch
        Selector pursuitConditions = new Selector("Pursuit Conditions (Selector)"); // Hunt Branch
        Leaf sawSomething = new Leaf("Saw Something (Conditions Leaf)", SawSomething); // Hunt Branch
        Leaf heardLoudNoise = new Leaf("Heard Loud Noise (Condition Leaf)", HeardLoudNoise); // Hunt Branch
        Leaf announcePursuit = new Leaf("Announce Pursuit (Action Leaf)", AnnouncePursuit); // Hunt Branch
        Selector pursuitStyleSelector = new Selector("Pursuit Style Selector (Selector)"); // Hunt Branch
        Sequence directPursuit = new Sequence("Direct Pursuit (Sequence)"); // Hunt Branch
        Leaf runToLastSeenPosition = new Leaf("Run To Last Seen Position (Action Leaf)", RunToLastSeenPosition); // Hunt Branch
        BehaviourTree huntConditionTree = new BehaviourTree(); // Hunt Branch
        DependencySequence stalkBehaviour = new DependencySequence("Stalk Behaviour (Dependancy Sequence)", huntConditionTree, agent); // Hunt Branch
        Sequence stalkConditions = new Sequence("Hunt Conditions (Sequence)"); // Hunt Branch
        Leaf runToHotspotPoint = new Leaf("Run to Hotspot Point (Action Leaf)", RunToHotspotPoint); // Hunt Branch
        Leaf generateSearchPoints = new Leaf("Generate Search Points (Action Leaf)", GenerateSearchPoints); // Hunt Branch
        BehaviourTree searchLoopConditionTree = new BehaviourTree(); // Hunt Branch
        Loop searchArea = new Loop("Search Area (Loop)", searchLoopConditionTree); // Hunt Branch
        Sequence searchLoopConditions = new Sequence("Search Loop Conditions (Sequence)"); // Hunt Branch
        Leaf searchPointsLeft = new Leaf("Search Points Left (Condition Leaf)", SearchPointsLeft); // Hunt Branch
        Inverter noNewHotspot = new Inverter("No new Hotspot (Inverter)"); // Hunt Branch
        Leaf newHotspot = new Leaf("New Hotspot?", IsNewHotspot); // Hunt Branch
        Leaf goToPoint = new Leaf("Go To Point (Action Leaf)", GoToPoint); // Hunt Branch
        Leaf lookAround = new Leaf("Look Around! (Action Leaf)", LookAround); // Hunt Branch
        Sequence investigate = new Sequence("Investigate (Sequence)"); // Hunt Branch
        Sequence soundCheck = new Sequence("Sound Check (Sequence)"); // Hunt Branch
        Leaf heardSomething = new Leaf("Heard Something? (Condition Leaf)", HeardSomething); // Hunt Branch
        Sequence investigateSequence = new Sequence("Investigate Sequence (Sequence)"); // Hunt Branch
        Leaf goToInterestPoint = new Leaf("Go To Interest Point (Action Leaf)", GoToInterestPoint); // Hunt Branch

        // ----------- ( Patrol Branch build ) -----------
        // Patrol condition Tree (BT)
        playerNotVisible.AddChild(isPlayerVisible);
        notSuspicious.AddChild(isSuspicious);
        patrolConditions.AddChild(playerNotVisible);
        patrolConditions.AddChild(notSuspicious);
        patrolConditionTree.AddChild(patrolConditions);

        // Patrol behaviour Branch
        patrolBehaviour.AddChild(wanderRandomly);

        // ---------- ( Hunt Branch ) ------------------

        // Hunt Conditions Tree (BT)
        stalkConditions.AddChild(playerNotVisible);
        huntConditionTree.AddChild(stalkConditions);

        // Search Loop Condition Tree (BT)
        noNewHotspot.AddChild(newHotspot);
        searchLoopConditions.AddChild(searchPointsLeft);
        searchLoopConditions.AddChild(noNewHotspot);
        searchLoopConditionTree.AddChild(searchLoopConditions);

        // Search area (loop)
        searchArea.AddChild(goToPoint);
        searchArea.AddChild(lookAround);

        // Stalk behaviour (Sequence)
        stalkBehaviour.AddChild(runToHotspotPoint);
        stalkBehaviour.AddChild(generateSearchPoints);
        stalkBehaviour.AddChild(searchArea);

        // Direct Pursuit (Sequence)
        directPursuit.AddChild(isPlayerVisible);
        directPursuit.AddChild(runToLastSeenPosition);

        // Pursuit Style Selector (Selector)
        pursuitStyleSelector.AddChild(directPursuit);
        pursuitStyleSelector.AddChild(stalkBehaviour);

        // Pursuit Condiditons (Selector)
        pursuitConditions.AddChild(sawSomething);
        pursuitConditions.AddChild(heardLoudNoise);

        // Pursue Sequence (Sequence)
        pursueSequence.AddChild(pursuitConditions);
        pursueSequence.AddChild(announcePursuit);
        pursueSequence.AddChild(pursuitStyleSelector);

        // Investigate Sequence (Sequence)
        investigateSequence.AddChild(goToInterestPoint);
        investigateSequence.AddChild(lookAround);

        // Sound Check (Sequence)
        soundCheck.AddChild(isSuspicious);
        soundCheck.AddChild(heardSomething);

        // Investigate (Sequence)
        investigate.AddChild(soundCheck);
        investigate.AddChild(investigateSequence);

        // Hunt Behavior (Selector)
        huntBehaviour.AddChild(pursueSequence);
        huntBehaviour.AddChild(investigate);

        // ----------- ( Kill Branch build ) -----------

        // Kill Behaviour (Sequence)
        killBehaviour.AddChild(playerInAttackRange);
        killBehaviour.AddChild(killPlayer);

        // ----------- ( Final Tree build ) -----------
        entityRoot.AddChild(killBehaviour);
        entityRoot.AddChild(huntBehaviour);
        entityRoot.AddChild(patrolBehaviour);

        tree.AddChild(entityRoot);
        tree.PrintTree();
    }

    // ----------- ( Condition nodes ) -----------
    /*
     *  Checks if the player is visile via the balckboard 
     */
    public Node.Status IsPlayerVisible()
    {
        if(Blackboard.Instance.isPlayerVisible)
        {
            return Node.Status.SUCCESS;
        }
        return Node.Status.FAILURE;
    }
    
    /*
     *  Checks if there is a new hotspot point via the blacboard
     */
    public Node.Status IsNewHotspot()
    {
        if (Blackboard.Instance.CheckIfNewHotspot())
        {
            return Node.Status.SUCCESS;
        }

        return Node.Status.FAILURE;
    }

    /*
     *  Checks if the enemy is suspicious via the blacboard
     */
    public Node.Status IsSuspicious()
    {
        if (Blackboard.Instance.isSuspicious)
        {
            return Node.Status.SUCCESS;
        }

        return Node.Status.FAILURE;
    }

    /*
     *  Checks if the player is in attack range via the blackboard
     */
    public Node.Status IsPlayerInAttackRange()
    {
        if (Blackboard.Instance.playerInAttackRange)
        {
            return Node.Status.SUCCESS;
        }

        return Node.Status.FAILURE;
    }

    /*
     *  Checks if the enemy recently heard something via the blackboard
     */
    public Node.Status HeardSomething()
    {
        if (Blackboard.Instance.heardNoise)
        {
            return Node.Status.SUCCESS;
        }

        return Node.Status.FAILURE;
    }

    /*
     *  Checks if there's any search points left via the blacboard
     */
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

    /*
     *  Checks if the enemy has heard a loud noice recently via the blackboard
     */
    public Node.Status HeardLoudNoise()
    {
        if (Blackboard.Instance.heardLoudNoise)
        {
            return Node.Status.SUCCESS;
        }

        return Node.Status.FAILURE;
    }

    /*
     *  Chacks if the enemy has seen the player recently via the blacboard
     */
    public Node.Status SawSomething()
    {
        if (Blackboard.Instance.isPlayerVisible)
        {
            return Node.Status.SUCCESS;
        }
        return Node.Status.FAILURE;
    }

    // ----------- ( Action nodes ) -----------
    /*
     * Moving to the current first searchpoint found in the blackboard. 
     * When the current index 0 point is reached it gets removed.
     * 
     * Agents color changes to ORANGE for debugging
     */
    public Node.Status GoToPoint() 
    {
        rend.material.color = Color.orange;

        // Walking
        Blackboard.Instance.UpdateMovementSpeed(preset.walkSpeed);

        Node.Status s = GoToLocation(Blackboard.Instance.searchPoints[0]);

        if (s == Node.Status.SUCCESS)
        {
            Blackboard.Instance.searchPoints.RemoveAt(0);
        }

        return s;
    }

    /*
     *  Running to the players last seen position found in the blackboard.
     *  
     *  Agents color changes to MAGENTA for debugging
     */
    public Node.Status RunToLastSeenPosition()
    {
        rend.material.color = Color.magenta;

        // Running
        Blackboard.Instance.UpdateMovementSpeed(preset.runSpeed);

        Node.Status s = GoToLocation(Blackboard.Instance.lastSeenPosition);

        return s;
    }

    /*
     * This runs when the enemy has reached it's desired location and wants to search the are
     * NOTE: This function does not work for some reason and needs to be fixed!
     * 
     * When looing around the agenst color is YELLOW
     */
    public Node.Status LookAround() 
    {
        rend.material.color = Color.yellow;

        Node.Status s = turningModule.LookAround();

        return s;
    }

    /*
     * This node prompts the generation of search points inside the hotsopt area.
     * if the points don't get created or are null the node fails
     * 
     * During this time the enemy will be GRAY, but it won't really be visible
     */
    public Node.Status GenerateSearchPoints() 
    {
        rend.material.color = Color.gray;

        List<Vector3> points = hotspotModule.GetRandomPoints(preset.searchPointAmount, Blackboard.Instance.hotspotOrigin);

        if (points.Count <= 0 || points == null)
        {
            return Node.Status.FAILURE;
        }

        return Node.Status.SUCCESS;
    }

    /*
     * This node is for the agent to run to the hotspot point that is registered in the blackboard
     * NOTE: i dont know why there's logic to set the new hotspot point? 
     * Maybe to make sure it's updated
     * 
     * While moving to the hotspot point the agent is PURPLE
     */
    public Node.Status RunToHotspotPoint()
    {
        rend.material.color = Color.purple;

        // Running
        Blackboard.Instance.UpdateMovementSpeed(preset.runSpeed);

        Blackboard.Instance.SetCurrentHotspot();

        Node.Status s = GoToLocation(Blackboard.Instance.hotspotOrigin);

        return s;
    }

    /*
     *  This node makes the entity wal to it's current interest point
     *  
     *  While doing so the agents color is BLUE
     */
    public Node.Status GoToInterestPoint()
    {
        rend.material.color = Color.blue;

        // Waliking
        Blackboard.Instance.UpdateMovementSpeed(preset.walkSpeed);

        Node.Status s = GoToLocation(Blackboard.Instance.interestPoint);

        return s;
    }

    /*
     * Generating random patrol point inside the partol area.
     * This goes on infinitely till the enemy detects someting
     * 
     * While patroling the agent it green
     */
    public Node.Status WanderRandomly()
    {
        rend.material.color = Color.green;

        // Walikng
        Blackboard.Instance.UpdateMovementSpeed(preset.walkSpeed);

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

    /*
     * This is a placeholder function
     * Needs implementation!
     * 
     * During announcement the agent will be AZURE color
     */
    public Node.Status AnnouncePursuit()
    {
        rend.material.color = Color.azure;

        Node.Status s = pursuit.AnnouncePlayerPursuit();

        return s;
    }

   /*
    * This runs when the player gets too close to the enemy.
    * NOTE: In the future this will be updated to have some actual logic
    * 
    * Agents color changes to RED for debugging 
    */
    public Node.Status KillPlayer()
    {
        rend.material.color = Color.red;

        killPlayerModule.KillPlayerAction();

        return Node.Status.SUCCESS;
    }
}