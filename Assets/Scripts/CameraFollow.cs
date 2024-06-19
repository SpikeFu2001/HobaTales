using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // The target the camera should follow (the player)
    public Transform player;
    [SerializeField]
    private Vector3 lookTarget;

    // The offset distance between the player and camera
    public Vector3 offset = new Vector3(0.0f,2.36f,-3.35f);
    public float cameraAheadDelta = 4.64f;

    void LateUpdate()
    {
        // Desired position of the camera
        Vector3 desiredPosition = player.position + offset;
        transform.position = desiredPosition;

        // Look ahead of player
        lookTarget = new Vector3(player.position.x, player.position.y, player.position.z + cameraAheadDelta);
        transform.LookAt(lookTarget);
    }
}
