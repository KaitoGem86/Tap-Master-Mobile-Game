using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectPuzzlePopUpController : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Image background;
    [SerializeField] TMP_Text notifiaction;
    [SerializeField] TMP_Text tapToContinue;
    [SerializeField] GameObject[] pieces;
    int index;
    PuzzleStatus status;


    public int Index
    {
        get { return index; }
        set { index = value; }
    }

    public PuzzleStatus Status
    {
        get { return status; }
        set { status = value; }
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        var seq = DOTween.Sequence();
        seq.Append(this.transform.DOScale(Vector3.one, duration: 0.3f).SetEase(Ease.InBack));
        seq.AppendCallback(() => CollectingNotice());
    }

    // Update is called once per frame
    void Update()
    {
        TapToExit();
    }

    void CollectingNotice()
    {
        for (int i = 0; i < pieces.Length; i++)
        {
            if (i == index)
            {
                pieces[i].SetActive(true);
            }
            else pieces[i].SetActive(status.status[i]);
        }
        Color a = pieces[index].GetComponent<Image>().color;
        a.a = 0;
        pieces[index].GetComponent<Image>().DOColor(a, duration: 1.5f).SetDelay(0.4f).OnComplete(() => pieces[index].SetActive(false));
    }

    void ExitWithAnim()
    {
        this.transform.DOScale(Vector3.zero, duration: 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            GameManager.Instance.isOnMenu = false;
            GameManager.Instance.blockPool.canRotate = true;
            this.gameObject.SetActive(false);
            this.transform.localScale = Vector3.one;
        });
    }

    void TapToExit()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ExitWithAnim();
        }
    }

    public void PreEnable()
    {
        this.transform.localScale = Vector3.zero;
    }
}
