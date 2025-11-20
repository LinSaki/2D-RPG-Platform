using UnityEngine;

public class HealthTracker : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 1.2f, 0);

    void LateUpdate()
    {
        transform.position = player.position + offset;
    }
}
