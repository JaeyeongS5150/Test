using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] private Slider _hpSlider;

    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (_mainCamera == null)
        {
            return;
        }

        transform.rotation = Quaternion.LookRotation(transform.position - _mainCamera.transform.position);
    }

    public void InitHP(int maxHp)
    {
        _hpSlider.minValue = 0;
        _hpSlider.maxValue = maxHp;
        _hpSlider.value = maxHp;
    }

    public void UpdateHP(int currentHp)
    {
        _hpSlider.value = currentHp;
    }
}