using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{

    [Header("이동 설정")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 10f;

    [Header("점프 설정")]
    [SerializeField] private float _jumpForce = 7f;

    [Header("땅 체크")]
    [SerializeField] private float _groundCheckDistance = 1.1f;
    [SerializeField] private LayerMask _groundLayer;

    [Header("피격 설정")]
    [SerializeField] private float _damageInterval = 0.5f;

    [Header("오브젝트 연결")]
    [SerializeField] private CameraFollow _cameraFollow;
    [SerializeField] private PlayerAnimation _playerAnim;
    [SerializeField] private PlayerAttack _playerAttack;

    private Rigidbody _rb;

    private Vector2 _moveInput;
    
    private float _nextDamageTime;

    private bool _isGrounded;
    private bool _wasGrounded;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        CheckGround();
        _wasGrounded = _isGrounded;
    }

    private void Update()
    {
        _wasGrounded = _isGrounded;
        CheckGround();


        if (!_wasGrounded && _isGrounded && !CheckDead())
        {
            _playerAnim.PlayLand();
        }
    }


    private void FixedUpdate()
    {
        if (CheckDead())
        {
            return;
        }

        MovePlayer();
    }


    private void OnMove(InputValue value)
    {
        if (CheckDead())
        {
            return;
        }

        _moveInput = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        if (CheckDead() || !value.isPressed || !_isGrounded)
        {
            return;
        }

        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        _playerAnim.PlayJump();
    }

    private void OnAttack(InputValue value)
    {
        if (CheckDead() || !value.isPressed)
        {
            return;
        }

        if (_playerAttack.PlayAttack())
        {
            _playerAnim.PlayAttack();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (CheckDead() || other.gameObject.layer != LayerMask.NameToLayer("Trap"))
        {
            return;
        }

        if (Time.time >= _nextDamageTime)
        {
            GameManager.Instance.PlayerTakeDamage(GameManager.Instance.TrapDamage);
            _nextDamageTime = Time.time + _damageInterval;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!GameManager.Instance.IsPlaying)
        {
            return;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("DeadZone"))
        {
            GameManager.Instance.GameOver();
        }
    }

    private void MovePlayer()
    {
        if (_cameraFollow.IsQuarterView)
        {
            QuarterViewMove();
        }
        else
        {
            NormalMove();
        }
    }

    #region ---플레이어 이동---

    private void NormalMove()
    {
        Vector3 moveDirection =
            transform.forward * _moveInput.y
            + transform.right * _moveInput.x;

        if (moveDirection.sqrMagnitude > 1f)
        {
            moveDirection.Normalize();
        }

        Vector3 velocity = _rb.velocity;

        velocity.x = moveDirection.x * _moveSpeed;
        velocity.z = moveDirection.z * _moveSpeed;

        _rb.velocity = velocity;
    }

    private void QuarterViewMove()
    {
        Vector3 moveDirection = new Vector3(_moveInput.x, 0f, _moveInput.y);

        if (moveDirection.sqrMagnitude > 1f)
        {
            moveDirection.Normalize();
        }

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
        }

        Vector3 velocity = _rb.velocity;

        velocity.x = moveDirection.x * _moveSpeed;
        velocity.z = moveDirection.z * _moveSpeed;

        _rb.velocity = velocity;
    }

    #endregion ---플레이어 이동---


    private void CheckGround()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _groundCheckDistance, _groundLayer);
    }

    public void Die()
    {
        _moveInput = Vector2.zero;

        Vector3 velocity = _rb.velocity;
        velocity.x = 0f;
        velocity.z = 0f;
        _rb.velocity = velocity;

        _playerAnim.PlayDead();
    }

    private bool CheckDead()
    {
        return GameManager.Instance.IsDead;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * _groundCheckDistance);
    }
}
