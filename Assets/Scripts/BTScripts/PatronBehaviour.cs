using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PatronBehaviour : BTAgent
{
 /*   
    public GameObject[] art;
    public GameObject frontDoor;
    public GameObject homeBase;

    [Range(0, 1000)]
    public int boredom = 0;

    public bool ticket = false;
    public bool isWaiting = false;

    new void Start()
    {
        base.Start();

        RandomSelector selectObject = new RandomSelector(name + "Select Art to View");
        
        for (int i = 0; i < art.Length; i++)
        {
            Leaf gta = new Leaf(name + "Go to " + art[i].name, i, GoToArt);
            selectObject.AddChild(gta);
        }

        Leaf goToFrontDoor = new Leaf(name + "Go To Front Door", GoToFrontDoor);
        Leaf goToHome = new Leaf(name + "Go To Home", GoToHome);
        Leaf isBored = new Leaf(name + "Is Bored", IsBored);
        Leaf isOpen = new Leaf(name + "Is Open", IsOpen);

        Sequence viewArt = new Sequence(name + "View Art");
        viewArt.AddChild(isOpen);
        viewArt.AddChild(isBored);
        viewArt.AddChild(goToFrontDoor);

        Leaf noTicket = new Leaf(name + "Wait fot Ticket", NoTicket);
        Leaf isWaiting = new Leaf(name + "Waiting for worker", IsWaiting);

        BehaviourTree waitForTicket = new BehaviourTree();
        waitForTicket.AddChild(noTicket);

        Loop getTicket = new Loop(name + "Get ticket", waitForTicket);
        getTicket.AddChild(isWaiting);

        viewArt.AddChild(getTicket);

        BehaviourTree whileBored = new BehaviourTree();
        whileBored.AddChild(isBored);

        Loop lookAtPaintings = new Loop(name + "Look", whileBored);
        lookAtPaintings.AddChild(selectObject);

        viewArt.AddChild(lookAtPaintings);

        viewArt.AddChild(goToHome);

        BehaviourTree galleryOpenCondition = new BehaviourTree();
        galleryOpenCondition.AddChild(isOpen);

        DependencySequence bePatron = new DependencySequence(name + "Be An Art Patron", galleryOpenCondition, agent);
        bePatron.AddChild(viewArt);

        Selector viewArtWithFallback = new Selector(name + "View Art With Fallback");
        viewArtWithFallback.AddChild(bePatron);
        viewArtWithFallback.AddChild(goToHome);

        tree.AddChild(viewArtWithFallback);

        StartCoroutine("IncreaseBoredom");
    }

    private IEnumerator IncreaseBoredom()
    {
        while (true)
        {
            boredom = Mathf.Clamp(boredom + 20, 0, 1000);
            yield return new WaitForSeconds(Random.Range(1, 5));
        }
    }

    public Node.Status GoToArt(int i)
    {
        if (!art[i].activeSelf)
        {
            return Node.Status.FAILURE;
        }
            
        Node.Status s = GoToLocation(art[i].transform.position);

        if(s == Node.Status.SUCCESS)
        {
            boredom = Mathf.Clamp(boredom - 150, 0, 1000);
        }

        return s;
    }

    public Node.Status GoToFrontDoor()
    {
        Node.Status s = GoToDoor(frontDoor);
        return s;
    }

    public Node.Status GoToHome()
    {
        Node.Status s = GoToLocation(homeBase.transform.position);
        isWaiting = false;
        return s;
    }

    public Node.Status IsBored()
    {
        if(boredom < 100)
        {
            return Node.Status.FAILURE;
        }
        return Node.Status.SUCCESS;
    }

    public Node.Status NoTicket()
    {
        if(ticket || IsOpen() == Node.Status.FAILURE)
        {
            return Node.Status.FAILURE;
        }
        else
        {

            return Node.Status.SUCCESS;
        }
    }

    public Node.Status IsWaiting()
    {
        // Could add some logic that checks if the patron is already qued 
        // causing some bugs
        if(Blackboard.Instance.RegisteredPatron(this.gameObject))
        {
            isWaiting = true;
            return Node.Status.SUCCESS;
        }
        else
        {
            return Node.Status.FAILURE;
        }
    }
    */
}