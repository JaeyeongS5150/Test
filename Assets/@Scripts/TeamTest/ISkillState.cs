using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillState
{
    /// <summary>PlayerSkill에서 호출될 메서드</summary>
    void UseSkill(int index);
}
