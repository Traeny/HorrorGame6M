using Player_Script;
using UnityEngine;

public class InteractableLocker : MonoBehaviour, IInteractable
{
    public GameObject playerLockerPosition;
    public GameObject player;
    


    public string GetDescription()
    {
        return "Get into locker";
    }

    public void Interact()
    {
        CharacterController controller = player.GetComponent<CharacterController>();

        controller.enabled = false;
        player.transform.SetPositionAndRotation(playerLockerPosition.transform.position, playerLockerPosition.transform.rotation);
        controller.enabled = true;

        FPController playerController = player.GetComponent<FPController>();

        playerController.hidingInLocker = true;


        Debug.Log("Interacting!");
        return;
    }
}
