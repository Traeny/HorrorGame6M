using UnityEngine;
using UnityEditor;

namespace Entity_Script
{
    [CustomEditor(typeof(MainVisionCone))]
    public class MainVisionConeEditor : Editor
    {
        
        public EnemyPreset preset;

        private void OnSceneGUI()
        {
            MainVisionCone fov = (MainVisionCone)target;

            Handles.color = Color.white;
            Handles.DrawWireArc(fov.eyeOrigin.position, Vector3.up, Vector3.forward, 360, fov.preset.mainConeFovRadius);

            Vector3 viewAngle1 = DirectionFromAngle(fov.eyeOrigin.eulerAngles.y, -fov.preset.mainConeAngle / 2);

            Vector3 viewAngle2 = DirectionFromAngle(fov.eyeOrigin.eulerAngles.y, fov.preset.mainConeAngle / 2);

            Handles.color = Color.blue;

            Handles.DrawLine(fov.eyeOrigin.position, fov.eyeOrigin.position + viewAngle1 * fov.preset.mainConeFovRadius);
            Handles.DrawLine(fov.eyeOrigin.position, fov.eyeOrigin.position + viewAngle2 * fov.preset.mainConeFovRadius);

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

