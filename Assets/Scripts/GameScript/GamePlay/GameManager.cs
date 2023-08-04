using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] public GameObject GameWinMenu;
    [SerializeField] private GameObject GameOverMenu;
    [SerializeField] public GameObject bombButton;

    public int blockPrice = 100;
    public int effectPrice = 100;
    public GameObject mainCam;
    public BlockPool blockPool;
    public GameObject selectBlock;
    public GameObject input;
    public CoinController gainCoinsAnim;

    [SerializeField]
    internal int currentLevel = 1;
    internal int camSize;

    [SerializeField]
    internal int totalBlocks;
    internal int countBlocks;
    internal int countTouchs;
    internal bool isOnMenu = false;
    internal bool allowedVibrating;
    public int coin;
    public LevelData data;
    private void Awake()
    {
        //SoundManager.instance.PlayBackgroundMusic();

        if (PlayerPrefs.GetInt("Easy Level List", 0) == 0)
        {
            PlayerPrefs.SetInt("Easy Level List", 1);
            PlayerPrefs.SetInt($"Level 1 passed", 1);

        }

        if (PlayerPrefs.GetInt("Medium Level List", 0) == 0)
        {
            PlayerPrefs.SetInt("Medium Level List", 11);
            PlayerPrefs.SetInt($"Level 11 passed", 1);

        }

        if (PlayerPrefs.GetInt("Hard Level List", 0) == 0)
        {
            PlayerPrefs.SetInt("Hard Level List", 31);
            PlayerPrefs.SetInt($"Level 31 passed", 1);
        }

        Application.targetFrameRate = 120;
        PlayerPrefs.SetInt($"Level {currentLevel} passed", 1);
        Instance = this;
        AchievementGoalsController.ReadData(GameWinMenu.GetComponent<WinGamePanelController>().achievementPanel.reachedData);
        GameWinMenu.GetComponent<WinGamePanelController>().achievementPanel.InitializeList();
        GameWinMenu.SetActive(false);
    }

    private void Start()
    {
        GameWinMenu.SetActive(false);
        GameOverMenu.SetActive(false);
        currentLevel = PlayerPrefs.GetInt("Current Level", 0) + 1;
        coin = 0;
        bombButton.SetActive(currentLevel >= 6);

        if (!UIManager.instance.isAwake)
        {

            PlayerPrefs.SetInt($"Level {currentLevel} passed", 1);
            GameManager.Instance.blockPool.StartInit(currentLevel);
            ParticleController.instance.InitializeSystem();
            countBlocks = blockPool.Size;
            totalBlocks = countBlocks;
            countTouchs = countBlocks + 700;
            UIManager.instance.UpdateBlocksNum();
            UIManager.instance.UpdateTouchsNum();
            UIManager.instance.SetLevelText();
        }
    }

    public void WinGame()
    {
        SoundManager.instance.PlayWinGameSound();
        ParticleController.instance.OnWinGame();
        if (allowedVibrating)
        {
            Debug.Log("Vibrate");
            VibrationManager.Vibrate(30);
        }
        GameManager.Instance.selectBlock.SetActive(false);
        if (currentLevel == data.numberOfLevels)
        {
            PlayerPrefs.SetInt($"Level {currentLevel} passed", 1);
            currentLevel = data.numberOfLevels - 1;
        }
        PlayerPrefs.SetInt($"Level {currentLevel} passed", 1);
        PlayerPrefs.SetInt("Easy Level List", PlayerPrefs.GetInt("Easy Level List") < currentLevel ? currentLevel : PlayerPrefs.GetInt("Easy Level List"));
        PlayerPrefs.SetInt("Medium Level List", PlayerPrefs.GetInt("Medium Level List") < currentLevel ? currentLevel : PlayerPrefs.GetInt("Medium Level List"));
        PlayerPrefs.SetInt("Hard Level List", PlayerPrefs.GetInt("Hard Level List") < currentLevel ? currentLevel : PlayerPrefs.GetInt("Hard Level List"));

        AchievementGoalsController.UpdateList(totalBlocks, currentLevel);
        float time = 1.5f;
        StartCoroutine(EnableWinGamePanel(time));
        //GameWinMenu.SetActive(true);
    }

    IEnumerator EnableWinGamePanel(float t)
    {
        while (t > 0)
        {
            t -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        GameWinMenu.SetActive(true);
        yield return null;
    }

    public void NextLevel()
    {
        PlayerPrefs.SetInt("Current Level", currentLevel);
        SceneManager.LoadScene("SampleScene");
    }

    public void ChangeLevel(int i)
    {
        Time.timeScale = 1;
        PlayerPrefs.SetInt("Current Level", i - 1);
        SceneManager.LoadScene("SampleScene");
    }

    public void GameOver()
    {
        SoundManager.instance.PlayGameOverSound();
        if (allowedVibrating)
        {
            Debug.Log("Vibrate");
            VibrationManager.Vibrate(30);
        }
        GameManager.Instance.selectBlock.SetActive(false);
        GameOverMenu.SetActive(true);
    }
}
