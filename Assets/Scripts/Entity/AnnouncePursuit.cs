using UnityEngine;

public class AnnouncePursuit : MonoBehaviour
{
    private bool inAction;
    private float elapsed = 0f;
    private float duration = 1f;

    public Node.Status AnnouncePlayerPursuit()
    {
        if (!inAction)
        {
            inAction = true;
            elapsed = 0f;
        }

        if (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            return Node.Status.RUNNING;
        }
        else
        {
            inAction = false;
            return Node.Status.SUCCESS;
        }
    }

}
