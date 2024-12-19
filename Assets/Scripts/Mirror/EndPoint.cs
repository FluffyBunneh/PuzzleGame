using UnityEngine;

public class DoorRaycastHit : MonoBehaviour
{
    public GameObject door;  // The door GameObject to disable
    public float raycastDistance = 10f;  // Distance of the raycast
    public LayerMask raycastLayerMask;  // LayerMask to specify which objects the raycast can hit

    void Update()
    {
        // Perform a raycast from the camera or any other point you choose
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward); // Ray coming from the camera or player

        // Cast the ray and check if it hits something
        if (Physics.Raycast(ray, out hit, raycastDistance, raycastLayerMask))
        {
            // Check if the raycast hits the door object
            if (hit.collider.gameObject == door)
            {
                // Disable the door
                door.SetActive(false);
            }
        }
    }
}
