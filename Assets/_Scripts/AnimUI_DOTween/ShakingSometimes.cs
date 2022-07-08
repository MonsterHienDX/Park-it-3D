using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Animator))]
public class ShakingSometimes : MonoBehaviour
{
    private RectTransform rectTransform;
    [SerializeField] private float shakeDuration = 1;
    [SerializeField] private float delay = 3;
    // [SerializeField] private float strength = 45;
    // [SerializeField] private int vibrato = 7;
    // [SerializeField] private float randomness = 9;
    // [SerializeField] private bool fadeOut = false;
    private float _delay;
    private Animator _animator;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        _animator = GetComponent<Animator>();
        _delay = delay;
    }

    private void Update()
    {
        // if (delay > 0)
        // {
        //     delay -= Time.deltaTime;
        //     Shake(0);
        // }
        // else
        // {
        //     Shake(shakeDuration);
        //     delay = _delay;
        // }
    }

    private void Start()
    {
        // StartCoroutine(Shake());
        Shake();
    }
    private float maxRotate = 30f;
    private Vector3 v3 = Vector3.one;
    private void Shake()
    {
        // rectTransform.DOShakeRotation(shakeDuration, strength, vibrato, randomness, fadeOut);

        // transform.DORotate(v3 *= (maxRotate * -1), shakeDuration)
        //     .SetLoops(30, LoopType.Yoyo);
        // yield return ExtensionClass.GetWaitForSeconds(delay);
        // StartCoroutine(Shake());

        // rectTransform.DOPunchScale()

    }
}
