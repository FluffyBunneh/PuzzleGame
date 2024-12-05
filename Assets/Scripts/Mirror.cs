using UnityEngine;

public class Mirror : MonoBehaviour
{
    public LayerMask reflectableLayer;   // Layer to reflect light on
    public LineRenderer lightBeam;       // LineRenderer for the light beam
    public Transform mirrorSurface;      // The transform representing the mirror's surface (the part that rotates)

    [Header("Interaction Settings")]
    public float rotationSpeed = 100f;   // Speed of mirror rotation
    public float rotationDistance = 5f;  // Distance at which the player can rotate the mirror
    private bool isRotating = false;     // Track if the player is rotating the mirror

    private PlayerMovement playerMovementScript; // Reference to the player's movement script
    private Transform playerTransform;           // Reference to the player's transform

    [Header("Mirror Settings")]
    public bool isFirstMirror = false; // Mark the first mirror that shoots the light beam
    private static Mirror firstMirror;  // Static reference to the first mirror

    private void Start()
    {
        // Get the player movement script (assuming it's attached to the player)
        playerMovementScript = FindObjectOfType<PlayerMovement>();
        playerTransform = Camera.main.transform; // Assuming the main camera is attached to the player

        // If this is the first mirror, set it as the firstMirror
        if (isFirstMirror)
        {
            firstMirror = this;
        }

        // Initially, disable the LineRenderer (not reflecting light for non-first mirrors)
        if (lightBeam != null)
        {
            lightBeam.enabled = isFirstMirror; // First mirror will always have the LineRenderer enabled
        }
    }

    private void Update()
    {
        // Check the distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // Handle mirror rotation when the player is within range
        if (distanceToPlayer <= rotationDistance && Input.GetKey(KeyCode.F))
        {
            isRotating = true;
            // Disable player movement while rotating the mirror
            if (playerMovementScript != null)
                playerMovementScript.DisableMovement();

            RotateMirror(); // Rotate the mirror with A/D keys
        }
        else
        {
            // Enable movement when mirror rotation is not happening
            if (isRotating)
            {
                isRotating = false;
                if (playerMovementScript != null)
                    playerMovementScript.EnableMovement();
            }
        }

        // If this is the first mirror, shoot the light
        if (this == firstMirror)
        {
            ShootLight();
        }
        else
        {
            ReflectLight();
        }
    }

    // Function to shoot light from the first mirror
    private void ShootLight()
    {
        if (mirrorSurface == null || lightBeam == null) return;

        Vector3 start = mirrorSurface.position; // Start position of the light
        Vector3 direction = mirrorSurface.forward; // Direction of the light
        lightBeam.SetPosition(0, start);

        // Cast a ray from the mirror surface and check for collisions with the reflectable objects
        RaycastHit hit;
        if (Physics.Raycast(start, direction, out hit, Mathf.Infinity, reflectableLayer))
        {
            lightBeam.SetPosition(1, hit.point); // Set the light beam endpoint

            Debug.Log("Raycast hit: " + hit.collider.name); // Debugging hit info

            // Check if the hit object is another mirror, and recursively shoot light from that mirror
            Mirror nextMirror = hit.collider.GetComponent<Mirror>();
            if (nextMirror != null)
            {
                nextMirror.ReflectLight(); // Reflect light on the next mirror
            }

            // Check if the hit object is an endpoint
            EndPoint endPoint = hit.collider.GetComponent<EndPoint>();
            if (endPoint != null)
            {
                endPoint.LightDetected(); // Trigger endpoint detection
            }
        }
        else
        {
            // If no hit, extend the light beam indefinitely
            lightBeam.SetPosition(1, start + direction * 100f);
        }
    }

    // Function to reflect light on non-first mirrors (these mirrors just reflect light)
    private void ReflectLight()
    {
        if (mirrorSurface == null || lightBeam == null) return;

        // Only reflect if the LineRenderer is enabled (meaning light is being reflected)
        if (!lightBeam.enabled) return;

        Vector3 start = mirrorSurface.position; // Start position of the light
        Vector3 direction = mirrorSurface.forward; // Direction of the light

        // Cast a ray from the mirror surface and check for collisions
        RaycastHit hit;
        if (Physics.Raycast(start, direction, out hit, Mathf.Infinity, reflectableLayer))
        {
            Debug.Log("Raycast hit on mirror: " + hit.collider.name); // Debugging hit info

            // Get the position and direction of the hit object
            Vector3 hitPoint = hit.point;
            Vector3 hitNormal = hit.normal;

            // Reflect the light direction based on the hit normal (calculate the reflection)
            Vector3 reflectDirection = Vector3.Reflect(direction, hitNormal); // Reflect based on the normal

            // Set the new end point for the light beam based on the reflected direction
            lightBeam.SetPosition(0, start); // Set the start position to the mirror surface
            lightBeam.SetPosition(1, hitPoint); // Set the end position to the hit point

            // Check if the hit object is another mirror and reflect the light off it
            Mirror nextMirror = hit.collider.GetComponent<Mirror>();
            if (nextMirror != null)
            {
                nextMirror.ReflectLight(); // Call ReflectLight on the next mirror if it exists
            }

            // Check if the hit object is an endpoint
            EndPoint endPoint = hit.collider.GetComponent<EndPoint>();
            if (endPoint != null)
            {
                endPoint.LightDetected(); // Trigger endpoint detection
            }
        }
    }

    // Function to rotate the mirror
    private void RotateMirror()
    {
        float horizontalInput = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f;
        }

        // Rotate the entire mirror object around the Y-axis (horizontal axis)
        transform.Rotate(Vector3.up * horizontalInput * rotationSpeed * Time.deltaTime);
    }

    // Visualize the range in the editor (for debugging purposes)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rotationDistance); // Show the rotation range in the editor
    }
}
