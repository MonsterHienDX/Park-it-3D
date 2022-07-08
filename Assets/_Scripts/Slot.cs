using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public bool isEmpty = true;
    public bool isSelected;
    public bool isVertical;
    public Transform entryTrans;

    private void Start()
    {
        isEmpty = true;
    }
}
