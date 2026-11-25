using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Instance")]
    public static LevelManager Instance;

    [Header("Counter")]
    private int _extinguishedFires = 0;
    private int _burnedOutFires = 0;

    [Header("UI")]
    [SerializeField] private Canvas _finalCanvas;
    [SerializeField] private TextMeshProUGUI _victoryFailText;
    [SerializeField] private TextMeshProUGUI _burnedOutText;
    [SerializeField] private TextMeshProUGUI _extinguishedText;
    [SerializeField] private Button _returnButton;
    [SerializeField] private Button _retryButton;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if(Instance == null) { Instance = this; }
        else if (Instance != this) {Destroy(this.gameObject); }
        _extinguishedFires = 0;
        _burnedOutFires = 0;

        _finalCanvas.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        EventBus.OnExtinguished += CountExtinguishedFires;
        EventBus.OnBurnedOut += AddBurnedOutFires;

        _retryButton.onClick.AddListener(RetryLevel);
        _returnButton.onClick.AddListener(ReturnMainMenu);
    }

    private void OnDisable()
    {
        EventBus.OnExtinguished -= CountExtinguishedFires;
        EventBus.OnExtinguished -= AddBurnedOutFires;

        _retryButton.onClick.RemoveListener(RetryLevel);
        _returnButton.onClick.RemoveListener(ReturnMainMenu);
    }

    private void CountExtinguishedFires(int quantity)
    {
        _extinguishedFires += quantity;
        CheckWinLoseCondition();
    }

    private void AddBurnedOutFires(int quantity)
    {
        _burnedOutFires += quantity;
        CheckWinLoseCondition();
    }

    private void CheckWinLoseCondition()
    {
        int totalInactiveFires = _extinguishedFires + _burnedOutFires;

        if (totalInactiveFires >= IIgnitableCount.Instance.IignitableCount)
        {
            if(_extinguishedFires >= totalInactiveFires) { EndGame(true); }
            else { EndGame(false); }
        }
    }

    private void EndGame(bool isVictory)
    {
        _finalCanvas.gameObject.SetActive(true);
        if (isVictory)
        {
            _victoryFailText.text = "Victory!";
            _victoryFailText.color = Color.blue;
            StatsText();
            if(SceneManager.GetActiveScene().name == "Lvl1") 
            { 
                JsonConverter.Instance.PlayerInfo.canAccessSecondLevel = true;
                JsonConverter.Instance.SaveJson();
            }
        }
        else
        {
            _victoryFailText.text = "Fail";
            _victoryFailText.color = Color.red;
            StatsText();
        }

        Time.timeScale = 0f;
    }

    private void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ReturnMainMenu()
    {
        ScenesManager.Instance.LoadMenu();
    }

    private void StatsText()
    {
        _burnedOutText.text = "Burned Out Fires: " + _burnedOutFires.ToString();
        _burnedOutText.color = Color.red;

        _extinguishedText.text = "Extinguished Fires: " + _extinguishedFires.ToString();
        _extinguishedText.color = Color.blue;
    }
}