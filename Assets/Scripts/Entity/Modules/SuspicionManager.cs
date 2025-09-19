using UnityEngine;

public class SuspicionManager : MonoBehaviour
{
    [Header("Debug")]
    public GameObject kysymysmerkki;

    [Range(0, 200)]
    public float suspicionLevel;
    public float suspicionDecayRate = 10f;
    public float suspicionThreshold = 100f;

    void Update()
    {
        suspicionLevel -= suspicionDecayRate * Time.deltaTime;
        suspicionLevel = Mathf.Clamp(suspicionLevel, 0, 200);

        Blackboard.Instance.isSuspicious = suspicionLevel >= suspicionThreshold;

        if(suspicionLevel >= suspicionThreshold)
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
        suspicionLevel = Mathf.Clamp(suspicionLevel + amount, 0, 200);
    }

}
