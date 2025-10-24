using Player_Script;
using UnityEngine;

public class InteractableLockerDoor : MonoBehaviour, IInteractable
{
    public GameObject playerLockerExitPosition;
    public GameObject player;

    public string GetDescription()
    {
        return "Leave locker";
    }

    public void Interact()
    {
        CharacterController controller = player.GetComponent<CharacterController>();

        controller.enabled = false;
        player.transform.SetPositionAndRotation(playerLockerExitPosition.transform.position, playerLockerExitPosition.transform.rotation);
        controller.enabled = true;

        FPController playerController = player.GetComponent<FPController>();

        playerController.hidingInLocker = false;


        Debug.Log("Interacting!");
        return;
    }
}
