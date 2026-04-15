using UnityEngine;
using UnityEngine.AI;

public class HearingSensor : MonoBehaviour
{
    public EnemyPreset preset;

    [Header("Components")]
    public SuspicionManager suspicionManager;
    public NoiseType noiseType { get; private set; } = NoiseType.Medium;
    public float noiseTime { get; private set; } = 0f;
    public float loudNoiseTime { get; private set; } = 0f;
    [SerializeField] string hearingDebug = "";
    public float timeSinceHeardNoise => Time.time - noiseTime;
    private float timeSinceHeardLoudNoise => Time.time - loudNoiseTime;
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
        noiseType = noise.type;

        // LOUD
        if (noise.type == NoiseType.Loud)
        {
            Blackboard.Instance.heardLoudNoise = true;
            loudNoiseTime = Time.time;

            Blackboard.Instance.UpdateHotspotOrigin(noise.position);
            suspicionManager.AddSuspicion(preset.suspicionGainOnLoudNoise);
        }
        // MEDIUM
        else if(noise.type == NoiseType.Medium)
        {
            Blackboard.Instance.heardNoise = true;
            noiseTime = Time.time;

            //Blackboard.Instance.UpdateHotspotOrigin(noise.position);
            suspicionManager.AddSuspicion(preset.suspicionGainOnMediumNoise);
        }
        // SILENT
        else if(noise.type == NoiseType.Silent)
        {
            Blackboard.Instance.heardNoise = true;
            noiseTime = Time.time;

            suspicionManager.AddSuspicion(preset.suspicionGainOnSilentNoise);
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
            //return;
        }

        hearingDebug = $" Type = {noiseType}";

        if(timeSinceHeardNoise >= preset.entryExpirationTime)
        {
            ForgetNoise();
        }

        if(timeSinceHeardLoudNoise >= preset.forgetLoudNoiseTime)
        {
            ForgetLoudNoise();
        }
    }

    public void ForgetNoise()
    {
        Blackboard.Instance.heardNoise = false;
    }

    public void ForgetLoudNoise()
    {
        Blackboard.Instance.heardLoudNoise = false;
    }
}
