using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HearingSensor))]
public class HearingSensorEditor : Editor
{
    private void OnSceneGUI()
    {
        if (Blackboard.Instance.heardNoise == false)
        {
            return;
        }

        Handles.color = Color.magenta;
        Handles.SphereHandleCap(0, Blackboard.Instance.lastHeardPosition, Quaternion.identity, 0.2f, EventType.Repaint);
    }
}
