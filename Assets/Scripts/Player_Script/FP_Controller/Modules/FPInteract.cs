using TMPro;
using UnityEngine;

namespace Player_Script
{
    public class FPInteract : FPControllerModule
    {
        public Camera mainCamera;
        public float interactionDistance = 2f;

        public GameObject interactionUI;
        public TextMeshProUGUI interactionText;

        private IInteractable currentInteractable;

        void Start()
        {
            controller.TryInteract += OnTryInteract;
        }

        private void Update()
        {
            InteractionRay();
        }

        void OnTryInteract()
        {
            if (currentInteractable != null)
            {
                currentInteractable.Interact();
            }
        }

        void InteractionRay()
        {
            Ray ray = mainCamera.ViewportPointToRay(Vector3.one / 2f);
            RaycastHit hit;
            currentInteractable = null;

            bool hitSomething = false;

            if(Physics.Raycast(ray, out hit, interactionDistance))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();

                if(interactable != null)
                {
                    currentInteractable = interactable;
                    hitSomething = true;
                    interactionText.text = interactable.GetDescription();
                }
            }
            interactionUI.SetActive(hitSomething);
        }
    }
}

