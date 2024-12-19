using UnityEngine;

public class Button : MonoBehaviour
{
    public TimedPuzzle puzzleManager; // Assign the puzzle manager in the Inspector
    private bool isPlayerNearby = false;

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            puzzleManager.ButtonPressed(gameObject);
            isPlayerNearby = false; 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assumes the player has a "Player" tag
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
}
