using UnityEngine;

public class NoiseSystem : MonoBehaviour
{
    [SerializeField]
    LayerMask entityLayer;

    public static NoiseSystem Instance { get; private set; }

    public void MakeNoise(NoiseInfo noise)
    {
        var colliders = Physics.OverlapSphere(noise.position, noise.radius, entityLayer, QueryTriggerInteraction.Ignore);

        foreach (var collider in colliders)
        {
            HearingSensor listener = collider.GetComponentInParent<HearingSensor>();

            listener.OnNoiseHeard(noise);
        }
    }

    private void Awake()
    {
        Instance = this;
    }

}
