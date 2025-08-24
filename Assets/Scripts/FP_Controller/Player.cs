using UnityEngine.InputSystem;
using UnityEngine;

namespace Player_Script
{
    [RequireComponent(typeof(FPController))]
    public class Player : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] FPController fpController;

        #region Input Handling

        private void OnMove(InputValue value)
        {
            fpController.moveInput = value.Get<Vector2>();
        }

        private void OnLook(InputValue value)
        {
            fpController.lookInput = value.Get<Vector2>();
        }

        private void OnSprint(InputValue value)
        {
            fpController.sprintInput = value.isPressed;
        }

        private void OnJump(InputValue value)
        {
            if (value.isPressed)
            {
                fpController.TryJump?.Invoke();
            }
        }

        void OnCrouch(InputValue value)
        {
            if (value.isPressed)
            {
                Activity.TryToggle(fpController.Crouch);
            }
        }

        #endregion

        #region Unity Methods
        private void OnValidate()
        {
            if (fpController == null) fpController = GetComponent<FPController>();
        }

        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        #endregion
    }
}

