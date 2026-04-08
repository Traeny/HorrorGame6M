using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/EnemyPreset")]
public class EnemyPreset : ScriptableObject
{
    [Header("Multifunctional")]
    public float delay = 0.2f;

    [Header("Field Of View Module")]
    public float fovRadius = 10f;
    public float angle = 100f;
    public LayerMask targetMask; // Player
    public LayerMask obstructionMask; // FOV block 

    [Header("Kill player module")]
    public float attackRange = 2.5f;

    [Header("Hearing Sensor Module")]
    public float entryExpirationTime = 3f;

    [Header("Suspicion Manager Module")]
    public float suspicionDecayRate = 10f;
    public float suspicionThreshold = 100f;
    public float maxSuspicionLevel = 200f;

    [Header("Hotspot Module")]
    public float hotspotRadius = 5f;

}
