using System.Collections;
using UnityEngine;

public class TimedPuzzle : MonoBehaviour
{
    public GameObject[] buttons;
    public float timeLimit = 6f;
    private float timer;
    private bool isTimerRunning = false;
    private int buttonsPressed = 0;
    private bool isPuzzleComplete = false;
    public GameObject gameshit;

    void Update()
    {
        if (isTimerRunning)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                ResetPuzzle();
            }
        }
    }

    public void ButtonPressed(GameObject button)
    {
        if (isPuzzleComplete) return;

        if (!isTimerRunning)
        {
            isTimerRunning = true;
            timer = timeLimit;
        }

        Renderer buttonRenderer = button.GetComponent<Renderer>();
        if (buttonRenderer != null)
        {
            buttonRenderer.material.color = Color.green;
        }

        button.GetComponent<Collider>().enabled = false;

        buttonsPressed++;

        if (buttonsPressed == buttons.Length)
        {
            PuzzleComplete();
        }
    }

    private void PuzzleComplete()
    {
        isTimerRunning = false;
        isPuzzleComplete = true;
        gameshit.SetActive(false);
        Debug.Log("Puzzle Complete!");

    }

    private void ResetPuzzle()
    {
        if (isPuzzleComplete) return;

        Debug.Log("Puzzle Failed. Reset");
        isTimerRunning = false;
        buttonsPressed = 0;

        foreach (GameObject button in buttons)
        {
            Renderer buttonRenderer = button.GetComponent<Renderer>();
            if (buttonRenderer != null)
            {
                buttonRenderer.material.color = Color.red;
            }

            button.GetComponent<Collider>().enabled = true;
        }
    }
}
