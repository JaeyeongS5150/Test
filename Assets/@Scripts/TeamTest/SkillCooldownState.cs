using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCooldownState : ISkillState
{
    private SkillCooldown _skillCooldown;

    public SkillCooldownState(SkillCooldown skillCooldown)
    {
        _skillCooldown = skillCooldown;
    }

    public void UseSkill(int index)
    {
        float remainTime = _skillCooldown.RemainingTime;
        Debug.Log($"스킬을 사용할 수 없습니다. / 남은 시간 {remainTime}");
    }
}
