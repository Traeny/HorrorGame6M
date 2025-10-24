using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    public string GetDescription()
    {
        return "Get into locker";
    }

    public void Interact()
    {
        Debug.Log("Interacting!");
        return;
    }

}
