using UnityEngine;

namespace Player_Script
{
    public class Activity : MonoBehaviour
    {
        public virtual bool Active { get; protected set; } = false;

        #region Static Methods

        public static bool IsActive(Activity activity)
        {
            return activity != null && activity.Active;
        }

        public static bool TryStart(Activity activity)
        {
            return activity != null && activity.TryStartActivety();
        }

        public static bool TryStop(Activity activity)
        {
            return activity != null && activity.TryStopActivety();
        }

        public static bool TryToggle(Activity activity)
        {
            return activity != null && activity.TryToggleActivity();
        }

        #endregion

        public virtual bool CanStartActivity()
        {
            return true;
        }

        protected virtual void StartActivity()
        {
            Active = true;
        }

        public virtual bool CanStopActivety()
        {
            return true;
        }

        protected virtual void StopActivety()
        {
            Active = false;
        }

        public bool TryStartActivety()
        {
            if (!CanStartActivity())
            {
                return false;
            }

            StartActivity();
            return true;
        }

        public bool TryStopActivety()
        {
            if (!CanStopActivety())
            {
                return false;
            }

            StopActivety();
            return true;
        }

        public bool TryToggleActivity()
        {
            if (Active)
            {
                return TryStopActivety();
            }
            return TryStartActivety();
        }
    }

}
