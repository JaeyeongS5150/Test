using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator _animator;
    private bool _isDead;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayAttack()
    {
        if (_isDead)
        {
            return;
        }

        _animator.SetTrigger("Attack");
    }

    public void PlayHit()
    {
        if (_isDead)
        {
            return;
        }

        _animator.SetTrigger("Hit");
    }

    public void PlayDead()
    {
        if (_isDead)
        {
            return;
        }

        _isDead = true;

        _animator.ResetTrigger("Attack");
        _animator.ResetTrigger("Hit");
        _animator.SetTrigger("Dead");
    }
}
