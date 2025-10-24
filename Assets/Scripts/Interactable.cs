using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    public string GetDescription()
    {
        return "basic interact";
    }

    public void Interact()
    {
        Debug.Log("Interacting!");
        return;
    }

}
