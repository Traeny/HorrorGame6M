using UnityEngine;

public class Loop : Node
{
    BehaviourTree dependancy;

    public Loop(string n, BehaviourTree d) 
    {
        name = n;
        dependancy = d;
    }

    public override Status Process()
    {
        if(dependancy.Process() == Status.FAILURE)
        {
            return Status.SUCCESS;
        }

        Status childStatus = children[currentChild].Process();

        if (childStatus == Status.RUNNING)
        {
            return Status.RUNNING;
        }

        if (childStatus == Status.FAILURE)
        {
            currentChild = 0;

            foreach (Node n in children)
            {
                n.Reset();
            }

            return Status.FAILURE;
        }

        currentChild++;

        if (currentChild >= children.Count)
        {
            currentChild = 0;
        }

        return Status.RUNNING;
    }
}
