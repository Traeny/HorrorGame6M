using System.Collections;
using UnityEngine;

public class PlayerInAttackRange : MonoBehaviour
{
    public GameObject player;

    public float delay = 0.1f;

    public float attackRange;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player Missing / Not Found!");
            return;
        }

        StartCoroutine(DistanceRoutine());
    }

    private IEnumerator DistanceRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;
            InAttackRange();
        }
    }

    private void InAttackRange()
    {
        float distanceToTarget = Vector3.Distance(transform.position, player.transform.position);

        if(distanceToTarget <= attackRange)
        {
            Blackboard.Instance.playerInAttackRange = true;
        }
        else
        {
            Blackboard.Instance.playerInAttackRange = false;
        }
    }
}
