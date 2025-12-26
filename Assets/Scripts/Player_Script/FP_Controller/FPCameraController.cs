using UnityEngine;

namespace Player_Script
{
    public class FPCameraController : MonoBehaviour
    {
        [SerializeField] Transform cameraTarget;

        FPController controller;

        [SerializeField] float smoothTime = 0.1f;

        private Vector3 smoothDampVelocity;

        private void Awake()
        {
            controller = GetComponentInParent<FPController>();

            if(controller == null ) 
            {
                Debug.Log("FPController not found in hierarcgy.");
            }
        }

        private void Update()
        {
            Vector3 targetPosition = cameraTarget.position + controller.cameraPositionOffset;

            controller.CameraTransform.position = Vector3.SmoothDamp(
                controller.CameraTransform.position,
                targetPosition,
                ref smoothDampVelocity,
                smoothTime);


        }
    }

   
}

