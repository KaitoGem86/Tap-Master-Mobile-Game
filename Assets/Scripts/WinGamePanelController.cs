using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinGamePanelController : MonoBehaviour
{
    [SerializeField] TMP_Text coinText;
    // Start is called before the first frame update
    private void Start()
    {
        SetSummaryCoin();
    }

    // Update is called once per frame
    void Update()
    {
        TapToContinue();
    }

    void TapToContinue()
    {
        if (InputController.instance.CheckSelect())
        {
            GameManager.Instance.NextLevel();
        }
    }

    void SetSummaryCoin()
    {
        coinText.SetText($"+ {GameManager.Instance.coin}");
    }
}
