using Player_Script;
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
            // Here you can type down when the enetity can start sprinting
            // When noticed player, sound is far or loud enough, etc
            return true;
        }
    }
}

