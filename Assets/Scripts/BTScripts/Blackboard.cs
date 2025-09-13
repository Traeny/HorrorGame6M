using UnityEngine;

public class Blackboard : MonoBehaviour
{
    static Blackboard instance;

    [Header("Conditions")]
    public bool isPlayerVisible = false;
    public bool playerInAttackRange = false;
    public bool canReachPlayer = false;

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
}
