using UnityEngine;
using System.Collections.Generic;

public class DoorManager : MonoBehaviour
{
    public List<GameObject> BwingDoors;
    public List<GameObject> MiddleLabDoors;
    public List<GameObject> ExitDoor;

    public void UnlockButtonPressed(int button)
    {
        if (button == 1)
        {
            UnlockDoors(BwingDoors);
        }
        else if (button == 2) {
            UnlockDoors(MiddleLabDoors);
        }
        else if (button == 3)
        {
            UnlockDoors(ExitDoor);
        }
    }

    public void UnlockDoors(List<GameObject> doors)
    {
        foreach (GameObject door in doors) {
            
            AutomaticDoor doorScript = door.GetComponent<AutomaticDoor>();

            if(doorScript != null)
            {
                doorScript.doorLocked = false;
            }

        }
    }
}
