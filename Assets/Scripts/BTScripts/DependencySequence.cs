using UnityEngine;
using UnityEngine.AI;

public class DependencySequence : Node
{
    BehaviourTree dependency;
    NavMeshAgent agent;

    public DependencySequence(string n, BehaviourTree d, NavMeshAgent a)
    {
        name = n;
        dependency = d;
        agent = a;
    }

    public override Status Process()
    {
        Status dependencyStatus = dependency.Process();

        if (dependencyStatus == Status.FAILURE)
        {
            agent.ResetPath();
            foreach (Node n in children)
            {
                n.Reset();
            }
            return Status.FAILURE;
        }
        // Prevent main sequence from running until all dependency nodes are checked
        if (dependencyStatus == Status.RUNNING) 
        { 
            return Status.RUNNING; 
        }

        Status childStatus = children[currentChild].Process();

        if(childStatus == Status.RUNNING)
        {
            return Status.RUNNING;
        }

        if(childStatus == Status.FAILURE)
        {
            return childStatus;
        }

        currentChild++;

        if(currentChild >= children.Count)
        {
            currentChild = 0;
            return Status.SUCCESS;
        }

        return Status.RUNNING;
    }
}
