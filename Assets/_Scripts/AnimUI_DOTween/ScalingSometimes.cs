using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ScalingSometimes : MonoBehaviour
{
    private RectTransform rectTransform;
    [SerializeField] private float scaleDuration = 0.7f;
    [SerializeField] private float delay = 1;

    [SerializeField] private float endScaleValue = 1.2f;
    [SerializeField] private int vibrato = 5;
    [SerializeField] private float elasticity = 0.2f;
    private float _delay;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        _delay = delay;
    }

    private void Start()
    {
        Scale();
    }

    private void Scale()
    {
        // rectTransform.DOPunchScale(Vector3.one * endScaleValue, scaleDuration, vibrato, elasticity);
        rectTransform.DOScale(Vector3.one * endScaleValue, scaleDuration)
            .SetLoops(-1, LoopType.Yoyo);
    }

}
