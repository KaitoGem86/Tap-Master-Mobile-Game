using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] GameObject levelList;

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
}
