using UnityEngine;
using System.Collections;

public class RobberBehaviour : BTAgent
{
    /*
    
    public GameObject van;
    public GameObject backdoor;
    public GameObject frontdoor;
    public GameObject cop;

    public GameObject[] art;

    GameObject pickup;

    [Range(0, 1000)]
    public int money = 800;

    Leaf goToBackDoor;
    Leaf goToFrontDoor;

    new void Start()
    {
        base.Start();

        Leaf hasGotMoney = new Leaf("Has Got Money", HasMoney);

        RandomSelector selectObject = new RandomSelector("Select Object to Steal");
        for (int i = 0; i < art.Length; i++)
        {
            Leaf gta = new Leaf("Go to " + art[i].name, i, GoToArt);
            selectObject.AddChild(gta);
        }

        goToBackDoor = new Leaf("Go To Backdoor", GoToBackDoor, 2);
        goToFrontDoor = new Leaf("Go To Frontdoor", GoToFrontDoor, 1);
        Leaf goToVan = new Leaf("Go To Van", GoToVan);
        PrioritySelector opendoor = new PrioritySelector("Open Door");

        Sequence runAway = new Sequence("Run Away");

        // Can See
        Leaf canSee = new Leaf("Can See Cop?", CanSeeCop);
        Leaf flee = new Leaf("Flee From Cop", FleeFromCop);

        Inverter invertMoney = new Inverter("Invert Money");
        invertMoney.AddChild(hasGotMoney);

        opendoor.AddChild(goToFrontDoor);
        opendoor.AddChild(goToBackDoor);

        runAway.AddChild(canSee);
        runAway.AddChild(flee);

        Inverter cantSeeCop = new Inverter("Cant See Cop");
        cantSeeCop.AddChild(canSee);

        Leaf isOpen = new Leaf("IsOpen", IsOpen);
        Inverter isClosed = new Inverter("Is Closed");
        isClosed.AddChild(isOpen);

        BehaviourTree stealConditions = new BehaviourTree();
        Sequence conditions = new Sequence("Stealing conditions");
        conditions.AddChild(isClosed);
        conditions.AddChild(invertMoney);
        conditions.AddChild(cantSeeCop);
        
        
        stealConditions.AddChild(conditions);
        DependencySequence steal = new DependencySequence("Steal Something", stealConditions, agent);
        steal.AddChild(opendoor);
        steal.AddChild(selectObject);
        steal.AddChild(goToVan);

        Selector stealWithFallback = new Selector("Steal With Callback");
        stealWithFallback.AddChild(steal);
        stealWithFallback.AddChild(goToVan);

        Selector beThief = new Selector("Be a thief");
        beThief.AddChild(stealWithFallback);
        beThief.AddChild(runAway);

        tree.AddChild(beThief);

        tree.PrintTree();

        StartCoroutine("DecreaseMoney");
    }

    private IEnumerator DecreaseMoney()
    {
        while (true)
        {
            money = Mathf.Clamp(money - 50, 0, 1000);
            yield return new WaitForSeconds(Random.Range(1, 5));
        }
    }

    public Node.Status CanSeeCop()
    {
        return CanSee(cop.transform.position, "Cop", 10, 80);
    }

    
    public Node.Status FleeFromCop()
    {
        return Flee(cop.transform.position, 10);
    }
    

    public Node.Status HasMoney()
    {
        if (money < 500)
            return Node.Status.FAILURE;
        return Node.Status.SUCCESS;
    }

    public Node.Status GoToArt(int i)
    {
        if (!art[i].activeSelf) return Node.Status.FAILURE;
        Node.Status s = GoToLocation(art[i].transform.position);
        if (s == Node.Status.SUCCESS)
        {
            art[i].transform.parent = this.gameObject.transform;
            pickup = art[i];
        }
        return s;
    }

    
    public Node.Status GoToBackDoor()
    {
        Node.Status s = GoToDoor(backdoor);
        if (s == Node.Status.FAILURE)
            goToBackDoor.sortOrder = 10;
        else
            goToBackDoor.sortOrder = 1;
        return s;
    }
    

    
    public Node.Status GoToFrontDoor()
    {
        Node.Status s = GoToDoor(frontdoor);
        if (s == Node.Status.FAILURE)
            goToFrontDoor.sortOrder = 10;
        else
            goToFrontDoor.sortOrder = 1;
        return s;
    }
    

    
    public Node.Status GoToVan()
    {
        Node.Status s = GoToLocation(van.transform.position);

        if (s == Node.Status.SUCCESS)
        {
            if (pickup != null)
            {
                money += 300;
                pickup.SetActive(false);
                pickup = null;
            }
        }
        return s;
    }

    */
}