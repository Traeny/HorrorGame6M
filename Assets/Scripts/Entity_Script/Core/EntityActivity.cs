using UnityEngine;

namespace Entity_Script
{
    public class EntityActivity : MonoBehaviour
    {
        public virtual bool EntityActive { get; protected set; } = false;

        #region Static Methods

        public static bool IsActive(EntityActivity activity)
        {
            return activity != null && activity.EntityActive;
        }

        public static bool TryStart(EntityActivity activity)
        {
            return activity != null && activity.TryStartActivity();
        }

        public static bool TryStop(EntityActivity activity)
        {
            return activity != null && activity.TryStopActivity();
        }

        public static bool TryToggle(EntityActivity activity)
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
            EntityActive = true;
        }

        public virtual bool CanStopActivity()
        {
            return true;
        }

        protected virtual void StopActivity()
        {
            EntityActive = false;
        }

        public bool TryStartActivity()
        {
            if (!CanStartActivity())
            {
                return false;
            }

            StartActivity();
            return true;
        }

        public bool TryStopActivity()
        {
            if (!CanStopActivity())
            {
                return false;
            }

            StopActivity();
            return true;
        }

        public bool TryToggleActivity()
        {
            if (EntityActive)
            {
                return TryStopActivity();
            }
            return TryStartActivity();
        }
    }
}

