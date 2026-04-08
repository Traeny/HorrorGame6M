using UnityEngine;

public class PlayerInAttackRange : MonoBehaviour
{
    public EnemyPreset preset;

    [Header("Components")]
    public GameObject player;
    private float timer = 0;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player Missing!");
            return;
        }

        timer = preset.delay;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            InAttackRange();
            timer = preset.delay;
        }
    }

    private void InAttackRange()
    {
        float distanceToTarget = Vector3.Distance(transform.position, player.transform.position);

        if(distanceToTarget <= preset.attackRange)
        {
            Blackboard.Instance.playerInAttackRange = true;
        }
        else
        {
            Blackboard.Instance.playerInAttackRange = false;
        }
    }
}
