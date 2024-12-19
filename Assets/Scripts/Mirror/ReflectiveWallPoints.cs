using UnityEngine;
using System.Collections.Generic;

public class ReflectiveWallPoints : MonoBehaviour
{
    [Tooltip("List of target points this wall will face sequentially.")]
    public List<Transform> targetPoints; // Points to rotate towards
}
