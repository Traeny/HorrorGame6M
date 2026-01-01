using System.Collections;
using UnityEngine;

public class IKLegCoordinator : MonoBehaviour
{
    [Header("Leg References")]
    public IKLegController leftLeg;
    public IKLegController rightLeg;

    [Header("Rotation Stepping")]
    public float rotationStepThreshold = 25f;

    float accumulatedYaw;
    public float lastYaw;
    bool leftNext = true;

    bool isSteppingSequenceRunning;

    void Start()
    {
        lastYaw = transform.eulerAngles.y;
    }

    void Update()
    {
        float currentYaw = transform.eulerAngles.y;
        float deltaYaw = Mathf.Abs(Mathf.DeltaAngle(lastYaw, currentYaw));

        if (deltaYaw >= rotationStepThreshold && !isSteppingSequenceRunning)
        {
            lastYaw = currentYaw;
            StartCoroutine(StepSequence());
        }
    }

    IEnumerator StepSequence()
    {
        isSteppingSequenceRunning = true;

        if (leftNext)
        {
            leftLeg.RequestStep();
            yield return new WaitUntil(() => !leftLeg.isStepping);

            rightLeg.RequestStep();
            yield return new WaitUntil(() => !rightLeg.isStepping);
        }
        else
        {
            rightLeg.RequestStep();
            yield return new WaitUntil(() => !rightLeg.isStepping);

            leftLeg.RequestStep();
            yield return new WaitUntil(() => !leftLeg.isStepping);
        }

        leftNext = !leftNext;
        isSteppingSequenceRunning = false;
    }
}
