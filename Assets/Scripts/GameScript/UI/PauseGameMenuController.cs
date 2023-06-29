using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGameMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseGameMenu;
    [SerializeField] private GameObject blockListPanel;
    [SerializeField] private GameObject gamePlayModeMenu;

    float width;


    public void OnEnable()
    {
        //var r = this.transform.position;
        //Debug.Log(r);
        //if (r == UIManager.instance.canvas.transform.position)
        //    this.transform.position = r + Vector3.right * width;
        //else
        //    r += Vector3.left * width;
        this.transform.DOMove(UIManager.instance.canvas.transform.position, duration: 0.3f).SetEase(Ease.InOutSine);
    }


    public void ExitPanel()
    {
        Vector3 r = this.transform.position + Vector3.right * width;
        this.transform.DOMove(r, duration: 0.3f).SetEase(Ease.InSine).OnComplete(Exit); GameManager.Instance.blockPool.gameObject.SetActive(true);
    }

    private void Start()
    {
        width = UIManager.instance.canvas.pixelRect.width;
    }

    void Exit()
    {
        GameManager.Instance.selectBlock.SetActive(true);
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
}
