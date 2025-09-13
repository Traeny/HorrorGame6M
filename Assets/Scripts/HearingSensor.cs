using UnityEngine;

public class HearingSensor : MonoBehaviour
{
    public bool noiseHeard { get; private set; } = false;
    public Vector3 noisePosition { get; private set; }

    public NoiseType noiseType { get; private set; } = NoiseType.Medium;

    public float noiseTime { get; private set; } = 0f;

    public float timeSinceHeardNoise => Time.time - noiseTime;

    public void OnNoiseHeard(NoiseInfo noise)
    {
        // Left at 6min 49s
    }
}
