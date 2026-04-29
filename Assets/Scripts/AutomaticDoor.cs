using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{
    [Header("Door Animation")]
    public float doorAnimSpeed = 1f;
    public float closeDoorTime = 1f;
    private float timer = 0f;
    public bool isOpen = false;

    [Header("Door 1")]
    public GameObject door1;
    public Transform door1OpenPos;
    public Transform door1ClosePos;

    [Header("Door 2")]
    public GameObject door2;
    public Transform door2OpenPos;
    public Transform door2ClosePos;

    [Header("Sensor")]
    public Transform sensorCenter;
    public LayerMask entitieLayers;
    public float sensorRadius = 5f;

    [Header("Door Indicator")]
    public Renderer openIndicatorRendered;
    public Renderer locedIndicatorRenderer;

    [Header("Locked")]
    public bool doorLocked = true;

    private void Start()
    {
        timer = closeDoorTime;
    }

    private void Update()
    {
        if (!doorLocked)
        {
            CheckForEnities();
        }

        if (timer <= 0)
        {
            CloseDoors();
            isOpen = false;
        }

        // maybe add a time check for this so not every frame
        if (doorLocked)
        {
            openIndicatorRendered.material.color = Color.gray;
            locedIndicatorRenderer.material.color = Color.red;
        }
        else if (!doorLocked)
        {
            openIndicatorRendered.material.color = Color.lightGreen;
            locedIndicatorRenderer.material.color = Color.gray;
        }
    }

    private void CheckForEnities()
    {
        Collider[] sensorCheck = Physics.OverlapSphere(
            sensorCenter.position,
            sensorRadius,
            entitieLayers
        );

        if (sensorCheck.Length != 0)
        {
            timer = closeDoorTime;
            OpenDoors();
            isOpen = true;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    private void OpenDoors()
    {
        door1.transform.position = 
            Vector3.MoveTowards(
                door1.transform.position, 
                door1OpenPos.position, 
                doorAnimSpeed * Time.deltaTime
            );

        door2.transform.position =
            Vector3.MoveTowards(
                door2.transform.position,
                door2OpenPos.position,
                doorAnimSpeed * Time.deltaTime
            );

        isOpen = true;
    }

    private void CloseDoors()
    {
        door1.transform.position =
            Vector3.MoveTowards(
                door1.transform.position,
                door1ClosePos.position,
                doorAnimSpeed * Time.deltaTime
            );

        door2.transform.position =
            Vector3.MoveTowards(
                door2.transform.position,
                door2ClosePos.position,
                doorAnimSpeed * Time.deltaTime
            );
        
        isOpen = false;
    }

    private void OnDrawGizmos()
    {
        Vector3 center = sensorCenter != null
            ? sensorCenter.transform.position
            : transform.position;

        Collider[] hits = Physics.OverlapSphere(center, sensorRadius, entitieLayers);

        // Red if detecting something, green if not
        Gizmos.color = hits.Length > 0 ? Color.red : Color.green;

        Gizmos.DrawWireSphere(center, sensorRadius);
    }
}
