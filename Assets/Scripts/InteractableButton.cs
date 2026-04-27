using UnityEngine;

public class InteractableButton : MonoBehaviour, IInteractable
{
    [Header("Button Anim")]
    public GameObject button;
    public Transform raisedPosition;
    public Transform LoweredPosition;
    public float buttonMoveSpeed = 1f;
    public float animTime = 1f;
    public bool isDown = false;
    public bool isUp = true;

    [Header("UI")]
    public string buttonNotPressed;
    public string buttonPressed;

    private string informPlayer;

    public DoorManager doorManager;

    public int buttonNumber;

    private void Start()
    {
        informPlayer = buttonNotPressed;
    }


    private void Update()
    {
        if (!isDown && !isUp)
        {
            ButtonDownAimation();
        }

        else if (isDown && !isUp)
        {
            ButtonUpAimation();
        }
    }

    public string GetDescription()
    {
        return informPlayer;
    }

    public void Interact()
    {
        doorManager.UnlockButtonPressed(buttonNumber);
        isUp = false;
    }

    private void ButtonDownAimation()
    {
        button.transform.position =
            Vector3.MoveTowards(
            button.transform.position,
            LoweredPosition.position,
            animTime * Time.deltaTime
        );

        if(button.transform.position == LoweredPosition.position)
        {
            isDown = true;
            informPlayer = buttonPressed;
        }
    }
    private void ButtonUpAimation()
    {
        button.transform.position =
            Vector3.MoveTowards(
            button.transform.position,
            raisedPosition.position,
            animTime * Time.deltaTime
        );

        if (button.transform.position == raisedPosition.position)
        {
            isUp = true;
            isDown = false;
        }
    }
}