using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("공격")]
    [SerializeField] private int _damage = 1;
    [SerializeField] private Vector3 _attackCube = new Vector3(1.5f, 1.5f, 1.5f);
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _attackCooldown = 0.08f;

    private float _nextAttackTime;

    public bool PlayAttack()
    {
        if (Time.time < _nextAttackTime)
        {
            return false;
        }

        _nextAttackTime = Time.time + _attackCooldown;

        Collider[] hitEnemies = Physics.OverlapBox(_attackPoint.position, _attackCube / 2f, _attackPoint.rotation, _enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>()?.TakeDamage(_damage);
        }

        return true;
    }

    private void OnDrawGizmos()
    {
        if (_attackPoint == null)
        {
            return;
        }

        Gizmos.color = Color.yellow;

        Gizmos.matrix = Matrix4x4.TRS(_attackPoint.position, _attackPoint.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, _attackCube);
    }

}
