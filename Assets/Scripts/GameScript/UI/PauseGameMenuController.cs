using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class PauseGameMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseGameMenu;
    [SerializeField] private GameObject blockListPanel;
    [SerializeField] private GameObject gamePlayModeMenu;
    [SerializeField] private Button button1;
    [SerializeField] private Button button2;


    float width;
    RectTransform rect;
    bool isAwake = false;
    public static bool isInteractable = false;

    public void OnEnable()
    {
        //var r = this.transform.position;
        //Debug.Log(r);
        //if (r == UIManager.instance.canvas.transform.position)
        //    this.transform.position = r + Vector3.right * width;
        //else
        //    r += Vector3.left * width;
        if (!isAwake)
        {
            isAwake = true;
        }
        else
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
        TapToContinue();
    }

    void Exit()
    {
        GameManager.Instance.isOnMenu = false;
        this.gameObject.SetActive(false);
    }

    public void OpenBlockListPanel()
    {
        blockListPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void OpenLevelList()
    {
        gamePlayModeMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }

    void TapToContinue()
    {
        if (Input.GetMouseButtonDown(0) && isInteractable && Input.mousePosition.y < blockListPanel.transform.position.y /*&& Input.mousePosition.y > gamePlayModeMenu.transform.position.y*/)
        {
            var t = rect.position + Vector3.right * width;
            this.transform.DOMove(t, duration: 0.3f).SetEase(Ease.InOutSine).OnComplete(Continue);
        }
    }

    void Continue()
    {
        GameManager.Instance.isOnMenu = false;
        this.gameObject.SetActive(false);
    }
}
