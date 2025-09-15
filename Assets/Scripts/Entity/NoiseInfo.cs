using UnityEngine;

public enum NoiseType
{
    Silent,
    Medium,
    Loud
}

public struct NoiseInfo 
{
    public NoiseType type;
    public Vector3 position;
    public float radius;
}
