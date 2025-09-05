using UnityEngine;


namespace Entity_Script
{
    public class EntitySprint : EntityActivityController
    {
        protected override void Awake()
        {
            base.Awake();

            controller.Sprint = this;
        }

        public override bool CanStartActivity()
        {
            // The entity can Sprint if:
            // - He is chasing the player
            // - Heard a sound from far away

            return true;
        }
    }
}

