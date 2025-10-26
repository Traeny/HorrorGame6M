using Player_Script;
using UnityEngine;

public class HidePlayer : MonoBehaviour
{
    public Collider playerCollider;

    void Start()
    {
        Collider hideCollider = GetComponent<Collider>();

        GameObject player = GameObject.FindWithTag("Player");

        playerCollider = player.GetComponent<Collider>();

        Physics.IgnoreCollision(playerCollider, hideCollider);
    }
}