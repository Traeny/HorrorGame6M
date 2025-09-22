using UnityEngine;
using UnityEngine.AI;

public class HearingSensor : MonoBehaviour
{
    [Header("Debug")]
    public GameObject ears;

    public SuspicionManager suspicionManager;

    public NoiseType noiseType { get; private set; } = NoiseType.Medium;

    public float noiseTime { get; private set; } = 0f;

    public float entryExpirationTime = 3f;

    [SerializeField] string hearingDebug = "";

    public float timeSinceHeardNoise => Time.time - noiseTime;

    private void Start()
    {
        suspicionManager = GetComponentInParent<SuspicionManager>();

        if(suspicionManager == null)
        {
            Debug.LogError("Suspicion manager reference missing in hearing sensor!");
        }
    }

    private void Update()
    {
        UpdateHearing();
    }

    public void OnNoiseHeard(NoiseInfo noise)
    {
        Blackboard.Instance.heardNoise = true;
        ears.SetActive(true); // Debug

        noiseTime = Time.time;
        noiseType = noise.type;

        // This needs to be modified based on the noise intensity
        suspicionManager.AddSuspicion(25f);

        if(NavMesh.SamplePosition(noise.position, out NavMeshHit hit, 4f, NavMesh.AllAreas))
        {
            Blackboard.Instance.lastHeardPosition = hit.position;
            Blackboard.Instance.UpdateInterestPoint(hit.position);
        }
        else // Why do we set the position to be same in both?
        {
            Blackboard.Instance.lastHeardPosition = hit.position;
            Blackboard.Instance.UpdateInterestPoint(hit.position);
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
        ears.SetActive(false); // Debug
    }
}
