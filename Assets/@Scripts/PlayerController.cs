using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody _rigidbody;
    Camera _camera;
    float _camSpeed = 8f;
    bool _isGround = true;
    Vector3 _camOffset = new Vector3(0, 10, -10);

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _camera = Camera.main;
        _camera.transform.position = transform.position + _camOffset;
        _camera.transform.rotation = Quaternion.Euler(new Vector3(30, 0, 0));
    }

    private void Update()
    {
        Jump();
        CameraMove();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        if(Input.GetButton(Define.Horizontal)||Input.GetButton(Define.Vertical))
        {
            float h = Input.GetAxis(Define.Horizontal);
            float v = Input.GetAxis(Define.Vertical);

            Vector3 movement = new Vector3(h, _rigidbody.linearVelocity.y, v);
            Vector3 localMovement = transform.TransformDirection(movement);
            _rigidbody.linearVelocity = localMovement.normalized * 10;
        }
        else
        {
            _rigidbody.linearVelocity = new Vector3(0, _rigidbody.linearVelocity.y, 0);
        }
    }

    void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space)&&_isGround)
        {
            _isGround = false;
            _rigidbody.AddForce(Vector3.up * 300f);
        }
    }

    void CameraMove()
    {
        _camera.transform.position = transform.position + _camOffset;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            _isGround = true;
        }
    }
}
