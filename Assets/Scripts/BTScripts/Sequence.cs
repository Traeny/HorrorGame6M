using UnityEngine;

public class Sequence : Node
{
    public Sequence(string n)
    {
        name = n;
    }

    public override Status Process()
    {
        //Debug.Log("Sequence: " + name + " " + currentChild);

        Status childStatus = children[currentChild].Process();

        if(childStatus == Status.RUNNING)
        {
            return Status.RUNNING;
        }

        if(childStatus == Status.FAILURE)
        {
            currentChild = 0;
            
            foreach(Node n in children)
            {
                n.Reset();
            }

            return Status.FAILURE;
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

// can have multiple children. All leafs need to return success. runs first child to last