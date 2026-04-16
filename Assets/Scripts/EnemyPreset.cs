using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/EnemyPreset")]
public class EnemyPreset : ScriptableObject
{
    [Header("Multifunctional")]
    public float delay = 0.2f;
    public LayerMask targetMask; // Player
    public LayerMask obstructionMask; // FOV block 

    [Header("Movement")]
    public float walkSpeed = 3.5f;
    public float runSpeed = 5f;

    [Header("Vision")]
    [Header("Main Cone")]
    public float mainConeFovRadius = 10f;
    public float mainConeAngle = 90f;
    public float forgetClimpseTime = 3f;

    [Header("Focus Cone")]
    public float focuseConeFovRadius = 15f;
    public float focuseConeAngle = 150f;

    [Header("Kill player module")]
    public float attackRange = 2.5f;

    [Header("Hearing Sensor Module")]
    public float entryExpirationTime = 3f;
    public float forgetLoudNoiseTime = 5f;

    public float suspicionGainOnLoudNoise = 100f;
    public float suspicionGainOnMediumNoise = 50f;
    public float suspicionGainOnSilentNoise = 10f;

    [Header("Suspicion Manager Module")]
    public float suspicionDecayRate = 10f;
    public float suspicionThreshold = 100f;
    public float highlySuspiciousTreshold = 150f;
    public float maxSuspicionLevel = 200f;

    [Header("Hotspot Module")]
    public float hotspotRadius = 5f;
    public int searchPointAmount = 3;

}
