using Player_Script;
using UnityEngine;

public class InteractableLocker : MonoBehaviour, IInteractable
{
    public GameObject playerLockerPosition;
    // Teipillä ja rakkausdella
    public GameObject player;

    [SerializeField] Transform outsidePos;
    [SerializeField] Transform insidePos;
    [SerializeField] Transform outsideHandPos;
    [SerializeField] Transform insideHandPos;


    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    public string GetDescription()
    {
        return "Get into locker";
    }

    public void Interact()
    {
        CharacterController controller = player.GetComponent<CharacterController>();
        FPController playerController = player.GetComponent<FPController>();

        controller.enabled = false;
        
        // Replace this logic
        player.transform.SetPositionAndRotation(playerLockerPosition.transform.position, playerLockerPosition.transform.rotation);

        // The animation needs the outside and inside locker position as well ad the inside and outside door hand positions
        // These will be used as targets gor the IKs

        //var playerAnimationScript = player.GetComponentInChildren<FPFullBodyAnimator>();

        // 1. We call a script that has the logic for enter locker animation
        //StartCoroutine(playerAnimationScript.EnterLockerAnimation(outsidePos, outsideHandPos, insidePos, insideHandPos));

        // After the loop has been finished we can move to the camera animation
        controller.enabled = true;

        playerController.hidingInLocker = true;
    }
}
