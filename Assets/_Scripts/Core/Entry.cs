using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entry : MonoBehaviour
{
    public Slot[] slots;
    public bool isVertical;
    public Transform gateTrans;

    public void Init()
    {
        isVertical = (this.transform.localEulerAngles.y == 0 || this.transform.localEulerAngles.y == 180) ? true : false;

        slots = GetComponentsInChildren<Slot>();

        foreach (Slot s in slots) s.gatePos = gateTrans.position;
    }

    public Slot GetSlot(int index = 0)
    {
        return slots[index];
    }

}