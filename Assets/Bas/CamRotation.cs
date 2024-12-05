using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotation : MonoBehaviour
{
    public Vector2 _camRot;
    public float _rotateSpeed;
    
    void FixedUpdate()
    {

        _camRot.y -= Input.GetAxis("Mouse Y") * _rotateSpeed * Time.deltaTime;
        _camRot.x += Input.GetAxis("Mouse X") * _rotateSpeed * Time.deltaTime;
        _camRot.y = Mathf.Clamp(_camRot.y, -53.702f, 64.832f);
        transform.rotation = Quaternion.Euler(_camRot.y, _camRot.x, 0f);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
