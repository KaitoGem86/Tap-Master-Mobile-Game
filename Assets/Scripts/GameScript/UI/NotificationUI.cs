using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationUI : MonoBehaviour
{
    [SerializeField] Transform uNotification;

    private void Start()
    {
        uNotification.DOScale(1.2f, duration: 1).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.OutSine);
    }
}
