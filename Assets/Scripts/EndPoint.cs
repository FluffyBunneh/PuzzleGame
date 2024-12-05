using UnityEngine;

public class EndPoint : MonoBehaviour
{
    private bool isLit = false;

    public void LightDetected()
    {
        if (!isLit)
        {
            isLit = true;
            Debug.Log("Puzzle Solved! The endpoint is lit!");
        }
    }

    private void Update()
    {
        isLit = false;
    }
}
