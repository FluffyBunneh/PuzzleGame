using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GhostMode : MonoBehaviour
{
    public Vector3 _offset;
    public bool _ghostModeActive;
    public CharacterControllerScript _characterController;
    private Vector3 _moveDirXZ;
    public CamMovement _camMove;
    public Transform _camPos;
    public float _flySpeed;
    public BoxCollider _col;
    // Start is called before the first frame update
    void Start()
    {
        _camMove = GetComponent<CamMovement>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ActivateGhostMode();
        MoveCam();
        DieToLight();

    }
    public void ActivateGhostMode()
    {
        Ray _ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit _hit;
        if (Physics.Raycast(_ray, out _hit, 50) && _ghostModeActive)
        {

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log(_hit.collider.name);
                if (_hit.collider.gameObject.tag == "Doll")
                {
                    _characterController = _hit.collider.GetComponent<CharacterControllerScript>();

                    transform.SetParent(_hit.collider.transform.Find("CamPos"), true);
                    transform.rotation = Quaternion.Euler(transform.parent.forward);
                    transform.position = transform.parent.position;
                    _ghostModeActive = false;
                }

            }
        }
        Debug.DrawRay(_ray.origin, _ray.direction, Color.red);
        if (Input.GetMouseButtonDown(1) && !_ghostModeActive)
        {
            _characterController.enabled = false;
            _characterController = null;
            _ghostModeActive = true;
            transform.SetParent(null);
        }
        if (!_ghostModeActive)
        {
            _camMove.enabled = false;
            _characterController.enabled = true;
        }
        else if (_ghostModeActive)
        {
            _camMove.enabled = true;
        }
    }
    public void MoveCam()
    {
        if (_ghostModeActive)
        {
            _moveDirXZ.x = Input.GetAxis("Horizontal") * _flySpeed * Time.deltaTime;
            _moveDirXZ.z = Input.GetAxis("Vertical") * _flySpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.E))
            {
                _moveDirXZ.y = 1 * _flySpeed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                _moveDirXZ.y = -1 *_flySpeed * Time.deltaTime;
            }
            else
            {
                _moveDirXZ.y = 0;
            }
            transform.Translate(_moveDirXZ);
        }
    }
    public void DieToLight()
    {
        _col = GetComponent<BoxCollider>();
        if (_ghostModeActive)
        {
            _col.enabled = true;
        }
        else
        {
            _col.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 6)
        {
            SceneManager.LoadScene(0);
        }
    }
}
