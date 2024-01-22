using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text remainingBlocks;
    [SerializeField] private TMP_Text remainingTouchs;
    [SerializeField] private TMP_Text CoinText;
    [SerializeField] private GameObject pauseGameMenu;
    [SerializeField] private GameObject blockListPanel;
    [SerializeField] private GameObject levelListPanel;
    [SerializeField] public GameObject existingPanel;
    [SerializeField] private GameObject puzzleRewardPanel;
    [SerializeField] public GameObject puzzleRewardNotification;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] public Canvas canvas;
    [SerializeField] public GameObject dailyRewardPanel;
    [SerializeField] private GameObject dailyRewardNotification;
    [SerializeField] private CollectRewardPopUpController collectCoinPopUp;
    [SerializeField] private CollectPuzzlePopUpController collectPuzzlePopUp;
    [SerializeField] public CollectPuzzleNotice imageNotice;
    [SerializeField] public CancelAreaController cancelArea;
    [SerializeField] public CollectDailyRewardPopUpController dailyRewardPopUp;

    DateTime now;
    public static UIManager instance;
    public bool isAwake;
    public GameObject PuzzleRewardPanel
    {
        get { return puzzleRewardPanel; }
    }

    public GameObject BlockListPanel
    {
        get => blockListPanel;
    }

    public CollectRewardPopUpController CollectCoinPopUp
    {
        get => collectCoinPopUp;
        set => collectCoinPopUp = value;
    }

    public CollectPuzzlePopUpController CollectPuzzlePopUp
    {
        get => collectPuzzlePopUp;
        set => collectPuzzlePopUp = value;
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Is Awake", 0);
        settingPanel.GetComponent<SettingPanel>().SaveSoundState();
        puzzleRewardPanel.GetComponent<PuzzleRewardPanel>().SaveStatus();
    }

    private void Awake()
    {
        settingPanel.GetComponent<SettingPanel>().InitializeSound();
        isAwake = PlayerPrefs.GetInt("Is Awake", 0) == 0;
        instance = this;
        if (isAwake)
        {
            PlayerPrefs.SetInt("Is Awake", 1);
            pauseGameMenu.gameObject.SetActive(true);
            dailyRewardPanel.gameObject.SetActive(true);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        settingPanel.GetComponent<SettingPanel>().InitializeVibrating();
        puzzleRewardNotification.SetActive(PlayerPrefs.GetInt("Having new puzzle's piece", 0) == 1);

        if (!pauseGameMenu.activeSelf)
        {
            float t = pauseGameMenu.transform.position.x + canvas.pixelRect.width;
            pauseGameMenu.GetComponent<RectTransform>().position = new Vector3(t, pauseGameMenu.transform.position.y, pauseGameMenu.transform.position.z);
        }

        blockListPanel.GetComponent<BlockListPanelController>().InitBlockSkinLists();
        blockListPanel.transform.position = this.transform.position + Vector3.left * canvas.pixelRect.width;
        blockListPanel.SetActive(false);

        levelListPanel.transform.position = this.transform.position + Vector3.right * canvas.pixelRect.width;
        levelListPanel.SetActive(false);

        puzzleRewardPanel.GetComponent<PuzzleRewardPanel>().InitPuzzles();
        puzzleRewardPanel.transform.position = this.transform.position + Vector3.right * canvas.pixelRect.width;
        puzzleRewardPanel.SetActive(false);

        settingPanel.transform.position = this.transform.position + Vector3.right * canvas.pixelRect.width;
        settingPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        dailyRewardNotification.SetActive(DailyRewardSystem.CheckCanCollect());
    }

    private void FixedUpdate()
    {
        DateTime now = DateTime.Now;
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
        remainingBlocks.SetText($"{GameManager.Instance.countBlocks} Blocks");
    }

    public void UpdateTouchsNum()
    {
        remainingTouchs.SetText($"{GameManager.Instance.countTouchs} Moves");
    }

    public void ReplayGame()
    {
        UIManager.instance.pauseGameMenu.SetActive(false);
        SceneManager.LoadScene("SampleScene");
    }

    public void PauseGame()
    {
        GameManager.Instance.camMoving.CanRotate = false;
        GameManager.Instance.isOnMenu = true;
        pauseGameMenu.SetActive(true);
    }

    public void ChooseBlock()
    {
        GameManager.Instance.camMoving.CanRotate = false;
        GameManager.Instance.isOnMenu = true;
        blockListPanel.SetActive(true);
        UIManager.instance.SetCoinText();
    }

    public void ChooseLevel()
    {
        GameManager.Instance.camMoving.CanRotate = false;
        GameManager.Instance.isOnMenu = true;
        levelListPanel.SetActive(true);
    }

    public void ChoosePuzzlePanel()
    {
        puzzleRewardNotification.SetActive(false);
        PlayerPrefs.SetInt("Having new puzzle's piece", 0);
        GameManager.Instance.camMoving.CanRotate = false;
        GameManager.Instance.isOnMenu = true;
        puzzleRewardPanel.SetActive(true);
    }

    public void ChooseDailyRewardPanel()
    {
        GameManager.Instance.camMoving.CanRotate = false;
        GameManager.Instance.isOnMenu = true;
        dailyRewardPanel.SetActive(true);
    }

    public void CollectPuzzleNotice(int i)
    {
        imageNotice.gameObject.SetActive(true);
        imageNotice.NoticeOnCollect();
        imageNotice.t.OnComplete(() =>
        {
            var p_s = PuzzleRewardPanel.GetComponent<PuzzleRewardPanel>().NotCollectReward;

            PuzzleRewardPanel.GetComponent<PuzzleRewardPanel>().Puzzles[p_s[i]].OnCollectPieces();
            CollectPuzzlePopUp.Status = PuzzleRewardPanel.GetComponent<PuzzleRewardPanel>().Statuses[p_s[i]];
            CollectPuzzlePopUp.Index = PuzzleRewardPanel.GetComponent<PuzzleRewardPanel>().Puzzles[p_s[i]].p_index;
            CollectPuzzlePopUp.PreEnable();
            CollectPuzzlePopUp.gameObject.SetActive(true);
            puzzleRewardNotification.SetActive(true);
            PlayerPrefs.SetInt("Having new puzzle's piece", 1);
            imageNotice.gameObject.SetActive(false);
        });
    }
}
