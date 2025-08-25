using UnityEngine;

namespace Player_Script
{
    public class FPCameraLand : FPActivity
    {
        enum EState
        {
            Land,
            Recovery
        }

        EState state = EState.Land;
        private float stateTimer = 0f;

        protected override void Awake()
        {
            base.Awake();

            controller.Landed.AddListener(OnLanded);
        }

        private void Update()
        {
            stateTimer += Time.deltaTime;

            if (Active)
            {
                if(state == EState.Land && stateTimer >= 0.2f)
                {
                    ChangeState(EState.Recovery);
                }
                else if(state == EState.Recovery && stateTimer >= 0.3f)
                {
                    TryStop(this);
                }
            }
        }

        protected override void StopActivity()
        {
            controller.cameraPositionOffset = Vector3.zero;
        }

        private void OnLanded()
        {
            TryStart(this);
            ChangeState(EState.Land);
        }

        private void ChangeState(EState newState)
        {
            state = newState;

            stateTimer = 0f;

            if(state == EState.Land)
            {
                controller.cameraPositionOffset = new Vector3(0f, -2f, 0f);
            }
        }
    }
}

