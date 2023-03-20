using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    #region Singletons
    private static UIManager instance;
    public static UIManager Instance
    {
        get { return instance; }
    }
    private WorldGenerator worldGenerator;
    private GameManager gameManager;
    #endregion

    [Header("References")]
    [SerializeField]
    private GameObject pressAnyKey;

    [SerializeField]
    private GameObject game;
    [SerializeField]
    private TextMeshProUGUI timerLabel;
    [SerializeField]
    private TextMeshProUGUI distanceLabel;
    private RectTransform distanceLabelBackground;

    [SerializeField]
    private GameObject pause;

    [SerializeField]
    private GameObject gameOver;
    [SerializeField]
    private TextMeshProUGUI gameOverTimerLabel;
    [SerializeField]
    private TextMeshProUGUI gameOverDistanceLabel;

    [HideInInspector]
    public bool PressAnyKeyToPlay = true;
    [HideInInspector]
    public bool IsPause = true;
    [HideInInspector]
    public bool IsGameOver = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        worldGenerator = WorldGenerator.Instance;

        worldGenerator.OnDistanceChange += OnDistanceChange;

        distanceLabelBackground = distanceLabel.rectTransform.parent as RectTransform;
    }

    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !PressAnyKeyToPlay)
        {
            TogglePause();
        }
    }

    private void OnGameTimeChange(float time)
    {
        TimeSpan playedTimeSpan = TimeSpan.FromSeconds(time);
        timerLabel.text = playedTimeSpan.ToString(@"mm\:ss\.fff");
    }

    private void OnDistanceChange(float distance)
    {   
        distanceLabel.text = distance.ToString();
        distanceLabelBackground.sizeDelta = new Vector2(20.0f * distanceLabel.text.Length + 25.0f, 40.0f);
        distanceLabelBackground.anchoredPosition = new Vector2(distanceLabelBackground.sizeDelta.x / 2.0f, -70.0f);
    }

    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
        this.gameManager.OnGameTimerChange += OnGameTimeChange;
    }

    public void TogglePressAnyKey()
    {
        pressAnyKey.SetActive(!pressAnyKey.activeSelf);
        PressAnyKeyToPlay = !PressAnyKeyToPlay;
        IsPause = false;
    }

    public void ToggleGameUI()
    {
        game.SetActive(!game.activeSelf);
    }

    public void TogglePause()
    {
        game.SetActive(!game.activeSelf);
        pause.SetActive(!pause.activeSelf);
        IsPause = !IsPause;
        Cursor.visible = !Cursor.visible;
    }

    public void ToggleGameOver()
    {
        gameOver.SetActive(!gameOver.activeSelf);
        IsGameOver = !IsGameOver;
        ToggleGameUI();
        Cursor.visible = !Cursor.visible;

        StringBuilder sb = new StringBuilder("Time played: ");
        sb.Append(timerLabel.text);
        gameOverTimerLabel.text = sb.ToString();

        sb.Clear();

        sb.Append("Distance: ");
        sb.Append(distanceLabel.text);
        gameOverDistanceLabel.text = sb.ToString();
    }

    public void ButtonRetry()
    {
        SceneManager.LoadScene(0);
    }

    public void ButtonExit()
    {
        Application.Quit();
    }
}
