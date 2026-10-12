using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("타겟")]
    [SerializeField] private Transform _target;

    [Header("3인칭")]
    [SerializeField] private Vector3 _thirdPersonOffset = new Vector3(0f, 2f, -4f);

    [Header("1인칭")]
    [SerializeField] private Vector3 _firstPersonOffset = new Vector3(0f, 1.7f, 0.2f);

    [Header("쿼터뷰")]
    [SerializeField] private Vector3 _quarterViewOffset = new Vector3(6f, 8f, -6f);

    [Header("카메라 세팅")]
    [SerializeField] private float _mouseSensitivity = 3f;

    [Header("수직 회전")]
    [SerializeField] private float _minPitch = -30f;
    [SerializeField] private float _maxPitch = 70f;

    private bool _isFirstPerson = false;
    private bool _isQuarterView = false;

    private float _yaw;
    private float _pitch;

    public bool IsQuarterView => _isQuarterView;

    private void Start()
    {
        _yaw = _target.eulerAngles.y;
    }

    private void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsPlaying)
        {
            return;
        }

        if (!_isQuarterView)
        {
            RotateCamera();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            _isFirstPerson = !_isFirstPerson;
            _isQuarterView = false;
            _yaw = _target.eulerAngles.y;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            _isQuarterView = !_isQuarterView;
            _isFirstPerson = false;

            if (!_isQuarterView)
            {
                _yaw = _target.eulerAngles.y;
            }
        }
    }

    private void LateUpdate()
    {
        if (_target == null)
        {
            return;
        }

        if (_isQuarterView)
        {
            QuarterViewCamera();
        }
        else if (_isFirstPerson)
        {
            FirstPersonCamera();
        }
        else
        {
            ThirdPersonCamera();
        }
    }

    private void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _yaw += mouseX * _mouseSensitivity;
        _pitch -= mouseY * _mouseSensitivity;

        _pitch = Mathf.Clamp(_pitch, _minPitch, _maxPitch);

        _target.rotation = Quaternion.Euler(0f, _yaw, 0f);
    }

    private void ThirdPersonCamera()
    {
        Vector3 targetPosition = _target.position + _target.rotation * _thirdPersonOffset;

        transform.position = targetPosition;

        Vector3 lookPosition = _target.position + Vector3.up * 1.5f;

        transform.LookAt(lookPosition);
    }

    private void FirstPersonCamera()
    {
        Vector3 targetPosition = _target.position + _target.rotation * _firstPersonOffset;

        transform.position = targetPosition;
        transform.rotation = _target.rotation;
    }

    private void QuarterViewCamera()
    {
        Vector3 targetPosition = _target.position + _quarterViewOffset;

        transform.position = targetPosition;

        Vector3 lookPosition = _target.position + Vector3.up;

        transform.LookAt(lookPosition);
    }
}