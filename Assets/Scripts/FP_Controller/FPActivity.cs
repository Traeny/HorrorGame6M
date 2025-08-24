using UnityEngine;

namespace Player_Script
{
    public class FPActivity : Activity
    {
        protected FPController controller;
        protected FPControllerPreset preset;

        protected virtual void Awake()
        {
            controller = GetComponentInParent<FPController>();

            if (controller == null)
            {
                Debug.LogError($"{GetType().Name} could not find FPController in parent of {gameObject.name}");
            }
        }

    }
}

