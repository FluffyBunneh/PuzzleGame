using UnityEngine;

public class LaserPuzzle : MonoBehaviour
{
    public Transform startPoint;          // The starting point of the laser
    public float maxDistance = 50f;       // Maximum distance for the laser
    public LayerMask reflectableLayer;    // Layer mask for the reflective surfaces
    public LayerMask buttonLayer;         // Layer mask for the button (end point)
    public LineRenderer lineRenderer;     // LineRenderer to visualize the laser
    public float laserWidth = 0.05f;      // Laser beam width
    public Color laserColor = Color.red;  // Laser color
    public GameObject door;               // Reference to the door GameObject

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = laserWidth;
        lineRenderer.endWidth = laserWidth;
        lineRenderer.startColor = laserColor;
        lineRenderer.endColor = laserColor;

        if (startPoint == null || lineRenderer == null)
        {
            Debug.LogError("Please assign all necessary references!");
            return;
        }
    }

    private void Update()
    {
        ShootLaser();
    }

    void ShootLaser()
    {
        Vector3 laserOrigin = startPoint.position;
        Vector3 direction = transform.forward;  // The laser shoots in the object's forward direction

        // Initialize line renderer with 1 point at the start
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, laserOrigin);

        // Start casting the ray in the forward direction
        CastReflections(laserOrigin, direction);
    }

    void CastReflections(Vector3 laserOrigin, Vector3 direction)
    {
        RaycastHit hit;
        // Cast a ray from the laser's current position in the given direction
        if (Physics.Raycast(laserOrigin, direction, out hit, maxDistance, reflectableLayer | buttonLayer))
        {
            // Update the laser path with the new hit point
            int currentPosition = lineRenderer.positionCount;
            lineRenderer.positionCount++; // Add a new point for this hit
            lineRenderer.SetPosition(currentPosition, hit.point);

            // Check if the laser hits the button (end point)
            if (((1 << hit.collider.gameObject.layer) & buttonLayer) != 0)
            {
                // Disable the door when the laser hits the button
                DisableDoor();

                // Stop further reflections once it hits the button
                return;
            }

            // Reflect the ray and continue bouncing if not hitting the button
            Vector3 reflectedDirection = Vector3.Reflect(direction, hit.normal);
            laserOrigin = hit.point;  // Set new origin for the next raycast
            direction = reflectedDirection;  // Update direction for the next raycast

            // Recursively continue the reflections
            CastReflections(laserOrigin, direction);
        }
        else
        {
            // If no hit, show the laser traveling directly in the direction for the max distance
            int currentPosition = lineRenderer.positionCount;
            lineRenderer.positionCount++; // Add a final point at max distance
            lineRenderer.SetPosition(currentPosition, laserOrigin + direction * maxDistance);
        }
    }

    void DisableDoor()
    {
        if (door != null)
        {
            door.SetActive(false);  // Disable the door
            Debug.Log("Door disabled!");
        }
    }
}
