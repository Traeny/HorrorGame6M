using Entity_Script;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerInAttackRange))]
public class PlayerInAttackRangeEditor : Editor
{
    private void OnSceneGUI()
    {
        PlayerInAttackRange fov = (PlayerInAttackRange)target;

        Handles.color = Color.red;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.attackRange);
    }

}
