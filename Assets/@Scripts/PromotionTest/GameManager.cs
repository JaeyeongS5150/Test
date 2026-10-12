using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public enum GameState
    {
        Ready,
        Play,
        Clear,
        GameOver
    }

    [Header("게임 설정")]
    [SerializeField] private float _timeLimit = 60f;

    [Header("플레이어 설정")]
    [SerializeField] private int _hp = 50;
    [SerializeField] private int _maxHp = 50;

    [Header("외부요인 설정")]
    [SerializeField] private int _trapDamage = 5;

    [Header("오브젝트 연결")]
    [SerializeField] private GameObject _player;

    [Header("UI")]
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private GameObject _gameUI;
    [SerializeField] private GameObject _clearPanel;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _hpText;
    [SerializeField] private Slider _hpSlider;

    private PlayerAnimation _playerAnimation;
    private bool _isDead;

    private float _remainingTime;
    private GameState _gameState = GameState.Ready;

    public bool IsPlaying => _gameState == GameState.Play;
    public bool IsDead => _gameState == GameState.GameOver;
    public int TrapDamage => _trapDamage;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        _playerAnimation = _player.GetComponent<PlayerAnimation>();
    }

    void Start()
    {
        InitGame();
    }


    void Update()
    {
        if (!IsPlaying)
        {
            return;
        }

        UpdateTimer();
    }

    private void InitGame()
    {
        _gameState = GameState.Ready;

        _remainingTime = _timeLimit;
        _hp = _maxHp;

        _startPanel.SetActive(true);
        _gameUI.SetActive(false);
        _clearPanel.SetActive(false);
        _gameOverPanel.SetActive(false);

        _hpSlider.minValue = 0;
        _hpSlider.maxValue = _maxHp;

        UpdateHPUI();
        UpdateTimeUI();

        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void StartGame()
    {
        if (_gameState != GameState.Ready)
        {
            return;
        }

        _gameState = GameState.Play;

        _startPanel.SetActive(false);
        _gameUI.SetActive(true);
        _gameOverPanel.SetActive(false);

        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void UpdateTimer()
    {
        _remainingTime -= Time.deltaTime;

        if (_remainingTime <= 0f)
        {
            _remainingTime = 0f;
            UpdateTimeUI();
            GameOver();
            return;
        }

        UpdateTimeUI();
    }

    public void PlayerHeal(int healAmount)
    {
        if (!IsPlaying || healAmount <= 0)
        {
            return;
        }

        _hp += healAmount;
        _hp = Mathf.Clamp(_hp, 0, _maxHp);

        UpdateHPUI();

        Debug.Log($"Player Heal : + {healAmount}");

    }

    public void PlayerTakeDamage(int damage)
    {
        if (!IsPlaying)
        {
            return;
        }

        _hp -= damage;
        _hp = Mathf.Clamp(_hp, 0, _maxHp);

        UpdateHPUI();

        Debug.Log("Player HP : " + _hp);

        if (_hp <= 0)
        {
            GameOver();
            return;
        }

        _playerAnimation.PlayHit();
    }

    private void UpdateHPUI()
    {
        _hpSlider.value = _hp;
        _hpText.text = "HP " + _hp + " / " + _maxHp;
    }

    private void UpdateTimeUI()
    {
        _timeText.text = "TIME  " + Mathf.CeilToInt(_remainingTime);
    }

    public void ClearGame()
    {
        if (!IsPlaying)
        {
            return;
        }

        _gameState = GameState.Clear;

        _gameUI.SetActive(false);
        _clearPanel.SetActive(true);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void GameOver()
    {
        if (!IsPlaying)
        {
            return;
        }

        _gameState = GameState.GameOver;

        _player.GetComponent<PlayerMove>().Die();

        _gameUI.SetActive(false);
        _gameOverPanel.SetActive(true);

        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
