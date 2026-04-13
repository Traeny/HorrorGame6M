using UnityEditor;
using UnityEngine;

namespace Entity_Script
{
    [CustomEditor(typeof(FocuseVisionCone))]
    public class FocuseVisionConeEditor : Editor
    {
        private void OnSceneGUI()
        {
            FocuseVisionCone fov = (FocuseVisionCone)target;

            Handles.color = Color.white;
            Handles.DrawWireArc(fov.eyeOrigin.position, Vector3.up, Vector3.forward, 360, fov.preset.focuseConeFovRadius);

            Vector3 viewAngle1 = DirectionFromAngle(fov.eyeOrigin.eulerAngles.y, -fov.preset.focuseConeAngle / 2);

            Vector3 viewAngle2 = DirectionFromAngle(fov.eyeOrigin.eulerAngles.y, fov.preset.focuseConeAngle / 2);

            Handles.color = Color.yellow;

            Handles.DrawLine(fov.eyeOrigin.position, fov.eyeOrigin.position + viewAngle1 * fov.preset.focuseConeFovRadius);
            Handles.DrawLine(fov.eyeOrigin.position, fov.eyeOrigin.position + viewAngle2 * fov.preset.focuseConeFovRadius);

            if (Blackboard.Instance.isPlayerVisible)
            {
                Handles.color = Color.green;

                Handles.DrawLine(fov.transform.position, fov.player.transform.position);
            }
        }

        private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
        {
            angleInDegrees += eulerY;

            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }
}

