using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] GameObject levelList;
    [SerializeField] RectTransform rect;

    public void ReplayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ToLevelList()
    {
        this.gameObject.SetActive(false);
        levelList.SetActive(true);
        GameManager.Instance.blockPool.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        GameManager.Instance.selectBlock.SetActive(false);
        this.transform.localScale = Vector3.zero;
        this.transform.DOScale(Vector3.one, duration: 1).SetEase(Ease.InSine);
    }
}
