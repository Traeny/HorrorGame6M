using System.Collections.Generic;
using UnityEngine;

public class Blackboard : MonoBehaviour
{
    static Blackboard instance;

    [Header("Conditions")]
    public bool isPlayerVisible = false;
    public bool playerInAttackRange = false;
    public bool canReachLocation = false;
    public bool heardNoise = false;
    public bool isSuspicious = false;

    [Header("Position info")]
    public Vector3 interestPoint;
    public Vector3 hotspotOrigin;
    public List<Vector3> searchPoints;
    public Vector3 lastHeardPosition;
    public Vector3 lastSeenPosition;
    private Vector3 currentHotspot;

    [Header("Movement Speed")]
    public float moveSpeed;

    public static Blackboard Instance
    {
        get
        {
            if (!instance)
            {
                Blackboard[] blackboards = Object.FindObjectsByType<Blackboard>(FindObjectsSortMode.None);
                if(blackboards != null)
                {
                    if(blackboards.Length == 1)
                    {
                        instance = blackboards[0];
                        return instance;
                    }
                }
                GameObject go = new GameObject("Blackboard", typeof(Blackboard));
                instance = go.GetComponent<Blackboard>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }

        set
        {
            instance = value as Blackboard;
        }
    }

    public void UpdateInterestPoint(Vector3 position)
    {
        interestPoint = position;
    }

    public void UpdateHotspotOrigin(Vector3 newPosition)
    {
        hotspotOrigin = newPosition;
    }

    public Vector3 GetHotsopotLocation()
    {
        return hotspotOrigin;
    }

    public void SetSearchPointList(List<Vector3> newpoints)
    {
        searchPoints = newpoints;
    }

    public bool CheckIfNewHotspot()
    {
        if(hotspotOrigin != currentHotspot)
        {
            return true;
        }

        return false;
    }

    public void SetCurrentHotspot()
    {
        currentHotspot = hotspotOrigin;
    }

    public void UpdateMovementSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }
}
