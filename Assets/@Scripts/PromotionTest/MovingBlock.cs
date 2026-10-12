using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    [Header("블록 설정")]
    [SerializeField] private Vector3 _moveDirection = Vector3.right;
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _moveDistance = 2f;
    [SerializeField] private float _waitTime = 0.5f;

    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private Vector3 _targetPosition;

    private float _waitTimer;
    private bool _isWaiting;

    void Start()
    {
        _startPosition = transform.position;
        _endPosition = _startPosition + _moveDirection.normalized * _moveDistance;
        _targetPosition = _endPosition;
    }


    void Update()
    {
        if (_isWaiting)
        {
            _waitTimer += Time.deltaTime;

            if (_waitTimer >= _waitTime)
            {
                _waitTimer = 0f;
                _isWaiting = false;
                ChangeDirection();
            }

            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _targetPosition) < 0.01f)
        {
            _isWaiting = true;
        }
    }

    private void ChangeDirection()
    {
        if (_targetPosition == _endPosition)
        {
            _targetPosition = _startPosition;
        }
        else
        {
            _targetPosition = _endPosition;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(_startPosition, _endPosition);
        Gizmos.DrawWireCube(_startPosition, transform.localScale);
        Gizmos.DrawWireCube(_endPosition, transform.localScale);
    }
    
}
