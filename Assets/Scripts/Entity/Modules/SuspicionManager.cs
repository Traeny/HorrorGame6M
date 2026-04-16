using UnityEngine;

public class SuspicionManager : MonoBehaviour
{
    public EnemyPreset preset;

    [Header("Debug")]
    public GameObject kysymysmerkki;

    [Range(0, 200)]
    public float suspicionLevel;

    void Update()
    {
        suspicionLevel -= preset.suspicionDecayRate * Time.deltaTime;
        suspicionLevel = Mathf.Clamp(suspicionLevel, 0, preset.maxSuspicionLevel);

        Blackboard.Instance.isSuspicious = suspicionLevel >= preset.suspicionThreshold;
        Blackboard.Instance.isHighlySuspicious = suspicionLevel >= preset.highlySuspiciousTreshold;

        if(suspicionLevel >= preset.suspicionThreshold)
        {
            kysymysmerkki.SetActive(true);
        }
        else
        {
            kysymysmerkki.SetActive(false);
        }
    }

    public void AddSuspicion(float amount)
    {
        suspicionLevel = Mathf.Clamp(suspicionLevel + amount, 0, preset.maxSuspicionLevel);
    }

}
