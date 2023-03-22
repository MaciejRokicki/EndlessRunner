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
    private TextMeshProUGUI pressAnyKeyTimerLabel;

    [SerializeField]
    private GameObject game;
    [SerializeField]
    private TextMeshProUGUI timerLabel;
    [SerializeField]
    private TextMeshProUGUI distanceLabel;
    private RectTransform distanceLabelBackground;
    private RectTransform gameOverTimerProgressBar;
    [SerializeField]
    private RectTransform gameOverTimerProgressBarFill;

    [SerializeField]
    private GameObject pause;

    [SerializeField]
    private GameObject gameOver;
    [SerializeField]
    private TextMeshProUGUI gameOverTimerLabel;
    [SerializeField]
    private TextMeshProUGUI gameOverDistanceLabel;

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
        gameOverTimerProgressBar = gameOverTimerProgressBarFill.parent as RectTransform;
    }

    private void Start()
    {
        Cursor.visible = false;
    }

    private void OnPressAnyKeyTimerChange(float time)
    {
        if(time > 0.0f)
        {
            pressAnyKeyTimerLabel.text = time.ToString();
        }
        else
        {
            TogglePressAnyKeyTimerLabel();
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

    private void OnGameOverTimerChange(float time)
    {
        if(time == 0.0f)
        {
            gameOverTimerProgressBar.gameObject.SetActive(false);
        }
        else
        {
            gameOverTimerProgressBar.gameObject.SetActive(true);
            gameOverTimerProgressBarFill.offsetMax = new Vector2(
                -((gameManager.GameOverTimer - time) / gameManager.GameOverTimer * gameOverTimerProgressBar.sizeDelta.x),
                gameOverTimerProgressBarFill.offsetMax.y
            );
        }
    }

    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
        this.gameManager.OnPressAnyKeyTimerChange += OnPressAnyKeyTimerChange;
        this.gameManager.OnGameTimerChange += OnGameTimeChange;
        this.gameManager.OnGameOverTimerChange += OnGameOverTimerChange;
    }

    public void TogglePressAnyKey() => pressAnyKey.SetActive(!pressAnyKey.activeSelf);

    public void TogglePressAnyKeyTimerLabel() => pressAnyKeyTimerLabel.gameObject.SetActive(!pressAnyKeyTimerLabel.gameObject.activeSelf);

    public void ToggleGameUI() => game.SetActive(!game.activeSelf);

    public void TogglePause()
    {
        pause.SetActive(!pause.activeSelf);
        Cursor.visible = !Cursor.visible;
    }

    public void ToggleGameOver()
    {
        gameOver.SetActive(!gameOver.activeSelf); 
        Cursor.visible = !Cursor.visible;

        StringBuilder sb = new StringBuilder("Time played: ");
        sb.Append(timerLabel.text);
        gameOverTimerLabel.text = sb.ToString();

        sb.Clear();

        sb.Append("Distance: ");
        sb.Append(distanceLabel.text);
        gameOverDistanceLabel.text = sb.ToString();
    }

    public void ButtonResume()
    {
        Cursor.visible = !Cursor.visible;
        gameManager.IsPause = false;
    }

    public void ButtonRetry() => SceneManager.LoadScene(0);

    public void ButtonExit() => Application.Quit();
}
