using UnityEngine;

namespace Entity_Script
{
    public class EntityPatrol : EntityActivityController
    {
        protected override void Awake()
        {
            base.Awake();

            controller.Patrol = this;
        }
        public override bool CanStartActivity()
        {
            // The entity can patrol if:
            // - There isn't any points of inteterst to investigate
            // - The entitiy doesn't have signs of the players location

            // - Bonus* We aren't idling on the patrol position

            return true;
        }

        private void Update()
        {
            // Try to start activity

            // Try to stop activity
        }
    }
}

