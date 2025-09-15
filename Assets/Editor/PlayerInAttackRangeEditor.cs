using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerInAttackRange))]
public class PlayerInAttackRangeEditor : Editor
{
    private void OnSceneGUI()
    {
        PlayerInAttackRange entity = (PlayerInAttackRange)target;

        Handles.color = Color.red;
        Handles.DrawWireArc(entity.transform.position, Vector3.up, Vector3.forward, 360, entity.attackRange);
    }
}
