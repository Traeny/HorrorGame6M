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

        // ----------- ( KILL BRANCH ) -----------
        Sequence killBranch = new Sequence("Kill Branch (Sequence)");
        Leaf playerInAttackRange = new Leaf("Is Player In Attack Range? (Condition Leaf)", IsPlayerInAttackRange);
        Leaf killPlayer = new Leaf("Kill Player! (Action Leaf)", KillPlayer);

        // ----------- ( CHASE BRANCH ) -----------
        Sequence chaseBranch = new Sequence("Chase Branch (Sequence)");
        Leaf isPlayerVisible = new Leaf("Is Player Visible? (Condition Leaf)", IsPlayerVisible);
        Leaf setChaseBoolActive = new Leaf("Set Chase Bool Active (Action Leaf)", SetChaseBoolActive);
        Leaf runToLastSeenPosition = new Leaf("Run To Last Seen Position (Action Leaf)", RunToLastSeenPosition);

        // ----------- ( ANNOUNCE PURSUIT BRANCH ) -----------
        Sequence announcePursuitBranch = new Sequence("Announce Pursuit Branch (Sequence");
        Sequence prePursuitCondition = new Sequence("Per Pursuit Conditions (Sequence)");
        Leaf isHighlySuspicious = new Leaf("Is Highly Suspicious? (Condidion Leaf)", IsHighlySuspicious);
        Inverter playerNotVisible = new Inverter("Player Not Visible? (Inverter)");
        Leaf chaseNotActive = new Leaf("Chase Not Active (Condition Leaf)", ChaseNotActive);
        Leaf hasNotAnnouncedPursuit = new Leaf("Has Not Announced Pursuit (Condition Leaf)", HasNotAnnouncedPursuit);
        Leaf playAnnouncement = new Leaf("Announce Pursuit (Action Leaf)", AnnouncePursuit);
        Leaf setHasAnnounced = new Leaf("Set Has Announed (Action Leaf)", SetHasAnnounced);

        // ----------- ( STALK BRANCH ) -----------
        BehaviourTree stalkConditionTree = new BehaviourTree();
        DependencySequence stalkBranch = new DependencySequence("Stalk Branch (Dependancy Sequence)", stalkConditionTree, agent);
        Sequence stalkConditions = new Sequence("Stalk conditions (Sequence)");
        Leaf runToHotspotPoint = new Leaf("Run to Hotspot Point (Action Leaf)", RunToHotspotPoint);
        Leaf generateSearchPoints = new Leaf("Generate Search Points (Action Leaf)", GenerateSearchPoints);
        Leaf hasAnnounced = new Leaf("Has announced pursuit (condition leaf)", HasAnnouncedPursuit);
        BehaviourTree searchLoopConditionTree = new BehaviourTree();
        Loop searchArea = new Loop("Search Area (Loop)", searchLoopConditionTree);
        Leaf goToPoint = new Leaf("Go To Point (Action Leaf)", GoToPoint);
        Leaf lookAround = new Leaf("Look Around! (Action Leaf)", LookAround);
        Sequence searchLoopConditions = new Sequence("Search Loop Conditions (Sequence)");
        Leaf searchPointsLeft = new Leaf("Search Points Left (Condition Leaf)", SearchPointsLeft);
        Inverter noNewHotspotPoint = new Inverter("No New Hotspot Point (Inverter");
        Leaf newHotspotPoint = new Leaf("New Hotspot Point (Condition Leaf)", IsNewHotspot);

        // ----------- ( INVESTIGATE BRANCH ) -----------
        Sequence investigateBranch = new Sequence("Investigate Branch (Sequence)");
        Sequence allowInvestigate = new Sequence("Allow investigation (Sequence)");
        Leaf isSuspicious = new Leaf("Entity is Suspicious? (Condition Leaf)", IsSuspicious);
        Selector senseCheck = new Selector("Sense Chec (Selector)");
        Leaf heardSomething = new Leaf("Heard Something? (Condition Leaf)", HeardSomething);
        Leaf sawSomething = new Leaf("Saw Something (Conditions Leaf)", SawSomething);
        BehaviourTree investigateConditionTree = new BehaviourTree();
        DependencySequence investigate = new DependencySequence("Investigate (DepSeq)", investigateConditionTree, agent);
        Sequence investigateConditions = new Sequence("Investigate Conditions (Sequence)");
        Leaf goToInterestPoint = new Leaf("Go To Interest Point (Action Leaf)", GoToInterestPoint);

        // ----------- ( PATROL BRANCH ) -----------
        BehaviourTree patrolConditionTree = new BehaviourTree();
        Sequence patrolConditions = new Sequence("Patrol Conditions (Sequence)");
        Inverter notSuspicious = new Inverter("Entity Not Suspicious? (Inverter)");
        DependencySequence patrolBranch = new DependencySequence("Patrol Behaviour (Dependancy Sequence)", patrolConditionTree, agent);
        Leaf wanderRandomly = new Leaf("Wander Randomly! (Action Leaf)", WanderRandomly);
        
        // ----------- ( Patrol Branch build ) -----------
        playerNotVisible.AddChild(isPlayerVisible);
        notSuspicious.AddChild(isSuspicious);
        patrolConditions.AddChild(playerNotVisible);
        patrolConditions.AddChild(notSuspicious);
        patrolConditionTree.AddChild(patrolConditions);

        patrolBranch.AddChild(wanderRandomly);

        // ----------- ( Investigate Branch build ) -----------
        investigateConditions.AddChild(playerNotVisible);
        investigateConditionTree.AddChild(investigateConditions);
        investigate.AddChild(goToInterestPoint);
        investigate.AddChild(lookAround);
        senseCheck.AddChild(heardSomething);
        senseCheck.AddChild(sawSomething);
        allowInvestigate.AddChild(isSuspicious);
        allowInvestigate.AddChild(senseCheck);
        investigateBranch.AddChild(allowInvestigate);
        investigateBranch.AddChild(investigate);

        // ----------- ( Stalk Branch build ) -----------
        noNewHotspotPoint.AddChild(newHotspotPoint);
        searchLoopConditions.AddChild(searchPointsLeft);
        searchLoopConditions.AddChild(noNewHotspotPoint);
        searchLoopConditions.AddChild(playerNotVisible);
        searchLoopConditionTree.AddChild(searchLoopConditions);
        stalkConditions.AddChild(playerNotVisible);
        stalkConditionTree.AddChild(stalkConditions);
        searchArea.AddChild(goToPoint);
        searchArea.AddChild(lookAround);
        stalkBranch.AddChild(hasAnnounced);
        stalkBranch.AddChild(runToHotspotPoint);
        stalkBranch.AddChild(generateSearchPoints);
        stalkBranch.AddChild(searchArea);

        // ----------- ( Announce Pursuit Branch build ) -----------
        prePursuitCondition.AddChild(isHighlySuspicious);
        prePursuitCondition.AddChild(playerNotVisible);
        prePursuitCondition.AddChild(chaseNotActive);
        prePursuitCondition.AddChild(hasNotAnnouncedPursuit);
        announcePursuitBranch.AddChild(prePursuitCondition);
        announcePursuitBranch.AddChild(playAnnouncement);
        announcePursuitBranch.AddChild(setHasAnnounced);

        // ----------- ( Chase Branch build ) -----------
        chaseBranch.AddChild(isPlayerVisible);
        chaseBranch.AddChild(setChaseBoolActive);
        chaseBranch.AddChild(runToLastSeenPosition);

        // ----------- ( Kill Branch build ) -----------
        killBranch.AddChild(playerInAttackRange);
        killBranch.AddChild(killPlayer);

        // ----------- ( Final Tree build ) -----------
        entityRoot.AddChild(killBranch);
        //entityRoot.AddChild(chaseBranch);
        //entityRoot.AddChild(announcePursuitBranch);
        //entityRoot.AddChild(stalkBranch);
        //entityRoot.AddChild(investigateBranch);
        //entityRoot.AddChild(patrolBranch);

        tree.AddChild(entityRoot);
        tree.PrintTree();
    }

    // ----------- ( Condition nodes ) -----------
    /*
     *  Checks if the player is visile via the balckboard.
     *  If the player is not visible the chase state bool gets set to false.
     */
    public Node.Status IsPlayerVisible()
    {
        if(Blackboard.Instance.isPlayerVisible)
        {
            return Node.Status.SUCCESS;
        }
        Blackboard.Instance.chaseStateActive = false;
        return Node.Status.FAILURE;
    }

    /*
     * Check if the chase state is bool is not active via the blacboard.
     */
    public Node.Status ChaseNotActive()
    {
        if (!Blackboard.Instance.chaseStateActive)
        {
            return Node.Status.SUCCESS;
        }

        return Node.Status.FAILURE;
    }

    /*
     * Checks if the enemy has announced it's bursuit 
     */
    public Node.Status HasNotAnnouncedPursuit()
    {
        if (!Blackboard.Instance.hasAnnouncedPursuit)
        {
            return Node.Status.SUCCESS;
        }
        return Node.Status.FAILURE;
    }

    public Node.Status HasAnnouncedPursuit()
    {
        if (Blackboard.Instance.hasAnnouncedPursuit)
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

    public Node.Status IsHighlySuspicious()
    {
        if (Blackboard.Instance.isHighlySuspicious)
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
            Blackboard.Instance.hasAnnouncedPursuit = false;
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

    public Node.Status IsNewHotspot()
    {
        if (Blackboard.Instance.CheckIfNewHotspot())
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
     * Setting the chase state active bool to true and then making sure the change was successful
     * before returning success. If the change failed the node returns failure.
     */
    public Node.Status SetChaseBoolActive()
    {
        Blackboard.Instance.chaseStateActive = true;

        if (Blackboard.Instance.chaseStateActive)
        {
            return Node.Status.SUCCESS;
        }

        return Node.Status.FAILURE;
    }

   /*
    * Setting the has announced pirsuit bool to true and then making sure the change was successful
    * before returning success. If the change failed the node returns failure.
    */
    public Node.Status SetHasAnnounced()
    {
        Blackboard.Instance.hasAnnouncedPursuit = true;

        if (Blackboard.Instance.hasAnnouncedPursuit)
        {
            return Node.Status.SUCCESS;
        }

        return Node.Status.FAILURE;
    }

    /*
     *  Running to the players last seen position found in the blackboard.
     *  
     *  Agents color changes to MAGENTA for debugging
     */
    public Node.Status RunToLastSeenPosition()
    {
        rend.material.color = Color.brown;

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
     */
    public Node.Status GenerateSearchPoints() 
    {
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
        rend.material.color = Color.black;

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
