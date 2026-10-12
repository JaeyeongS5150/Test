using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System;

public class SkillCooldown : MonoBehaviour
{
    [SerializeField] private PlayerSkill _playerSkill;
    [SerializeField] private float _skillCooldown = 3.0f;

    private float _remainingTime;
    private CancellationTokenSource _cooldownCts;

    public float RemainingTime => _remainingTime;
    public float CooldownTime => _skillCooldown;

    /// <summary>쿨타임 시작 유니테스크 메서드</summary>
    public async UniTask StartCooldown()
    {
        _cooldownCts = new CancellationTokenSource();
        _remainingTime = _skillCooldown;

        try
        {
            while (_remainingTime > 0)
            {
                _remainingTime -= Time.deltaTime;
                await UniTask.Yield(cancellationToken: _cooldownCts.Token);
            }
        }

        catch (OperationCanceledException)
        {
            Debug.Log("쿨타임 초기화");
        }

        finally
        {
            _cooldownCts.Dispose();
            _cooldownCts= null;
            _remainingTime = 0f;

            _playerSkill.OriginColor();
            _playerSkill.ChangeState(new SkillReadyState(_playerSkill));
        }
    }

    /// <summary>쿨타임 초기화 메서드</summary>
    public void CancelCooldown()
    {
        if (_cooldownCts == null)
        {
            return;
        }

        _cooldownCts.Cancel();
    }
}
