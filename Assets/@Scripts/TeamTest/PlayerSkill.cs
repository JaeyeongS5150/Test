using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkill : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private SkillCooldown _skillCooldown;

    private ISkillState _currentState;

    private MeshRenderer _mesh;
    private Color _originColor;
    private Color[] _skillColor = {Color.blue, Color.green, Color.red, Color.cyan};

    private InputAction _skillAction;
    private InputAction _resetSkillAction;

    public SkillCooldown Cooldown => _skillCooldown;

    private void Awake()
    {
        _mesh = GetComponent<MeshRenderer>();
        _skillAction = _playerInput.actions["Skill"];
        _resetSkillAction = _playerInput.actions["ResetSkill"];
    }

    private void OnEnable()
    {
        _skillAction.performed += OnSkill;
        _resetSkillAction.performed += OnResetSkill;
    }

    private void OnDisable()
    {
        _skillAction.performed -= OnSkill;
        _resetSkillAction.performed -= OnResetSkill;
    }

    private void Start()
    {
        _originColor = _mesh.material.color;
        ChangeState(new SkillReadyState(this));
    }

    private void OnSkill(InputAction.CallbackContext ctx)
    {
        switch (ctx.control.name)
        {
            case "q":
                _currentState.UseSkill(0);
                break;
            case "w":
                _currentState.UseSkill(1);
                break;
            case "e":
                _currentState.UseSkill(2);
                break;
            case "r":
                _currentState.UseSkill(3);
                break;
        }
    }

    /// <summary>스킬 초기화</summary>
    private void OnResetSkill(InputAction.CallbackContext ctx)
    {
        _skillCooldown.CancelCooldown();
    }

    /// <summary>스킬 사용</summary>
    public void ExecuteSkill(int index)
    {
        ChangeColor(index);
    }

    /// <summary>스킬에 따른 색 변화</summary>
    private void ChangeColor(int index)
    {
        _mesh.material.color = _skillColor[index];
    }

    /// <summary>기본 색으로 초기화</summary>
    public void OriginColor()
    {
        _mesh.material.color = _originColor;
    }

    /// <summary>ready or cooldown 상태 변화</summary>
    public void ChangeState(ISkillState nextState)
    {
        _currentState = nextState;
    }
}
