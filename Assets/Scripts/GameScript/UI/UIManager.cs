using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text remainingBlocks;
    [SerializeField] private TMP_Text remainingTouchs;
    [SerializeField] private TMP_Text CoinText;
    [SerializeField] private GameObject pauseGameMenu;
    [SerializeField] private GameObject blockListPanel;
    [SerializeField] private GameObject gamePlayModeMenu;
    [SerializeField] private TMP_Text levelText;

    public static UIManager instance;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        pauseGameMenu.SetActive(false);
        blockListPanel.SetActive(false);
        gamePlayModeMenu.SetActive(false);
        SetLevelText();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetCoinText()
    {
        CoinText.SetText(PlayerPrefs.GetInt("Coin", 0).ToString());
    }



    public void SetLevelText()
    {
        levelText.SetText($"Level " + GameManager.Instance.currentLevel);
    }

    public void UpdateBlocksNum()
    {
        remainingBlocks.SetText($"Remaining Blocks: {GameManager.Instance.countBlocks}");
    }

    public void UpdateTouchsNum()
    {
        remainingTouchs.SetText($"Remaining Touchs: {GameManager.Instance.countTouchs}");
    }

    public void ReplayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseGameMenu.SetActive(true);
    }

    public void ChooseBlock()
    {
        Time.timeScale = 0f;
        blockListPanel.SetActive(true);
        GameManager.Instance.blockPool.gameObject.SetActive(false);
    }

    public void ChoosePlayMode()
    {
        Time.timeScale = 0f;
        gamePlayModeMenu.SetActive(true);
        GameManager.Instance.blockPool.gameObject.SetActive(false);
    }


}
