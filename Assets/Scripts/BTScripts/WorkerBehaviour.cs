using UnityEngine;

public class WorkerBehaviour : BTAgent
{
    /*
    public GameObject office;
    [SerializeField] GameObject patron;

    new void Start()
    {
        base.Start();

        Leaf goToPatron = new Leaf(name + "Go To patron", GoToPatron);
        Leaf goToOffice = new Leaf(name + "Go To Office", GoToOffice);
        Leaf allocatePatron = new Leaf(name + "Allocate Patron", AllocatePatron);
        Leaf patronStillWaiting = new Leaf(name + "Is patron waiting", PatronWaiting);

        Sequence getPatron = new Sequence(name + "Find a patron");
        getPatron.AddChild(allocatePatron);

        BehaviourTree waiting = new BehaviourTree();
        waiting.AddChild(patronStillWaiting);

        DependencySequence moveToPatron = new DependencySequence(name + "Moving to patron", waiting, agent);
        moveToPatron.AddChild(goToPatron);

        getPatron.AddChild(moveToPatron);

        Selector beWorker = new Selector(name + "Be Worker");

        beWorker.AddChild(getPatron);
        beWorker.AddChild(goToOffice);

        tree.AddChild(beWorker);
    }
    
    public Node.Status PatronWaiting()
    {
        if(patron == null)
        {
            return Node.Status.FAILURE;
        }

        if (patron.GetComponent<PatronBehaviour>().isWaiting)
        {
            return Node.Status.SUCCESS;
        }

        return Node.Status.FAILURE;
    }

    public Node.Status GoToPatron()
    {
        if(patron == null)
        {
            return Node.Status.FAILURE;
        }

        Node.Status s = GoToLocation(patron.transform.position);

        if (s == Node.Status.SUCCESS)
        {
            patron.GetComponent<PatronBehaviour>().ticket = true;

            patron = null;
        }

        return s;
    }

    public Node.Status AllocatePatron()
    {
        if (Blackboard.Instance.patrons.Count == 0)
        {
            return Node.Status.FAILURE;
        }

        patron = Blackboard.Instance.patrons.Pop();

        if(patron == null)
        {
            return Node.Status.FAILURE;
        }

        return Node.Status.SUCCESS;
    }

    public Node.Status GoToOffice()
    {
        Node.Status s = GoToLocation(office.transform.position);
        patron = null;
        return s;
    }
    */
}