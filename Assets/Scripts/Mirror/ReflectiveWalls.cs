using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReflectiveWallRotation : MonoBehaviour
{
    public float rotationSpeed = 1f;
    public float interactionDistance = 5f;
    public Transform player;

    private GameObject nearestReflectiveWall;
    private List<Transform> targetPoints = null;
    private int currentTargetIndex = -1;
    private bool isRotating = false;

    void Update()
    {
        if (!isRotating)
        {
            DetectReflectiveWall();

            if (nearestReflectiveWall != null && Input.GetKeyDown(KeyCode.F))
            {
                InteractWithWall();
            }
        }
    }

    void DetectReflectiveWall()
    {
        RaycastHit hit;

        if (Physics.Raycast(player.position, player.forward, out hit, interactionDistance))
        {
            if (hit.collider.CompareTag("Reflectable"))
            {
                nearestReflectiveWall = hit.collider.gameObject;

                ReflectiveWallPoints wallPoints = nearestReflectiveWall.GetComponent<ReflectiveWallPoints>();
                if (wallPoints != null && targetPoints == null)
                {
                    targetPoints = wallPoints.targetPoints;
                    currentTargetIndex = -1;
                }
            }
        }
        else
        {
            nearestReflectiveWall = null;
            targetPoints = null;
        }
    }

    void InteractWithWall()
    {
        if (nearestReflectiveWall != null && targetPoints != null && targetPoints.Count > 0)
        {
            MoveToNextTarget();
        }
    }

    void MoveToNextTarget()
    {
        currentTargetIndex = (currentTargetIndex + 1) % targetPoints.Count;

        Transform target = targetPoints[currentTargetIndex];
        isRotating = true;
        StartCoroutine(RotateToFaceTarget(nearestReflectiveWall.transform, target.position));
    }

    IEnumerator RotateToFaceTarget(Transform wallTransform, Vector3 targetPosition)
    {
        Quaternion startRotation = wallTransform.rotation;
        Vector3 directionToTarget = (targetPosition - wallTransform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        float elapsedTime = 0f;
        float duration = 1.5f / rotationSpeed;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            t = 1f - Mathf.Pow(1f - t, 2);

            wallTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        wallTransform.rotation = targetRotation;
        isRotating = false;
    }
}
