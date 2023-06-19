using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject GameWinMenu;
    public static GameManager Instance;

    public int blockPrice = 100;
    public GameObject mainCam;
    public BlockPool blockPool;

    internal int currentLevel = 1;
    internal int camSize;
    internal int count;
    public int coin;
    public LevelData data;
    private void Awake()
    {
        Application.targetFrameRate = 120;
        PlayerPrefs.SetInt($"Level {currentLevel} passed", 1);
        Instance = this;
    }


    private void Start()
    {
        GameWinMenu.SetActive(false);
        coin = 0;
        currentLevel = PlayerPrefs.GetInt("Current Level", 0) + 1;
        PlayerPrefs.SetInt($"Level {currentLevel} passed", 1);
        GameManager.Instance.blockPool.StartInit(currentLevel);
        count = blockPool.Size;
        //count = blockPool.Width * blockPool.Height * blockPool.Length;
        UIManager.instance.UpdateBlocksNum();
    }
    public void WinGame()
    {
        if (currentLevel == data.numberOfLevels)
        {

            PlayerPrefs.SetInt($"Level {currentLevel} passed", 1);
            currentLevel = data.numberOfLevels - 1;
        }
        PlayerPrefs.SetInt($"Level {currentLevel} passed", 1);
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
}
