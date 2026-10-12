using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody _rb;
    private bool _isDead;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        if (_isDead)
        {
            return;
        }

        UpdateMovementAnimation();
    }

    private void UpdateMovementAnimation()
    {
        Vector3 horizontalVelocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
        _animator.SetFloat("Speed", horizontalVelocity.magnitude);
    }

    public void PlayJump()
    {
        if (_isDead)
        {
            return;
        }

        _animator.ResetTrigger("Land");
        _animator.SetTrigger("Jump");
    }

    public void PlayLand()
    {
        if (_isDead)
        {
            return;
        }

        _animator.ResetTrigger("Jump");
        _animator.SetTrigger("Land");
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

        _animator.ResetTrigger("Jump");
        _animator.ResetTrigger("Land");
        _animator.ResetTrigger("Hit");
        _animator.SetFloat("Speed", 0f);
        _animator.SetTrigger("Dead");
    }
}
