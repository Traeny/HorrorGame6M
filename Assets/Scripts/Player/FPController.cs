using UnityEngine;
using Unity.Cinemachine;

[RequireComponent(typeof(CharacterController))]
public class FPController : MonoBehaviour
{
    [Header("Movement Parameters")]
    public float maxSpeed = 3.5f;

    [Header("Looking Parameters")]
    public Vector2 lookSensitivity = new Vector2(0.1f, 0.1f);
    public float pitchLimit = 85f;
    [SerializeField] float currentPitch = 0f;

    public float CurrentPitch
    {
        get => currentPitch;

        set
        {
            currentPitch = Mathf.Clamp(value, -pitchLimit, pitchLimit);
        }
    }

    [Header("Inputs")]
    public Vector2 moveInput;
    public Vector2 lookInput;

    [Header("Components")]
    [SerializeField] CharacterController characterController;
    [SerializeField] CinemachineCamera fpCamera;

    private void OnValidate()
    {
        if(characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }
    }

    private void Update()
    {
        MoveUpdate();
        LookUpdate();
    }

    void MoveUpdate()
    {
        Vector3 motion = transform.forward * moveInput.y + transform.right * moveInput.x;
        motion.y = 0f;
        motion.Normalize();

        characterController.Move(motion * maxSpeed * Time.deltaTime);
    }

    void LookUpdate()
    {
        Vector2 input = new Vector2(lookInput.x * lookSensitivity.x, lookInput.y * lookSensitivity.y);

        // looking up and down
        CurrentPitch -= input.y;
        fpCamera.transform.localRotation = Quaternion.Euler(CurrentPitch, 0f, 0f);

        // looking left and right
        transform.Rotate(Vector3.up * input.x);
    }
}
