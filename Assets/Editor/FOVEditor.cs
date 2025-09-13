using UnityEngine;
using UnityEditor;

namespace Entity_Script
{
    [CustomEditor(typeof(FieldOfView))]
    public class FOVEditor : Editor
    {
        private void OnSceneGUI()
        {
            FieldOfView fov = (FieldOfView)target;

            Handles.color = Color.white;
            Handles.DrawWireArc(fov.eyeOrigin.position, Vector3.up, Vector3.forward, 360, fov.radius);

            Vector3 viewAngle1 = DirectionFromAngle(fov.eyeOrigin.eulerAngles.y, -fov.angle / 2);

            Vector3 viewAngle2 = DirectionFromAngle(fov.eyeOrigin.eulerAngles.y, fov.angle / 2);

            Handles.color = Color.yellow;

            Handles.DrawLine(fov.eyeOrigin.position, fov.eyeOrigin.position + viewAngle1 * fov.radius);
            Handles.DrawLine(fov.eyeOrigin.position, fov.eyeOrigin.position + viewAngle2 * fov.radius);

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

