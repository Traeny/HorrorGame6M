using Player_Script;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{

    public GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player Missing / Not Found!");
            return;
        }
    }

    public void KillPlayerAction()
    {
        Debug.Log("Killed the player!");
    }

}
