using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
public class ButtonAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        this.transform.DOScale(0.85f, 0.2f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        this.transform.DOScale(1f, 0.2f);
    }

}

