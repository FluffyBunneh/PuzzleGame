using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerScript : MonoBehaviour
{
    public Rigidbody _rb;
    public float _moveSpeed;
    public float _maxSpeed;
    public Vector3 _moveDirXZ;
    public GameObject _cam;

    public void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {

        _moveDirXZ.x = Input.GetAxis("Horizontal");
        _moveDirXZ.z = Input.GetAxis("Vertical");


        Vector3 move = transform.right * _moveDirXZ.x + transform.forward * _moveDirXZ.z;
        
        _rb.MoveRotation(Quaternion.Euler(0, _cam.transform.eulerAngles.y, 0));
        transform.rotation = Quaternion.Euler(0, _cam.transform.eulerAngles.y, 0);
        _rb.velocity = move * _maxSpeed;

        if (_rb.velocity.magnitude > _maxSpeed)
        {
            _rb.AddForce(move * -_moveSpeed, ForceMode.Force);
        }
    }
}
