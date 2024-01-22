using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class CancelAreaController : MonoBehaviour
{
    [SerializeField] Image cancelArea;

    Tween t;
    bool check = true;
    // Start is called before the first frame update

    public void Update()
    {
        if (check && Input.GetMouseButton(0) && Vector3.Distance(Input.mousePosition, UIManager.instance.cancelArea.GetComponent<RectTransform>().position) < UIManager.instance.cancelArea.GetComponent<RectTransform>().rect.width / 2)
        {
            FingerOnCancel();
            check = false;
        }
        else if (Input.GetMouseButtonUp(0) || Input.GetMouseButton(0) && Vector3.Distance(Input.mousePosition, UIManager.instance.cancelArea.GetComponent<RectTransform>().position) > UIManager.instance.cancelArea.GetComponent<RectTransform>().rect.width / 2)
        {
            FingerOutCancel();
            check = true;
            if (Input.GetMouseButtonUp(0) && Vector3.Distance(Input.mousePosition, UIManager.instance.cancelArea.GetComponent<RectTransform>().position) < UIManager.instance.cancelArea.GetComponent<RectTransform>().rect.width / 2)
            {
                GameManager.Instance.bombButton.GetComponent<BombButtonController>().bombImage.SetActiveElement(false);
                GameManager.Instance.camMoving.CanRotate = true;
            }
        }
    }

    public void FingerOnCancel()
    {
        //Debug.Log("scale");
        t = cancelArea.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), duration: 1f).SetLoops(-1, LoopType.Yoyo);
    }

    public void FingerOutCancel()
    {
        t.Kill();
        cancelArea.transform.DOScale(Vector3.one, duration: 0.3f);
    }

}
