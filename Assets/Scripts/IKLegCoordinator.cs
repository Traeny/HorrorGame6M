using UnityEngine;

public class IKLegCoordinator : MonoBehaviour
{
    [Header("Leg References")]
    public IKLegController leftLeg;
    public IKLegController rightLeg;

    [Header("Rotation Stepping")]
    public float rotationStepThreshold = 25f;

    float accumulatedYaw;
    float lastYaw;
    bool leftNext = true;

    void Start()
    {
        lastYaw = transform.eulerAngles.y;
    }

    void Update()
    {
        float currentYaw = transform.eulerAngles.y;
        float deltaYaw = Mathf.Abs(Mathf.DeltaAngle(lastYaw, currentYaw));

        accumulatedYaw += deltaYaw;
        lastYaw = currentYaw;

        if (accumulatedYaw >= rotationStepThreshold)
        {
            if (leftNext)
            {
                rightLeg.LockFoot();
                leftLeg.RequestStep();
            }
            else
            {
                leftLeg.LockFoot();
                rightLeg.RequestStep();
            }

        leftNext = !leftNext;
        accumulatedYaw = 0f;
        }
    }
}
