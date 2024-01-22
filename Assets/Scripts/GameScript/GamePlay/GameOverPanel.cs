using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] GameObject levelList;
    [SerializeField] RectTransform rect;

    public void ReplayGame()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("SampleScene");
    }

    public void ToLevelList()
    {
        Time.timeScale = 1.0f;
        this.gameObject.SetActive(false);
        levelList.SetActive(true);
        GameManager.Instance.blockPool.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        GameManager.Instance.selectBlock.SetActive(false);
        this.transform.localScale = Vector3.zero;
        this.transform.DOScale(Vector3.one, duration: 1).SetEase(Ease.InSine).OnComplete(() => { Time.timeScale = 0f; });
    }
}
