using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private const float ZTILT_ANGLE = 5f;
    private const float MAX_ANGLE_X = 60f;

    [Header("Movement Variables")]
    [SerializeField] private float _moveSpeed = 20f;

    [SerializeField] private float _maxMovespeed = 350f;
    [SerializeField] private float _jumpHeight = 1.5f;
    [SerializeField] private int _jumpCount = 1;
    [SerializeField] private float _drag = 3f;

    [Header("Camera Variables")]
    [SerializeField] private Transform _cameraAnchor;

    [SerializeField] private Transform _camera;
    [SerializeField] private float _mouseSensitivity;
    private Vector3 _input;
    private Vector3 _velocity;
    private CharacterController _cc;
    private int _jumps;
    public float MouseSensitivity { get => _mouseSensitivity; set => _mouseSensitivity = value; }
    public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _input = Vector2.zero;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    private void Update()
    {
        _input = (_cameraAnchor.forward * Input.GetAxis("Vertical")) + (_cameraAnchor.right * Input.GetAxis("Horizontal")).normalized;
        UpdateMovement();
        UpdateCamera();
    }

    private void UpdateMovement()
    {
        Vector3 inputCopy = _input;
        inputCopy.y = 0;
        _velocity += _moveSpeed * Time.deltaTime * inputCopy;
        _velocity.x *= (1 - _drag * Time.deltaTime);
        _velocity.z *= (1 - _drag * Time.deltaTime);
        
        // If its grounded make sure to reset jump count and velocity
        if (_cc.isGrounded)
        {
            _velocity.y = 0;
            _jumps = _jumpCount;
        } 

        // Jump stuff
        if (Input.GetButtonDown("Jump") && _jumps > 0)
        {
            _velocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * Physics.gravity.y);
            _jumps--;
        }
        

        // Gravity x2
        _velocity.y += Physics.gravity.y * Time.deltaTime * 2f;

        _velocity.x = Mathf.Clamp(_velocity.x, -_maxMovespeed, _maxMovespeed);
        _velocity.z = Mathf.Clamp(_velocity.z, -_maxMovespeed, _maxMovespeed);
        _cc.Move(_velocity * Time.deltaTime);
    }

    private void UpdateCamera()
    {
        float rotX = -Input.GetAxis("Mouse Y") * MouseSensitivity;
        float rotY = Input.GetAxis("Mouse X") * MouseSensitivity;

        rotX = Mathf.Clamp(rotX, -MAX_ANGLE_X, MAX_ANGLE_X);
        _cameraAnchor.rotation = Quaternion.Euler(
            _cameraAnchor.eulerAngles.x + rotX,
            _cameraAnchor.eulerAngles.y,
            0);

        float tiltValue = -Input.GetAxis("Horizontal") * ZTILT_ANGLE;
        Quaternion targetRot = Quaternion.Euler(_camera.localRotation.x, _camera.localRotation.y, tiltValue);
        _camera.localRotation = Quaternion.Lerp(_camera.localRotation, targetRot, Time.deltaTime * 3f);

        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + rotY, transform.eulerAngles.z);
    }

    public void Teleport(Vector3 position)
    {
        _cc.enabled = false;
        transform.position = position;
        _cc.enabled = true;
    }
}