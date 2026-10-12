using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private SkillCooldown _skillCooldown;
    [SerializeField] private Image _cooldownFill;
    [SerializeField] private TMP_Text _cooldownText;

    private void Update()
    {
        _cooldownFill.fillAmount = _skillCooldown.RemainingTime / _skillCooldown.CooldownTime;

        if (_skillCooldown.RemainingTime > 0f)
        {
            _cooldownText.text = _skillCooldown.RemainingTime.ToString("F1");
        }
        else
        {
            _cooldownText.text = "";
        }
    }
}
