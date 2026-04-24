using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{
    [Header("Door 1")]
    public GameObject door1;
    public Transform door1OpenPos;
    public Transform door1ClosePos;

    [Header("Door 2")]
    public GameObject door2;
    public Transform door2OpenPos;
    public Transform door2ClosePos;

    [Header("Others for now")]
    public GameObject sensorCenter;
    public float doorAnimSpeed = 1f;
    public float closeDoorTime = 1f;

    public LayerMask entitieLayers;
    public float sensorRadius = 5f;

    private float timer = 0f;


    private void Start()
    {
        timer = closeDoorTime;
    }

    private void Update()
    {
        CheckForEnities();

        if (timer <= 0)
        {
            CloseDoors();
        }
    }

    private void CheckForEnities()
    {
        Collider[] sensorCheck =
            Physics.OverlapSphere(
                transform.position,
                sensorRadius,
                entitieLayers
            );

        if(sensorCheck.Length != 0)
        {
            timer = closeDoorTime;
            OpenDoors();
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
    }

}
