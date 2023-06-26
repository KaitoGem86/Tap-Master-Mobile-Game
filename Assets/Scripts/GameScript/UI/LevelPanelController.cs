using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPanelController : MonoBehaviour
{


    public void ExitPanel()
    {
        Time.timeScale = 1;
        GameManager.Instance.blockPool.gameObject.SetActive(true);
        GameManager.Instance.selectBlock.SetActive(true);
        gameObject.SetActive(false);
    }
}
