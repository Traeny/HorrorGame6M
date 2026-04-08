using UnityEngine;
using UnityEngine.AI;

public class HearingSensor : MonoBehaviour
{
    public EnemyPreset preset;

    [Header("Debug")]
    public GameObject ears;
    public float entryExpirationTime = 3f;

    [Header("Components")]
    public SuspicionManager suspicionManager;
    public NoiseType noiseType { get; private set; } = NoiseType.Medium;
    public float noiseTime { get; private set; } = 0f;

    [SerializeField] string hearingDebug = "";

    public float timeSinceHeardNoise => Time.time - noiseTime;

    private float timer = 0f;

    private void Start()
    {
        suspicionManager = GetComponentInParent<SuspicionManager>();

        if(suspicionManager == null)
        {
            Debug.Log("Suspicion manager reference missing in hearing sensor!");
            return;
        }
        timer = preset.delay;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            UpdateHearing();
            timer = preset.delay;
        }
    }

    public void OnNoiseHeard(NoiseInfo noise)
    {
        Blackboard.Instance.heardNoise = true;
        ears.SetActive(true);

        noiseTime = Time.time;
        noiseType = noise.type;

        if (noise.type == NoiseType.Loud)
        {
            Blackboard.Instance.UpdateHotspotOrigin(noise.position);
            suspicionManager.AddSuspicion(100f);
        }
        else if(noise.type == NoiseType.Medium)
        {
            Blackboard.Instance.UpdateHotspotOrigin(noise.position);
            suspicionManager.AddSuspicion(50f);
        }
        else if(noise.type == NoiseType.Silent)
        {
            suspicionManager.AddSuspicion(10f);
        }


        if(NavMesh.SamplePosition(noise.position, out NavMeshHit hit, 4f, NavMesh.AllAreas))
        {
            Blackboard.Instance.lastHeardPosition = hit.position;
            Blackboard.Instance.UpdateInterestPoint(hit.position);
        }
        else
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

        hearingDebug = $" Type = {noiseType}";

        if(timeSinceHeardNoise >= preset.entryExpirationTime)
        {
            ForgetNoise();
        }
    }

    public void ForgetNoise()
    {
        Blackboard.Instance.heardNoise = false;
        ears.SetActive(false);
    }
}
