using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillReadyState : ISkillState
{
    private PlayerSkill _playerSkill;

    public SkillReadyState(PlayerSkill playerSkill)
    {
        _playerSkill = playerSkill;
    }

    public void UseSkill(int index)
    {
        _playerSkill.ExecuteSkill(index);
        _playerSkill.ChangeState(new SkillCooldownState(_playerSkill.Cooldown));
        _playerSkill.Cooldown.StartCooldown().Forget();
    }
}
