using System.Collections;
using UnityEngine;

public class IKLegController : MonoBehaviour
{
    [Header("IK Target")]
    public Transform IKTarget;

    [Header("Raycast")]
    public LayerMask groundLayer;
    public float rayLength = 5f;

    [Header("Step Motion")]
    public float stepHeight = 0.5f;
    public float stepSpeed = 3f;

    bool isStepping;
    bool isLocked;

    Vector3 lockedPosition;
    Quaternion lockedRotation;

    void LateUpdate()
    {
        if (isLocked)
        {
            IKTarget.position = lockedPosition;
            IKTarget.rotation = lockedRotation;
        }
    }

    public void RequestStep()
    {
        if (isStepping)
        {
            return;
        }
            
        UnlockFoot();

        Vector3 start = IKTarget.position;
        Vector3 end = GetFootTarget();

        StartCoroutine(StepArc(start, end));
    }


    Vector3 GetFootTarget()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up;

        if (Physics.Raycast(origin, Vector3.down, out hit, rayLength, groundLayer))
        {
            return hit.point;
        }
            
        return IKTarget.position;
    }

    IEnumerator StepArc(Vector3 start, Vector3 end)
    {
        isStepping = true;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * stepSpeed;

            Vector3 pos = Vector3.Lerp(start, end, t);
            float arc = 4f * t * (1f - t);
            pos.y += arc * stepHeight;

            IKTarget.position = pos;
            yield return null;
        }

        IKTarget.position = end;
        LockFoot();

        isStepping = false;
    }


    public void LockFoot()
    {
        isLocked = true;
        lockedPosition = IKTarget.position;
        lockedRotation = IKTarget.rotation;
    }

    public void UnlockFoot()
    {
        isLocked = false;
    }

}
