using Player_Script;
using UnityEngine;

public class InteractableLockerDoor : MonoBehaviour, IInteractable
{
    public GameObject playerLockerExitPosition;
    public GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    public string GetDescription()
    {
        return "Leave locker";
    }

    public void Interact()
    {
        // We get the player character controller reference
        CharacterController controller = player.GetComponent<CharacterController>();

        //We get the players FPController scripts reference
        FPController playerController = player.GetComponent<FPController>();

        // Here we disable the character controller
        controller.enabled = false;

        // Then we move the player to a specific position and rotation
        player.transform.SetPositionAndRotation(playerLockerExitPosition.transform.position, playerLockerExitPosition.transform.rotation);

        // After moving the player we enable the character controller
        controller.enabled = true;

        // After the player has exited the locker we set the hiding bool to false
        playerController.hidingInLocker = false;
    }
}
