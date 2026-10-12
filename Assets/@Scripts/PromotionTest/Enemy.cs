using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("체력 설정")]
    [SerializeField] private int _maxHp = 30;

    [Header("감지 설정")]
    [SerializeField] private float _sightRange = 8f;
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private float _rotationSpeed = 5f;

    [Header("공격 설정")]
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _attackInterval = 1.5f;

    [Header("오브젝트 연결")]
    [SerializeField] private Transform _player;
    [SerializeField] private EnemyAnimation _enemyAnim;
    [SerializeField] private EnemyHealthUI _healthUI;

    private int _currentHp;
    private float _nextAttackTime;
    private bool _isDead;

    private void Awake()
    {
        _currentHp = _maxHp;
    }

    private void Start()
    {
        _healthUI.InitHP(_maxHp);
    }

    private void Update()
    {
        if (_isDead || _player == null)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, _player.position);

        if (distance <= _sightRange)
        {
            LookAtPlayer();

            if (distance <= _attackRange)
            {
                AttackPlayer();
            }
        }
    }

    private void LookAtPlayer()
    {
        Vector3 direction = _player.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }

    private void AttackPlayer()
    {
        if (Time.time < _nextAttackTime)
        {
            return;
        }

        _nextAttackTime = Time.time + _attackInterval;

        _enemyAnim.PlayAttack();
        AttackDamage();
    }

    public void AttackDamage()
    {
        if (_isDead || _player == null)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, _player.position);

        if (distance <= _attackRange)
        {
            GameManager.Instance.PlayerTakeDamage(_damage);
        }
    }

    public void TakeDamage(int damage)
    {
        if (_isDead)
        {
            return;
        }

        _currentHp -= damage;
        _currentHp = Mathf.Clamp(_currentHp, 0, _maxHp);

        _healthUI.UpdateHP(_currentHp);

        if (_currentHp <= 0)
        {
            Die();
            return;
        }

        _enemyAnim.PlayHit();
    }

    private void Die()
    {
        _isDead = true;
        _enemyAnim.PlayDead();

        Collider enemyCollider = GetComponent<Collider>();

        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _sightRange);
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}