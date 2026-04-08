using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/EnemyPreset")]
public class EnemyPreset : ScriptableObject
{
    [Header("Multifunctional")]
    public float delay = 0.1f;

    [Header("Field Of View Module")]
    public float radius = 10f;
    public float angle = 100f;
    public LayerMask targetMask; // Player
    public LayerMask obstructionMask; // FOV block


}
