using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelPanelController : MonoBehaviour
{
    [Header("Elements of panel")]
    [SerializeField] GameObject easyLevelList;
    [SerializeField] GameObject mediumLevelList;
    [SerializeField] GameObject hardLevelList;
    [SerializeField] Toggle easyLevelToggle;
    [SerializeField] Toggle mediumLevelToggle;
    [SerializeField] Toggle hardLevelToggle;
    [SerializeField] LevelItem changeLevelShortCut;

    int easyLevelMax;
    int mediumLevelMax;
    int hardLevelMax;
    public static GameObject currentLevelList;

    RectTransform rect;
    float width;

    public void OnEnable()
    {
        easyLevelMax = PlayerPrefs.GetInt("Easy Level List", 0);
        mediumLevelMax = PlayerPrefs.GetInt("Medium Level List", 0);
        hardLevelMax = PlayerPrefs.GetInt("Hard Level List", 0);

        int currentLevel = PlayerPrefs.GetInt("Current Level", 0) + 1;
        if (currentLevel < 11)
        {
            currentLevelList = easyLevelList;
            easyLevelToggle.isOn = true;
        }
        else if (currentLevel < 31 && 11 < currentLevel)
        {
            Debug.Log(mediumLevelList.name);
            currentLevelList = mediumLevelList;
            mediumLevelToggle.isOn = true;
        }
        else if (currentLevel > 30)
        {
            currentLevelList = hardLevelList;
            hardLevelToggle.isOn = true;
        }
        easyLevelList.SetActive(false);
        mediumLevelList.SetActive(false);
        hardLevelList.SetActive(false);
        Debug.Log(currentLevelList.name);
        currentLevelList.SetActive(true);
        this.transform.DOMove(UIManager.instance.canvas.transform.position, duration: 0.3f).SetEase(Ease.InOutSine);
    }

    private void Start()
    {

        rect = GetComponent<RectTransform>();
        width = UIManager.instance.canvas.pixelRect.width;
    }

    public void ExitPanel()
    {
        Vector3 r = this.transform.position + Vector3.right * width;
        this.transform.DOMove(r, duration: 0.3f).SetEase(Ease.InSine).OnComplete(Exit);
    }

    void Exit()
    {
        GameManager.Instance.blockPool.canRotate = true;
        GameManager.Instance.isOnMenu = false || UIManager.instance.isAwake;
        gameObject.SetActive(false);
    }

    public void ChooseEasyLevelList()
    {
        if (easyLevelToggle.isOn)
        {
            easyLevelList.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
            changeLevelShortCut.Level = easyLevelMax;
            changeLevelShortCut.SetLevelText(easyLevelMax);
            easyLevelList.SetActive(true);
            currentLevelList.SetActive(false);
            currentLevelList = easyLevelList;
            mediumLevelToggle.interactable = true;
            hardLevelToggle.interactable = true;
            easyLevelToggle.interactable = false;
        }
    }

    public void ChooseMediumLevelList()
    {
        if (mediumLevelToggle.isOn)
        {
            mediumLevelList.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
            changeLevelShortCut.Level = mediumLevelMax;
            changeLevelShortCut.SetLevelText(mediumLevelMax);
            mediumLevelList.SetActive(true);
            currentLevelList.SetActive(false);
            currentLevelList = mediumLevelList;
            easyLevelToggle.interactable = true;
            hardLevelToggle.interactable = true;
            mediumLevelToggle.interactable = false;
        }
    }

    public void ChooseHardLevelList()
    {
        if (hardLevelToggle.isOn)
        {
            hardLevelList.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
            changeLevelShortCut.Level = hardLevelMax;
            changeLevelShortCut.SetLevelText(hardLevelMax);
            hardLevelList.SetActive(true);
            currentLevelList.SetActive(false);
            currentLevelList = hardLevelList;
            easyLevelToggle.interactable = true;
            mediumLevelToggle.interactable = true;
            hardLevelToggle.interactable = false;
        }
    }
}
