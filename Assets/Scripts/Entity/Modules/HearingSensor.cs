using UnityEngine;
using UnityEngine.AI;

public class HearingSensor : MonoBehaviour
{
    public Vector3 noisePosition;

    public NoiseType noiseType { get; private set; } = NoiseType.Medium;

    public float noiseTime { get; private set; } = 0f;

    public float entryExpirationTime = 3f;

    [SerializeField] string hearingDebug = "";

    public float timeSinceHeardNoise => Time.time - noiseTime;

    private void Update()
    {
        UpdateHearing();
    }

    public void OnNoiseHeard(NoiseInfo noise)
    {
        Blackboard.Instance.heardNoise = true;

        noiseTime = Time.time;
        noiseType = noise.type;

        if(NavMesh.SamplePosition(noise.position, out NavMeshHit hit, 4f, NavMesh.AllAreas))
        {
            noisePosition = hit.position;
            Blackboard.Instance.lastHeardPosition = hit.position;
        }
        else
        {
            noisePosition = noise.position;
            Blackboard.Instance.lastHeardPosition = noise.position;
        }
    }

    private void UpdateHearing()
    {
        if(Blackboard.Instance.heardNoise == false)
        {
            hearingDebug = "NONE";
            return;
        }

        hearingDebug = $"Heard Noise {Mathf.RoundToInt(timeSinceHeardNoise)} s ago.\n\r";
        hearingDebug = $" Type = {noiseType}";

        if(timeSinceHeardNoise >= entryExpirationTime)
        {
            ForgetNoise();
        }
    }

    public void ForgetNoise()
    {
        Blackboard.Instance.heardNoise = false;
    }
}
