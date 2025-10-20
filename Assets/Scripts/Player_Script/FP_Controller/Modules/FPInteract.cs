using UnityEngine;

namespace Player_Script
{
    public class FPInteract : FPControllerModule
    {
        void Start()
        {
            controller.TryInteract += OnTryInteract;
        }

        private void OnTryInteract()
        {
            // Raycast to the interact layer. It should give info to the script and based on that we decide what to do
        }
    }
}

