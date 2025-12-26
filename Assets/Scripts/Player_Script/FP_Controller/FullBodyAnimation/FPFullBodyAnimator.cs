using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Player_Script
{
    public class FPFullBodyAnimator : MonoBehaviour
    {
        [SerializeField] FPController controller;
        [SerializeField] Animator animator;
        [SerializeField] Rig leftLegRig;
        [SerializeField] Rig rightLegRig;

        private void OnValidate()
        {
            if(animator == null)
            {
                animator = GetComponent<Animator>();
            }

            if(controller == null)
            {
                controller = GetComponent<FPController>();
            }
        }

        private void Start()
        {
            controller.Jumped.AddListener(() =>
            {
                animator.SetBool("Jump", true);
            });

            controller.Landed.AddListener(() =>
            {
                animator.SetBool("Jump", false);
            });
        }

        private void Update()
        {
            Vector3 velocity = controller.currentVelocity;

            float forwardSpeed = Vector3.Dot(velocity, transform.forward);
            float strafeSpeed = Vector3.Dot(velocity, transform.right);

            animator.SetFloat("Speed", forwardSpeed);
            animator.SetFloat("StrafeSpeed", strafeSpeed);

            animator.SetBool("Crouch", Activity.IsActive(controller.Crouch));

            animator.SetBool("Fall", controller.verticalVelocity <= -0.1f && !controller.Grounded);

            if(!Activity.IsActive(controller.Crouch) && controller.currentVelocity.sqrMagnitude > 0.01f)
            {
                leftLegRig.weight = 0f;
                rightLegRig.weight = 0f;
            }
            else
            {
                leftLegRig.weight = 1f;
                rightLegRig.weight = 1f;
            }

        }
    }
}


