using UnityEngine;

public class CopBehaviour : BTAgent
{/*
    public GameObject[] patrolPoints;
    public GameObject robber;

    new void Start()
    {
        base.Start();

        Sequence selectPatrolPoint = new Sequence("Select patrol point");

        for(int i = 0; i < patrolPoints.Length; i++)
        {
            Leaf patrolPoint = new Leaf("Go To " + patrolPoints[i].name, i, GoToPoint);

            selectPatrolPoint.AddChild(patrolPoint);
        }

        Sequence chaseRobber = new Sequence("Chase Robber");

        Leaf canSee = new Leaf("Can See Robber", canSeeRobber);
        Leaf chase = new Leaf("Chase Robber", ChaseRobber);

        chaseRobber.AddChild(canSee);
        chaseRobber.AddChild(chase);

        Inverter cantSeeRobber = new Inverter("Can't see robber");
        cantSeeRobber.AddChild(canSee);

        BehaviourTree patrolConditions = new BehaviourTree();

        Sequence conditions = new Sequence("Patrol Conditions");

        conditions.AddChild(cantSeeRobber);

        patrolConditions.AddChild(conditions);

        DependencySequence patrol = 
            new DependencySequence("Patrol until", patrolConditions, agent);

        patrol.AddChild(selectPatrolPoint);

        Selector beCop = new Selector("Be a Cop");

        beCop.AddChild(patrol);
        beCop.AddChild(chase);

        tree.AddChild(beCop);
    }

    public Node.Status GoToPoint(int i)
    {
        Node.Status s = GoToLocation(patrolPoints[i].transform.position);

        return s;
    }

    public Node.Status canSeeRobber()
    {
        return CanSee(robber.transform.position, "Robber", 5, 60);
    }

    Vector3 rememberLocation;
    public Node.Status ChaseRobber()
    {
        float chaseDistance = 10f;

        if(state == ActionState.IDLE)
        {
            rememberLocation = 
                this.transform.position - 
                (transform.position - robber.transform.position).normalized * 
                chaseDistance;   
        }
        return GoToLocation(rememberLocation);
    }
    */
}
