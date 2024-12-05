using UnityEngine;

public class ReflectiveWallRotation : MonoBehaviour
{
    public float rotationSpeed = 50f;     // Speed of wall rotation
    public float interactionDistance = 5f; // Maximum distance for interaction
    public Transform player;              // Reference to the player (to disable movement)

    private GameObject nearestReflectiveWall;  // The nearest reflective wall to interact with
    private bool isInteracting = false;        // Whether the player is interacting with the wall

    private PlayerMovement playerMovement; // Reference to player movement script

    void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // Cast a ray from the player's position to detect walls
        DetectReflectiveWall();

        // If the player is near a reflective wall and presses F
        if (nearestReflectiveWall != null && Input.GetKeyDown(KeyCode.F))
        {
            StartInteraction();
        }

        // If the player is interacting with the wall, allow rotation with A/D keys
        if (isInteracting)
        {
            RotateWall();
        }
    }

    void DetectReflectiveWall()
    {
        RaycastHit hit;
        // Raycast in front of the player to find a reflective wall
        if (Physics.Raycast(player.position, player.forward, out hit, interactionDistance))
        {
            if (hit.collider.CompareTag("Reflectable")) // Only interact with reflective surfaces
            {
                nearestReflectiveWall = hit.collider.gameObject;
            }
        }
        else
        {
            nearestReflectiveWall = null;
        }
    }

    void StartInteraction()
    {
        if (nearestReflectiveWall != null)
        {
            // Disable player movement
            playerMovement.enabled = false;

            // Start interaction with the reflective wall
            isInteracting = true;
        }
    }

    void RotateWall()
    {
        // Rotate the nearest reflective wall using A and D keys
        if (Input.GetKey(KeyCode.A))
        {
            nearestReflectiveWall.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            nearestReflectiveWall.transform.Rotate(Vector3.down * rotationSpeed * Time.deltaTime);
        }

        // If the player releases F, stop interacting
        if (Input.GetKeyUp(KeyCode.F))
        {
            StopInteraction();
        }
    }

    void StopInteraction()
    {
        // Enable player movement again and stop interaction
        playerMovement.enabled = true;
        isInteracting = false;
    }
}
