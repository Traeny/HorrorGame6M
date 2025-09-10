using UnityEngine;

public class Selector : Node
{
    public Selector(string n)
    {
        name = n;
    }

    public override Status Process()
    {
        Status childStatus = children[currentChild].Process();

        if (childStatus == Status.RUNNING)
        {
            return Status.RUNNING;
        }

        if(childStatus == Status.SUCCESS)
        {
            currentChild = 0;
            return Status.SUCCESS;
        }

        currentChild++;

        if (currentChild >= children.Count)
        {
            currentChild = 0;
            return Status.FAILURE;
        }

        return Status.RUNNING;
    }
}

// needs only one child to be successful, the first one to be successful gets activated
// goes from first child to last. fails if all childs fail