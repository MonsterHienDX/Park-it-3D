using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PointingSomethings : MonoBehaviour
{
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    [SerializeField] private Vector3 endValue;
    [SerializeField] private float duration;
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        StartMove();
    }

    private void OnDisable()
    {
        StopMove();
    }

    public void StartMove()
    {
        _canvasGroup.DOFade(1f, Const.PANEL_SLIDE_SPEED);
        _rectTransform.DOAnchorPos(endValue, duration)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void StopMove()
    {
        _canvasGroup.DOFade(0f, Const.PANEL_SLIDE_SPEED);
    }


}
