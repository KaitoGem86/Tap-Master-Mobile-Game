using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] public GameObject GameWinMenu;
    [SerializeField] private GameObject GameOverMenu;

    public int blockPrice = 100;
    public GameObject mainCam;
    public BlockPool blockPool;
    public GameObject selectBlock;
    public GameObject input;

    [SerializeField]
    internal int currentLevel = 1;

    internal int camSize;

    [SerializeField]
    internal int totalBlocks;

    internal int countBlocks;
    internal int countTouchs;

    public int coin;
    public LevelData data;
    private void Awake()
    {
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
        coin = 0;
        currentLevel = PlayerPrefs.GetInt("Current Level", 0) + 1;
        PlayerPrefs.SetInt($"Level {currentLevel} passed", 1);
        GameManager.Instance.blockPool.StartInit(currentLevel);
        countBlocks = blockPool.Size;
        totalBlocks = countBlocks;
        countTouchs = countBlocks + 7;
        UIManager.instance.UpdateBlocksNum();
        UIManager.instance.UpdateTouchsNum();
    }

    public void WinGame()
    {
        GameManager.Instance.selectBlock.SetActive(false);
        if (currentLevel == data.numberOfLevels)
        {
            PlayerPrefs.SetInt($"Level {currentLevel} passed", 1);
            currentLevel = data.numberOfLevels - 1;
        }
        PlayerPrefs.SetInt($"Level {currentLevel} passed", 1);
        AchievementGoalsController.UpdateList(totalBlocks, currentLevel);
        GameWinMenu.SetActive(true);
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
        GameManager.Instance.selectBlock.SetActive(false);
        GameOverMenu.SetActive(true);
    }
}
