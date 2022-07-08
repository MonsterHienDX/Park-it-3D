using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveAheadThenBack : MonoBehaviour
{
    private RectTransform _rectTransform;

    [SerializeField] private RectTransform target;
    [SerializeField] private float duration;
    public bool isActive = true;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        Move();
    }

    private void Move()
    {
        _rectTransform.DOMove(new Vector3(0, 0, 0), duration, true);
    }

}
