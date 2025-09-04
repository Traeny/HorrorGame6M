using UnityEngine;

namespace Entity_Script
{
    public class EntityActivityController : EntityActivity
    {
        protected EntityController controller;
        protected EntityControllerPreset preset;

        protected virtual void Awake()
        {
            if (controller == null)
            {
                Debug.LogError($"{GetType().Name} could not find EntityController in {gameObject.name}");
            }

            preset = controller.preset;

            if (preset == null)
            {
                Debug.LogError($"{GetType().Name} could not find EntityControllerPreset on {controller.name}");
            }

        }
    }
}

