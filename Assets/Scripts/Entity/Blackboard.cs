using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.AI;

public class Blackboard : MonoBehaviour
{
    static Blackboard instance;

    [Header("Conditions")]
    [Header("Vision")]
    public bool isPlayerVisible = false;
    public bool sawSomething = false;

    [Header("Hearing")]
    public bool heardNoise = false;
    public bool heardLoudNoise = false;

    [Header("Suspicion")]
    public bool isSuspicious = false;
    public bool isHighlySuspicious = false;

    [Header("Attack")]
    public bool playerInAttackRange = false;

    [Header("Location")]
    public bool canReachLocation = false;

    [Header("State info")]
    public bool chaseStateActive = false;
    public bool hasAnnouncedPursuit = false;

    [Header("Position info")]
    public Vector3 interestPoint;
    public Vector3 hotspotOrigin;
    public Vector3 lastHeardPosition;
    public Vector3 lastSeenPosition;
    private Vector3 currentHotspot;

    [Header("Movement Speed")]
    public float moveSpeed;

    [Header("Search point list")]
    public List<Vector3> searchPoints;

    

    

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

    public void UpdateChaseState(bool newState)
    {
        chaseStateActive = newState;
    }

    public void UpdateHasAnnouncedPursuit(bool newState)
    {
        hasAnnouncedPursuit = newState;
    }
}
