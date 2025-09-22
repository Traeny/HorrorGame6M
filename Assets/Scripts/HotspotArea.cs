using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HotspotArea : MonoBehaviour
{
    public float radius = 5f; // This could be randomized a bit
    public int pointCount = 3;
    public Vector3 origin = Vector3.zero;

    private List<Vector3> points = new List<Vector3>();

    public List<Vector3> GetRandomPoints(int count, Vector3 newOrigin)
    {
        origin = newOrigin;

        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < count; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection.y = 0f;

            Vector3 randomPoint = newOrigin + randomDirection;

            NavMeshHit hit;
            Vector3 finalPosition = newOrigin;

            if (NavMesh.SamplePosition(randomPoint, out hit, 2f, NavMesh.AllAreas))
            {
                finalPosition = hit.position;
            }

            points.Add(finalPosition);
        }

        Blackboard.Instance.SetSearchPointList(points);
        return points;
    }

    // ---------- ( Debugging ) -----------
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        DrawCircle(origin, radius, 50);

        Gizmos.color = Color.red;
        foreach (var p in points)
        {
            Gizmos.DrawSphere(p, 0.2f);
        }
    }

    private void DrawCircle(Vector3 center, float radius, int segments)
    {
        float angle = 0f;
        Vector3 prevPoint = center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;

        for (int i = 1; i <= segments; i++)
        {
            angle = i * 2 * Mathf.PI / segments;
            Vector3 nextPoint = center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }
}
