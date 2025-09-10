using UnityEngine;

public class RandomSelector : Node
{
    bool shuffled = false;

    public RandomSelector(string n)
    {
        name = n;
    }

    public override Status Process()
    {
        if (!shuffled)
        {
            // Shuffle comes from Utils
            children.Shuffle();
            shuffled = true;
        }

        Status childStatus = children[currentChild].Process();

        if (childStatus == Status.RUNNING)
        {
            return Status.RUNNING;
        }

        if (childStatus == Status.SUCCESS)
        {
            currentChild = 0;
            shuffled = false;
            return Status.SUCCESS;
        }
        currentChild++;

        if (currentChild >= children.Count)
        {
            currentChild = 0;
            shuffled = false;
            return Status.FAILURE;
        }

        return Status.RUNNING;
    }
}