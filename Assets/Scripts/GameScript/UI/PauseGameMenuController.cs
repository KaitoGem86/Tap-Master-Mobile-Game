using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseGameMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseGameMenu;
    [SerializeField] private GameObject blockListPanel;
    [SerializeField] private GameObject gamePlayModeMenu;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject tapPivot;
    [SerializeField] private Button button1;
    [SerializeField] private Button button2;


    float width;
    RectTransform rect;
    public static bool isInteractable = false;


    public void OnEnable()
    {
        this.transform.DOMove(UIManager.instance.canvas.transform.position, duration: 0.3f).SetEase(Ease.InOutSine);
    }

    public void ExitPanel()
    {
        Vector3 r = this.transform.position + Vector3.right * width;
        this.transform.DOMove(r, duration: 0.3f).SetEase(Ease.InSine).OnComplete(Exit);
        GameManager.Instance.blockPool.gameObject.SetActive(true);
    }

    private void Start()
    {
        rect = this.GetComponent<RectTransform>();
        width = UIManager.instance.canvas.pixelRect.width;
    }

    private void Update()
    {
        if (blockListPanel.activeSelf || gamePlayModeMenu.activeSelf || settingPanel.activeSelf)
            return;
        TapToContinue();
    }

    void Exit()
    {
        GameManager.Instance.isOnMenu = false;
        GameManager.Instance.blockPool.canRotate = true;
        this.gameObject.SetActive(false);
        if (UIManager.instance.isAwake)
        {
            GameManager.Instance.currentLevel = PlayerPrefs.GetInt("Current Level", 0) + 1;
            PlayerPrefs.SetInt($"Level {GameManager.Instance.currentLevel} passed", 1);
            GameManager.Instance.blockPool.StartInit(GameManager.Instance.currentLevel);
            ParticleController.instance.InitializeSystem();
            GameManager.Instance.countBlocks = GameManager.Instance.blockPool.Size;
            GameManager.Instance.totalBlocks = GameManager.Instance.countBlocks;
            GameManager.Instance.countTouchs = GameManager.Instance.countBlocks + 700;
            UIManager.instance.UpdateBlocksNum();
            UIManager.instance.UpdateTouchsNum();
            UIManager.instance.isAwake = false;
            UIManager.instance.SetLevelText();
        }
    }

    public void OpenBlockListPanel()
    {
        if (!UIManager.instance.isAwake)
        {
            ExitPanel();
        }
        blockListPanel.SetActive(true);
        //this.gameObject.SetActive(false);
    }


    public void OpenLevelList()
    {
        if (!UIManager.instance.isAwake)
        {
            ExitPanel();
        }
        gamePlayModeMenu.SetActive(true);
        //this.gameObject.SetActive(false);
    }

    public void OpenSettingPanel()
    {
        if (!UIManager.instance.isAwake)
        {
            ExitPanel();
        }
        settingPanel.SetActive(true);
    }

    void TapToContinue()
    {
        if (Input.GetMouseButtonDown(0) && isInteractable && Input.mousePosition.y < tapPivot.transform.position.y - tapPivot.GetComponent<RectTransform>().rect.width / 2 /*&& Input.mousePosition.y > gamePlayModeMenu.transform.position.y*/)
        {
            var t = rect.position + Vector3.right * width;
            this.transform.DOMove(t, duration: 0.3f).SetEase(Ease.InOutSine).OnComplete(Exit);
        }
    }

    void Continue()
    {
        GameManager.Instance.isOnMenu = false;
        this.gameObject.SetActive(false);
    }
}
