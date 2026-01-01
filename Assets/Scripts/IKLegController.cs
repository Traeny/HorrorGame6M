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

    public bool isStepping;

    [Header("Body Reference")]
    public Transform body;


    public void RequestStep()
    {
        if (isStepping)
        {
            return;
        }

        Vector3 startPos = IKTarget.position;
        Quaternion startRot = IKTarget.transform.rotation;

        RaycastHit hit;
        Vector3 endPos = startPos;
        Quaternion endRot = startRot;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayLength, groundLayer))
        {
            endPos = hit.point;
            endRot = Quaternion.LookRotation(body.forward);
        }

        StartCoroutine(StepArc(startPos, endPos, startRot, endRot));
    }

    IEnumerator StepArc(
        Vector3 startPos,
        Vector3 endPos,
        Quaternion startRot,
        Quaternion endRot)
    {
        isStepping = true;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * stepSpeed;

            Vector3 pos = Vector3.Lerp(startPos, endPos, t);
            float arc = 4f * t * (1f - t);
            pos.y += arc * stepHeight;

            IKTarget.position = pos;
            IKTarget.transform.rotation = Quaternion.Slerp(startRot, endRot, t);

            yield return null;
        }

        IKTarget.position = endPos;
        IKTarget.transform.rotation = endRot;

        isStepping = false;
    }
}
