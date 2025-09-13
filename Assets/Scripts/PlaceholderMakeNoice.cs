using UnityEngine;
using UnityEngine.InputSystem;

public class PlaceholderMakeNoice : MonoBehaviour
{
    void OnMakeNoise(InputValue input)
    {
        MakeNoise(new NoiseInfo
        {
            position = gameObject.transform.position,
            radius = 10f,
            type = NoiseType.Medium
        });
    }

    public void MakeNoise(NoiseInfo noiseInfo)
    {
        NoiseSystem.Instance?.MakeNoise(noiseInfo);
    }
}
